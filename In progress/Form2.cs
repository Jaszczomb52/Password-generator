using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Web.UI;
using static System.IO.FileLoadException;
using System.Data.SQLite;
using System.Configuration;

namespace losowanieHasla
{
    public partial class Form2 : Form
    {
        static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        static string connType = config.AppSettings.Settings["connection"].Value;
        static string hide = config.AppSettings.Settings["hiding"].Value;
        int edytowana = 0;
        int wybrana = 0;
        string hostTworzenie = @"server=localhost;userid=root;password=;";
        string host = @"server=localhost;userid=root;
             password=;database=hasla;";
        ///////
        string tekst;
        string ip;
        string uzytkownik;
        string haslo;
        string baza;
        bool zmienione;
        ///////
        public Form2(string tekst,string ip, string uzytkownik, string haslo, string baza, bool zmienione)
        {
            InitializeComponent();
            textBox2.Text = tekst;
            if (zmienione == true && connType=="SQL")
            {
                hostTworzenie = @"server=" + ip + ";userid=" + uzytkownik + ";password=" + haslo + ";";
                host = @"server=" + ip + ";userid=" + uzytkownik + ";password=" + haslo + ";"+"database=hasla;";
            }
            this.Width = 272;

            this.tekst = tekst;
            this.ip = ip;
            this.uzytkownik = uzytkownik;
            this.haslo = haslo;
            this.baza = baza;
            this.zmienione = zmienione;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                if (connType == "SQL")
                {
                    Sql.data(hostTworzenie, "", "CREATE DATABASE IF NOT EXISTS hasla;" +
                        "USE hasla;" +
                        "CREATE TABLE IF NOT EXISTS dane(" +
                        "id int auto_increment," +
                        "nazwa varchar(50) not null," +
                        "haslo varchar(250) not null," +
                        "primary key(id));", new string[] { }, connType);
                }
                else
                {
                    Sql.data(hostTworzenie, "", "CREATE TABLE IF NOT EXISTS dane(" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                        "nazwa text NOT NULL," +
                        "haslo text NOT NULL);", new string[] { }, connType);
                }

                if (hide == "true")
                {
                    List<string> list = Sql.data(host, "read", "select id,nazwa,'*****' from dane", new string[] { "string", "string", "string" }, connType);
                    comboBox1.Items.AddRange(list.ToArray());
                    textBox2.UseSystemPasswordChar = true;
                }
                else
                {
                    List<string> list = Sql.data(host, "read", "select * from dane", new string[] { "string", "string", "string" }, connType);
                    comboBox1.Items.AddRange(list.ToArray());
                }

                // language
                if (config.AppSettings.Settings["language"].Value == "Svenska")
                {
                    this.Text = "Sparande";
                    button1.Text = "Radera";
                    button2.Text = "Spara lösenord";
                    button3.Text = "Redigera";
                    button4.Text = "Radera lösenord";
                    Skopiuj.Text = "Kopiera lösenord";
                    textBox1.Text = "Lösenordsnamn";
                }
                if (config.AppSettings.Settings["language"].Value == "English")
                {
                    this.Text = "Saving";
                    button1.Text = "Delete";
                    button2.Text = "Save password";
                    button3.Text = "Edit";
                    button4.Text = "Delete passwords";
                    Skopiuj.Text = "Copy password";
                    textBox1.Text = "Password name";
                }
            }
            catch(Exception ex)
            {
                if (config.AppSettings.Settings["language"].Value == "Polski")
                {
                    MessageBox.Show("Błąd", "Błąd bazy podczas ładowania danych z bazy. Sprawdź połączenie z bazą lub wybierz bazę offline w ustawieniach.");
                }
                if (config.AppSettings.Settings["language"].Value == "English")
                {
                    MessageBox.Show("Error", "Database error while loading data from the database. Check the connection to the base or select an offline base in the settings.");
                }
                if (config.AppSettings.Settings["language"].Value == "Svenska")
                {
                    MessageBox.Show("Fel", "Databasfel vid laddning av data från databasen. Kontrollera anslutningen till basen eller välj en offline bas i inställningarna.");
                }
            }
            

            if(config.AppSettings.Settings["mode"].Value =="dark")
            {
                Color black = Color.FromArgb(0, 0, 0);
                Color white = Color.FromArgb(255, 255, 255);
                Color grey = Color.FromArgb(70, 70, 70);
                Button[] button = { button1, button2, button3, button4, Skopiuj };
                for (int i = 0; i < button.Length; i++)
                {
                    button[i].BackColor = grey;
                }
                this.BackColor = black; this.ForeColor = white;
                comboBox1.BackColor = black; comboBox1.ForeColor = white;
                textBox1.BackColor = black; textBox1.ForeColor = white;
                textBox2.BackColor = black; textBox2.ForeColor = white;

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(comboBox1.Text.Split((")").ToCharArray())[0]);
                List<string> list;
                if (hide == "true")
                {
                    list = Sql.data(host, "read", "SELECT id,nazwa,'*****' FROM dane WHERE id=" + id.ToString(), new string[] { "string", "string", "string" }, connType);
                }
                else
                {
                    list = Sql.data(host, "read", "SELECT * FROM dane WHERE id=" + id.ToString(), new string[] { "string", "string", "string" }, connType);
                }
                textBox1.Text = list[0].Split(new string[] { ") " }, StringSplitOptions.None)[1].Split(new string[] { ", " }, StringSplitOptions.None)[0];
                textBox2.Text = list[0].Split(new string[] { ") " }, StringSplitOptions.None)[1].Split(new string[] { ", " }, StringSplitOptions.None)[1];
                edytowana = id;
            }
            catch(Exception ex)
            {
                if(config.AppSettings.Settings["language"].Value == "Polski")
                {
                    MessageBox.Show("Nie wybrałeś hasła do edycji");
                }
                if (config.AppSettings.Settings["language"].Value == "English")
                {
                    MessageBox.Show("You have not chosen a password to edit");
                }
                if (config.AppSettings.Settings["language"].Value == "Svenska")
                {
                    MessageBox.Show("Du har inte valt ett lösenord att redigera");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (edytowana != 0)
            {
                try
                {
                    Sql.data(host, "", "UPDATE dane SET nazwa = '" + textBox1.Text.ToString() + "', haslo = '" + textBox2.Text.ToString() + "' WHERE id = " + edytowana,
                        new string[] { }, connType);
                }
                catch (Exception)
                {
                    if (config.AppSettings.Settings["language"].Value == "Polski")
                    {
                        MessageBox.Show("Coś poszło nie tak ¯\\_(ツ)_/¯");
                    }
                    if (config.AppSettings.Settings["language"].Value == "English")
                    {
                        MessageBox.Show("Something went wrong ¯\\_(ツ)_/¯");
                    }
                    if (config.AppSettings.Settings["language"].Value == "Svenska")
                    {
                        MessageBox.Show("Något gick fel ¯\\_(ツ)_/¯");
                    }
                }
            }
            else if (edytowana == 0)
            {
                if (textBox1.Text.ToString().Contains(','))
                {
                    if (config.AppSettings.Settings["language"].Value == "Polski")
                    {
                        MessageBox.Show("Proszę nie używać przecinków w nazwie");
                    }
                    if (config.AppSettings.Settings["language"].Value == "English")
                    {
                        MessageBox.Show("Please do not use commas in the name");
                    }
                    if (config.AppSettings.Settings["language"].Value == "Svenska")
                    {
                        MessageBox.Show("Använd inte kommatecken i namnet");
                    }
                }
                else if(connType!="SQL")
                {
                    Sql.data(host, "", "INSERT INTO dane('nazwa','haslo') VALUES('" + textBox1.Text.ToString() + "','" + textBox2.Text.ToString() + "');",new string[] { }, connType);
                }
                else
                {
                    Sql.data(host, "", "INSERT INTO dane VALUES(0,'" + textBox1.Text.ToString() + "','" + textBox2.Text.ToString() + "');", new string[] { }, connType);
                }
            }
            else
            {
                if (config.AppSettings.Settings["language"].Value == "Polski")
                {
                    MessageBox.Show("Te bo ja się nie wykonuję");
                }
                if (config.AppSettings.Settings["language"].Value == "English")
                {
                    MessageBox.Show("These because I don't do");
                }
                if (config.AppSettings.Settings["language"].Value == "Svenska")
                {
                    MessageBox.Show("Detta för det gör jag inte");
                }
            }
            comboBox1.Items.Clear();
            if (hide == "true")
            {
                List<string> list = Sql.data(host, "read", "select id,nazwa,'*****' from dane", new string[] { "string", "string", "string" }, connType);
                comboBox1.Items.AddRange(list.ToArray());
            }
            else
            {
                List<string> list = Sql.data(host, "read", "select * from dane", new string[] { "string", "string", "string" }, connType);
                comboBox1.Items.AddRange(list.ToArray());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sql.data(host, "", "DELETE FROM dane WHERE id=" + wybrana, new string[] { }, connType);
            comboBox1.Items.Clear();
            List<string> list = Sql.data(host, "read", "select * from dane", new string[] { "string", "string", "string" }, connType);
            comboBox1.Items.AddRange(list.ToArray());
            edytowana = 0;
        }

        public void Skopiuj_Click(object sender, EventArgs e)
        {
            try
            {
                if (hide == "true")
                {
                    int id = int.Parse(comboBox1.Text.Split(new string[] { ") " }, StringSplitOptions.None)[0]);
                    string haslo = Sql.data(host, "read", "select * from dane where id="+id.ToString(),new string[] {"string","string","string" }, connType)[0];
                    haslo = haslo.Split(new string[] { ") " }, StringSplitOptions.None)[1].Split(new string[] { ", " }, StringSplitOptions.None)[1];
                    Clipboard.SetText(haslo);
                }
                else
                {
                    int pozycja = comboBox1.Text.ToString().IndexOf(',') + 2;
                    string haslo = comboBox1.Text.ToString();
                    Clipboard.SetText(haslo.Substring(pozycja));
                }
            }
            catch (Exception exc)
            {
                if (config.AppSettings.Settings["language"].Value == "Polski")
                {
                    MessageBox.Show("Wygląda na to, że nie wybrałeś co mamy skopiować");
                }
                if (config.AppSettings.Settings["language"].Value == "English")
                {
                    MessageBox.Show("Looks like you didn't choose what to copy");
                }
                if (config.AppSettings.Settings["language"].Value == "Svenska")
                {
                    MessageBox.Show("Det verkar som om du inte valde vad du skulle kopiera");
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            wybrana = int.Parse(comboBox1.Text.Split((")").ToCharArray())[0]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //"DROP DATABASE hasla;";
        }

        private void expand_Click(object sender, EventArgs e)
        {
            if (this.Width >= 456)
            {
                this.Width = 272;
                this.expand.Text = ">";
            }
            else
            {
                this.Width = 456;
                this.expand.Text = "<";
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            DialogResult dialog = DialogResult.No;
            if (config.AppSettings.Settings["language"].Value == "Polski")
            {
                dialog = MessageBox.Show("Na pewno chcesz usunąć tabelę?" + Environment.NewLine + "(wszystkie hasła zostaną usunięte)", "Usuń tabelę", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            if (config.AppSettings.Settings["language"].Value == "English")
            {
                dialog = MessageBox.Show("You sure you want to delete the table?" + Environment.NewLine + "(all passwords will be deleted)", "Delete the table", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            if (config.AppSettings.Settings["language"].Value == "Svenska")
            {
                dialog = MessageBox.Show("Är du säker på att du vill ta bort tabellen?" + Environment.NewLine + "(alla lösenord raderas)", "Ta bort tabellen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            
            if(dialog == DialogResult.Yes)
            {
                if (config.AppSettings.Settings["language"].Value == "Polski")
                {
                    MessageBox.Show("Usunięto");
                }
                if (config.AppSettings.Settings["language"].Value == "English")
                {
                    MessageBox.Show("Deleted");
                }
                if (config.AppSettings.Settings["language"].Value == "Svenska")
                {
                    MessageBox.Show("Raderade");
                }
                Sql.data(host, "", "DROP TABLE dane", new string[] { }, connType);
                Form f = new Form2(tekst,ip,uzytkownik,haslo,baza,zmienione);
                f.TopMost = true;
                f.Visible = true;
                this.Dispose();
            }
        }

        private void button4_MouseHover(object sender, EventArgs e)
        {
            //button4.BackColor = Color.FromArgb(255,0,0);
        }

    }
}

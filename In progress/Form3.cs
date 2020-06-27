using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;

namespace losowanieHasla
{
    public partial class Form3 : Form
    {
        static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        string host = @"server=localhost;userid=root; password=;database=hasla;";
        string directoryWrite;
        string directoryLoad;
        string tekst;
        string ip;
        string uzytkownik;
        string haslo;
        string baza;
        bool zmienione;

        public Form3(string tekst, string ip, string uzytkownik, string haslo, string baza, bool zmienione)
        {
            this.tekst = tekst;
            this.ip = ip;
            this.uzytkownik = uzytkownik;
            this.haslo = haslo;
            this.baza = baza;
            this.zmienione = zmienione;
            

            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2 - Height / 2);
            InitializeComponent();
            string conn = config.AppSettings.Settings["connection"].Value;
            string hide = config.AppSettings.Settings["hiding"].Value;
            string mode = config.AppSettings.Settings["mode"].Value;
            string id = config.AppSettings.Settings["id"].Value;


            if (conn == "SQLite")
            {
                checkBox1.Checked = true;
            }
            if(hide == "true")
            {
                checkBox2.Checked = true;
            }
            if(mode == "dark")
            {
                Color black = Color.FromArgb(0, 0, 0);
                Color white = Color.FromArgb(255, 255, 255);
                Color grey = Color.FromArgb(70, 70, 70);
                this.BackColor = black;
                this.ForeColor = white;
                Button[] button = {button1,button2,button3,button4,button5};
                for(int i = 0 ; i<button.Length ; i++)
                {
                    button[i].BackColor = grey;
                }
                comboBox1.BackColor = grey;
                comboBox1.ForeColor = white;
                checkBox3.Checked = true;
            }
            if(id == "true")
            {
                checkBox4.Checked = true;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            List<string> list = new List<string>{"Polski","English","Svenska"};
            comboBox1.Items.AddRange(list.ToArray());
            comboBox1.Text = config.AppSettings.Settings["language"].Value;

            // language

            if (config.AppSettings.Settings["language"].Value == "English")
            {
                checkBox1.Text = "Offline mode";
                checkBox2.Text = "Hide passwords";
                checkBox3.Text = "Dark mode";
                button1.Text = "Save";
                button2.Text = "Export offline passwords";
                button3.Text = "Import offline passwords";
                button4.Text = "Export online passwords";
                button5.Text = "Import online passwords";
                linkLabel1.Text = "Help";
                this.Text = "Settings";
            }
            if (config.AppSettings.Settings["language"].Value == "Svenska")
            {
                checkBox1.Text = "Offlineläge";
                checkBox2.Text = "Dölj lösenord";
                checkBox3.Text = "Mörkt läge";
                button1.Text = "Spara";
                button2.Text = "Exportera lösenord offline";
                button3.Text = "Importera lösenord offline";
                button4.Text = "Exportera lösenord online";
                button5.Text = "Importera lösenord online";
                linkLabel1.Text = "Hjälp";
                this.Text = "Inställningar";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (checkBox1.Checked)
            {
                Usage.Checkboxes(config,"connection", "SQLite");
            }
            if(checkBox1.Checked == false)
            {
                Usage.Checkboxes(config, "connection", "SQL");
            }
            if(checkBox2.Checked)
            {
                Usage.Checkboxes(config, "hiding", "true");
            }
            if(checkBox2.Checked == false)
            {
                Usage.Checkboxes(config, "hiding", "false");
            }
            if(checkBox3.Checked)
            {
                Usage.Checkboxes(config, "mode", "dark");
            }
            if(checkBox3.Checked == false)
            {
                Usage.Checkboxes(config, "mode", "light");
            }
            if(checkBox4.Checked)
            {
                Usage.Checkboxes(config, "id", "true");
            }
            if(checkBox4.Checked == false)
            {
                Usage.Checkboxes(config, "id", "false");
            }

            Usage.Checkboxes(config, "language", comboBox1.Text);
            Application.Restart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //export offline
            try
            {
                saveFileDialog1.ShowDialog();
                List<string> list = Sql.data(host, "read", "select * from dane", new string[] { "string", "string", "string" }, "SQLite");
                string text = "";
                for(int i = 0; i<list.Count; i++)
                {
                    text += list[i] + Environment.NewLine;
                }
                File.WriteAllText(directoryWrite, text);
                text = "";
                saveFileDialog1.Reset();
            }
            catch (Exception ex)
            {
                if (config.AppSettings.Settings["language"].Value == "Polski")
                {
                    MessageBox.Show("Wystąpił błąd podczas eksportu pliku");
                }
                if (config.AppSettings.Settings["language"].Value == "English")
                {
                    MessageBox.Show("There was an error when exporting the file");
                }
                if (config.AppSettings.Settings["language"].Value == "Svenska")
                {
                    MessageBox.Show("Det uppstod ett fel vid export av filen");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //import offline
            try
            {
                openFileDialog1.ShowDialog();
                string[] data = File.ReadAllLines(directoryLoad);
                for (int i = 0; i < data.Length; i++)
                {
                    string temp = data[i].Split(")".ToCharArray())[1];
                    Sql.data(host, "", "INSERT INTO dane('nazwa','haslo') VALUES('" + temp.Split(",".ToCharArray())[0] + "','" + temp.Split(",".ToCharArray())[1] + "');", new string[] { }, "SQLite");
                }
            }
            catch (Exception ex)
            {
                if (config.AppSettings.Settings["language"].Value == "Polski")
                {
                    MessageBox.Show("Wystąpił błąd podczas importu pliku");
                }
                if (config.AppSettings.Settings["language"].Value == "English")
                {
                    MessageBox.Show("There was an error when importing the file");
                }
                if (config.AppSettings.Settings["language"].Value == "Svenska")
                {
                    MessageBox.Show("Det uppstod ett fel vid importen av filen");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //export online
            try
            {
                saveFileDialog1.ShowDialog();
                List<string> list = Sql.data(host, "read", "select * from dane", new string[] { "string", "string", "string" }, "SQL");
                string text = "";
                for (int i = 0; i < list.Count; i++)
                {
                    text += list[i] + Environment.NewLine;
                }
                File.WriteAllText(directoryWrite, text);
                text = "";
                saveFileDialog1.Reset();
            }
            catch (Exception ex)
            {
                if (config.AppSettings.Settings["language"].Value == "Polski")
                {
                    MessageBox.Show("Wystąpił błąd podczas eksportu pliku");
                }
                if (config.AppSettings.Settings["language"].Value == "English")
                {
                    MessageBox.Show("There was an error when exporting the file");
                }
                if (config.AppSettings.Settings["language"].Value == "Svenska")
                {
                    MessageBox.Show("Det uppstod ett fel vid export av filen");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //import online
            try
            {
                openFileDialog1.ShowDialog();
                string[] data = File.ReadAllLines(directoryLoad);
                for (int i = 0; i < data.Length; i++)
                {
                    string temp = data[i].Split(")".ToCharArray())[1];
                    Sql.data(host, "", "INSERT INTO dane VALUES(0,'" + temp.Split(",".ToCharArray())[0] + "','" + temp.Split(",".ToCharArray())[1] + "');", new string[] { }, "SQL");
                }
            }
            catch (Exception ex)
            {
                if (config.AppSettings.Settings["language"].Value == "Polski")
                {
                    MessageBox.Show("Wystąpił błąd podczas importu pliku");
                }
                if (config.AppSettings.Settings["language"].Value == "English")
                {
                    MessageBox.Show("There was an error when importing the file");
                }
                if (config.AppSettings.Settings["language"].Value == "Svenska")
                {
                    MessageBox.Show("Det uppstod ett fel vid importen av filen");
                }
            }
}

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            directoryWrite = saveFileDialog1.FileName;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            directoryLoad = openFileDialog1.FileName;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (config.AppSettings.Settings["language"].Value == "Polski")
            {
                MessageBox.Show("Dane dostępowe do bazy danych są opcjonalne do wpisania. Domyślnie"
                          + " program łączy się do bazy na serwerze lokalnym (127.0.0.1)."
                          + "Aby zmienić dane wpisz je w odpowiednie okienka i kliknij przycisk 'Zmień dane'."
                          + Environment.NewLine + Environment.NewLine
                          + "Program podczas niektórych akcji (np. po kliknięciu w przycisk \"Zapisz\" w ustawieniach)"
                          + "resetuje się w celu wprowadzenia zmian aby wszystkie dane były aktualne."
                          + "Zauważysz przez to zniknięcie i pojawienie się okienka na nowo."
                          , "Pomoc");
            }
            if (config.AppSettings.Settings["language"].Value == "English")
            {
                MessageBox.Show("Database access data is optional for entry. By default"
                          + " the program connects to the base on the local server (127.0.0.1)."
                          + " To change the data enter it in the appropriate boxes and click 'Change data'."
                          + Environment.NewLine + Environment.NewLine
                          + "Program during some actions (e.g. after clicking the \"Save\" button in the settings)"
                          + "resets to make changes so that all data is up-to-date."
                          + "You will notice the disappearance and the window will appear again."
                          ,"Help");
            }
            if (config.AppSettings.Settings["language"].Value == "Svenska")
            {
                MessageBox.Show("Databasåtkomstdata är valfria för inmatning. Som standard"
                          + " programmet ansluter till basen på den lokala servern (127.0.0.1)."
                          + " För att ändra data, ange den i lämpliga rutor och klicka på 'Ändra data'."
                          + Environment.NewLine + Environment.NewLine
                          + "Programmera under vissa åtgärder (t.ex. efter att du har klickat på\"Spara\"i inställningarna)"
                          + "återställs för att göra ändringar så att alla data är uppdaterade."
                          + " Du kommer att märka försvinnandet och fönstret kommer att visas igen."
                          , "Hjälp");
            }
        }
    }
}

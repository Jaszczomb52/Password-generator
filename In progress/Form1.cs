using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Data.SQLite;
using System.Configuration;

namespace losowanieHasla
{
    public partial class Form1 : Form
    {
        static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        string baza = "";
        string ip = "";
        string uzytkownik = "";
        string haslo = "";
        bool zmienione = false;


        public Form1()
        {
           InitializeComponent();
           this.Width = 400;
           KeyPreview = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (config.AppSettings.Settings["mode"].Value == "dark")
            {
                Color black = Color.FromArgb(0, 0, 0);
                Color white = Color.FromArgb(255, 255, 255);
                Color grey = Color.FromArgb(70, 70, 70);
                Button[] button = { button1, button2, button3 };
                TextBox[] text = { textBox3, textBox4, textBox5 };
                for (int i = 0; i < button.Length; i++)
                {
                    button[i].BackColor = grey;
                    text[i].BackColor = black;
                    text[i].ForeColor = white;
                }
                this.BackColor = black; this.ForeColor = white;
                textBox1.BackColor = black; textBox1.ForeColor = white;
                textBox2.BackColor = black; textBox2.ForeColor = white;

            }

            textBox2.Focus();
            // language setting
            if (config.AppSettings.Settings["language"].Value == "Svenska")
            {
                this.Text = "Lösenordsgenerator";
                label1.Text = "Tryck på knappen för att skapa ett lösenord";
                label2.Text = "Lösenords längd";
                label6.Text = "Databasdata";
                button1.Text = "Generera!";
                button2.Text = "Redigera lösenord";
                button3.Text = "Ändra data";
                textBox3.Text = "IP-adress";
                textBox4.Text = "Användare";
                textBox5.Text = "Lösenord";
                checkBox1.Text = "Säkert lösenord";
            }
            if (config.AppSettings.Settings["language"].Value == "English")
            {
                this.Text = "Password generator";
                label1.Text = "Press the button to generate a password";
                label2.Text = "Password length";
                label6.Text = "Database data";
                button1.Text = "Generate!";
                button2.Text = "Edit passwords";
                button3.Text = "Change the data";
                textBox3.Text = "IP adress";
                textBox4.Text = "User";
                textBox5.Text = "Password";
                checkBox1.Text = "Safe password";
            }
        }

        public void Form1_Layout(object sender, LayoutEventArgs e)
        {

        }


     // WŁASNE FUNKCJE WYKONUJĄCE
        // FORM 1 - generowanie 
        private void Button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                try
                {
                    if(int.Parse(textBox2.Text) <= 255)
                    {
                        wstaw(Sec.GetUniqueKeySec(int.Parse(textBox2.Text)));
                    }
                    else
                    {
                        if (config.AppSettings.Settings["language"].Value == "Polski")
                        {
                            MessageBox.Show("Maksymalna długość hasła to 255 znaków");
                        }
                        if (config.AppSettings.Settings["language"].Value == "English")
                        {
                            MessageBox.Show("Maximum password length is 255 characters");
                        }
                        if (config.AppSettings.Settings["language"].Value == "Svenska")
                        {
                            MessageBox.Show("Max lösenordslängd är 255 tecken");
                        }
                    }
                }
                catch (System.FormatException)
                {
                    powiadomienie();
                }
            }
            else
            {
                try
                {
                    if (int.Parse(textBox2.Text) <= 255)
                    {
                        wstaw(Sec.GetUniqueKey(int.Parse(textBox2.Text)));
                    }
                    else
                    {
                        if (config.AppSettings.Settings["language"].Value == "Polski")
                        {
                            MessageBox.Show("Maksymalna długość hasła to 255 znaków");
                        }
                        if (config.AppSettings.Settings["language"].Value == "English")
                        {
                            MessageBox.Show("Maximum password length is 255 characters");
                        }
                        if (config.AppSettings.Settings["language"].Value == "Svenska")
                        {
                            MessageBox.Show("Max lösenordslängd är 255 tecken");
                        }
                    }
                }
                catch (System.FormatException)
                {
                    powiadomienie();
                }
            }
        }

        // FORM 2 - zapisywanie do bazy

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2(textBox1.Text, ip, uzytkownik, haslo, baza, zmienione);
            f.ShowDialog();
        }

        // wyświetlanie powiadomienia

        private void powiadomienie()
        {
            // Initializes the variables to pass to the MessageBox.Show method.
            string message = "";
            string caption = "";
            if(config.AppSettings.Settings["language"].Value == "Polski")
            {
                message = "Proszę w polu \"Długość hasła\" podać liczbę.";
                caption = "Wykryto błąd w polu wprowadzania";
                textBox2.Focus();
            }
            if(config.AppSettings.Settings["language"].Value == "Svenska")
            {
                message = "Ange ett nummer i fältet \"Lösenordslängd \".";
                caption = "Ett fel upptäcktes i inmatningsfältet";
                textBox2.Focus();
            }
            if(config.AppSettings.Settings["language"].Value == "English")
            {
                message = "Please enter a number in field \"Password length\".";
                caption = "An error was detected in the input field";
                textBox2.Focus();
            }
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;

            // Displays the MessageBox.
            result = MessageBox.Show(message, caption, buttons);
        }

            // FUNKCJE UŻYTKOWE

        private void wstaw(string a)
        {
            textBox1.Text = a;
            if (config.AppSettings.Settings["language"].Value == "Polski")
            {
                label4.Text = "Zapisz hasło w bezpiecznym miejscu";
            }
            if (config.AppSettings.Settings["language"].Value == "Svenska")
            {
                label4.Text = "Spara ditt lösenord på ett säkert ställe";
            }
            if (config.AppSettings.Settings["language"].Value == "English")
            {
                label4.Text = "Save your password in a safe place";
            }
        }

        

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            zmienione = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text!= "Adres IP" && textBox4.Text!="Użytkownik" && textBox5.Text!= "Hasło" ||
                textBox3.Text != "IP adress" && textBox4.Text != "User" && textBox5.Text!= "Password" ||
                textBox3.Text != "IP-adress" && textBox4.Text != "Användare" && textBox5.Text != "Lösenord")
            {
                try
                {
                    ip = textBox3.Text;
                    uzytkownik = textBox4.Text;
                    haslo = textBox5.Text;
                }
                catch (Exception)
                {
                    if (config.AppSettings.Settings["language"].Value == "Polski")
                    {
                        MessageBox.Show("Błędne dane bazy");
                    }
                    if (config.AppSettings.Settings["language"].Value == "Svenska")
                    {
                        MessageBox.Show("Felaktiga databasdata");
                    }
                    if (config.AppSettings.Settings["language"].Value == "English")
                    {
                        MessageBox.Show("Incorrect database data");
                    }
                }
            }
            else if (textBox3.Text != "Adres IP" && textBox4.Text!="Użytkownik" ||
                     textBox3.Text != "IP adress" && textBox4.Text != "User" ||
                     textBox3.Text != "IP-adress" && textBox4.Text != "Användare")
            {
                if (config.AppSettings.Settings["language"].Value == "Polski")
                {
                    MessageBox.Show("Podaj hasło (w przypadku jego braku kliknij na pole wprowadzania hasła)");
                }
                if (config.AppSettings.Settings["language"].Value == "Svenska")
                {
                    MessageBox.Show("Ange lösenordet (om det inte finns något lösenord, klicka på fältet för lösenordsinmatning)");
                }
                if (config.AppSettings.Settings["language"].Value == "English")
                {
                    MessageBox.Show("Enter the password (if there is no password, click on the password entry field)");
                }
            }
            else if(textBox3.Text != "Adres IP" || textBox3.Text != "IP adress" || textBox3.Text != "IP-adress")
            {
                if (config.AppSettings.Settings["language"].Value == "Polski")
                {
                    MessageBox.Show("Podaj nazwę użytkownika");
                }
                if (config.AppSettings.Settings["language"].Value == "Svenska")
                {
                    MessageBox.Show("Ange användarnamnet");
                }
                if (config.AppSettings.Settings["language"].Value == "English")
                {
                    MessageBox.Show("Enter the user name");
                }
            }
            else
            {
                if (config.AppSettings.Settings["language"].Value == "Polski")
                {
                    MessageBox.Show("Nie zmieniono danych bazy");
                }
                if (config.AppSettings.Settings["language"].Value == "Svenska")
                {
                    MessageBox.Show("Databasen har inte ändrats");
                }
                if (config.AppSettings.Settings["language"].Value == "English")
                {
                    MessageBox.Show("The database has not been changed");
                }
            }
        }

        private void expand_Click(object sender, EventArgs e)
        {
            if(this.Width > 400)
            {
                this.Width = 400;
                this.expand.Text = ">";
            }
            else
            {
                this.Width = 580;
                this.expand.Text = "<";
            }
        }

        private void gear_Click(object sender, EventArgs e)
        {
            Form settings = new Form3("",ip,uzytkownik,haslo,baza,zmienione);
            settings.Visible = true;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                if (checkBox1.Checked == true)
                {
                    try
                    {
                        if (int.Parse(textBox2.Text) <= 255)
                        {
                            wstaw(Sec.GetUniqueKeySec(int.Parse(textBox2.Text)));
                        }
                        else
                        {
                            if (config.AppSettings.Settings["language"].Value == "Polski")
                            {
                                MessageBox.Show("Maksymalna długość hasła to 255 znaków");
                            }
                            if (config.AppSettings.Settings["language"].Value == "English")
                            {
                                MessageBox.Show("Maximum password length is 255 characters");
                            }
                            if (config.AppSettings.Settings["language"].Value == "Svenska")
                            {
                                MessageBox.Show("Max lösenordslängd är 255 tecken");
                            }
                        }
                    }
                    catch (System.FormatException)
                    {
                        powiadomienie();
                    }
                }
                else
                {
                    try
                    {
                        if (int.Parse(textBox2.Text) <= 255)
                        {
                            wstaw(Sec.GetUniqueKey(int.Parse(textBox2.Text)));
                        }
                        else
                        {
                            if (config.AppSettings.Settings["language"].Value == "Polski")
                            {
                                MessageBox.Show("Maksymalna długość hasła to 255 znaków");
                            }
                            if (config.AppSettings.Settings["language"].Value == "English")
                            {
                                MessageBox.Show("Maximum password length is 255 characters");
                            }
                            if (config.AppSettings.Settings["language"].Value == "Svenska")
                            {
                                MessageBox.Show("Max lösenordslängd är 255 tecken");
                            }
                        }
                    }
                    catch (System.FormatException)
                    {
                        powiadomienie();
                    }
                }
            }
        }
    }
}


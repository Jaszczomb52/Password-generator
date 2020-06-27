using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SQLite;

namespace losowanieHasla
{
    class Sql
    {
        static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        static string connType = config.AppSettings.Settings["connection"].Value;

        public static SQLiteConnection connect()
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = CreateConnection();
            return sqlite_conn;
        }

        static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=hasla.db; Version = 3; New = True; Compress = True; ");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite_conn;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////

        public static List<string> data(string host, string typeOfAction, string statement, string[] rdrTypes, string connType)
        {
            
            if (connType == "SQL")
            {
                List<string> list = new List<string>();
                string cs = host;

                MySqlConnection conn = null;
                MySqlDataReader rdr = null;

                try
                {
                    conn = new MySqlConnection(cs);
                    conn.Open();

                    string stm = statement;
                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    rdr = cmd.ExecuteReader();

                    if (typeOfAction == "read")
                    {
                        while (rdr.Read())
                        {

                            string temp = "";
                            for (int i = 0; i < rdrTypes.Length; i++)
                            {
                                if (rdrTypes[i] == "string" && i == 0)
                                {
                                    temp += rdr.GetString(i) + ") ";
                                }
                                else if (rdrTypes[i] == "string" && i == rdrTypes.Length - 1)
                                {
                                    temp += rdr.GetString(i);
                                }
                                else if (rdrTypes[i] == "string")
                                {
                                    temp += rdr.GetString(i) + ", ";
                                }
                            }

                            list.Add(temp);
                        }
                    }

                    //this.textBox1.Text = (dane).ToString();
                    return list;
                }
                catch (MySqlException ex)
                {
                    if(config.AppSettings.Settings["language"].Value == "Polski")
                    {
                        MessageBox.Show("Błąd: błąd połączenia z bazą danych. Proszę skontaktować się ze mną poprzez email: *tu będzie email pomocy");
                    }
                    if(config.AppSettings.Settings["language"].Value == "English")
                    {
                        MessageBox.Show("Error: database connection error. Please contact with me on email: *there will be support email*");
                    }
                    if(config.AppSettings.Settings["language"].Value == "Svenska")
                    {
                        MessageBox.Show("Fel: databasanslutningsfel. Vänligen kontakta mig via e-post: *det kommer att finnas supportmail*");
                    }
                    return list;
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            else
            {
                
                    List<string> list = new List<string>();
                    SQLiteConnection conn = connect();
                    //conn.Open();
                try
                {
                    SQLiteDataReader rdr;
                    SQLiteCommand sqlite_cmd;
                    sqlite_cmd = conn.CreateCommand();
                    sqlite_cmd.CommandText = statement;
                    rdr = sqlite_cmd.ExecuteReader();

                    if (typeOfAction == "read")
                    {
                        while (rdr.Read())
                        {
                            string temp = "";
                            for (int i = 0; i < rdrTypes.Length; i++)
                            {
                                if (rdrTypes[i] == "string" && i == 0)
                                {
                                    temp += rdr.GetInt32(i).ToString() + ") ";
                                }
                                else if (rdrTypes[i] == "string" && i == rdrTypes.Length - 1)
                                {
                                    temp += rdr.GetString(i);
                                }
                                else if (rdrTypes[i] == "string")
                                {
                                    temp += rdr.GetString(i) + ", ";
                                }
                            }
                            
                            list.Add(temp);
                        }
                    }

                    //this.textBox1.Text = (dane).ToString();
                    return list;
                }
                catch (SQLiteException ex)
                {
                    if (config.AppSettings.Settings["language"].Value == "Polski")
                    {
                        MessageBox.Show("Błąd: błąd połączenia z bazą danych. Proszę skontaktować się ze mną poprzez email: *tu będzie email pomocy");
                    }
                    if (config.AppSettings.Settings["language"].Value == "English")
                    {
                        MessageBox.Show("Error: database connection error. Please contact with me on email: *there will be support email*");
                    }
                    if (config.AppSettings.Settings["language"].Value == "Svenska")
                    {
                        MessageBox.Show("Fel: databasanslutningsfel. Vänligen kontakta mig via e-post: *det kommer att finnas supportmail*");
                    }
                    return list;
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }

            }
        }
    }
}

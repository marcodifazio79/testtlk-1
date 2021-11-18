using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;


namespace TestCasseTLK
{
    public partial class Form1 : Form
    {
        MySqlConnection connection;

        string serverip = "10.10.10.71";
        string database = "listener_DB";
        string uid = "bot_user";
        string password = "Qwert@#!99";
        //string connectionString = "server = localhost; database = lister_DB; uid = root; pwd = ;";
        string connectionString = "server = 10.10.10.71; database = listener_DB; uid = bot_user; pwd = Qwert@#!99;";

        string id_Fake = "3973";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)

        {
 
            connection = new MySqlConnection(connectionString);

            //string splittransferred_data2 = "porco01010101";
            //string splittransferred_data0 = "<TPK =W5";
            //if (!IsNumeric(splittransferred_data2) & !(splittransferred_data0.Contains("<TPK=$I")))
            //{

            //    Console.WriteLine("è un concentratore");
            //}




            //if (connection.State == ConnectionState.Closed) connection.Open();
            //MySqlCommand newcmd;
            //string query;
            //try
            //{
            //    query = "select* from DatiCassa where Id =88;";
            //    newcmd = new MySqlCommand(query, connection);
            //    MySqlDataReader dataReader = newcmd.ExecuteReader();
            //    while (dataReader.Read())
            //    {
            //        Console.WriteLine(dataReader["Id"].ToString() );

            //    }

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("ERROR - DeleteCashTransTables: " + ex.Message);
            //    //connection.Close();

            //}

            //RemoveMachine();
            Svuota_DB();
            //check_DatiConnessione();
            // test_daticash_convert();
        }
        public static bool IsNumeric(string strText)
        {
            bool bres = false;
            try
            {
                //Console.WriteLine(strText);   
                Int64 result = Convert.ToInt64(strText);
                bres = true;
                return bres;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return bres;
            }
        }

        private void test_daticash_convert()
        {
            string splittedCashPacket = "9400";
            string splittedCashPacket_previous = " 8850";
            int previousParam = Convert.ToInt32(splittedCashPacket_previous);
            int actualParam = Convert.ToInt32(splittedCashPacket);

            string valueres = ((float)(actualParam - previousParam) / 100).ToString();


            string dataCass_Qty1 = (Convert.ToInt32(splittedCashPacket) - Convert.ToInt32(splittedCashPacket_previous)).ToString();

            //float value =(float) intsplittedCashPacket_previous / 100;

            // string dato_flottato = ((float)valueres / 100).ToString();

            string dataCass_Cashless = ((Convert.ToInt32(splittedCashPacket) - Convert.ToInt32(splittedCashPacket_previous)) / 100).ToString();
        }

        private void check_DatiConnessione()
        {
            try
            {
                MySqlConnection connection;
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string query = "SELECT distinct (id_Macchina) FROM machinesconnectiontrace WHERE transferred_data LIKE '%M3%';";
                query = "SELECT id,mid FROM `machines` WHERE version LIKE'%838' order by mid";
                //string query = "SELECT distinct (id_Macchina) FROM MachinesConnectionTrace WHERE transferred_data LIKE '<TPK=$M3%';";

                List<string> Midlist = new List<string>();
                List<string> IdfromMachines = new List<string>();
                List<string> Versionlist = new List<string>();
                Dictionary<string, string> id_vers_List = new Dictionary<string, string>();
                Dictionary<string, string> id_mid_List = new Dictionary<string, string>();
                MySqlDataReader dataReader;


                MySqlCommand cmd = new MySqlCommand(query, connection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    id_mid_List.Add(dataReader["id"].ToString(), dataReader["mid"].ToString());
                }

                if (File.Exists(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\pacchetto838.txt")) File.Delete(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\pacchetto838.txt");

                StreamWriter newWriter = new StreamWriter(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\pacchetto838.txt", false);
                StreamReader newreader = new StreamReader(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\idM3-2811.txt", Encoding.UTF8);

                string tmpstr= newreader.ReadToEnd();
                tmpstr = tmpstr.Replace("\r","");

                string[] idlist = tmpstr.Split('\n');
                string[] midlist;

                newreader.Close();
                int i = 0;
                

                Console.WriteLine("ID_Machines,CE,Version,time_creation,last_communication");
                foreach(string idtemp in id_mid_List.Keys)
                {
                    string t_mid = "";
                    id_mid_List.TryGetValue(idtemp,out t_mid);
                    newWriter.WriteLine("##################### CE  "+ t_mid);
                    dataReader.Close();
                    query = "SELECT transferred_data,time_stamp FROM machinesconnectiontrace WHERE id_Macchina = '" + idtemp + "';"; 
                    cmd = new MySqlCommand(query, connection);
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        newWriter.WriteLine(dataReader["time_stamp"].ToString()+" - "+dataReader["transferred_data"].ToString());

                    }
                }
                newWriter.Close();

                    for (i=0;i<idlist.Length;i++)
                {

                    query = "SELECT mid,version,time_creation,last_communication FROM Machines WHERE id = '" + idlist[i] + "'";
                    cmd = new MySqlCommand(query, connection);
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {

                        newWriter.WriteLine(idlist[i]+","+ dataReader["mid"].ToString() +","+dataReader["version"].ToString() + "," +dataReader["time_creation"].ToString() + "," +dataReader["last_communication"].ToString());
                           

                        query = "SELECT transferred_data FROM MachinesConnectionTrace WHERE id_Macchina = '" + idlist[i] + "' AND time_stamp like '2021-10-28%'";
                        dataReader.Close();
                        cmd = new MySqlCommand(query, connection);
                        dataReader = cmd.ExecuteReader();
                        Console.WriteLine("transferred_data");
                        while (dataReader.Read())
                        {
                            newWriter.WriteLine(dataReader["transferred_data"].ToString());
                        }
                  

                    }

                    dataReader.Close();

                }
                newWriter.Close();
                //Dictionary<String, String>.ValueCollection keys = FullList_NSR_CI.Values;
            }
            catch(Exception e)
            {
                Console.WriteLine("Error:check_DatiConnessione - " + e.Message);
            }
        }

        private void Remove_SingleMachine()
        {
            MySqlConnection connection;

            string id = txtIdtoDelete.Text;
            string id_Fake = "3973";
            connection = new MySqlConnection(connectionString);
            string query;
            MySqlCommand newcmd;
            if (OpenConnection() == true)
            {
                //Create Command
                //update MachinesConnectionTrace set id_Macchina = 101 where id_Macchina =
                query = "update MachinesConnectionTrace set id_Macchina = " + id_Fake + " where id_Macchina = " + id;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();

                query = "update RemoteCommand set id_Macchina = " + id_Fake + " where id_Macchina = " + id;
                //query = "Delete FROM RemoteCommand  where id_Macchina = " + id;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();

                query = "Delete FROM Log  where ID_machine = " + id;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();

                query = "Delete FROM MachinesAttributes   where id_Macchina = " + id;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();

                query = "Delete FROM CashTransaction   where ID_Machines = " + id;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();

                query = "Delete FROM Machines   where id = " + id;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();
                
                connection.Close();
            }
            //close Connection
            

        }

        private void Svuota_DB()
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            MySqlCommand newcmd;
            string query = "";

            query = "Delete FROM Log;";
            newcmd = new MySqlCommand(query, connection);
            newcmd.ExecuteNonQuery();

            query = "Delete FROM RemoteCommand;";
            newcmd = new MySqlCommand(query, connection);
            newcmd.ExecuteNonQuery();

            query = "Delete FROM MachinesAttributes;";
            newcmd = new MySqlCommand(query, connection);
            newcmd.ExecuteNonQuery();

            query = "Delete from MachinesConnectionTrace;";
            newcmd = new MySqlCommand(query, connection);
            newcmd.ExecuteNonQuery();

            query = "Delete FROM CashTransaction;";
            newcmd = new MySqlCommand(query, connection);
            newcmd.ExecuteNonQuery();

            query = "Delete FROM Machines;";
            newcmd = new MySqlCommand(query, connection);
            newcmd.ExecuteNonQuery();

        }
            private void RemoveMachine()
        {
            //MySqlConnection connection;
            //connection = new MySqlConnection(connectionString);
            if(connection.State==ConnectionState.Closed) connection.Open();

            string query = "";
            query = "select * from Machines where id=11437;";
            //query = " SELECT * from Machines where last_communication like '2021-11-10 14%';";
            //query = "SELECT * FROM Machines where mid like '%RecuperoInCorso..%'";
            //query = "SELECT * FROM Machines where mid like '%Duplicato%'";


            List<string> idlist = new List<string>();
                //Openconnection
            if (connection.State==ConnectionState.Open)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                        
                while (dataReader.Read())
                {
                     idlist.Add(dataReader["id"].ToString());
                }
                //idlist.Add("144");
                dataReader.Close();
                int counter = 0;     
               
                foreach (string id in idlist)
                {
                    //return;
                    if (DeleteLogTables(id))
                    {
                        if (DeleteRemoteCommand(id))
                        {
                            if (DeleteMachinesAttributesTables(id))
                            {
                                if (DeleteMachinesConnectionTrace(id))
                                {
                                    if (DeleteCashTransTables(id))
                                    {
                                        if (DeleteFromMachinestable(id)) Console.WriteLine(counter++);
                                    }
                                }
                            }
                        }
                    }

                }
                
           


                connection.Close();

                //return list to be displayed
               
            }
            else
            {
                Console.WriteLine("OCROPOID");
            }
        }
        private bool DeleteLogTables(string id_machine)
        {
            //MySqlConnection connection;
            //connection = new MySqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();
            MySqlCommand newcmd;
            string query;
            try
            {
                query = "Delete FROM Log   where ID_machine = " + id_machine;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR - DeleteLogTables: " + e.Message);
                //connection.Close();
                return false;
            }
        }
        private bool DeleteMachinesAttributesTables(string id_machine)
        {
            //MySqlConnection connection;
            //connection = new MySqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open(); 
            MySqlCommand newcmd;
            string query;
            try
            {
                query = "Delete FROM MachinesAttributes   where id_Macchina = " + id_machine;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR - DeleteMachinesAttributesTables: " + e.Message);
                //connection.Close();
                return false;
            }
        }
        private bool DeleteCashTransTables(string id_machine)
        {
            //MySqlConnection connection;
            //connection = new MySqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();
            MySqlCommand newcmd;
            string query;
            try
            {
                query = "Delete FROM CashTransaction   where ID_Machines = " + id_machine;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();
                //connection.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR - DeleteCashTransTables: " + e.Message);
                //connection.Close();
                return false;
            }
        }


        private bool DeleteFromMachinestable(string id_machine)
        {
            //MySqlConnection connection;
            //connection = new MySqlConnection(connectionString);
            //connection.Open();
            if (connection.State == ConnectionState.Closed) connection.Open();
            MySqlCommand newcmd;
            string query;
            try
            {
                query = "Delete FROM Machines   where id = " + id_machine;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();

                //connection.Close();
                return true;
            }
            catch(MySqlException e)
            {
                Console.WriteLine("ERROR - DeleteFromMachinestable: " + e.Message);
                connection.Close();
                return false;
            }
        }
        private bool DeleteRemoteCommand(string id_machine)
        {
            //MySqlConnection connection;
            //connection = new MySqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();
            MySqlCommand newcmd;
            string query;
            try
            {
                query = "Delete FROM RemoteCommand  where id_Macchina = " + id_machine;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();
                //connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine("ERROR - DeleteRemoteCommand: " + e.Message);
                //connection.Close();
                return false;
            }

        }

        private bool DeleteMachinesConnectionTrace(string id_machine)
        {
            //MySqlConnection connection;
            //connection = new MySqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();
            MySqlCommand newcmd;
            string query;
            try
            {
                query = "Delete from MachinesConnectionTrace where id_Macchina = " + id_machine;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();
                //connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine("ERROR - DeleteMachinesConnectionTrace: " + e.Message);
                //connection.Close();
                return false;
            }
            
        }
        private bool OpenConnection()
        {
            try
            {
               // connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        private void btnDelSingleMachine_Click(object sender, EventArgs e)
        {
            Remove_SingleMachine();
        }
    }
}

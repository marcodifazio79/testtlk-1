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


namespace TestCasseTLK
{
    public partial class Form1 : Form
    {

        
        string serverip = "10.10.10.71";

        string database = "listener_DB";
        string uid = "bot_user";
        string password = "Qwert@#!99";
        string connectionString = "server = 10.10.10.71; database = listener_DB; uid = bot_user; pwd = Qwert@#!99;";

        string id_Fake = "3973";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void check_DatiConnessione()
        {


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
        private void StartConnection()
        {
            MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            connection.Open();
            string query = "Select *  from Machines Order By last_communication limit 20";
            //string query = "SELECT * from Machines where last_communication like '2021-10-12%'";
            //string query = "SELECT * FROM Machines where mid like '%RecuperoInCorso..211021091021928%'";
            //string query = "SELECT * FROM Machines where mid like '%Duplicato%'";



            List<string> idlist = new List<string>();
                //Openconnection
            if (connection.State==ConnectionState.Open)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                        
                //Read the data and store them in the list
                while (dataReader.Read())
                {
           //         Console.WriteLine (dataReader["mid"] + "");

                    idlist.Add( dataReader["id"].ToString());

             //       Console.WriteLine (dataReader["imei"] + "");
                    //list[2].Add(dataReader["age"] + "");
                }
               // idlist.Add("4058");
                //close Data Reader
                dataReader.Close();


               
                //connection.Close();
               

                foreach (string id in idlist)
                {
                    FreeMachinesConnectionTrace(id);
                    FreeRemoteCommand(id);
                    FreeOtherTables(id);
                }
                
                
                //connection.Open();

                query = "delete from MachinesConnectionTrace where id_Macchina = " + id_Fake;
                cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();

                //return list to be displayed
               
            }
            else
            {
                Console.WriteLine("OCROPOID");
            }
        }
        private bool FreeOtherTables(string id_machine)
        {
            MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand newcmd;
            string query;
            try
            {
                query = "Delete FROM MachinesAttributes   where id_Macchina = " + id_machine;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();

                query = "Delete FROM CashTransaction   where ID_Machines = " + id_machine;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();

                query = "Delete FROM Machines   where id = " + id_machine;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();

                connection.Close();
                return true;
            }
            catch(MySqlException e)
            {
                Console.WriteLine("ERROR - FreeOtherTables: " + e.Message);
                return false;
            }
        }
        private bool FreeRemoteCommand(string id_machine)
        {
            MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand newcmd;
            string query;
            try
            {
                query = "Delete FROM RemoteCommand  where id_Macchina = " + id_machine;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine("ERROR - FreeRemoteCommand: " + e.Message);
                query = "update RemoteCommand set id_Macchina = " + id_Fake + " where id_Macchina = " + id_machine;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }


        }

        private bool FreeMachinesConnectionTrace(string id_machine)
        {
            MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand newcmd;
            string query;
            try
            {
                query = "Delete * from MachinesConnectionTrace where id_Macchina = " + id_machine;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine("ERROR - FreeMachinesConnectionTrace: " + e.Message);
                query = "update MachinesConnectionTrace set id_Macchina = " + id_Fake + " where id_Macchina = " + id_machine;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            
        }
        private bool OpenConnection()
        {
            try
            {
              //  connection.Open();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Data;

namespace TestCasseTLK
{
    public static class DB_Cleaner_Fun
    {
        static MySqlConnection connection;

        //string serverip = "95.61.6.94";
        static string serverip = "10.10.10.37";
        static string database = "listener_DB";
        static string uid = "bot_user";
        static string password = "Qwert@#!99";

        //string connectionString = "server=95.61.6.94;database=listener_DB;uid=bot_user;pwd=Qwert@#!99;";
        static string connectionString = "server = 10.10.10.37; database = listener_DB; uid = bot_user; pwd = Qwert@#!99;";

        private static bool UpdateLogTables(string id_machinetodelete, string idmachinetoupdate)
        {
            //listener_DBContext DB = new listener_DBContext();
            //string connectionString = GetConnectString();
            //MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();
            try
            {
                MySqlCommand newcmd;
                string query = "Update Log set ID_machine='" + idmachinetoupdate + "'  where  ID_machine  = " + id_machinetodelete;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in UpdateLogTables: " + e.Message);
                return false;
            }
        }
        private static bool UpdateRemoteCommandTables(string id_machinetodelete, string idmachinetoupdate)
        {
            //listener_DBContext DB = new listener_DBContext();
            //string connectionString = GetConnectString();
            //MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();
            try
            {
                MySqlCommand newcmd;
                string query = "Update RemoteCommand set id_Macchina ='" + idmachinetoupdate + "'  where  id_Macchina  = " + id_machinetodelete;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in UpdateRemoteCommandTables: " + e.Message);
                return false;
            }
        }
        private static bool DeleteRemoteCommandTables(string id_machine)
        {
            //string connectionString = GetConnectString();
            //MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();
            MySqlCommand newcmd;
            string query;
            try
            {

                query = "Delete FROM RemoteCommand   where id_Macchina = " + id_machine;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR - RemoteCommand: " + e.Message);
                //connection.Close();
                return false;
            }
        }
        private static bool DeleteMachinesAttributesTables(string id_machine)
        {
            //string connectionString = GetConnectString();
            //MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
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
        private static bool UpdateMachinesConnectionTraceTables(string id_machinetodelete, string idmachinetoupdate)
        {
            //listener_DBContext DB = new listener_DBContext();
            //string connectionString = GetConnectString();
            //MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();
            try
            {
                MySqlCommand newcmd;
                string query = "Update MachinesConnectionTrace set id_Macchina ='" + idmachinetoupdate + "'  where  id_Macchina  = " + id_machinetodelete;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in UpdateMachinesConnectionTraceTables: " + e.Message);
                return false;
            }
        }
        private static bool UpdateCashTransactionTables(string id_machinetodelete, string idmachinetoupdate)
        {
            //listener_DBContext DB = new listener_DBContext();
            //string connectionString = GetConnectString();
            //MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();
            try
            {
                MySqlCommand newcmd;
                string query = "Update CashTransaction set ID_Machines ='" + idmachinetoupdate + "'  where  ID_Machines  = " + id_machinetodelete;
                newcmd = new MySqlCommand(query, connection);
                newcmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in UpdateCashTransactionTables: " + e.Message);
                return false;
            }
        }
        public static Boolean DeleteMachine(string idtodelete, string idtoupdate)
        {

            bool valreturn = false;
            try
            {
                if (idtoupdate == "")
                {
                    if(DeleteLogTables(idtodelete)) Console.WriteLine("DeleteMachine:DeleteLogTables ID  to Delete=" + idtodelete + " -OK");
                }
                else
                {
                    if (UpdateLogTables(idtodelete, idtoupdate)) Console.WriteLine("DeleteMachine:UpdateLogTables ID to Update=" + idtoupdate + ", ID to Delete=" + idtodelete + " -OK");
                }
                if (idtoupdate == "")
                {
                    if (DeleteRemoteCommandTables(idtodelete)) Console.WriteLine("DeleteMachine:DeleteLogTables ID  to Delete=" + idtodelete + " -OK");
                }
                else
                {
                    if (UpdateRemoteCommandTables(idtodelete, idtoupdate)) Console.WriteLine("DeleteMachine:UpdateRemoteCommandTables ID to Update=" + idtoupdate + ", ID to Delete=" + idtodelete + " -OK");
                }
                if (DeleteMachinesAttributesTables(idtodelete)) Console.WriteLine("DeleteMachine:DeleteMachinesAttributesTables  ID to Delete=" + idtodelete + " -OK");
                if (idtoupdate == "")
                {
                    if (DeleteCashTransTables(idtodelete)) Console.WriteLine("DeleteMachine:DeleteCashTransTables ID  to Delete=" + idtodelete + " -OK");
                }
                else
                {
                    if (UpdateCashTransactionTables(idtodelete, idtoupdate)) Console.WriteLine("DeleteMachine:UpdateCashTransactionTables ID to Update=" + idtoupdate + ", ID to Delete=" + idtodelete + " -OK");
                }
                if (idtoupdate == "")
                {
                    if (DeleteMachinesConnectionTrace(idtodelete)) Console.WriteLine("DeleteMachine:DeleteCashTransTables ID  to Delete=" + idtodelete + " -OK");
                }
                else
                {
                    if (UpdateMachinesConnectionTraceTables(idtodelete, idtoupdate)) Console.WriteLine("DeleteMachine:UpdateMachinesConnectionTraceTables ID to Update=" + idtoupdate + ", ID to Delete=" + idtodelete + " -OK");
                }
                if (DeleteFromMachinestable(idtodelete))
                {
                    Console.WriteLine("DeleteMachine:DeleteFromMachinestable ID to Delete=" + idtodelete + " -OK");
                    valreturn = true;
                }


                // DB.SaveChanges();

                valreturn = true;

                return valreturn;
            }
            catch (Exception e)
            {
                Console.WriteLine(DateTime.Now.ToString("yy/MM/dd,HH:mm:ss") + " : DeleteMachine: " + e.Message);
                Console.WriteLine(DateTime.Now.ToString("yy/MM/dd,HH:mm:ss") + " : DeleteMachine: " + e.StackTrace);
                return valreturn;
            }
        }
        private static bool DeleteFromMachinestable(string id_machine)
        {
            //string connectionString = GetConnectString();
            //MySqlConnection connection;
            //connection = new MySqlConnection(connectionString);
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
            catch (MySqlException e)
            {
                Console.WriteLine("ERROR - DeleteFromMachinestable: " + e.Message);
                connection.Close();
                return false;
            }
        }

        private static bool DeleteLogTables(string id_machine)
        {
            MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
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
            catch (Exception e)
            {
                Console.WriteLine("ERROR - DeleteLogTables: " + e.Message);
                //connection.Close();
                return false;
            }
        }

        private static bool DeleteCashTransTables(string id_machine)
        {
            //MySqlConnection connection;
            //connection = new MySqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();
            MySqlCommand newcmd;
            string query;
            try
            {
                query = "Delete FROM CashTransaction   where id = " + id_machine;
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

        private static bool DeleteRemoteCommand(string id_machine)
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

        private static bool DeleteMachinesConnectionTrace(string id_machine)
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
    }
}

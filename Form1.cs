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

        public struct DataMachines
        {
            public string Id_machines;
            public string Id_machinesCT;
            public string last_comunication;
            public string mid;
            public string tempmid;
            public string ipaddress;
            public string version;

        }
        DataMachines[] machineInfo;

        MySqlConnection connection;

        string serverip = "95.61.6.94";
        //string serverip =  "10.10.10.37";
        string database = "listener_DB";
        string uid = "bot_user";
        string password = "Qwert@#!99";
        //string connectionString = "server = localhost; database = lister_DB; uid = root; pwd = ;";
        string connectionString = "server=95.61.6.94;database=listener_DB;uid=bot_user;pwd=Qwert@#!99;";
        //string connectionString = "server = 10.10.10.37; database = listener_DB; uid = bot_user; pwd = Qwert@#!99;";

        //string id_Fake = "3973";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)

        {
            connection = new MySqlConnection(connectionString);

            connection.Open();
            //string data_transfered = "$M1,00021905,01,100,50,605,10,0,20,0,50,61,100,200,200,17,0,0,9999,3,00056700,566,0,,,30,11,1*6F,00021905,110,89314404000711313412,863071016974070,18,,,1,0,30,240,13+02";

            //string[] split_data_transfered = data_transfered.Split(',');
            //Console.WriteLine(split_data_transfered[split_data_transfered.Length - 5]);

            //split_data_transfered[split_data_transfered.Length - 5] = "1";


            //LoadModemToConfig("172.16.162.17", 123456789123456, "77770001");
            //LoadModemToConfig("172.16.162.18", 123123123123123, "77770001");
            //LoadModemToConfig("172.16.162.19", 456456456456456, "77770001");
            //LoadModemToConfig("172.16.162.20", 789789789789789, "77770001");

            // tryjoin();


            // testMID(); 
            // RemoveMachine();

            //check_Dati();
            // test_daticash_convert();
            // Aggiorna_Instagram();
        }

        private void Aggiorna_Instagram()
        {
            MySqlConnection connection;
            MySqlDataReader dataReader;
            MySqlCommand cmd;
            List<string> IDMachinesToDel = new List<string>();
            List<string> IDMachinesToUpdate = new List<string>();
            List<string> MidToUpdate = new List<string>();

            connection = new MySqlConnection(connectionString);
            connection.Open();
            string query = "";
            query = "select id, version from  Machines where mid like 'Duplicato%' and Version like '10%';";
            cmd = new MySqlCommand(query, connection);
            dataReader = cmd.ExecuteReader();

            int counter = 0;

            while (dataReader.Read())
            {
                if (dataReader["version"].ToString() == "105" | dataReader["version"].ToString() == "106")
                {
                    counter++;
                    IDMachinesToDel.Add(dataReader["id"].ToString());
                }
            }
            //dataReader.Close();

            foreach (string idtodel in IDMachinesToDel)
            {
                dataReader.Close();
                //query = "select  transferred_data from  MachinesConnectionTrace where id_Macchina = " + idtodel + " and transferred_data like '<TPK=$I2%' Limit 1;";
                query = "select  transferred_data from  MachinesConnectionTrace where id_Macchina = " + idtodel + " and transferred_data like '<MID=%' Limit 1;";
                cmd = new MySqlCommand(query, connection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    //string[] splitStr = dataReader["transferred_data"].ToString().Split(',');
                    //MidToUpdate.Add(splitStr[1]);
                    string tmpstr= dataReader["transferred_data"].ToString().Replace("<","");
                    
                    string[] splitStr = tmpstr.Split('>');
                    MidToUpdate.Add(splitStr[0].Substring(4, splitStr[0].Length -4));


                }
            }

            foreach (string mid in MidToUpdate)
            {
                  dataReader.Close();
                query = "select id from Machines where mid = " + mid + " Limit 1; ";
                cmd = new MySqlCommand(query, connection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    IDMachinesToUpdate.Add(dataReader["id"].ToString());
                }
            }

            string[] id_todel = IDMachinesToDel.ToArray();
            string[] id_toupdate = IDMachinesToUpdate.ToArray();
            for (int i=0;i<counter;i++)
            {
                DeleteMachine(id_todel[i], id_toupdate[i]);
                Console.WriteLine("machine deleted: " + i.ToString());
            }


        }

        private void tryjoin()
        {
            MySqlConnection connection;
            MySqlDataReader dataReader;
            MySqlCommand cmd;

            List<string> IdfromMachines = new List<string>();
            List<string> InfoMachines = new List<string>();
            List<string> InfoMachinesCT = new List<string>();

            //Dictionary<string, string> instaToChange = new Dictionary<string, string>();

            connection = new MySqlConnection(connectionString);
            connection.Open();
            string query = "";
            query = "select id, ip_address,mid,last_communication from Machines where IsOnline =1 and mid like 'RecuperoInCorso%' and ip_address not like '172.16%';";


            // query = "select * from Machines where  mid like '%" + idread + "%';";
            cmd = new MySqlCommand(query, connection);
            dataReader = cmd.ExecuteReader();

            int counter = 0;

            while (dataReader.Read())
            {
                counter++;
                InfoMachines.Add(dataReader["id"].ToString() + "," + dataReader["last_communication"].ToString() + "," + dataReader["ip_address"].ToString() + "," + dataReader["mid"].ToString());
            }
            dataReader.Close();
            machineInfo = new DataMachines[counter];
            int k=0;
            counter = 0;
            foreach(string info in InfoMachines)
            {
                string[] splitInfo = info.Split(',');
                machineInfo[k].Id_machines = splitInfo[0];
                machineInfo[k].ipaddress = splitInfo[2];
                machineInfo[k].last_comunication = splitInfo[1];
                machineInfo[k].tempmid = splitInfo[3];

               
                query = "select id,transferred_data from MachinesConnectionTrace where id_Macchina = '"+ machineInfo[k].Id_machines + "' and transferred_data like '<MID=%';";
                cmd = new MySqlCommand(query, connection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    counter++;
                    InfoMachinesCT.Add(dataReader["id"].ToString() + "," + dataReader["transferred_data"].ToString());
                }
                dataReader.Close();
                string tmpmidnew = "";
                foreach (string infoCT in InfoMachinesCT)
                {
                    string[] infoCTsplit = infoCT.Split(',');
                    machineInfo[k].Id_machinesCT = machineInfo[k].Id_machinesCT+ infoCTsplit[0] + ",";

                    if (tmpmidnew != infoCTsplit[1])
                    {
                        tmpmidnew = infoCTsplit[1];
                        infoCTsplit[1] = infoCTsplit[1].Replace("<", "");
                        string[] resplitinfoct = infoCTsplit[1].Split('>');
                        machineInfo[k].mid = resplitinfoct[0].Replace("MID=", "");
                        machineInfo[k].version = resplitinfoct[1].Replace("VER=", "");
                    }
                }



                    k++;
            }



            int i = 0;
            int x = 0;
            string tmpMID = "";
            for (i = 0; i < machineInfo.Length; i++)
            {
                if (machineInfo[i].mid == null | machineInfo[i].Id_machinesCT == null)
                {
                    if (DeleteFromMachinestable(machineInfo[i].Id_machines)) Console.WriteLine("L'ID " + machineInfo[i].Id_machines + "è stato eliminato");
                }
                else
                {

                    query = " update MachinesConnectionTrace set id_Macchina=" + machineInfo[i].Id_machines + " where  transferred_data like '<MID=" + machineInfo[i].mid + ">%';";
                    cmd = new MySqlCommand(query, connection);
                    dataReader = cmd.ExecuteReader();
                    dataReader.Close();


                    query = "update Machines set mid = '" + machineInfo[i].mid + "', version = " + machineInfo[i].version + " where id = " + machineInfo[i].Id_machines + ";";
                    cmd = new MySqlCommand(query, connection);
                    dataReader = cmd.ExecuteReader();
                    dataReader.Close();

                    machineInfo[i].Id_machinesCT = machineInfo[i].Id_machinesCT.Substring(0, machineInfo[i].Id_machinesCT.Length - 1);
                    string[] IdCTtoremove = machineInfo[i].Id_machinesCT.Split(',');
                    for (x = 0; x < IdCTtoremove.Length; x++)
                    {
                        query = "update CashTransaction set ID_Machines = '" + machineInfo[i].Id_machines + "' where ID_Machines = " + IdCTtoremove[x] + ";";
                        cmd = new MySqlCommand(query, connection);
                        dataReader = cmd.ExecuteReader();
                        dataReader.Close();
                        //query = "update DatiCassa set ID_Machines = '" + machineInfo[i].mid + "' where ID_Machines = " + IdCTtoremove[x] + ";";
                        //cmd = new MySqlCommand(query, connection);
                        //dataReader = cmd.ExecuteReader();
                        //dataReader.Close();
                    }

                }
            }
            //if (instaToChangeIdToremove[i] == instaToChangeIdOK[i])
            //    {
            //        if (DeleteLogTables(instaToChangeIdToremove[i]))
            //        {
            //            if (DeleteRemoteCommand(instaToChangeIdToremove[i]))
            //            {
            //                if (DeleteMachinesAttributesTables(instaToChangeIdToremove[i]))
            //                {
            //                    if (DeleteFromMachinestable(instaToChangeIdToremove[i])) Console.WriteLine(counter++);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (DeleteFromMachinestable(instaToChangeIdToremove[i])) Console.WriteLine(counter++);
            //        }
            //    }
            


            for (i=0;i< machineInfo.Length;i++)
            {
               //dataReader.Close();
                query = "select id,id_Macchina,time_stamp, transferred_data from  MachinesConnectionTrace where id_Macchina='" + machineInfo[i].Id_machines + "' and transferred_data like '<MID=%';";
                cmd = new MySqlCommand(query, connection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    //newWriter.WriteLine(dataReader["id"].ToString() + ","+ dataReader["id_Macchina"].ToString() + "," + dataReader["time_stamp"].ToString() + "," + dataReader["transferred_data"].ToString());
                }
            }
           // newWriter.Close();


        }
        private void testMID()
        {
            //connection = new MySqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();

            string query = "";
            query = "select* from MachinesConnectionTrace where transferred_data like '<MID=%';";

            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            StreamWriter newWriter = new StreamWriter(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\vifica MID.txt", false);

            while (dataReader.Read())
            {
                newWriter.WriteLine(dataReader["transferred_data"].ToString().Substring(0,10));
            }
            //idlist.Add("144");
            dataReader.Close();
            newWriter.Close();


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

        private void check_Dati()
        {
            try
            {
                MySqlConnection connection;
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string query = "";
                //query = "SELECT distinct (id_Macchina) FROM machinesconnectiontrace WHERE transferred_data LIKE '%M3%';";
                //query = "SELECT id,mid FROM `machines` WHERE version LIKE'%838' order by mid";
                //string query = "SELECT distinct (id_Macchina) FROM MachinesConnectionTrace WHERE transferred_data LIKE '<TPK=$M3%';";
                
                query = "select * from Machines where IsOnline = 0;";
                query = "select * from CashTransaction";
                query = "select * from Machines where IsOnline = 0 order by version;";
                query = "select ID from Machines where mid like 'Recupero%'";


                List<string> Midlist = new List<string>();
                List<string> IdfromMachines = new List<string>();
                List<string> Versionlist = new List<string>();
                Dictionary<string, string> id_vers_List = new Dictionary<string, string>();
                Dictionary<string, string> id_mid_List = new Dictionary<string, string>();
                MySqlDataReader dataReader;

                if (File.Exists(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\Recupero.txt")) File.Delete(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\Recupero.txt");

                StreamWriter newWriter = new StreamWriter(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\Recupero.txt", false);

                MySqlCommand cmd = new MySqlCommand(query, connection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    //id_mid_List.Add(dataReader["ODM"].ToString(), dataReader["ID_Machines"].ToString());
                    IdfromMachines.Add(dataReader["id"].ToString());
                    //newWriter.WriteLine(dataReader["mid"].ToString() + "," + dataReader["version"].ToString() + "," + dataReader["last_communication"].ToString() );
                }

    
                foreach (string idtemp in IdfromMachines)
                {
                    string t_mid = "";

                    dataReader.Close();
                    query = "SELECT transferred_data,time_stamp FROM MachinesConnectionTrace WHERE id_Macchina = '" + idtemp + "';";
                    cmd = new MySqlCommand(query, connection);
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        if (dataReader["transferred_data"].ToString().StartsWith("<MID="))
                            newWriter.WriteLine(dataReader["time_stamp"].ToString() + " - " + dataReader["transferred_data"].ToString());
                    }


                }
                newWriter.Close();

                //newWriter.Close();
                return;

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
                    query = "SELECT transferred_data,time_stamp FROM machinesconnectiontrace WHERE id_Macchina = '" + idtemp +  "';"; 
                    cmd = new MySqlCommand(query, connection);
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {

                        newWriter.WriteLine(dataReader["mid"].ToString()+" - "+dataReader["version"].ToString());

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
            MySqlConnection connection;
            connection = new MySqlConnection(connectionString);

            if (connection.State==ConnectionState.Closed) connection.Open();

            string query = "";
            query = "select * from  Machines where IsOnline=0 and mid like '77770001_%';";
            //query = "select * from Machines where IsOnline = 0;";
            //query = "select * from Machines where imei = 869153046561855";

            //query = "SELECT * from Machines where last_communication like '2021-11-10 14%';";
            // query = "SELECT * FROM Machines where mid like '%RecuperoInCorso..%'";
            //query = "SELECT * FROM Machines where mid like '%Duplicato%'";
            //query = "select id from Machines where last_communication Like '2021-11-19%';";


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
                    if (DeleteLogTables(id)) Console.WriteLine("DeleteLogTables: OK");
                    if (DeleteRemoteCommand(id)) Console.WriteLine("DeleteRemoteCommand: OK");
                    if (DeleteMachinesAttributesTables(id)) Console.WriteLine("DeleteMachinesAttributesTables: OK");
                    if (DeleteCashTransTables(id)) Console.WriteLine("DeleteCashTransTables: OK");
                    if (DeleteMachinesConnectionTrace(id)) Console.WriteLine("DeleteMachinesConnectionTrace: OK");
                    if (DeleteFromMachinestable(id)) Console.WriteLine(counter++); Console.WriteLine("DeleteFromMachinestable: OK");

                }

                connection.Close();

                //return list to be displayed
               
            }
            else
            {
                Console.WriteLine("");
            }
        }
        public  Boolean DeleteMachine(string idtodelete, string idtoupdate)
        {

            bool valreturn = false;
            try
            {
                if (UpdateLogTables(idtodelete, idtoupdate)) Console.WriteLine("DeleteMachine:UpdateLogTables ID to Update=" + idtoupdate + ", ID to Delete=" + idtodelete + " -OK");

                if (UpdateRemoteCommandTables(idtodelete, idtoupdate)) Console.WriteLine("DeleteMachine:UpdateRemoteCommandTables ID to Update=" + idtoupdate + ", ID to Delete=" + idtodelete + " -OK");
                if (DeleteMachinesAttributesTables(idtodelete)) Console.WriteLine("DeleteMachine:DeleteMachinesAttributesTables  ID to Delete=" + idtodelete + " -OK");
                if (UpdateCashTransactionTables(idtodelete, idtoupdate)) Console.WriteLine("DeleteMachine:UpdateCashTransactionTables ID to Update=" + idtoupdate + ", ID to Delete=" + idtodelete + " -OK");
                if (UpdateMachinesConnectionTraceTables(idtodelete, idtoupdate)) Console.WriteLine("DeleteMachine:UpdateMachinesConnectionTraceTables ID to Update=" + idtoupdate + ", ID to Delete=" + idtodelete + " -OK");
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

        }        /// 
                 /// </summary>

        private  bool UpdateLogTables(string id_machinetodelete, string idmachinetoupdate)
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
        private  bool UpdateRemoteCommandTables(string id_machinetodelete, string idmachinetoupdate)
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
        private  bool DeleteMachinesAttributesTables(string id_machine)
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
        private  bool UpdateMachinesConnectionTraceTables(string id_machinetodelete, string idmachinetoupdate)
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
        private  bool UpdateCashTransactionTables(string id_machinetodelete, string idmachinetoupdate)
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
        private  bool DeleteFromMachinestable(string id_machine)
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
            DeleteMachine(txtIdtoDelete.Text, txtIdtoUpdate.Text);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

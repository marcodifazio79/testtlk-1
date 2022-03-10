using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace TestCasseTLK
{
    public partial class Form1 : Form
    {

        public struct DataMachinesConnTrace
        {
            
            public string time_stamp;
            public string ip_address;
            public string send_or_recv;
            public string transferred_data;
            public string id_Macchina;
            public string telemetria_status;

        }
        DataMachinesConnTrace[] MCTraceInfo;
        DataMachinesConnTrace Tmp_MCTraceInfo;
        public struct DataMachines
        {
            public string Id_machines;
            public string Id_machinesCT;
            public string last_comunication;
            public string mid;
            public string imei;
            public string statusonline;
            public string tempmid;
            public string ipaddress;
            public string version;

        }
        DataMachines[] machineInfo;
        public struct InfoDuplicati
        {
            public string Id_machines;
            public string mid;
            public string statusonline;
        }
        InfoDuplicati[] infiMIDduplicati_online;
        InfoDuplicati[] infiMIDduplicati_offline;


        MySqlConnection connection;

        //string serverip = "95.61.6.94";
        string serverip =  "10.10.10.37";
        string database = "listener_DB";
        string uid = "bot_user";
        string password = "Qwert@#!99";
        
        //string connectionString = "server=95.61.6.94;database=listener_DB;uid=bot_user;pwd=Qwert@#!99;";
        string connectionString = "server = 10.10.10.37; database = listener_DB; uid = bot_user; pwd = Qwert@#!99;";

        public string[] filename;
        public string[] fileFullpath;
        public string[] filepath;

        //string id_Fake = "3973";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)

        {
            //testConnessionespagna();

            //connection = new MySqlConnection(connectionString);

            // tryjoin();
            //WriteList(true);
            //RemoveRecuperoxxx();
            // RemoveMID_DUPLICATOxxx();
            //return;
            //ReadList_RemoveDuplicates(); 
            // RemoveMachine();
            //test_daticash_convert();
            // check_Dati();
            // test_daticash_convert();
            //Aggiorna_Instagram();
            //Svuota_DB();
            //RimuoviDuplicato7777();
            //RimuoviDuplicato();
            //testUbuntu20_Mysql8();
            //store_old_data();

        }

        private void store_old_data()
        {
            MySqlDataReader dataReader;
            MySqlCommand cmd;
            int counter = 0;
            //List<string> IDRowsToDelete = new List<string>();
            Dictionary<string, DataMachinesConnTrace> IDRowsToDelete = new Dictionary<string, DataMachinesConnTrace>();
            string lastday =DateTime.Now.AddDays(-8).ToString("yyyy-MM-dd");
            connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "select id  from  MachinesConnectionTrace where time_stamp like '"+lastday+"%' order by id desc limit 1;";
            cmd = new MySqlCommand(query, connection);
            dataReader = cmd.ExecuteReader();
            string id_temp="";
            while (dataReader.Read())
            {
                counter++;
                id_temp=dataReader["id"].ToString();

            }

            dataReader.Close();
            if (counter == 0) return;
            query = "select *  from  MachinesConnectionTrace where id < " + id_temp + ";";
            cmd = new MySqlCommand(query, connection);
            dataReader = cmd.ExecuteReader();

            counter = 0;
            while (dataReader.Read())
            {
                counter++;
                Tmp_MCTraceInfo.time_stamp = dataReader["time_stamp"].ToString();
                Tmp_MCTraceInfo.ip_address = dataReader["ip_address"].ToString(); 
                Tmp_MCTraceInfo.send_or_recv = dataReader["send_or_recv"].ToString();
                Tmp_MCTraceInfo.transferred_data = dataReader["transferred_data"].ToString();
                Tmp_MCTraceInfo.id_Macchina = dataReader["id_Macchina"].ToString();
                Tmp_MCTraceInfo.telemetria_status = dataReader["telemetria_status"].ToString();

                IDRowsToDelete.Add(dataReader["id"].ToString(),Tmp_MCTraceInfo);
            }

            dataReader.Close();

            string ConnStrServeBK = "server=192.168.17.208;database=WharehouseTLK;uid=tlk_service;pwd=Qwert@#!99;";
            MySqlConnection connection_BK = new MySqlConnection(ConnStrServeBK);
            connection_BK.Open();
            if (connection_BK.State == ConnectionState.Open)
            {
                foreach (DataMachinesConnTrace tmp_data_MCT in IDRowsToDelete.Values)
                {
                    string[] splittimestamp = tmp_data_MCT.time_stamp.Split(' ');
                    string tmptimestamp = splittimestamp[0].Substring(6, 4) + "-" + splittimestamp[0].Substring(3, 2) + "-" + splittimestamp[0].Substring(0, 2) + " " + splittimestamp[1];
                    query = "insert into MachinesConnectionTrace(time_stamp,ip_address,send_or_recv,transferred_data,id_Macchina,telemetria_status) values " +
                        "('" + tmptimestamp + "','" + tmp_data_MCT.ip_address + "','" + tmp_data_MCT.send_or_recv + "','" + tmp_data_MCT.transferred_data + "','" + tmp_data_MCT.id_Macchina + "','" + tmp_data_MCT.telemetria_status + "');";
                    cmd = new MySqlCommand(query, connection_BK);
                    cmd.ExecuteReader();
                }

                connection_BK.Close();

                foreach (string idtodel in IDRowsToDelete.Keys)
                {
                    query = "Delete from MachinesConnectionTrace  where id=" + idtodel + ";";
                    cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteReader();
                }
            }
            else
            {
                Console.WriteLine("Spostamentamento dati da MCT non possibile per impossibilità di avviare la connessione al DB di backup");
            }




        }
        private void testUbuntu20_Mysql8()
        {
            serverip = "192.168.17.208";
            connectionString = "server=192.168.17.208;database=dbtest;uid=testwriter;pwd=password;";
            connection = new MySqlConnection(connectionString);
            connection.Open();
            if (connection.State == ConnectionState.Open)
                MessageBox.Show("daje porco d");

            string Query = "insert into packet(rawPacket,ipAddress,port) values('nuovo pacchetto23','192.2.0.0','6598');";
            
                        
            MySqlCommand MyCommand2 = new MySqlCommand(Query, connection);
            
            MyCommand2.ExecuteReader();     // Here our query will be executed and data saved into the database.  
            MessageBox.Show("Save Data");

        }
        private void testConnessionespagna()
        {
            
            connectionString = "server=95.61.6.94;database=listener_DB;uid=bot_user;pwd=Qwert@#!99;";
            connectionString = "server=192.168.209.188;database=listener_DB;uid=bot_user;pwd=Qwert@#!99;";
            connection = new MySqlConnection(connectionString);
            connection.Open();
            if (connection.State == ConnectionState.Open)
                MessageBox.Show("daje porco d");

        }

        private void RemoveRecuperoxxx()
        {
            MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            string query = "";
            connection.Open();
            int counter = 0;
            int i = 0;
            MySqlDataReader dataReader;
            MySqlCommand cmd;
            Dictionary<string, string> IDMachinesSelect = new Dictionary<string, string>();
            Dictionary<string, string> RowMCTToDel = new Dictionary<string, string>();
            List<string> IDMachinesTodelete = new List<string>();



            /////preparazione file

            query = "select id, IsOnline from  Machines where mid like 'Recupero%';";
            cmd = new MySqlCommand(query, connection);
            dataReader = cmd.ExecuteReader();


            while (dataReader.Read())
            {
                counter++;
                IDMachinesTodelete.Add(dataReader["id"].ToString());

            }


            dataReader.Close();
            foreach (string id in IDMachinesTodelete)
            {
                query = "select count(id) from  MachinesConnectionTrace where id_Macchina = " + id + ";";

                using (var cmdcount = new MySqlCommand(query, connection))
                {
                    int count = Convert.ToInt32(cmdcount.ExecuteScalar());
                    if (count == 0)
                    {

                        query = "Delete from Machines where id="+id+ ";";
                        cmd = new MySqlCommand(query, connection);
                        cmd.ExecuteNonQuery();
   
                    }
                }
            }
        }


        private void RemoveMID_DUPLICATOxxx()
        {
            MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            string query = "";
            connection.Open();
            int counter = 0;
            int i = 0;
            MySqlDataReader dataReader;
            MySqlCommand cmd;
            Dictionary<string, string> IDMachinesSelect = new Dictionary<string, string>();
            Dictionary<string, string> RowMCTToDel = new Dictionary<string, string>();
            List<string> IDMachinesTodelete = new List<string>();

            // lettura dati da DB

            //query = "select id, IsOnline from  Machines where mid like 'Recupero%';";
            //cmd = new MySqlCommand(query, connection);
            //dataReader = cmd.ExecuteReader();

            //while (dataReader.Read())
            //{
            //    counter++;
            //    IDMachinesSelect.Add(dataReader["id"].ToString(), dataReader["IsOnline"].ToString());
            //}
            //dataReader.Close();

            // lettura dati da File
            string tmpstr = "";

            StreamReader newReader = new StreamReader(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\UltimiDoppi.txt");

            tmpstr = newReader.ReadToEnd();

            newReader.Close();
            tmpstr = tmpstr.Replace("\r", "");
            string[] RowData = tmpstr.Split('\n');
            for (int x = 0; x < RowData.Length; x++)
            {
                string[] splitRowData = RowData[x].Split(',');
                IDMachinesSelect.Add(splitRowData[0], splitRowData[1]);

            }
            /// fine lettura dati
            /////preparazione file
            ///

            counter = 0;
            if (File.Exists(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\MIDStringListRecupero.txt")) File.Delete(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\MIDStringListRecupero.txt");
            StreamWriter newWriter = new StreamWriter(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\MIDStringListRecupero.txt", false);
            foreach (string idtodel in IDMachinesSelect.Keys)
            {
                
                query = "select id, transferred_data,ip_address,id_Macchina from  MachinesConnectionTrace where id_Macchina = " + idtodel + " and transferred_data like '<MID=%' limit 1;";
                cmd = new MySqlCommand(query, connection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    string trdata = dataReader["transferred_data"].ToString();
                    if (trdata.Contains("<TCA=U"))
                        trdata = trdata.Substring(0, trdata.IndexOf("<TCA=U"));
                    newWriter.WriteLine(dataReader["id"].ToString() + "," + dataReader["id_Macchina"].ToString() + "," + dataReader["ip_address"].ToString() + "," + trdata);

                    counter++;
                    Console.WriteLine(counter);

                }
                dataReader.Close();

            }
            newWriter.Close();
           // 

            // fine preprazione file
            //return;



            newReader = new StreamReader(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\MIDStringList.txt");

            Dictionary<string, string> ListaMidDuplicatiOnline = new Dictionary<string, string>();
            Dictionary<string, string> ListaMidDuplicatiOffline = new Dictionary<string, string>();
            tmpstr = newReader.ReadToEnd();

            newReader.Close();
            tmpstr = tmpstr.Replace("\r", "");
            RowData = tmpstr.Split('\n');

            machineInfo = new DataMachines[RowData.Length];
            for (int x = 0; x < RowData.Length-1; x++)
            {
                if (RowData[x] != "")
                {
                    string[] splitRowData = RowData[x].Split(',');


                    if (RowData[x].Contains("TYP=2")) ////53001,<MID=00022942><VER=106><TYP=2>,10.10.123.265,11526
                    {

                        machineInfo[x].Id_machinesCT = splitRowData[0];
                        machineInfo[x].Id_machines = splitRowData[1];
                        machineInfo[x].ipaddress = splitRowData[2];
                        string[] tmpSplit2 = splitRowData[3].Split('>');
                        machineInfo[x].mid = tmpSplit2[0].Replace("<MID=", "");
                        machineInfo[x].version = tmpSplit2[1].Replace("<VER=", "");


                    }
                    else//53010,<MID=00012997-863071016987460><VER=110>,10.10.123.265,11526
                    {
                        machineInfo[x].Id_machinesCT = splitRowData[0];
                        machineInfo[x].Id_machines = splitRowData[1];
                        machineInfo[x].ipaddress = splitRowData[2];
                        string[] tmpSplit2 = splitRowData[3].Split('>');
                        tmpSplit2[0] = tmpSplit2[0].Replace("<MID=", "");
                        string[] tmpSplit3 = tmpSplit2[0].Split('-');
                        machineInfo[x].mid = tmpSplit3[0];
                        machineInfo[x].imei = tmpSplit3[1];

                        machineInfo[x].version = tmpSplit2[1].Replace("<VER=", "");

                    }
                }
            }
            for (i = 0; i < machineInfo.Length - 1; i++)
            {

                query = "select count(id) from  Machines where mid = "+ machineInfo[i].mid + ";";

                using (var cmdcount = new MySqlCommand(query, connection))
                {
                    int count = Convert.ToInt32(cmdcount.ExecuteScalar());
                    if(count>0)
                    {
                        int y = 0;
                        string[] tmpID = new string[count];
                        string tmpIDtoUpdate = "";
                        string[] tmpStatus = new string[count];
                        query = "select id,IsOnline from  Machines where mid = " + machineInfo[i].mid + " order by id ;";
                        cmd = new MySqlCommand(query, connection);
                        dataReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            tmpID[y]= dataReader["id"].ToString();
                            tmpStatus[y] = dataReader["IsOnline"].ToString();
                            if (tmpStatus[y] == "True") tmpIDtoUpdate = tmpID[y];
                            tmpIDtoUpdate = tmpID[0];
                            y++;
                        }
                        dataReader.Close();
                        if (count == 1)
                        {
                            DB_Cleaner_Fun.DeleteMachine(machineInfo[i].Id_machines, tmpID[0]);
                        }
                        else
                        {
                            for ( int k = 0; k < tmpID.Length; k++)
                            {
                                if (tmpID[k] != tmpIDtoUpdate) DB_Cleaner_Fun.DeleteMachine(tmpID[k], tmpIDtoUpdate);
                            }
                        }
                    }
                    else
                    {
                        if (machineInfo[i].imei == null) machineInfo[i].imei = "0";
                        query = "Update Machines set  ip_address='"+ machineInfo[i].ipaddress + "', imei=" + machineInfo[i].imei + ", mid=" + machineInfo[i].mid + ",version=" + machineInfo[i].version + " where id ='"+ machineInfo[i].Id_machines + "';";
                        cmd = new MySqlCommand(query, connection);
                        cmd.ExecuteNonQuery();
                    }
                     
                }


                //cmd = new MySqlCommand(query, connection);
                //dataReader = cmd.ExecuteReader();
                //counter = 0;

                //while (dataReader.Read())
                //{
                //    counter++;
                //    IDMachinesSelect.Add(dataReader["id"].ToString(), dataReader["IsOnline"].ToString());
                //}
            }

            int kount = 0;
            foreach (string mid in ListaMidDuplicatiOffline.Values)
            {
                string idtodelete = ListaMidDuplicatiOffline.Keys.ElementAt(kount);
                kount++;
                string idtoupdate = "";
                ListaMidDuplicatiOnline.TryGetValue(mid, out idtoupdate);
                if (idtoupdate == null)
                {
                    Console.WriteLine(mid);
                }
                else
                {
                    DB_Cleaner_Fun.DeleteMachine(idtodelete, idtoupdate);
                }

            }

            return;

        
        }
        private void RimuoviDuplicato7777()
        {

            MySqlConnection connection;
            MySqlDataReader dataReader;
            MySqlCommand cmd;
            Dictionary<string, string> IDMachinesToDelTemp = new Dictionary<string, string>();
            Dictionary<string, string> RowMCTToDel = new Dictionary<string, string>();
            List<string> IDMachinesTodelete = new List<string>();
            List<string> TMPIDMachinesTodelete = new List<string>();


            string query = "";

            connection = new MySqlConnection(connectionString);
            connection.Open();
            query = "select id from  Machines where mid like '77770001_%' or mid like '5555555%' or mid like 'TCC%' and IsOnline=0";
            cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
//            cmd.CommandTimeout = 60;
            dataReader = cmd.ExecuteReader();
            int k = 0;
            
            while (dataReader.Read())
            {
                IDMachinesTodelete.Add(dataReader["id"].ToString());
            }
            dataReader.Close();
            foreach (string id in IDMachinesTodelete)
            {
                DB_Cleaner_Fun.DeleteMachine(id, "");
            }

            IDMachinesTodelete.Clear();

            query = "select id from  Machines where mid like 'RecuperoInCorso%' and IsOnline=0";
            cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            //            cmd.CommandTimeout = 60;
            dataReader = cmd.ExecuteReader();
            k = 0;

            while (dataReader.Read())
            {
                TMPIDMachinesTodelete.Add(dataReader["id"].ToString());
            }
            dataReader.Close();  

            foreach (string id in TMPIDMachinesTodelete)
            {
                query = "select id_Macchina,transferred_data from  MachinesConnectionTrace where id like '" + id+"' limit 1;";
                cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                dataReader = cmd.ExecuteReader();
                k = 0;
                if (dataReader.RecordsAffected==-1) IDMachinesTodelete.Add(id);

                dataReader.Close();

            }
            foreach (string id in IDMachinesTodelete)
            {
                DB_Cleaner_Fun.DeleteMachine(id, "");
            }

            }
        private void RimuoviDuplicato()
        {
            MySqlConnection connection;
            MySqlDataReader dataReader;
            MySqlCommand cmd;
            Dictionary<string,string> IDMachinesToDelTemp = new Dictionary<string,string>();
            Dictionary<string, string> RowMCTToDel = new Dictionary<string, string>();
            List<string> IDMachinesToUpdate = new List<string>();
            List<string> MidToUpdate = new List<string>();

            string query = "";

            connection = new MySqlConnection(connectionString);
            connection.Open();

            FileSerch(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\NewFile");

            int i = 0;

            for (i=0;i< fileFullpath.Length;i++)
            {
                string MidFromTransferredData = filename[i].Replace("Id_duplicatiMID_","");
                string IDMachines_Online = "";
              
                StreamReader  newReader = new StreamReader(fileFullpath[i]);

                Dictionary<string, string> ListaMidDuplicatiOnline = new Dictionary<string, string>();
                Dictionary<string, string> ListaMidDuplicatiOffline = new Dictionary<string, string>();
                string tmpstr = newReader.ReadToEnd();

                newReader.Close();
                tmpstr = tmpstr.Replace("\r", "");
                string[] RowData = tmpstr.Split('\n');
                for (int x = 0; x < RowData.Length-1; x++)
                {
                    string[] rowSplit = RowData[x].Split(',');
                    if (rowSplit[3]=="True")
                    {
                        if(!ListaMidDuplicatiOnline.ContainsKey(rowSplit[1]))ListaMidDuplicatiOnline.Add(rowSplit[1], rowSplit[0]);
                    }
                    else
                    {
                        ListaMidDuplicatiOffline.Add(rowSplit[0], rowSplit[1]);
                    }

                }
                int kount = 0;
                foreach (string mid in ListaMidDuplicatiOffline.Values)
                {
                    string idtodelete = ListaMidDuplicatiOffline.Keys.ElementAt(kount);
                    kount++;
                    string idtoupdate="";
                    ListaMidDuplicatiOnline.TryGetValue(mid,out idtoupdate);
                    if (idtoupdate==null)
                    {
                        
                    }
                    else
                    {
                        DB_Cleaner_Fun.DeleteMachine(idtodelete, idtoupdate);
                    }

                }
                return;
                string[] IDMachines_To_Remove = tmpstr.Split('\n');

                for (int x=0;x<IDMachines_To_Remove.Length;x++)
                {

                    if (IDMachines_To_Remove[x]!= IDMachines_Online)
                    {
                        DB_Cleaner_Fun.DeleteMachine(IDMachines_To_Remove[x], IDMachines_Online);
                    }
                }

                //dataReader.Close();
            }

        }
        private void FileSerch(string pathFolder)
        {

            string[] arrayfile = Directory.GetFiles(pathFolder);
            if (arrayfile.Length > 0)
            {

                filename = new string[arrayfile.Length];
                fileFullpath = new string[arrayfile.Length];
                filepath = new string[arrayfile.Length];

                int i = 0;
                for (i = 0; i < arrayfile.Length; i++)
                {
                    filename[i] = Path.GetFileNameWithoutExtension(arrayfile[i]);
                    filepath[i] = Path.GetDirectoryName(arrayfile[i]);
                    fileFullpath[i] = Path.GetFullPath(arrayfile[i]);
                }
            }
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
            query = "select id, version from  Machines where mid like 'Recupero%' and Version like '10%';";
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
                DB_Cleaner_Fun.DeleteMachine(id_todel[i], id_toupdate[i]);
                Console.WriteLine("machine deleted: " + i.ToString());
            }

        }


        private void ReadList_RemoveDuplicates()
        {
            WriteList(true);

            StreamReader newreader = new StreamReader(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\MID da separare.txt");

            Dictionary<string, InfoDuplicati> listaID_MID= new  Dictionary<string, InfoDuplicati>();

            string tmpstr = newreader.ReadToEnd().Replace("\r","");
            newreader.Close();
            string[] RowData = tmpstr.Split('\n');

            machineInfo = new DataMachines[RowData.Length];
            int i = 0;

            for (i=0;i< RowData.Length-1;i++)
            {
                string[] infoSplit = RowData[i].Split(',');
                machineInfo[i].Id_machines = infoSplit[0];
                machineInfo[i].mid= infoSplit[1];
                machineInfo[i].statusonline = infoSplit[2];
               // listaID_MID.Add(infoSplit[0], infoSplit[1]);
            }

            i = 0;
            string tmpmid = "";
            string tmpstatus = "";
            string tmpID = "";
            int k = 0;
            
            if (File.Exists(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\MID_Doppi.csv")) File.Delete(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\MID_Doppi.txt");

            StreamWriter newWriter = new StreamWriter(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\MID_Doppi.txt", false);

            for (i = 0; i < RowData.Length - 1; i++)
            {
                tmpmid = machineInfo[i].mid;

                if (tmpmid == machineInfo[i + 1].mid)
                {
                    newWriter.WriteLine(machineInfo[i].Id_machines + "," + machineInfo[i].mid + "," + machineInfo[i].statusonline);
                    for (k = i + 1; k < RowData.Length - 1; k++)
                    {
                        if (tmpmid == machineInfo[k].mid)
                        {
                            newWriter.WriteLine(machineInfo[k].Id_machines + "," + machineInfo[k].mid + "," + machineInfo[k].statusonline);
                        }
                        else
                        {
                            i = k;
                            goto exitfor;
                        }
                    }
                exitfor:
                    i = i;
                }
            }
            newWriter.Close();
    //        return;

            tmpstr ="";
            
            
            StreamReader newReader = new StreamReader(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\MID_Doppi.txt");

            Dictionary<string, string> ListaMidDuplicatiOnline = new Dictionary<string, string>();
            Dictionary<string, string> ListaMidDuplicatiOffline = new Dictionary<string, string>();
            tmpstr = newReader.ReadToEnd();

            newReader.Close();
            tmpstr = tmpstr.Replace("\r", "");
            RowData = tmpstr.Split('\n');
            for (int x = 0; x < RowData.Length - 1; x++)
            {
                string[] rowSplit = RowData[x].Split(',');
                if (rowSplit[2] == "True")
                {
                    if (!ListaMidDuplicatiOnline.ContainsKey(rowSplit[1])) ListaMidDuplicatiOnline.Add(rowSplit[1], rowSplit[0]);
                }
                else
                {
                    ListaMidDuplicatiOffline.Add(rowSplit[0], rowSplit[1]);
                }

            }
            int kount = 0;
            foreach (string mid in ListaMidDuplicatiOffline.Values)
            {
                string idtodelete = ListaMidDuplicatiOffline.Keys.ElementAt(kount);
                kount++;
                string idtoupdate = "";
                ListaMidDuplicatiOnline.TryGetValue(mid, out idtoupdate);
                if (idtoupdate == null)
                {
                    Console.WriteLine(mid);
                }
                else
                {
                    DB_Cleaner_Fun.DeleteMachine(idtodelete, idtoupdate);
                }

            }

            return;
                
        }


        private void WriteList(bool FullList)
        {
            if (File.Exists(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\05022022.txt")) File.Delete(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\05022022.txt");
            StreamWriter newWriter = new StreamWriter(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\05022022.txt", false);
            connection = new MySqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();

            string query = "";
         

            if (FullList)
            {
                query = "select time_stamp,transferred_data from MachinesConnectionTrace where time_stamp like '2022-02-05%';";// "select id,mid,isonline  from  Machines order by mid;";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    newWriter.WriteLine(dataReader["time_stamp"].ToString() + "," + dataReader["transferred_data"].ToString());
                }
                dataReader.Close();
            }
            else
            {

                query = "select distinct(mid)  from  Machines;";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    newWriter.WriteLine(dataReader["mid"].ToString());
                }
                dataReader.Close();
            }
 
           
            newWriter.Close();



        }

        public  bool IsNumeric(string strText)
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
            MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            string query = "";

            if (connection.State == ConnectionState.Closed) connection.Open();

            if (connection.State == ConnectionState.Open)
            {
                query = "select id,IsOnline from  Machines where mid like 'Duplicato%';";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                //string[] midlist;
                Dictionary<string, string> listaID_DUplicati = new Dictionary<string, string>();
                List<string> listaID_DUplicatiDaAggiornare = new List<string>();

                while (dataReader.Read())
                {
                    
                    listaID_DUplicati.Add(dataReader["id"].ToString(), dataReader["IsOnline"].ToString());
                    //            midlist.Add(dataReader["id"].ToString(), dataReader["time_creation"].ToString());

                }
                //idlist.Add("144");
                dataReader.Close();
                int counter = 0;

                Dictionary<string, string> TrData_MID = new Dictionary<string, string>();
                if (File.Exists(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\MId_duplicati.txt")) File.Delete(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\MId_duplicati.txt");

                StreamWriter newWritew = new StreamWriter(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\MId_duplicati.txt", false);
               
                var statusID = listaID_DUplicati.ToArray();
                foreach (string idtemp in listaID_DUplicati.Keys)
                {
                   
                    dataReader.Close();
                    query = "select transferred_data from  MachinesConnectionTrace  where id_Macchina =" + idtemp + " and transferred_data like '<MID%';";
                    cmd = new MySqlCommand(query, connection);
                    cmd.CommandTimeout = 0;
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        string tdata = dataReader["transferred_data"].ToString();
                        string[] splittdata = tdata.Split('>');
                        splittdata[0] = splittdata[0].Replace("<MID=", "");

                        if (IsNumeric(splittdata[0]))
                        {
                            if (!TrData_MID.Keys.Contains(splittdata[0]))
                            {
                                TrData_MID.Add(splittdata[0],statusID[counter].ToString());
                                newWritew.WriteLine(splittdata[0]);
                            }
                        }
                        
                    }
                    counter++;
                }
                newWritew.Close();
               
                List<string> tmpId_Duplicati = new List<string>();

                foreach (string midtemp in TrData_MID.Keys)
                {
                    newWritew = new StreamWriter(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\NewFile\Id_duplicatiMID_" + midtemp+".txt", false);
                    dataReader.Close();
                    query = "select distinct(id_Macchina) from MachinesConnectionTrace  where transferred_data like '<MID=" + midtemp + "%';";
                    cmd = new MySqlCommand(query, connection);
                    cmd.CommandTimeout = 0;
                    Console.WriteLine(DateTime.Now.ToString());
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Console.WriteLine(DateTime.Now.ToString());
                        tmpId_Duplicati.Add(dataReader["id_Macchina"].ToString());
                        newWritew.WriteLine(dataReader["id_Macchina"].ToString());
                        //Console.WriteLine(tmpId_Duplicati.ToString()); ;
                    }
                    newWritew.Close();
                }
         



                //StreamReader newreader = new StreamReader(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\Id_duplicati.txt", Encoding.UTF8);

                //string tmpstr = newreader.ReadToEnd();
                //newreader.Close();
                //tmpstr = tmpstr.Replace("\r", "");

                string[] idlist = tmpId_Duplicati.ToArray();
                //string[] midlist;
                Dictionary<string, string> midlist = new Dictionary<string, string>();
                string id_online = "";



                int i = 0;
                for (i = 0; i < idlist.Length; i++)
                {
                    dataReader.Close();
                    query = "select id, time_creation from  Machines where IsOnline=1 and id=" + idlist[i] + ";";

                    //Openconnection
                    if (connection.State == ConnectionState.Open)
                    {
                        cmd = new MySqlCommand(query, connection);
                        dataReader = cmd.ExecuteReader();

                        while (dataReader.Read())
                        {
                            id_online = dataReader["id"].ToString();
                            //            midlist.Add(dataReader["id"].ToString(), dataReader["time_creation"].ToString());
                        }
                        //idlist.Add("144");
                        dataReader.Close();

                    }

                }
                for (i = 0; i < idlist.Length; i++)
                {
                    if (id_online != idlist[i])
                    {

                       // DB_Cleaner_Fun.DeleteMachine(idlist[i], id_online);
                        
                        Console.WriteLine(i);

                    }

                }
            }
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
                
                //query = "select * from Machines where IsOnline = 0;";
                //query = "select * from CashTransaction";
                //query = "select * from Machines where IsOnline = 0 order by version;";
               
                //query = " select distinct (id_Macchina) from  MachinesConnectionTrace  where transferred_data like '<MID=00022942>%' ;";
                query = "select id,mid,IsOnline,ip_address from Machines order by mid ASC";
               
                List<string> Midlist = new List<string>();
                List<string> IPlist = new List<string>();
                List<string> IdfromMachines = new List<string>();

                List<string> Versionlist = new List<string>();
                Dictionary<string, string> id_vers_List = new Dictionary<string, string>();
                Dictionary<string, string> id_mid_List = new Dictionary<string, string>();
                MySqlDataReader dataReader;

                if (File.Exists(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\Mid duplicati.txt")) File.Delete(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\Mid duplicati.txt");

                StreamWriter newWriter = new StreamWriter(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\Mid duplicati.txt", false);

                MySqlCommand cmd = new MySqlCommand(query, connection);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    //id_mid_List.Add(dataReader["ODM"].ToString(), dataReader["ID_Machines"].ToString());
                    //id_mid_List.Add(dataReader["id"].ToString(), dataReader["imei"].ToString());
                    //IPlist.Add(dataReader["ip_address"].ToString());
                    newWriter.WriteLine(dataReader["id"].ToString() + "," + dataReader["IsOnline"].ToString() + "," + dataReader["mid"].ToString());
                }
                int i = 0;
                newWriter.Close();
                return;
                foreach (string idtemp in id_mid_List.Keys)
                {
                    string t_mid = "";

                    dataReader.Close();
                    query = "SELECT transferred_data,time_stamp,id_Macchina FROM MachinesConnectionTrace WHERE id_Macchina = '" + idtemp + "';";
                    //query = "delete FROM MachinesConnectionTrace WHERE id = '" + idtemp + "' and transferred_data like '<MID=00007135-865733028324226><VER=110>';";
                    
                    

                    cmd = new MySqlCommand(query, connection);
                    //cmd.ExecuteNonQuery();

                    dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        if (dataReader["transferred_data"].ToString().StartsWith("<MID=") )
                        {
                            if (dataReader["transferred_data"].ToString().StartsWith("<MID=00007135") | dataReader["transferred_data"].ToString().StartsWith("<MID=77770001"))
                            { }
                            else
                            {
                                newWriter.WriteLine(IPlist.ElementAt(i) + " - " + dataReader["id_Macchina"].ToString() + " - " + dataReader["time_stamp"].ToString() + " - " + id_mid_List.Values.ElementAt(i) + " - " + dataReader["transferred_data"].ToString()); 
                            }
                              
                        }
                            
                    }
                    i++;

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
                i = 0;
                

                Console.WriteLine("ID_Machines,CE,Version,time_creation,last_communication");
                foreach(string idtemp in id_mid_List.Keys)
                {
                    string t_mid = "";
                    id_mid_List.TryGetValue(idtemp,out t_mid);
                    newWriter.WriteLine("##################### CE  "+ t_mid);
                    dataReader.Close();
                    query = "SELECT ip_address,transferred_data,time_stamp FROM machinesconnectiontrace WHERE id_Macchina = '" + idtemp +  "';"; 
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

            query = "Delete FROM CashTransaction;";
            newcmd = new MySqlCommand(query, connection);
            newcmd.ExecuteNonQuery();

            query = "Delete from MachinesConnectionTrace;";
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
            query = "select * from  Machines where IsOnline=0 and mid = '00010032;";
            //query = "select * from Machines where IsOnline=0 and  last_communication like '2021-12-19%' and mid like 'Recupero%';";
            //query = "select * from  Machines where imei =865733028324226 and mid like 'Duplicato%';";
            // query = "select * from Machines where mid like '00007135-%'";
            //query = "select * from Machines where IsOnline = 0;";
            //query = "select * from Machines where imei = 869153046561855";
            //query = "SELECT * FROM Machines where mid like '%Duplicato%'";
            //query = "select id from Machines where last_communication Like '2021-11-19%';";
            //query = "SELECT * from Machines where last_communication like '2021-11-10 14%';";
            //query = "SELECT * FROM Machines where mid like '%RecuperoInCorso..%' and IsOnline=0";
            query = "SELECT * FROM Machines";// and IsOnline=0";



            //Dictionary<string, string> IDMachines_MidToRemove = new Dictionary<string, string>();

            //StreamReader newReader = new StreamReader(@"C:\Users\mdifazio.DEDEMPVP\Desktop\temp\Mid duplicati.txt");

            //string tmpstr = newReader.ReadToEnd();

            //newReader.Close();
            //tmpstr = tmpstr.Replace("\r", "");
            //string[] DataMachine = tmpstr.Split('\n');

            //machineInfo = new DataMachines[DataMachine.Length];

            //string midappo = "";
            //for (int x = 0; x < DataMachine.Length - 1; x++)
            //{
            //    string[] infosplit = DataMachine[x].Split(',');

            //    machineInfo[x].Id_machines = infosplit[0];
            //    machineInfo[x].statusonline = infosplit[1];
            //    machineInfo[x].mid = infosplit[2];

            //}
            //string midval = "";
            //for (int x = 0; x < DataMachine.Length - 1; x++)
            //{
            //    string idtoupdate = "";
            //    if (machineInfo[x].statusonline == "True")
            //    {
            //        midval = machineInfo[x].mid;
            //        idtoupdate = machineInfo[x].Id_machines;

            //        for (int k = 0; k < DataMachine.Length - 1; k++)
            //        {
            //            if (machineInfo[k].mid == midval)
            //            {
            //                if (machineInfo[k].Id_machines != idtoupdate)
            //                    DB_Cleaner_Fun.DeleteMachine(machineInfo[k].Id_machines, idtoupdate);
            //            }

            //        }

            //    }
            //}





            List<string> idlist = new List<string>();
            //Openconnection
            if (connection.State == ConnectionState.Open)
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

                string id_to_update = "0";

                foreach (string id in idlist)
                {
                  if (id !=id_to_update)   DB_Cleaner_Fun.DeleteMachine(id, id_to_update);

                }

                connection.Close();

            }
            else
            {
                Console.WriteLine("");
            }
        }
        

           /// 
                 /// </summary>

    
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
            DB_Cleaner_Fun.DeleteMachine(txtIdtoDelete.Text, txtIdtoUpdate.Text);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtIdtoDelete_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

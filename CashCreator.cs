using MySql.Data.MySqlClient;
using System;

namespace TestCasseTLK
{
    public class CashData
    {
        public string CodeMa { get; set; }
        public string OdmTaskPalmare { get; set; }
        public string IdTelemetria { get; set; }
        public string DateB { get; set; }
        public string TipoDa { get; set; }
        public string CanaleGettone { get; set; }
        public string CanaleProve { get; set; }
        public string Ch1 { get; set; }
        public string Qty1 { get; set; }
        public string Ch2 { get; set; }
        public string Qty2 { get; set; }
        public string Ch3 { get; set; }
        public string Qty3 { get; set; }
        public string Ch4 { get; set; }
        public string Qty4 { get; set; }
        public string Ch5 { get; set; }
        public string Qty5 { get; set; }
        public string Ch6 { get; set; }
        public string Qty6 { get; set; }
        public string Ch7 { get; set; }
        public string Qty7 { get; set; }
        public string Ch8 { get; set; }
        public string Qty8 { get; set; }
        public string MdbVal2 { get; set; }
        public string MdbInc2 { get; set; }
        public string MdbTub2 { get; set; }
        public string MdbVal3 { get; set; }
        public string MdbInc3 { get; set; }
        public string MdbTub3 { get; set; }
        public string MdbVal4 { get; set; }
        public string MdbInc4 { get; set; }
        public string MdbTub4 { get; set; }
        public string MdbVal5 { get; set; }
        public string MdbInc5 { get; set; }
        public string MdbTub5 { get; set; }
        public string MdbVal6 { get; set; }
        public string MdbInc6 { get; set; }
        public string MdbTub6 { get; set; }
        public string Cashless { get; set; }
        public string Total { get; set; }
        public string Change { get; set; }
        public string Sales { get; set; }
        public string Consumabile { get; set; }
        public string Vend1Prc { get; set; }
        public string QtyV1 { get; set; }
        public string Vend2Prc { get; set; }
        public string QtyV2 { get; set; }
        public string Ticket { get; set; }
        public string Price { get; set; }
        public string MechValue { get; set; }
        public string Status { get; set; }


    }
    public static class CashCreator
    {
        static string serverip = "10.10.10.71";
        static string database = "listener_DB";
        static string uid = "bot_user";
        static string password = "Qwert@#!99";
        static string connectionString = "server = 10.10.10.71; database = listener_DB; uid = bot_user; pwd = Qwert@#!99;";
        static string transaction_Status = "";

        public static string prepare_data_cash_tablet(string mid,int cashTransactionID,string CasType,string MCT_lastCassPacket,string DataRequest,bool OnlineStatus,string transactionId, ref string strCassPacket,string lastDataCassPresent,ref string strLastCassPacket)
        {
            
            int machine_id = 0;
            try{

                //ricavo l' IdMachines dalla tabella CashTransaction tramite il cashTransactionID   

                MySqlConnection connection;

                connection = new MySqlConnection(connectionString);
                connection.Open();

                string query;
                MySqlCommand newcmd = connection.CreateCommand();
                query = "Select Id, IdMachines from CashTransaction where Id = '" + cashTransactionID + "'";
                MySqlDataReader reader = newcmd.ExecuteReader();
                while (reader.Read())
                {
                    machine_id =Convert.ToInt32(  reader.ToString());
                }

                //#############################################################################################à
                //Carico l'ultimo pacchetto di cassa ricevuto dalla tabella MachinesConnectionTrace


                //selezionare l'ultimo pacchetto di cassa verificando il mid sulla tabella DatiCassa
                string[] splittedLastCashPacketMCT_transaction= new   string[1];
                string[] splittedPreviousCashPacket_DC= new   string[1];
                string[] tmpsplittedCashPacket;
                
               // string strTransferredData="";
                string MCT_PrevCassPacket="";
                


                tmpsplittedCashPacket = MCT_lastCassPacket.Split(',');


               
                int loading_result = 0;
               
                if (!CheckCassPacket(CasType,tmpsplittedCashPacket,MCT_lastCassPacket,ref splittedLastCashPacketMCT_transaction,ref strLastCassPacket))
                {
                    Console.WriteLine(DateTime.Now.ToString("yy/MM/dd,HH:mm:ss") + " wrong cash M1 packet length (!=40)");
                    transaction_Status = "bad cash packet";
                    return "bad cash packet";
                }

                //controllo se è il l`unico pacchetto di cassa mai ricevuto da quella macchina o meno 

                var cmd = new MySqlCommand("SELECT COUNT(Mid) FROM DatiCassa where Mid= '" + mid + "'", connection);
                
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                    
                

                if (count>1)
                {
                    cmd = new MySqlCommand("SELECT COUNT(Mid) FROM DatiCassa where Mid = '" + mid + "' And Status = 'Inviata'");
                    if (Convert.ToInt32(cmd.ExecuteScalar())> 0)
                    {
                        query = "SELECT Id TransferredData FROM DatiCassa where Mid = '" + mid + "' And Status = 'Inviata' order by Id desc" ;
                        reader = newcmd.ExecuteReader();
                        while (reader.Read())
                        {
                            MCT_PrevCassPacket = reader.ToString();
                        }


                        tmpsplittedCashPacket= MCT_PrevCassPacket.Split(',');
                         string newstr="";   
                        if(CheckCassPacket(CasType,tmpsplittedCashPacket,MCT_lastCassPacket,ref splittedPreviousCashPacket_DC,ref newstr))
                        {
                            loading_result = buildPacket_better(CasType,splittedLastCashPacketMCT_transaction, splittedPreviousCashPacket_DC,OnlineStatus,transactionId,ref strCassPacket,lastDataCassPresent);
                        }
                        else
                        {
                            loading_result=1;
                        }
                    }
                    else
                    {
                        loading_result = buildPacket_better(CasType,splittedLastCashPacketMCT_transaction,OnlineStatus,transactionId,ref strCassPacket,lastDataCassPresent);
                        
                    }
                }
                else
                {
                    loading_result = buildPacket_better(CasType,splittedLastCashPacketMCT_transaction,OnlineStatus,transactionId,ref strCassPacket,lastDataCassPresent);
                
                }
                    
                if(loading_result == 0)
                {
                    transaction_Status = "Inviata";
                }
                else
                {
                    transaction_Status = "Errore =(";
                }   
        
                return transaction_Status;

            }catch (Exception e)
            {
                Console.WriteLine(DateTime.Now.ToString("yy/MM/dd,HH:mm:ss") + " prepare_data_cash_tablet error " + e.Message);
                Console.WriteLine(DateTime.Now.ToString("yy/MM/dd,HH:mm:ss") + " prepare_data_cash_tablet error " + e.StackTrace);
                return "";
            }
            
        }

        static bool CheckCassPacket(string CasType, string[] packet,string originalPacket,ref string[] PacketOK,ref string stringPacketOK )
        {
            try
            {
                if (CasType.Contains("M1"))
                {
                    if(packet.Length > 40)
                    {
                        packet=originalPacket.Split('<');
                        packet[1]="<"+packet[1];
                        stringPacketOK=packet[1];
                        PacketOK=packet[1].Split(',');
                    }
                    else
                    {
                        PacketOK=packet;
                        stringPacketOK=originalPacket;
                    }
                    if(PacketOK.Length != 40) return false;
                }

                if (CasType.Contains("M3"))
                {
                    if(packet.Length > 47)
                    {
                        packet=originalPacket.Split('<');
                        packet[1]="<"+packet[1];
                        stringPacketOK=packet[1];
                        PacketOK=packet[1].Split(',');
                    }
                    else
                    {
                        PacketOK=packet;
                        stringPacketOK=originalPacket;
                    }
                    if(PacketOK.Length != 47) return false;
                   
                }

                if (CasType.Contains("M5"))
                {
                    if(packet.Length > 55)
                    {
                        packet=originalPacket.Split('<');
                        packet[1]="<"+packet[1];
                        stringPacketOK=packet[1];
                        PacketOK=packet[1].Split(',');
                    }
                    else
                    {
                        PacketOK=packet;
                        stringPacketOK=originalPacket;
                    }
                    if(PacketOK.Length != 55) return false;
                   
                }

                if (CasType.Contains("W5"))
                {
                    if(packet.Length > 41)
                    {
                        packet=originalPacket.Split('<');
                        packet[1]="<"+packet[1];
                        stringPacketOK=packet[1];
                        PacketOK=packet[1].Split(',');
                    }
                    else
                    {
                        PacketOK=packet;
                        stringPacketOK=originalPacket;
                    }
                    if(PacketOK.Length != 41) return false;
                
                }

                if (CasType.Contains("I1"))
                {
                    if(packet.Length > 47)
                    {
                        packet=originalPacket.Split('<');
                        packet[1]="<"+packet[1];
                        stringPacketOK=packet[1];
                        PacketOK=packet[1].Split(',');
                    }
                    else
                    {
                        PacketOK=packet;
                        stringPacketOK=originalPacket;
                    }
                    if (PacketOK.Length != 47) return false;
                }
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception CheckCassPacket: " + e.StackTrace);
                return false;
            }
            
        }

        public static int buildPacket_better(string CasType,string[] theOnlyCashTransaction,bool OnlineStatus ,string transactionId, ref string strCassPacket,string lastDataCassPresent)
        {
            try{
                string[] splittedCashPacket = theOnlyCashTransaction;//.Split(',');
              
                if (CasType== "M1")
                {
                    if (splittedCashPacket[24]=="")splittedCashPacket[24]="0";
                    if (splittedCashPacket[23]=="")splittedCashPacket[23]="0";

                    CashData dataCass= new CashData();
                    {
                        dataCass.CodeMa=splittedCashPacket[1];//.ToString();
                        dataCass.OdmTaskPalmare = transactionId;
                        dataCass.IdTelemetria="0";
                        dataCass.DateB = DateTime.Now.ToString();
                        dataCass.TipoDa = "1";
                        dataCass.CanaleGettone = "1";
                        dataCass.CanaleProve = "8";
                        dataCass.Ch1 = ((float)Convert.ToInt32( splittedCashPacket[4])).ToString();
                        dataCass.Qty1 = Convert.ToInt32(splittedCashPacket[5]).ToString();
                        dataCass.Ch2 = ((float)Convert.ToInt32( splittedCashPacket[6])).ToString();
                        dataCass.Qty2 = Convert.ToInt32(splittedCashPacket[7]).ToString();
                        dataCass.Ch3 = ((float)Convert.ToInt32( splittedCashPacket[8])).ToString();
                        dataCass.Qty3 = Convert.ToInt32(splittedCashPacket[9]).ToString();
                        dataCass.Ch4 = ((float)Convert.ToInt32( splittedCashPacket[10])).ToString();
                        dataCass.Qty4 = Convert.ToInt32(splittedCashPacket[11]).ToString();
                        dataCass.Ch5 = ((float)Convert.ToInt32( splittedCashPacket[12])).ToString();
                        dataCass.Qty5 = Convert.ToInt32(splittedCashPacket[13]).ToString();
                        dataCass.Ch6 = ((float)Convert.ToInt32( splittedCashPacket[14])).ToString();
                        dataCass.Qty6 = Convert.ToInt32(splittedCashPacket[15]).ToString();
                        dataCass.Ch7 = ((float)Convert.ToInt32( splittedCashPacket[16])).ToString();
                        dataCass.Qty7 = Convert.ToInt32(splittedCashPacket[17]).ToString();
                        dataCass.Ch8 = ((float)Convert.ToInt32( splittedCashPacket[18])).ToString();
                        dataCass.Qty8 = Convert.ToInt32(splittedCashPacket[19]).ToString();
                        dataCass.MdbVal2 = "0";
                        dataCass.MdbInc2 = "0";
                        dataCass.MdbTub2 = "0";
                        dataCass.MdbVal3 = "0";
                        dataCass.MdbInc3 = "0";
                        dataCass.MdbTub3 = "0";
                        dataCass.MdbVal4 = "0";
                        dataCass.MdbInc4 = "0";
                        dataCass.MdbTub4 = "0";
                        dataCass.MdbVal5 = "0";
                        dataCass.MdbInc5 = "0";
                        dataCass.MdbTub5 = "0";
                        dataCass.MdbVal6 = "0";
                        dataCass.MdbInc6 = "0";
                        dataCass.MdbTub6 = "0";
                        dataCass.Cashless = ((float)Convert.ToInt32( splittedCashPacket[24])).ToString();
                        dataCass.Total = ((float)Convert.ToInt32( splittedCashPacket[20])).ToString();
                        dataCass.Change  = "0";
                        dataCass.Sales = splittedCashPacket[21];
                        dataCass.Consumabile = splittedCashPacket[23];
                        dataCass.Vend1Prc = "0";
                        dataCass.QtyV1 = "0";
                        dataCass.Vend2Prc = "0";
                        dataCass.QtyV2 = "0";
                        dataCass.Ticket = splittedCashPacket[22];
                        dataCass.Price = ((float)Convert.ToInt32( splittedCashPacket[3])).ToString();
                        dataCass.MechValue ="0"; 
                        dataCass.Status =OnlineStatus.ToString();

                    }
                    strCassPacket= CashDataToString(dataCass);
                  
                }                
                else if (CasType=="M3")
                {
                    string tmpSales =(Convert.ToInt32( splittedCashPacket[8])+Convert.ToInt32( splittedCashPacket[9])).ToString();

                    CashData dataCass= new CashData();
                    {			
                        dataCass.CodeMa=splittedCashPacket[1];//.ToString();
                        dataCass.OdmTaskPalmare = transactionId;
                        dataCass.IdTelemetria="0";                
                        dataCass.DateB = DateTime.Now.ToString();
                        dataCass.TipoDa ="205";                   
                        dataCass.CanaleGettone ="1";                 
                        dataCass.CanaleProve ="9";                    
                        dataCass.Ch1=((float)Convert.ToInt32( splittedCashPacket[10])).ToString();                  
                        dataCass.Qty1 =splittedCashPacket[11];                 
                        dataCass.Ch2=((float)Convert.ToInt32( splittedCashPacket[12])).ToString();                  
                        dataCass.Qty2 =splittedCashPacket[13];                 
                        dataCass.Ch3=((float)Convert.ToInt32( splittedCashPacket[14])).ToString();                  
                        dataCass.Qty3 =splittedCashPacket[15];                 
                        dataCass.Ch4=((float)Convert.ToInt32( splittedCashPacket[16])).ToString();                  
                        dataCass.Qty4 =splittedCashPacket[17];                 
                        dataCass.Ch5=((float)Convert.ToInt32( splittedCashPacket[18])).ToString();                  
                        dataCass.Qty5 =splittedCashPacket[19];                 
                        dataCass.Ch6=((float)Convert.ToInt32( splittedCashPacket[20])).ToString();                  
                        dataCass.Qty6 =splittedCashPacket[21];                 
                        dataCass.Ch7=((float)Convert.ToInt32( splittedCashPacket[22])).ToString();                  
                        dataCass.Qty7 =splittedCashPacket[23];                 
                        dataCass.Ch8=((float)Convert.ToInt32( splittedCashPacket[24])).ToString();                  
                        dataCass.Qty8 =splittedCashPacket[25];                 
                        dataCass.MdbVal2 ="0";                    
                        dataCass.MdbInc2 ="0";                    
                        dataCass.MdbTub2 ="0";                    
                        dataCass.MdbVal3 ="0";                    
                        dataCass.MdbInc3 ="0";                    
                        dataCass.MdbTub3 ="0";                    
                        dataCass.MdbVal4 ="0";                    
                        dataCass.MdbInc4 ="0";                    
                        dataCass.MdbTub4 ="0";                    
                        dataCass.MdbVal5 ="0";                    
                        dataCass.MdbInc5 ="0";                    
                        dataCass.MdbTub5 ="0";                    
                        dataCass.MdbVal6 ="0";                    
                        dataCass.MdbInc6 ="0";                    
                        dataCass.MdbTub6 ="0";                    
                        dataCass.Cashless ="0";
                        dataCass.Total=((float)Convert.ToInt32( splittedCashPacket[26])).ToString();                    
                        dataCass.Change  ="0";                    
                        dataCass.Sales = tmpSales;
                        dataCass.Consumabile ="0";                    
                        dataCass.Vend1Prc=((float)Convert.ToInt32( splittedCashPacket[4])).ToString();                   
                        dataCass.QtyV1 = splittedCashPacket[5];                    
                        dataCass.Vend2Prc=((float)Convert.ToInt32( splittedCashPacket[6])).ToString();                  
                        dataCass.QtyV2 = splittedCashPacket[7];                    
                        dataCass.Ticket ="0";               
                        dataCass.Price ="0";                  
                        dataCass.MechValue ="0";                  
                        dataCass.Status =OnlineStatus.ToString();;                   
                        
                    }
                     strCassPacket= CashDataToString(dataCass);
                   
                }
                else if (CasType=="M5")
                {
                    string tmpSales=(Convert.ToInt32( splittedCashPacket[7])+Convert.ToInt32( splittedCashPacket[8])).ToString(); 
                    CashData dataCass= new CashData();
                    {
                        dataCass.CodeMa=splittedCashPacket[1];//.ToString();
                        dataCass.OdmTaskPalmare = transactionId;
                        dataCass.IdTelemetria="0";
                        dataCass.DateB = DateTime.Now.ToString();
                        dataCass.TipoDa="8";
                        dataCass.CanaleGettone="1";
                        dataCass.CanaleProve="9";
                        dataCass.Ch1=((float)Convert.ToInt32( splittedCashPacket[12])).ToString();          
                        dataCass.Qty1=splittedCashPacket[13];         
                        dataCass.Ch2=((float)Convert.ToInt32( splittedCashPacket[14])).ToString();          
                        dataCass.Qty2=splittedCashPacket[15];         
                        dataCass.Ch3=((float)Convert.ToInt32( splittedCashPacket[16])).ToString();          
                        dataCass.Qty3=splittedCashPacket[17];         
                        dataCass.Ch4=((float)Convert.ToInt32( splittedCashPacket[18])).ToString();          
                        dataCass.Qty4=splittedCashPacket[19];         
                        dataCass.Ch5=((float)Convert.ToInt32( splittedCashPacket[20])).ToString();          
                        dataCass.Qty5=splittedCashPacket[21];         
                        dataCass.Ch6=((float)Convert.ToInt32( splittedCashPacket[22])).ToString();          
                        dataCass.Qty6=splittedCashPacket[23];         
                        dataCass.Ch7=((float)Convert.ToInt32( splittedCashPacket[24])).ToString();          
                        dataCass.Qty7=splittedCashPacket[25];         
                        dataCass.Ch8=((float)Convert.ToInt32( splittedCashPacket[26])).ToString();          
                        dataCass.Qty8=splittedCashPacket[27];         
                        dataCass.MdbVal2="0";            
                        dataCass.MdbInc2="0";            
                        dataCass.MdbTub2="0";            
                        dataCass.MdbVal3="0";            
                        dataCass.MdbInc3="0";            
                        dataCass.MdbTub3="0";            
                        dataCass.MdbVal4="0";            
                        dataCass.MdbInc4="0";            
                        dataCass.MdbTub4="0";            
                        dataCass.MdbVal5="0";            
                        dataCass.MdbInc5="0";            
                        dataCass.MdbTub5="0";            
                        dataCass.MdbVal6="0";            
                        dataCass.MdbInc6="0";            
                        dataCass.MdbTub6="0";            
                        dataCass.Cashless=((float)Convert.ToInt32( splittedCashPacket[29])).ToString();         
                        dataCass.Total=((float)Convert.ToInt32( splittedCashPacket[28])).ToString();            
                        dataCass.Change ="0";            
                        dataCass.Sales= tmpSales;         
                        dataCass.Consumabile="0";            
                        dataCass.Vend1Prc=((float)Convert.ToInt32( splittedCashPacket[4])).ToString();           
                        dataCass.QtyV1= splittedCashPacket[5];            
                        dataCass.Vend2Prc=((float)Convert.ToInt32( splittedCashPacket[6])).ToString();          
                        dataCass.QtyV2= splittedCashPacket[7];            
                        dataCass.Ticket="0";//Convert.ToInt32( splittedCashPacket[22]),          
                        dataCass.Price="0";          
                        dataCass.MechValue="0";          
                        dataCass.Status=OnlineStatus.ToString();      
	 
                     }
                      strCassPacket= CashDataToString(dataCass);

                 }
                else if (CasType=="W5")
                {
                    string tmpQTy8=(Convert.ToInt32( splittedCashPacket[10])+Convert.ToInt32( splittedCashPacket[1])+Convert.ToInt32( splittedCashPacket[22])+Convert.ToInt32( splittedCashPacket[23])).ToString();
                    
                    CashData dataCass= new CashData();
                    {
                        dataCass.CodeMa=splittedCashPacket[4];//.ToString();
                        dataCass.OdmTaskPalmare = transactionId;
                        dataCass.IdTelemetria="0";
                        dataCass.DateB = DateTime.Now.ToString();
                        dataCass.TipoDa="8";
                        dataCass.CanaleGettone="1";
                        dataCass.CanaleProve="9";
                        dataCass.Ch1=((float)Convert.ToInt32( splittedCashPacket[8])).ToString();          
                        dataCass.Qty1=splittedCashPacket[9];         
                        dataCass.Ch2=((float)Convert.ToInt32( splittedCashPacket[10])).ToString();          
                        dataCass.Qty2=splittedCashPacket[11];         
                        dataCass.Ch3=((float)Convert.ToInt32( splittedCashPacket[12])).ToString();          
                        dataCass.Qty3=splittedCashPacket[13];         
                        dataCass.Ch4=((float)Convert.ToInt32( splittedCashPacket[14])).ToString();          
                        dataCass.Qty4=splittedCashPacket[15];         
                        dataCass.Ch5=((float)Convert.ToInt32( splittedCashPacket[16])).ToString();          
                        dataCass.Qty5=splittedCashPacket[17];         
                        dataCass.Ch6=((float)Convert.ToInt32( splittedCashPacket[18])).ToString();          
                        dataCass.Qty6=splittedCashPacket[19];         
                        dataCass.Ch7=((float)Convert.ToInt32( splittedCashPacket[20])).ToString();          
                        dataCass.Qty7=splittedCashPacket[21];         
                        dataCass.Ch8=((float)Convert.ToInt32( splittedCashPacket[22])).ToString();          
                        dataCass.Qty8=tmpQTy8;         
                        dataCass.MdbVal2="0";            
                        dataCass.MdbInc2="0";            
                        dataCass.MdbTub2="0";            
                        dataCass.MdbVal3="0";            
                        dataCass.MdbInc3="0";            
                        dataCass.MdbTub3="0";            
                        dataCass.MdbVal4="0";            
                        dataCass.MdbInc4="0";            
                        dataCass.MdbTub4="0";            
                        dataCass.MdbVal5="0";            
                        dataCass.MdbInc5="0";            
                        dataCass.MdbTub5="0";            
                        dataCass.MdbVal6="0";            
                        dataCass.MdbInc6="0";            
                        dataCass.MdbTub6="0";            
                        dataCass.Cashless="0";
                        dataCass.Total=((float)Convert.ToInt32( splittedCashPacket[24])).ToString();            
                        dataCass.Change ="0";            
                        dataCass.Sales=Convert.ToInt32(splittedCashPacket[25]).ToString();         
                        dataCass.Consumabile="0";            
                        dataCass.Vend1Prc="0";          
                        dataCass.QtyV1= "0";            
                        dataCass.Vend2Prc="0";       
                        dataCass.QtyV2="0";
                        dataCass.Ticket=Convert.ToInt32( splittedCashPacket[26]).ToString();
                        dataCass.Price=((float)Convert.ToInt32( splittedCashPacket[7])).ToString();                
                        dataCass.MechValue="0";          
                        dataCass.Status=OnlineStatus.ToString();      
	 
                     }
                      strCassPacket= CashDataToString(dataCass);
                }
                else if (CasType== "I1" | CasType== "I2")
                {
                    if (splittedCashPacket[24]=="")splittedCashPacket[24]="0";
                    CashData dataCass= new CashData();
                    {
                        dataCass.CodeMa=splittedCashPacket[1];
                        dataCass.OdmTaskPalmare = transactionId;
                        dataCass.IdTelemetria="0";
                        dataCass.DateB = DateTime.Now.ToString();
                        dataCass.TipoDa = "1";
                        dataCass.CanaleGettone = "8";
                        dataCass.CanaleProve = "9";
                        dataCass.Ch1 = ((float)Convert.ToInt32( splittedCashPacket[4])).ToString();
                        dataCass.Qty1 = Convert.ToInt32(splittedCashPacket[5]).ToString();
                        dataCass.Ch2 = ((float)Convert.ToInt32( splittedCashPacket[6])).ToString();
                        dataCass.Qty2 = Convert.ToInt32(splittedCashPacket[7]).ToString();
                        dataCass.Ch3 = ((float)Convert.ToInt32( splittedCashPacket[8])).ToString();
                        dataCass.Qty3 = Convert.ToInt32(splittedCashPacket[9]).ToString();
                        dataCass.Ch4 = ((float)Convert.ToInt32( splittedCashPacket[10])).ToString();
                        dataCass.Qty4 = Convert.ToInt32(splittedCashPacket[11]).ToString();
                        dataCass.Ch5 = ((float)Convert.ToInt32( splittedCashPacket[12])).ToString();
                        dataCass.Qty5 = Convert.ToInt32(splittedCashPacket[13]).ToString();
                        dataCass.Ch6 = ((float)Convert.ToInt32( splittedCashPacket[14])).ToString();
                        dataCass.Qty6 = Convert.ToInt32(splittedCashPacket[15]).ToString();
                        dataCass.Ch7 = "0";
                        dataCass.Qty7 = "0";
                        dataCass.Ch8 = "0";
                        dataCass.Qty8 = "0";
                        dataCass.MdbVal2 = ((float)Convert.ToInt32( splittedCashPacket[20])).ToString();
                        dataCass.MdbInc2 = "0";
                        dataCass.MdbTub2 =  Convert.ToInt32(splittedCashPacket[21]).ToString();
                        dataCass.MdbVal3 = ((float)Convert.ToInt32( splittedCashPacket[22])).ToString();
                        dataCass.MdbInc3 = "0";
                        dataCass.MdbTub3 =  Convert.ToInt32(splittedCashPacket[23]).ToString();
                        dataCass.MdbVal4 = ((float)Convert.ToInt32( splittedCashPacket[24])).ToString();
                        dataCass.MdbInc4 = "0";
                        dataCass.MdbTub4 = Convert.ToInt32(splittedCashPacket[25]).ToString();
                        dataCass.MdbVal5 = "0";
                        dataCass.MdbInc5 = "0";
                        dataCass.MdbTub5 = "0";
                        dataCass.MdbVal6 = "0";
                        dataCass.MdbInc6 = "0";
                        dataCass.MdbTub6 = "0";
                        dataCass.Cashless = ((float)Convert.ToInt32( splittedCashPacket[33])).ToString();
                        dataCass.Total = ((float)Convert.ToInt32( splittedCashPacket[28])).ToString();
                        dataCass.Change  = ((float)Convert.ToInt32( splittedCashPacket[29])).ToString();
                        dataCass.Sales = splittedCashPacket[31];
                        dataCass.Consumabile = "0";
                        dataCass.Vend1Prc = "0";
                        dataCass.QtyV1 = "0";
                        dataCass.Vend2Prc = "0";
                        dataCass.QtyV2 = "0";
                        dataCass.Ticket =  "0";
                        dataCass.Price = ((float)Convert.ToInt32( splittedCashPacket[3])).ToString();
                        dataCass.MechValue ="0"; 
                        dataCass.Status =OnlineStatus.ToString();

                    }
                    strCassPacket= CashDataToString(dataCass);
                  
                }                

            }
            catch(Exception e)
            {
                
                resetCashValue(transactionId, ref strCassPacket,lastDataCassPresent);
                Console.WriteLine("Exception buildPacket_better: " + e.StackTrace);
                return 1;
            }
            return 0;
        }
        
        public static int buildPacket_better(string CasType,string[] LastCashTransaction, string[] previousTransaction,bool OnlineStatus,string transactionId,ref string strCassPacket,string lastDataCassPresent)
        {
            try
            {
                string[] splittedCashPacket = LastCashTransaction;//.Split(',');
                string[] splittedCashPacket_previous=previousTransaction;//;.Split(',');

                if (CasType=="M1")  
                {
                    if (splittedCashPacket[24]=="")splittedCashPacket[24]="0";
                    if (splittedCashPacket_previous[24]=="")splittedCashPacket_previous[24]="0";
                    if (splittedCashPacket[23]=="")splittedCashPacket[23]="0";
                    if (splittedCashPacket_previous[23]=="")splittedCashPacket_previous[23]="0";


                        CashData dataCass= new CashData();
                        {
                            
                            dataCass.CodeMa=splittedCashPacket[1];//.ToString();
                            dataCass.OdmTaskPalmare = transactionId;
                            dataCass.IdTelemetria = "0";
                            dataCass.DateB = DateTime.Now.ToString();
                            dataCass.TipoDa= "1";
                            dataCass.CanaleGettone= "1";
                            dataCass.CanaleProve= "8";
                            dataCass.Ch1=((float)Convert.ToInt32( splittedCashPacket[4])).ToString();
                            dataCass.Qty1=(Convert.ToInt32(splittedCashPacket[5]) - Convert.ToInt32(splittedCashPacket_previous[5])).ToString();
                            dataCass.Ch2=((float)Convert.ToInt32( splittedCashPacket[6])).ToString();
                            dataCass.Qty2=(Convert.ToInt32(splittedCashPacket[7])- Convert.ToInt32(splittedCashPacket_previous[7])).ToString();
                            dataCass.Ch3=((float)Convert.ToInt32( splittedCashPacket[8])).ToString();
                            dataCass.Qty3=(Convert.ToInt32(splittedCashPacket[9])- Convert.ToInt32(splittedCashPacket_previous[9])).ToString();
                            dataCass.Ch4=((float)Convert.ToInt32( splittedCashPacket[10])).ToString();
                            dataCass.Qty4=(Convert.ToInt32(splittedCashPacket[11])- Convert.ToInt32(splittedCashPacket_previous[11])).ToString();
                            dataCass.Ch5=((float)Convert.ToInt32( splittedCashPacket[12])).ToString();
                            dataCass.Qty5=(Convert.ToInt32(splittedCashPacket[13])- Convert.ToInt32(splittedCashPacket_previous[13])).ToString();
                            dataCass.Ch6=((float)Convert.ToInt32( splittedCashPacket[14])).ToString();
                            dataCass.Qty6=(Convert.ToInt32(splittedCashPacket[15])- Convert.ToInt32(splittedCashPacket_previous[15])).ToString();
                            dataCass.Ch7=((float)Convert.ToInt32( splittedCashPacket[16])).ToString();
                            dataCass.Qty7=(Convert.ToInt32(splittedCashPacket[17])- Convert.ToInt32(splittedCashPacket_previous[17])).ToString();
                            dataCass.Ch8=((float)Convert.ToInt32( splittedCashPacket[18])).ToString();
                            dataCass.Qty8=(Convert.ToInt32(splittedCashPacket[19])- Convert.ToInt32(splittedCashPacket_previous[19])).ToString();
                            dataCass.MdbVal2="0";
                            dataCass.MdbInc2="0";
                            dataCass.MdbTub2="0";
                            dataCass.MdbVal3="0";
                            dataCass.MdbInc3="0";
                            dataCass.MdbTub3="0";
                            dataCass.MdbVal4="0";
                            dataCass.MdbInc4="0";
                            dataCass.MdbTub4="0";
                            dataCass.MdbVal5="0";
                            dataCass.MdbInc5="0";
                            dataCass.MdbTub5="0";
                            dataCass.MdbVal6="0";
                            dataCass.MdbInc6="0";
                            dataCass.MdbTub6="0";
                            dataCass.Cashless=((float)Convert.ToInt32( splittedCashPacket[24])- (float)Convert.ToInt32(splittedCashPacket_previous[24])).ToString();
                            dataCass.Total=((float)Convert.ToInt32( splittedCashPacket[20]) - (float)Convert.ToInt32(splittedCashPacket_previous[20])).ToString();
                            dataCass.Change ="0";
                            dataCass.Sales=(Convert.ToInt32( splittedCashPacket[21]) - Convert.ToInt32(splittedCashPacket_previous[21])).ToString();
                            dataCass.Consumabile=(Convert.ToInt32( splittedCashPacket[23]) - Convert.ToInt32(splittedCashPacket_previous[23])).ToString();
                            dataCass.Vend1Prc="0";
                            dataCass.QtyV1="0";
                            dataCass.Vend2Prc="0";
                            dataCass.QtyV2="0";
                            dataCass.Ticket=(Convert.ToInt32( splittedCashPacket[22]) - Convert.ToInt32(splittedCashPacket_previous[22])).ToString();
                            dataCass.Price=((float)Convert.ToInt32( splittedCashPacket[3])).ToString();
                            dataCass.MechValue="0";
                            dataCass.Status=OnlineStatus.ToString();

                        }
                        strCassPacket= CashDataToString(dataCass);

                }
                else if (CasType=="M3")
                {
                    int tmpSales =Convert.ToInt32( splittedCashPacket[8])+Convert.ToInt32( splittedCashPacket[9]);
                    int tmpPrevSales =Convert.ToInt32(splittedCashPacket_previous[8])+Convert.ToInt32(splittedCashPacket_previous[9]);

                    CashData dataCass= new CashData();
                    {														
                        dataCass.CodeMa=splittedCashPacket[1];//.ToString();
                        dataCass.OdmTaskPalmare = transactionId;
                        dataCass.IdTelemetria="0";
                        dataCass.DateB = DateTime.Now.ToString();
                        dataCass.TipoDa=splittedCashPacket[3];
                        dataCass.CanaleGettone="1";
                        dataCass.CanaleProve="8";         
                        dataCass.Ch1=((float)Convert.ToInt32( splittedCashPacket[10])).ToString();
                        dataCass.Qty1=(Convert.ToInt32(splittedCashPacket[11]) - Convert.ToInt32(splittedCashPacket_previous[11])).ToString(); 
                        dataCass.Ch2=((float)Convert.ToInt32( splittedCashPacket[12])).ToString(); 
                        dataCass.Qty2=(Convert.ToInt32(splittedCashPacket[13])- Convert.ToInt32(splittedCashPacket_previous[13])).ToString();  
                        dataCass.Ch3=((float)Convert.ToInt32( splittedCashPacket[14])).ToString(); 
                        dataCass.Qty3=(Convert.ToInt32(splittedCashPacket[15])- Convert.ToInt32(splittedCashPacket_previous[15])).ToString();  
                        dataCass.Ch4=((float)Convert.ToInt32( splittedCashPacket[16])).ToString(); 
                        dataCass.Qty4=(Convert.ToInt32(splittedCashPacket[17])- Convert.ToInt32(splittedCashPacket_previous[17])).ToString();  
                        dataCass.Ch5=((float)Convert.ToInt32( splittedCashPacket[18])).ToString(); 
                        dataCass.Qty5=(Convert.ToInt32(splittedCashPacket[19])- Convert.ToInt32(splittedCashPacket_previous[19])).ToString();  
                        dataCass.Ch6=((float)Convert.ToInt32( splittedCashPacket[20])).ToString(); 
                        dataCass.Qty6=(Convert.ToInt32(splittedCashPacket[21])- Convert.ToInt32(splittedCashPacket_previous[21])).ToString();  
                        dataCass.Ch7=((float)Convert.ToInt32( splittedCashPacket[22])).ToString(); 
                        dataCass.Qty7=(Convert.ToInt32(splittedCashPacket[23])- Convert.ToInt32(splittedCashPacket_previous[23])).ToString();  
                        dataCass.Ch8=((float)Convert.ToInt32( splittedCashPacket[24])).ToString(); 
                        dataCass.Qty8=(Convert.ToInt32(splittedCashPacket[25])- Convert.ToInt32(splittedCashPacket_previous[25])).ToString();  
                        dataCass.MdbVal2="0";           
                        dataCass.MdbInc2="0";           
                        dataCass.MdbTub2="0";           
                        dataCass.MdbVal3="0";           
                        dataCass.MdbInc3="0";           
                        dataCass.MdbTub3="0";           
                        dataCass.MdbVal4="0";           
                        dataCass.MdbInc4="0";           
                        dataCass.MdbTub4="0";           
                        dataCass.MdbVal5="0";           
                        dataCass.MdbInc5="0";           
                        dataCass.MdbTub5="0";           
                        dataCass.MdbVal6="0";           
                        dataCass.MdbInc6="0";           
                        dataCass.MdbTub6="0";           
                        dataCass.Cashless="0";          
                        dataCass.Total=((float)Convert.ToInt32( splittedCashPacket[28]) - (float)Convert.ToInt32(splittedCashPacket_previous[28])).ToString(); 
                        dataCass.Change ="0";           
                        dataCass.Sales=(tmpSales-tmpPrevSales).ToString();         
                        dataCass.Consumabile="0";           
                        dataCass.Vend1Prc =((float)Convert.ToInt32( splittedCashPacket[4]) - (float)Convert.ToInt32(splittedCashPacket_previous[4])).ToString();          
                        dataCass.QtyV1=(Convert.ToInt32( splittedCashPacket[5])-Convert.ToInt32(splittedCashPacket_previous[5])).ToString(); 
                        dataCass.Vend2Prc=((float)Convert.ToInt32( splittedCashPacket[6])-(float)Convert.ToInt32(splittedCashPacket_previous[6])).ToString(); 
                        dataCass.QtyV2=(Convert.ToInt32( splittedCashPacket[7])-Convert.ToInt32(splittedCashPacket_previous[7])).ToString(); 
                        dataCass.Ticket="0";        
                        dataCass.Price="0";         
                        dataCass.MechValue="0";         
                        dataCass.Status=OnlineStatus.ToString();
                    }
                     strCassPacket= CashDataToString(dataCass);

															
                }
                else if (CasType=="M5")
                {
                    int tmpSales =Convert.ToInt32( splittedCashPacket[7])+Convert.ToInt32( splittedCashPacket[8]);
                    int tmpPrevSales =Convert.ToInt32(splittedCashPacket_previous[7])+Convert.ToInt32(splittedCashPacket_previous[8]);

                    CashData dataCass= new CashData();
                    {
                        dataCass.CodeMa=splittedCashPacket[1];//.ToString();
                        dataCass.OdmTaskPalmare = transactionId;
                        dataCass.IdTelemetria="0";
                        dataCass.DateB = DateTime.Now.ToString();
                        dataCass.TipoDa=splittedCashPacket[3];
                        dataCass.CanaleGettone="1";    
                        dataCass.CanaleProve="8"; 
                        dataCass.Ch1=((float)Convert.ToInt32( splittedCashPacket[12])).ToString();    
                        dataCass.Qty1=(Convert.ToInt32(splittedCashPacket[13]) - Convert.ToInt32(splittedCashPacket_previous[13])).ToString();     
                        dataCass.Ch2=((float)Convert.ToInt32( splittedCashPacket[14])).ToString();     
                        dataCass.Qty2=(Convert.ToInt32(splittedCashPacket[15])- Convert.ToInt32(splittedCashPacket_previous[15])).ToString(); 
                        dataCass.Ch3=((float)Convert.ToInt32( splittedCashPacket[16])).ToString();     
                        dataCass.Qty3=(Convert.ToInt32(splittedCashPacket[17])- Convert.ToInt32(splittedCashPacket_previous[17])).ToString(); 
                        dataCass.Ch4=((float)Convert.ToInt32( splittedCashPacket[18])).ToString();     
                        dataCass.Qty4=(Convert.ToInt32(splittedCashPacket[19])- Convert.ToInt32(splittedCashPacket_previous[19])).ToString(); 
                        dataCass.Ch5=((float)Convert.ToInt32( splittedCashPacket[20])).ToString();     
                        dataCass.Qty5=(Convert.ToInt32(splittedCashPacket[21])- Convert.ToInt32(splittedCashPacket_previous[21])).ToString(); 
                        dataCass.Ch6=((float)Convert.ToInt32( splittedCashPacket[22])).ToString();     
                        dataCass.Qty6=(Convert.ToInt32(splittedCashPacket[23])- Convert.ToInt32(splittedCashPacket_previous[23])).ToString(); 
                        dataCass.Ch7=((float)Convert.ToInt32( splittedCashPacket[24])).ToString();     
                        dataCass.Qty7=(Convert.ToInt32(splittedCashPacket[25])- Convert.ToInt32(splittedCashPacket_previous[25])).ToString(); 
                        dataCass.Ch8=((float)Convert.ToInt32( splittedCashPacket[26])).ToString();     
                        dataCass.Qty8=(Convert.ToInt32(splittedCashPacket[27])- Convert.ToInt32(splittedCashPacket_previous[27])).ToString(); 
                        dataCass.MdbVal2="0"; 
                        dataCass.MdbInc2="0"; 
                        dataCass.MdbTub2="0"; 
                        dataCass.MdbVal3="0"; 
                        dataCass.MdbInc3="0"; 
                        dataCass.MdbTub3="0"; 
                        dataCass.MdbVal4="0"; 
                        dataCass.MdbInc4="0"; 
                        dataCass.MdbTub4="0"; 
                        dataCass.MdbVal5="0"; 
                        dataCass.MdbInc5="0"; 
                        dataCass.MdbTub5="0"; 
                        dataCass.MdbVal6="0"; 
                        dataCass.MdbInc6="0"; 
                        dataCass.MdbTub6="0"; 
                        dataCass.Cashless=((float)Convert.ToInt32( splittedCashPacket[29])- (float)Convert.ToInt32(splittedCashPacket_previous[29])).ToString();  
                        dataCass.Total=((float)Convert.ToInt32( splittedCashPacket[28]) - (float)Convert.ToInt32(splittedCashPacket_previous[28])).ToString();     
                        dataCass.Change ="0"; 
                        dataCass.Sales=(tmpSales-tmpPrevSales).ToString();      
                        dataCass.Consumabile="0"; 
                        dataCass.Vend1Prc =((float)Convert.ToInt32( splittedCashPacket[4]) - (float)Convert.ToInt32(splittedCashPacket_previous[4])).ToString();          
                        dataCass.QtyV1=(Convert.ToInt32( splittedCashPacket[5])-Convert.ToInt32(splittedCashPacket_previous[5])).ToString(); 
                        dataCass.Vend2Prc=((float)Convert.ToInt32( splittedCashPacket[6])-(float)Convert.ToInt32(splittedCashPacket_previous[6])).ToString(); 
                        dataCass.QtyV2=(Convert.ToInt32( splittedCashPacket[7])-Convert.ToInt32(splittedCashPacket_previous[7])).ToString();
                        dataCass.Ticket="0";   
                        dataCass.Price="0";    
                        dataCass.MechValue="0";    
                        dataCass.Status=OnlineStatus.ToString(); 
                        }
                        strCassPacket= CashDataToString(dataCass);

                }
                else if (CasType=="W5")
                {
                    
                    
                    int tmpQTy8=Convert.ToInt32( splittedCashPacket[10])+Convert.ToInt32( splittedCashPacket[1])+Convert.ToInt32( splittedCashPacket[22])+Convert.ToInt32( splittedCashPacket[23]);
                    int tmpPrevQTy8=Convert.ToInt32(splittedCashPacket_previous[10])+Convert.ToInt32(splittedCashPacket_previous[1])+Convert.ToInt32(splittedCashPacket_previous[22])+Convert.ToInt32(splittedCashPacket_previous[23]);
                    
                    CashData dataCass= new CashData();
                    {
                        dataCass.CodeMa=splittedCashPacket[4];//.ToString();
                        dataCass.OdmTaskPalmare = transactionId;
                        dataCass.IdTelemetria="0";
                        dataCass.DateB = DateTime.Now.ToString();
                        dataCass.TipoDa="8";
                        dataCass.CanaleGettone="1";
                        dataCass.CanaleProve="9";
                        dataCass.Ch1=((float)Convert.ToInt32( splittedCashPacket[8])).ToString();
                        dataCass.Qty1=(Convert.ToInt32(splittedCashPacket[9]) - Convert.ToInt32(splittedCashPacket_previous[9])).ToString(); 
                        dataCass.Ch2=((float)Convert.ToInt32( splittedCashPacket[10])).ToString(); 
                        dataCass.Qty2=(Convert.ToInt32(splittedCashPacket[11])- Convert.ToInt32(splittedCashPacket_previous[11])).ToString();  
                        dataCass.Ch3=((float)Convert.ToInt32( splittedCashPacket[12])).ToString(); 
                        dataCass.Qty3=(Convert.ToInt32(splittedCashPacket[13])- Convert.ToInt32(splittedCashPacket_previous[13])).ToString();  
                        dataCass.Ch4=((float)Convert.ToInt32( splittedCashPacket[14])).ToString(); 
                        dataCass.Qty4=(Convert.ToInt32(splittedCashPacket[15])- Convert.ToInt32(splittedCashPacket_previous[15])).ToString();  
                        dataCass.Ch5=((float)Convert.ToInt32( splittedCashPacket[16])).ToString(); 
                        dataCass.Qty5=(Convert.ToInt32(splittedCashPacket[17])- Convert.ToInt32(splittedCashPacket_previous[17])).ToString();  
                        dataCass.Ch6=((float)Convert.ToInt32( splittedCashPacket[18])).ToString(); 
                        dataCass.Qty6=(Convert.ToInt32(splittedCashPacket[19])- Convert.ToInt32(splittedCashPacket_previous[19])).ToString();  
                        dataCass.Ch7=((float)Convert.ToInt32( splittedCashPacket[20])).ToString(); 
                        dataCass.Qty7=(Convert.ToInt32(splittedCashPacket[21])- Convert.ToInt32(splittedCashPacket_previous[21])).ToString();  
                        dataCass.Ch8=((float)Convert.ToInt32( splittedCashPacket[22])).ToString();        
                        dataCass.Qty8=(tmpQTy8-tmpPrevQTy8).ToString();         
                        dataCass.MdbVal2="0";            
                        dataCass.MdbInc2="0";            
                        dataCass.MdbTub2="0";            
                        dataCass.MdbVal3="0";            
                        dataCass.MdbInc3="0";            
                        dataCass.MdbTub3="0";            
                        dataCass.MdbVal4="0";            
                        dataCass.MdbInc4="0";            
                        dataCass.MdbTub4="0";            
                        dataCass.MdbVal5="0";            
                        dataCass.MdbInc5="0";            
                        dataCass.MdbTub5="0";            
                        dataCass.MdbVal6="0";            
                        dataCass.MdbInc6="0";            
                        dataCass.MdbTub6="0";            
                        dataCass.Cashless="0";
                        dataCass.Total=((float)Convert.ToInt32( splittedCashPacket[24]) - (float)Convert.ToInt32(splittedCashPacket_previous[24])).ToString();
                        dataCass.Change ="0";            
                        dataCass.Sales=dataCass.Total=(Convert.ToInt32( splittedCashPacket[25]) - Convert.ToInt32(splittedCashPacket_previous[25])).ToString();
                        dataCass.Consumabile="0";            
                        dataCass.Vend1Prc="0";          
                        dataCass.QtyV1= "0";            
                        dataCass.Vend2Prc="0";       
                        dataCass.QtyV2="0";
                        dataCass.Ticket=(Convert.ToInt32( splittedCashPacket[26]) - Convert.ToInt32(splittedCashPacket_previous[26])).ToString();
                        dataCass.Price=((float)Convert.ToInt32( splittedCashPacket[7]) - (float)Convert.ToInt32(splittedCashPacket_previous[7])).ToString();              
                        dataCass.MechValue="0";          
                        dataCass.Status=OnlineStatus.ToString();      
	 
                     }
                      strCassPacket= CashDataToString(dataCass);
                }
                else if (CasType== "I1"| CasType== "I2")
                {
                    if (splittedCashPacket[24]=="")splittedCashPacket[24]="0";
                    CashData dataCass= new CashData();
                    {
                       dataCass.CodeMa=splittedCashPacket[4];//.ToString();
                        dataCass.OdmTaskPalmare = transactionId;
                        dataCass.IdTelemetria="0";
                        dataCass.DateB = DateTime.Now.ToString();
                        dataCass.TipoDa="8";
                        dataCass.CanaleGettone="1";
                        dataCass.CanaleProve="9";
                        dataCass.Ch1=((float)Convert.ToInt32( splittedCashPacket[4])).ToString();
                        dataCass.Qty1=(Convert.ToInt32(splittedCashPacket[5]) - Convert.ToInt32(splittedCashPacket_previous[5])).ToString(); 
                        dataCass.Ch2=((float)Convert.ToInt32( splittedCashPacket[6])).ToString(); 
                        dataCass.Qty2=(Convert.ToInt32(splittedCashPacket[7])- Convert.ToInt32(splittedCashPacket_previous[7])).ToString();  
                        dataCass.Ch3=((float)Convert.ToInt32( splittedCashPacket[8])).ToString(); 
                        dataCass.Qty3=(Convert.ToInt32(splittedCashPacket[9])- Convert.ToInt32(splittedCashPacket_previous[9])).ToString();  
                        dataCass.Ch4=((float)Convert.ToInt32( splittedCashPacket[10])).ToString(); 
                        dataCass.Qty4=(Convert.ToInt32(splittedCashPacket[11])- Convert.ToInt32(splittedCashPacket_previous[11])).ToString();  
                        dataCass.Ch5=((float)Convert.ToInt32( splittedCashPacket[12])).ToString(); 
                        dataCass.Qty5=(Convert.ToInt32(splittedCashPacket[13])- Convert.ToInt32(splittedCashPacket_previous[13])).ToString();  
                        dataCass.Ch6=((float)Convert.ToInt32( splittedCashPacket[14])).ToString(); 
                        dataCass.Qty6=(Convert.ToInt32(splittedCashPacket[15])- Convert.ToInt32(splittedCashPacket_previous[15])).ToString();  
                        dataCass.Ch7="0";
                        dataCass.Qty7="0";
                        dataCass.Ch8="0";
                        dataCass.Qty8="0";
                        dataCass.MdbVal2 = ((float)Convert.ToInt32( splittedCashPacket[20]) - (float)Convert.ToInt32(splittedCashPacket_previous[20])).ToString();
                        dataCass.MdbInc2 = "0";
                        dataCass.MdbTub2 =  (Convert.ToInt32(splittedCashPacket[21])- Convert.ToInt32(splittedCashPacket_previous[21])).ToString();
                        dataCass.MdbVal3 = ((float)Convert.ToInt32( splittedCashPacket[22]) - (float)Convert.ToInt32(splittedCashPacket_previous[22])).ToString();
                        dataCass.MdbInc3 = "0";
                        dataCass.MdbTub3 = (Convert.ToInt32(splittedCashPacket[23])- Convert.ToInt32(splittedCashPacket_previous[23])).ToString();
                        dataCass.MdbVal4 = ((float)Convert.ToInt32( splittedCashPacket[24]) - (float)Convert.ToInt32(splittedCashPacket_previous[24])).ToString();
                        dataCass.MdbInc4 = "0";
                        dataCass.MdbTub4 = (Convert.ToInt32(splittedCashPacket[25])- Convert.ToInt32(splittedCashPacket_previous[25])).ToString();
                        dataCass.MdbVal5 = "0";
                        dataCass.MdbInc5 = "0";
                        dataCass.MdbTub5 = "0";
                        dataCass.MdbVal6 = "0";
                        dataCass.MdbInc6 = "0";
                        dataCass.MdbTub6 = "0";
                        dataCass.Cashless = ((float)Convert.ToInt32( splittedCashPacket[33]) - (float)Convert.ToInt32(splittedCashPacket_previous[33])).ToString();
                        dataCass.Total = ((float)Convert.ToInt32( splittedCashPacket[28]) - (float)Convert.ToInt32(splittedCashPacket_previous[28])).ToString();
                        dataCass.Change  = ((float)Convert.ToInt32( splittedCashPacket[29]) - (float)Convert.ToInt32(splittedCashPacket_previous[29])).ToString();
                        dataCass.Sales = (Convert.ToInt32(splittedCashPacket[31])- Convert.ToInt32(splittedCashPacket_previous[31])).ToString();  
                        dataCass.Consumabile = "0";
                        dataCass.Vend1Prc = "0";
                        dataCass.QtyV1 = "0";
                        dataCass.Vend2Prc = "0";
                        dataCass.QtyV2 = "0";
                        dataCass.Ticket =  "0";
                        dataCass.Price = ((float)Convert.ToInt32( splittedCashPacket[3]) - (float)Convert.ToInt32(splittedCashPacket_previous[3])).ToString();
                        dataCass.MechValue ="0"; 
                        dataCass.Status =OnlineStatus.ToString();

                    }
                    strCassPacket= CashDataToString(dataCass);
                  
                }                

            }
            catch(Exception e)
            {
                resetCashValue(transactionId,ref strCassPacket ,lastDataCassPresent);
                Console.WriteLine("[2] Exception buildPacket_better: " + e.StackTrace);
                return 1;
            }
            return 0;
        }
        public static void resetCashValue(string  transactionId,ref string strCassPacket,string lastDataCassPresent)
        {
            CashData dataCass= new CashData();
            {
                dataCass.CodeMa = "0";
                dataCass.OdmTaskPalmare = transactionId;
                dataCass.IdTelemetria="0";
                dataCass.DateB = lastDataCassPresent;
                dataCass.TipoDa = "0";
                dataCass.CanaleGettone  = "0";
                dataCass.CanaleProve = "0";
                dataCass.Ch1 = "0";
                dataCass.Qty1 = "0";
                dataCass.Ch2 = "0";
                dataCass.Qty2  = "0";
                dataCass.Ch3  = "0";
                dataCass.Qty3 = "0";
                dataCass.Ch4  = "0";
                dataCass.Qty4  = "0";
                dataCass.Ch5  = "0";
                dataCass.Qty5  = "0";
                dataCass.Ch6  = "0";
                dataCass.Qty6  = "0";
                dataCass.Ch7  = "0";
                dataCass.Qty7  = "0";
                dataCass.Ch8  = "0";
                dataCass.Qty8  = "0";
                dataCass.MdbVal2 = "0";
                dataCass.MdbInc2 = "0";
                dataCass.MdbTub2 = "0";
                dataCass.MdbVal3 = "0";
                dataCass.MdbInc3 = "0";
                dataCass.MdbTub3 = "0";
                dataCass.MdbVal4 = "0";
                dataCass.MdbInc4 = "0";
                dataCass.MdbTub4 = "0";
                dataCass.MdbVal5 = "0";
                dataCass.MdbInc5 = "0";
                dataCass.MdbTub5 = "0";
                dataCass.MdbVal6 = "0";
                dataCass.MdbInc6 = "0";
                dataCass.MdbTub6 = "0";
                dataCass.Cashless  = "0";
                dataCass.Total  = "0";
                dataCass.Change  = "0";
                dataCass.Sales  = "0";
                dataCass.Consumabile = "0";
                dataCass.Vend1Prc = "0";
                dataCass.QtyV1 = "0";
                dataCass.Vend2Prc = "0";
                dataCass.QtyV2 = "0";
                dataCass.Ticket  = "0";
                dataCass.Price  = "0";
                dataCass.MechValue ="0"; 
                dataCass.Status  = "0";

            }
             strCassPacket= CashDataToString(dataCass);
         
        }
         public static string CashDataToString(CashData dataCass)
        {
 
            string newcas= "{\"CodeMa\":\"" + dataCass.CodeMa + "\",\"OdmTaskPalmare\":\"" + dataCass.OdmTaskPalmare + "\",\"IdTelemetria\":\"" + dataCass.IdTelemetria + "\",\"DateB\":\"" + dataCass.DateB + "\",\"TipoDa\":\"" + dataCass.TipoDa + "\",\"CanaleGettone\":\"" + dataCass.CanaleGettone + "\",\"CanaleProve\":\"" + dataCass.CanaleProve + "\",\"Ch1\":\"" + dataCass.Ch1 + "\",\"Qty1\":\"" + dataCass.Qty1 + "\",\"Ch2\":\"" + dataCass.Ch2 + "\",\"Qty2\":\"" + dataCass.Qty2 + "\",\"Ch3\":\"" + dataCass.Ch3 + "\",\"Qty3\":\"" + dataCass.Qty3 + "\",\"Ch4\":\"" + dataCass.Ch4 + "\",\"Qty4\":\"" + dataCass.Qty4 + "\",\"Ch5\":\"" + dataCass.Ch5 + "\",\"Qty5\":\"" + dataCass.Qty5 + "\",\"Ch6\":\"" + dataCass.Ch6 + "\",\"Qty6\":\"" + dataCass.Qty6 + "\",\"Ch7\":\"" + dataCass.Ch7 + "\",\"Qty7\":\"" + dataCass.Qty7 + "\",\"Ch8\":\"" + dataCass.Ch8 + "\",\"Qty8\":\"" + dataCass.Qty8 + "\",\"MdbVal2\":\"" + dataCass.MdbVal2 + "\",\"MdbInc2\":\"" + dataCass.MdbInc2 + "\",\"MdbTub2\":\"" + dataCass.MdbTub2 + "\",\"MdbVal3\":\"" + dataCass.MdbVal3 + "\",\"MdbInc3\":\"" + dataCass.MdbInc3 + "\",\"MdbTub3\":\"" + dataCass.MdbTub3 + "\",\"MdbVal4\":\"" + dataCass.MdbVal4 + "\",\"MdbInc4\":\"" + dataCass.MdbInc4 + "\",\"MdbTub4\":\"" + dataCass.MdbTub4 + "\",\"MdbVal5\":\"" + dataCass.MdbVal5 + "\",\"MdbInc5\":\"" + dataCass.MdbInc5 + "\",\"MdbTub5\":\"" + dataCass.MdbTub5 + "\",\"MdbVal6\":\"" + dataCass.MdbVal6 + "\",\"MdbInc6\":\"" + dataCass.MdbInc6 + "\",\"MdbTub6\":\"" + dataCass.MdbTub6 + "\",\"Cashless\":\"" + dataCass.Cashless + "\",\"Total\":\"" + dataCass.Total + "\",\"Change\":\"" + dataCass.Change  + "\",\"Sales\":\"" + dataCass.Sales + "\",\"Consumabile\":\"" + dataCass.Consumabile + "\",\"Vend1Prc\":\"" + dataCass.Vend1Prc + "\",\"QtyV1\":\"" + dataCass.QtyV1 + "\",\"Vend2Prc\":\"" + dataCass.Vend2Prc + "\",\"QtyV2\":\"" + dataCass.QtyV2 + "\",\"Ticket\":\"" + dataCass.Ticket + "\",\"Price\":\"" + dataCass.Price + "\",\"MechValue\":\"" + dataCass.MechValue + "\",\"Status\":\"" + dataCass.Status + "\"}" ;
            return newcas ;

        }
         public static void LoadDatiCassaTable(string mid,string transactionId, string lastCassPacket,string CassJson,string transStatus)
        {
           
            try
            {
                MySqlConnection connection;
                connection = new MySqlConnection(connectionString);
                string query = "Insert into DatiCassa (Odm,Mid,TransferredData,JsonData,Status,DataSentToTelem) values ('" + transactionId + "','" + mid + "','" + lastCassPacket.TrimEnd() + "','" + CassJson.TrimEnd() + "','" + transStatus + "','" + null + "');";
                MySqlCommand MyCommand = new MySqlCommand(query, connection);
                connection.Open();
                MySqlDataReader MyReader = MyCommand.ExecuteReader();
                connection.Close();

            }

            
            catch (Exception ex )
            {
                Console.WriteLine("Exception LoadDatiCassaTable: " + ex.StackTrace);
            }
        }
        public static void UpdateDatiCassa(string transactionId ,string transStatus )
        {

            try
            {
                MySqlConnection connection;
                connection = new MySqlConnection(connectionString);

                string query = "Update DatiCassa set Status='"+ transStatus + "', DataSentToTelem= '"+ DateTime.Parse(DateTime.Now.ToString("yyyy / MM / dd,HH: mm: ss"))+ "' where Odm = '"+transactionId +"');";
                MySqlCommand MyCommand = new MySqlCommand(query, connection);
                connection.Open();
                MySqlDataReader MyReader = MyCommand.ExecuteReader();
                connection.Close();
            }
            catch (Exception ex )
            {
                Console.WriteLine("[2] Exception UpdateDatiCassa: " + ex.StackTrace);
            }
        }

    }
}
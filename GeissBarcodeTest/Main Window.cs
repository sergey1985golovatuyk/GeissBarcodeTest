using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeissDbConnectedLayer;
using System.Configuration;
using System.Xml.Linq;
using System.Threading;


namespace GeissBarcodeTest
{
    public partial class Form1 : Form
    {

        #region Common parameters:

        public IEnumerable<byte[]> CountEnum;
        SimensEthernet GEISS = new SimensEthernet();
        byte[] bytes = { };
        char[] charArray;
        int CountNumber;
        float RWT_1, RWT_2, RWT_3, RWT_4, RWT_5, RWT_6, RWT_7, RWT_8, RWT_9, RWT_10, RWT_11, RWT_12, RWT_14, RWT_15, RWT_16, RWT_13;
        #endregion
        // Global connection parameters:
        // Remote PLC node IP address
        private static string ipaddress; 
        public static string IpAddress
        {
            get { return ipaddress; }

            set {ipaddress = value;}
        }

        // Remote PLC node ReadPort
        private static int readport;

        public static int ReadPort
        {
        get { return readport; }

        set { readport = value; }
        }

        // Remote PLC node WritePort
        private static int writeport;

        public static int WritePort
        {
            get { return writeport; }

            set { writeport = value;}
        }

        
        #region Connection to SQL Database parameters
        // Get connection string from App.config
       public static string connectionString = ConfigurationManager.ConnectionStrings["GeissDbSqlProvider"].ConnectionString;

        // Creation DB object for connect to
        GeissDB geissDB = new GeissDB();    

        #endregion

        #region Program events handlers

        // Barcode Value change event
        public delegate void BarcodeChanged();
        public event BarcodeChanged OnBarcodeChange;
          public string BARCODE;
        public string barcode
        {
            get { return BARCODE; }

            set
            {
                if (value != BARCODE)
                {
                    BARCODE = value;
                    OnBarcodeChange();
                }

            }
        }
        #endregion

         



        public Form1()
        {
            InitializeComponent();
            ReadConnectionSettingsFromXml();
            disconnectToolStripMenuItem.Enabled = false;

            // Connection to SQL DB:
            try
            {
                geissDB.OpenConnection(connectionString);
                AppendLog("Succsefully connected to MSSQL GeissDB", true);

            }

            catch(Exception ex)
            {
                AppendLog("MSSQL DB not accessible....", true);
            }
            
            StatusIP_AddressLabel.Text = IpAddress;
        }

        #region Connect to SINUMERIK PLC
        public void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!GEISS.IsConnected)
            {
                disconnectToolStripMenuItem.Enabled = false;
                ReadConnectionSettingsFromXml();
                StatusIP_AddressLabel.Text = IpAddress;
                

            }
            // Open PLC Socket Connection
            GEISS.Connect2Port(IpAddress, ReadPort, WritePort);

            try
            {
                if (GEISS.IsConnected == true)
                {
                    ConnectionLinkLamp.BackColor = Color.LightGreen;
                    AppendLog("Connection to GEISS PLC is established!", true);
                    connectToolStripMenuItem.Enabled = false;
                    disconnectToolStripMenuItem.Enabled = true;
                }
                else
                {
                    ConnectionLinkLamp.BackColor = Color.Gray;
                    throw new Exception();
                }

            }


            catch (Exception ex)
            {
                AppendLog("Connection could't be established ( PLC socket closed connection)", true);
            }

            // Initialize Timer 1
            Timer1.Enabled = true;
            Timer1.Start();
            Timer1.Tick += new EventHandler(Timer1_Tick);

            // Initialize Timer 2
            Timer2.Enabled = true;
            Timer2.Start();
            Timer2.Tick += new EventHandler(Timer2_Tick);

            this.OnBarcodeChange += this.BarcodeValueChanged;            
        }
        #endregion

        #region Connection Status
        // Connection staus Lamp blinking
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (GEISS.IsConnected == true)
            {
                if (ConnectionLinkLamp.BackColor == Color.LightGreen)
                { ConnectionLinkLamp.BackColor = Color.Gray; }

                else if (ConnectionLinkLamp.BackColor == Color.Gray)
                { ConnectionLinkLamp.BackColor = Color.LightGreen; }
            }
        }
        #endregion

        #region Read Barcode_R
        // Read Barcode Data 'Barcode_R' and RWT[] values from PLC
        public void Timer2_Tick(object sender, EventArgs e)
        {
            if (GEISS.IsConnected)
            {
                byte[] bytes = new byte[30];
                string outString;
                GEISS.GetPlcValues("DB46.0",30,bytes,out outString);
                Encoding enc = Encoding.ASCII;
                var byteArray = bytes;
 
                charArray = enc.GetChars(byteArray);
                barcode = new string(charArray);
                BarcodeR.Text = barcode;

                // Get Car type from CharArray
                string CarType = (charArray.ElementAt(2).ToString() + charArray.ElementAt(3).ToString() + charArray.ElementAt(4).ToString());
                CountNumber = Convert.ToInt32(((charArray.ElementAt(26)).ToString() + (charArray.ElementAt(27)).ToString() +(charArray.ElementAt(28)).ToString() +(charArray.ElementAt(29)).ToString()));
                // Get RWT[] Values from DB46
                byte[] BytesRWT = new byte[64];
                GEISS.GetPlcValues("DB46.30", 64, BytesRWT, out outString);

                var RWTArray = BytesRWT;
                var chunkCounts = 16;
                var ArrayLenght = RWTArray.Count();
                var SubArrayLenght = (int)Math.Ceiling(ArrayLenght / (double)chunkCounts);
                var chunks = Enumerable.Range(0, chunkCounts).Select(i => RWTArray.Skip(i * SubArrayLenght).Take(SubArrayLenght).ToArray());
                CountEnum = chunks;

                // Return floating-point RWT[1] from Array

                var RWT_1_Array = (byte[])(CountEnum.ElementAt(0));
                Array.Reverse(RWT_1_Array);
                RWT_1 = BitConverter.ToSingle(RWT_1_Array, 0);
                RWT_1_Output.Text = ((float)BitConverter.ToSingle(RWT_1_Array, 0)).ToString();

                // Return floating-point RWT[2] from Array

                var RWT_2_Array = (byte[])(CountEnum.ElementAt(1));
                Array.Reverse(RWT_2_Array);
                RWT_2 = BitConverter.ToSingle(RWT_2_Array, 0);
                RWT_2_Output.Text = ((float)BitConverter.ToSingle(RWT_2_Array, 0)).ToString();

                // Return floating-point RWT[3] from Array

                var RWT_3_Array = (byte[])(CountEnum.ElementAt(2));
                Array.Reverse(RWT_3_Array);
                RWT_3 = BitConverter.ToSingle(RWT_3_Array, 0);
                RWT_3_Output.Text = ((float)BitConverter.ToSingle(RWT_3_Array, 0)).ToString();

                // Return floating-point RWT[4] from Array

                var RWT_4_Array = (byte[])(CountEnum.ElementAt(3));
                Array.Reverse(RWT_4_Array);
                RWT_4 = BitConverter.ToSingle(RWT_4_Array, 0);
                RWT_4_Output.Text = ((float)BitConverter.ToSingle(RWT_4_Array, 0)).ToString();

                // Return floating-point RWT[5] from Array
                var RWT_5_Array = (byte[])(CountEnum.ElementAt(4));
                Array.Reverse(RWT_5_Array);
                RWT_5 = BitConverter.ToSingle(RWT_5_Array, 0);
                RWT_5_Output.Text = ((float)BitConverter.ToSingle(RWT_5_Array, 0)).ToString();

                // Return floating-point RWT[6] from Array
                var RWT_6_Array = (byte[])(CountEnum.ElementAt(5));
                Array.Reverse(RWT_6_Array);
                RWT_6 = BitConverter.ToSingle(RWT_6_Array, 0);
                RWT_6_Output.Text = ((float)BitConverter.ToSingle(RWT_6_Array, 0)).ToString();


                // Return floating-point RWT[7] from Array
                var RWT_7_Array = (byte[])(CountEnum.ElementAt(6));
                Array.Reverse(RWT_7_Array);
                RWT_7 = BitConverter.ToSingle(RWT_7_Array, 0);
                RWT_7_Output.Text = ((float)BitConverter.ToSingle(RWT_7_Array, 0)).ToString();

                // Return floating-point RWT[8] from Array
                var RWT_8_Array = (byte[])(CountEnum.ElementAt(7));
                Array.Reverse(RWT_8_Array);
                RWT_8 = BitConverter.ToSingle(RWT_8_Array, 0);
                RWT_8_Output.Text = ((float)BitConverter.ToSingle(RWT_8_Array, 0)).ToString();

                // Return floating-point RWT[9] from Array
                var RWT_9_Array = (byte[])(CountEnum.ElementAt(8));
                Array.Reverse(RWT_9_Array);
                RWT_9 = BitConverter.ToSingle(RWT_9_Array, 0);
                RWT_9_Output.Text = ((float)BitConverter.ToSingle(RWT_9_Array, 0)).ToString();

                // Return floating-point RWT[10] from Array
                var RWT_10_Array = (byte[])(CountEnum.ElementAt(9));
                Array.Reverse(RWT_10_Array);
                RWT_10 = BitConverter.ToSingle(RWT_10_Array, 0);
                RWT_10_Output.Text = ((float)BitConverter.ToSingle(RWT_10_Array, 0)).ToString();

                // Return floating-point RWT[11] from Array
                var RWT_11_Array = (byte[])(CountEnum.ElementAt(10));
                Array.Reverse(RWT_11_Array);
                RWT_11 = BitConverter.ToSingle(RWT_11_Array, 0);
                RWT_11_Output.Text = ((float)BitConverter.ToSingle(RWT_11_Array, 0)).ToString();

                // Return floating-point RWT[12] from Array
                var RWT_12_Array = (byte[])(CountEnum.ElementAt(11));
                Array.Reverse(RWT_12_Array);
                RWT_12 = BitConverter.ToSingle(RWT_12_Array, 0);
                if (CarType != "QBR")
                {
                    RWT_12_Output.Text = ((float)BitConverter.ToSingle(RWT_12_Array, 0)).ToString();
                }
                else { RWT_12_Output.Text = "Not Used"; }

                // Return floating-point RWT[13] from Array
                var RWT_13_Array = (byte[])(CountEnum.ElementAt(12));
                Array.Reverse(RWT_13_Array);
                RWT_13 = BitConverter.ToSingle(RWT_13_Array, 0);
                if (CarType != "QBR")
                {
                    RWT_13_Output.Text = ((float)BitConverter.ToSingle(RWT_13_Array, 0)).ToString();
                }
                else { RWT_13_Output.Text = "Not Used"; }
                

                // Return floating-point RWT[14] from Array
                var RWT_14_Array = (byte[])(CountEnum.ElementAt(13));
                Array.Reverse(RWT_14_Array);
                RWT_14 = BitConverter.ToSingle(RWT_13_Array, 0);
                if (CarType != "QBR")
                {
                    RWT_14_Output.Text = ((float)BitConverter.ToSingle(RWT_14_Array, 0)).ToString();
                }
                else { RWT_14_Output.Text = "Not Used"; }

                // Return floating-point RWT[15] from Array
                var RWT_15_Array = (byte[])(CountEnum.ElementAt(14));
                Array.Reverse(RWT_15_Array);
                RWT_15 = BitConverter.ToSingle(RWT_15_Array, 0);
                if (CarType != "QBR")
                {
                    RWT_15_Output.Text = ((float)BitConverter.ToSingle(RWT_15_Array, 0)).ToString();
                }
                else { RWT_15_Output.Text = "Not Used"; }

                // Return floating-point RWT[16] from Array
                var RWT_16_Array = (byte[])(CountEnum.ElementAt(15));
                Array.Reverse(RWT_16_Array);
                RWT_16 = BitConverter.ToSingle(RWT_16_Array, 0);
                if (CarType != "QBR")
                {
                    RWT_16_Output.Text = ((float)BitConverter.ToSingle(RWT_16_Array, 0)).ToString();
                }
                else { RWT_16_Output.Text = "Not Used"; }              
              
            }
                     
        }

        #endregion

        #region Disconnect from SINUMERIC PLC
        public void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GEISS.Disconnect2Port();
            Timer1.Stop();
            Timer1.Enabled = false;
            //this.OnBarcodeChange -= this.BarcodeValueChanged;

            if (GEISS.IsConnected == false)
            {
                ConnectionLinkLamp.BackColor = Color.Red;
                AppendLog("Disconnected!", true);
                connectToolStripMenuItem.Enabled = true;
                disconnectToolStripMenuItem.Enabled = false;
            }

        }
        #endregion

        #region Insert new Part Barcode and RWT to DB

        public void InsertNewPartInDataBase()
        {
            Barcode newBarcode = new Barcode();
            newBarcode.CarType = (charArray.ElementAt(2).ToString() + charArray.ElementAt(3).ToString() + charArray.ElementAt(4).ToString());
            newBarcode.Barcode_LD = BARCODE;
            newBarcode.Date = (charArray.ElementAt(9).ToString() + charArray.ElementAt(10).ToString() + charArray.ElementAt(11).ToString() + charArray.ElementAt(12).ToString() +
                       charArray.ElementAt(13).ToString() + charArray.ElementAt(14).ToString() + charArray.ElementAt(15).ToString() + charArray.ElementAt(16).ToString() +
                       charArray.ElementAt(17).ToString() +charArray.ElementAt(18).ToString());
            newBarcode.Time = (charArray.ElementAt(20).ToString() + charArray.ElementAt(21).ToString() + charArray.ElementAt(22).ToString() + charArray.ElementAt(23).ToString() +
                       charArray.ElementAt(24).ToString());
            newBarcode.FIFO_Number = Convert.ToInt32(((charArray.ElementAt(26)).ToString() + (charArray.ElementAt(27)).ToString() + (charArray.ElementAt(28)).ToString() + (charArray.ElementAt(29)).ToString()));

            newBarcode.RWT_1 = RWT_1.ToString();
            newBarcode.RWT_2 = RWT_2.ToString();
            newBarcode.RWT_3 = RWT_3.ToString();
            newBarcode.RWT_4 = RWT_4.ToString();
            newBarcode.RWT_5 = RWT_5.ToString();
            newBarcode.RWT_6 = RWT_6.ToString();
            newBarcode.RWT_7 = RWT_7.ToString();
            newBarcode.RWT_8 = RWT_8.ToString();
            newBarcode.RWT_9 = RWT_9.ToString();
            newBarcode.RWT_10 = RWT_10.ToString();
            newBarcode.RWT_11 = RWT_11.ToString();
            newBarcode.RWT_12 = RWT_12.ToString();
            newBarcode.RWT_13 = RWT_13.ToString();
            newBarcode.RWT_14 = RWT_14.ToString();
            newBarcode.RWT_15 = RWT_15.ToString();
            newBarcode.RWT_16 = RWT_16.ToString();

            try
            {
                geissDB.InsertBarcode(newBarcode);
                AppendLog("Barcode " + BARCODE + " inserted in GeissDB OK", true);
            }

            catch(Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }

        }


        #endregion

        # region Append Data to Log
        public void AppendLog(string S, bool NewLine)
        {
            try
            {
                EventLog.AppendText(S);
                if (NewLine)
                    EventLog.AppendText(Environment.NewLine);

            }

            catch (Exception ex)
            {

            }

        }
        #endregion
         
        #region Barcode Value Changed

        public void BarcodeValueChanged()
        {

            InsertNewPartInDataBase();
        }
        
        
        #endregion
       
        #region Connection Settings Window opening
        private void connectionSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connection_Settings ConnSet = new Connection_Settings();

            try
            {
                if (!GEISS.IsConnected && !ConnSet.Visible)
                {
                    
                    ConnSet.Owner = this;
                    ConnSet.Visible = true;
                }
                else
                {
                    throw new Exception();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("To Change Connection Settings \n Disconnect from SINUMERIK!", "Warning!", MessageBoxButtons.OK);
            }
        }
        #endregion

        #region Read PLC configuration properties

        public void ReadConnectionSettingsFromXml()
        {
            // Check if configuration file existsin program directory
            string exeDir = Environment.CurrentDirectory;
            string filePath = Application.StartupPath + "\\ConnectionSettings.xml";

            try
            {
                XDocument connParams = XDocument.Load(filePath);
                XNode node = connParams.Root.FirstNode;
                XElement el = (XElement)node;
                string IpAddr = el.Attribute("IpAddress").Value.ToString();
                int rPort = Int32.Parse(el.Attribute("ReadPort").Value);
                int wPort = Int32.Parse(el.Attribute("WritePort").Value);

                IpAddress = IpAddr;
                ReadPort = rPort;
                WritePort = wPort;
            }

            catch(Exception ex)
            {
                MessageBox.Show("Connection Settings File not Created or \n Connection parameters not set!");
            }

        }


        #endregion

        #region 
        private void watchBarcodeRWTHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BarcodeRwtHistorian barcodeHist = new BarcodeRwtHistorian();
            barcodeHist.Owner = this;
            barcodeHist.Visible = true;
        }

        #endregion


    }
}

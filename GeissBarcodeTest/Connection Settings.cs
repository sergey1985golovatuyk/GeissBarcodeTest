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
using System.IO;
using System.Xml.XmlConfiguration;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace GeissBarcodeTest
{
    public partial class Connection_Settings : Form
    {

        public string PlcIpAddress;
        string rPort;
        string wPort;

        public Connection_Settings()
        {
            InitializeComponent();
            OK_Button.Enabled = false;
            string plcIpAddress = Form1.IpAddress;
            ReadPort.Text = Form1.ReadPort.ToString();
            WritePort.Text = Form1.WritePort.ToString();

            System.Net.IPAddress ipAddress;
            if(System.Net.IPAddress.TryParse(plcIpAddress,out ipAddress))
            {
                byte[] addressByte = ipAddress.GetAddressBytes();
                IP_1.Text = addressByte[0].ToString();
                IP_2.Text = addressByte[1].ToString();
                IP_3.Text = addressByte[2].ToString();
                IP_4.Text = addressByte[3].ToString();
            }
 

        }

        #region Connection Parameters Control and Text Change Handle
        private void IP_1_Key_Press(object sender, KeyPressEventArgs e)
        {

            if ((e.KeyChar >= '0') && (e.KeyChar <= '9') && (IP_1.Text.Length < 3))
                return;

            if ((e.KeyChar == (Char)Keys.Back))
                return;

            if ((Char.IsControl(e.KeyChar)) && (IP_1.Text.Length != 0))
            {
                if (e.KeyChar == (Char)Keys.Enter)
                {
                    IP_2.Focus();
                    return;
                }
            }

            e.Handled = true;
        }


        private void IP_1_TextChanged(object sender, EventArgs e)
        {
            if (IP_1.Text.Length != 0 && IP_2.Text.Length != 0 && IP_4.Text.Length != 0)
            {
                PlcIpAddress = (IP_1.Text + "." + IP_2.Text + "." + IP_3.Text + "." + IP_4.Text);
            }
        }


        private void IP_2_Key_Press(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9') && (IP_2.Text.Length < 3))
                return;

            if ((e.KeyChar == (Char)Keys.Back))
                return;

            if ((Char.IsControl(e.KeyChar)) && (IP_2.Text.Length != 0))
            {
                if (e.KeyChar == (Char)Keys.Enter)
                {
                    IP_3.Focus();
                    return;
                }
            }

            e.Handled = true;
        }

        private void IP_2_TextChanged(object sender, EventArgs e)
        {
            if (IP_1.Text.Length != 0 && IP_2.Text.Length != 0 && IP_4.Text.Length != 0)
            {
                PlcIpAddress = (IP_1.Text + "." + IP_2.Text + "." + IP_3.Text + "." + IP_4.Text);
            }

        }

        private void IP_3_Key_Press(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9') && (IP_3.Text.Length < 3))
                return;

            if ((e.KeyChar == (Char)Keys.Back))
                return;

            if ((Char.IsControl(e.KeyChar)) && (IP_3.Text.Length != 0))
            {
                if (e.KeyChar == (Char)Keys.Enter)
                {
                    IP_4.Focus();
                    return;
                }
            }

            e.Handled = true;
        }

        private void IP_3_TextChanged(object sender, EventArgs e)
        {
            if (IP_1.Text.Length != 0 && IP_2.Text.Length != 0 && IP_4.Text.Length != 0)
            {
                PlcIpAddress = (IP_1.Text + "." + IP_2.Text + "." + IP_3.Text + "." + IP_4.Text);
            }
        }


        private void IP_4_Key_Press(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9') && (IP_4.Text.Length < 3))
                return;

            if ((e.KeyChar == (Char)Keys.Back))
                return;

            if ((Char.IsControl(e.KeyChar)) && (IP_4.Text.Length != 0))
            {
                if (e.KeyChar == (Char)Keys.Enter)
                {
                    ReadPort.Focus();
                    return;
                }
            }

            e.Handled = true;
        }

        private void IP_4_TextChanged(object sender, EventArgs e)
        {
            if(IP_1.Text.Length != 0 && IP_2.Text.Length !=0 && IP_4.Text.Length !=0)
            {
                PlcIpAddress = (IP_1.Text + "." + IP_2.Text + "." + IP_3.Text + "." + IP_4.Text);
            }
        }

        private void ReadPort_Key_Press(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9') && (ReadPort.Text.Length < 1))
                return;

            if ((e.KeyChar == (Char)Keys.Back))
                return;

            if ((Char.IsControl(e.KeyChar)) && (ReadPort.Text.Length != 0))
            {
                if (e.KeyChar == (Char)Keys.Enter)
                {
                    WritePort.Focus();
                    return;
                }
            }

            e.Handled = true;
        }

        private void WritePort_Key_Press(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9') && (WritePort.Text.Length < 1))
                return;

            if ((e.KeyChar == (Char)Keys.Back))
                return;

            if ((Char.IsControl(e.KeyChar)) && (WritePort.Text.Length != 0))
            {
                if (e.KeyChar == (Char)Keys.Enter)
                {
                    OK_Button.Focus();
                    return;
                }
            }

            e.Handled = true;
        }

        private void WritePort_TextChanged(object sender, EventArgs e)
        {
            PlcIpAddress = (IP_1.Text + "." + IP_2.Text + "." + IP_3.Text + "." + IP_4.Text);
            if ((PlcIpAddress.Length !=0) && (ReadPort.Text.Length !=0) && (WritePort.Text.Length != 0))
                OK_Button.Enabled = true;
        }

        #endregion


        #region Close Connection Settings Windows and save parameters
        private void OK_Button_Click(object sender, EventArgs e)
        {


            // Check if configuration file existsin program directory
            string exeDir = Environment.CurrentDirectory;
            string filePath = Application.StartupPath + "\\ConnectionSettings.xml";
            
            string IpAddr = "";


            if (!System.IO.File.Exists(filePath))
            {
                SaveFileDialog saveDiag = new SaveFileDialog();
                saveDiag.FileName = "ConnectionSettings";
                saveDiag.DefaultExt = "*.xml";
                saveDiag.Filter = "XML Files (*.xml) |*.xml";
                if (saveDiag.ShowDialog() != DialogResult.OK)
                    return;

                System.Xml.XmlWriter writer = XmlWriter.Create(saveDiag.FileName);

                try
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("ConnectionParameters");
                    writer.WriteStartElement("ConnectionParameters");
                    writer.WriteAttributeString("IpAddress", PlcIpAddress);
                    writer.WriteAttributeString("ReadPort", ReadPort.Text);
                    writer.WriteAttributeString("WritePort", WritePort.Text);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    this.Close();

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

                finally
                {
                    if (writer != null)
                        writer.Close();
                }
            }

            else
            {
                XDocument connParams = XDocument.Load(filePath);

                XElement xElm = connParams.Root;
                XNode node = connParams.Root.FirstNode;
                XElement el = (XElement)node;
                IpAddr = el.Attribute("IpAddress").Value.ToString();
                rPort = (el.Attribute("ReadPort").Value);
                wPort = (el.Attribute("WritePort").Value);
                IpAddr = PlcIpAddress;
                rPort = ReadPort.Text;
                wPort = WritePort.Text;
                el.Attribute("IpAddress").Value = IpAddr;
                el.Attribute("ReadPort").Value = rPort;
                el.Attribute("WritePort").Value = wPort;
                connParams.Save(filePath);

                // Send data to Main Window Form
                Form1.IpAddress = PlcIpAddress;

                this.Close();
            }



            
            // Save connection params to *.xml

            //XmlDocument paramsXml = new XmlDocument();
            
           // paramsXml.LoadXml(File.ReadAllText(filePath));
           // XmlElement root = paramsXml.DocumentElement;
           // XmlElement Param = paramsXml.CreateElement("Connection Settings");
        }

        

        #endregion
    }
}


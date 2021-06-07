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

namespace GeissBarcodeTest
{
    public partial class BarcodeRwtHistorian : Form
    {

        int windowHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 150;
        int windowWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 10;

        
      
        public BarcodeRwtHistorian()
        {
            InitializeComponent();
            InquiryButton.Enabled = false;
            dataGridView.Height = windowHeight;
            dataGridView.Width = windowWidth;
        }

        private void QueryBarcodeRwtData()
        {

        }

        private void InquiryButton_Click(object sender, EventArgs e)
        {

            string date = dateTimePicker.Text;
            GeissDB db = new GeissDB();

            DataSet dbDataSet = db.GetDBDataAsDataSet();
            string connStr = Form1.connectionString;
            DataTable dt =  db.FillDataSet(dbDataSet, CarTypeSelect.Text, dateTimePicker.Text, connStr);
            
            dbDataSet.Tables.Add(dt);
           // DataGrid dataGrid = new DataGrid();
          //  dataGrid.DataSource = dbDataSet;
          //  dataGrid.Visible = true;
           // dataGrid.Show();

            dataGridView.DataSource = dbDataSet.Tables[0];

         
                       
        }

        private void CarTypeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            dateTimePicker.Focus();
        }

        private void dateTimepPicker_ValueChanged(object sender, EventArgs e)
        {
            InquiryButton.Enabled = true;
            InquiryButton.Focus();

        }




    }


}


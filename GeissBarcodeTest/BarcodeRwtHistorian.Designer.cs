namespace GeissBarcodeTest
{
    partial class BarcodeRwtHistorian
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.CarTypeLabel = new System.Windows.Forms.Label();
            this.CarTypeSelect = new System.Windows.Forms.ComboBox();
            this.DateLabel = new System.Windows.Forms.Label();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.InquiryButton = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.barcodeRwtHistorianBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barcodeRwtHistorianBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // CarTypeLabel
            // 
            this.CarTypeLabel.BackColor = System.Drawing.SystemColors.HighlightText;
            this.CarTypeLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CarTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CarTypeLabel.Location = new System.Drawing.Point(29, 31);
            this.CarTypeLabel.Name = "CarTypeLabel";
            this.CarTypeLabel.Size = new System.Drawing.Size(82, 24);
            this.CarTypeLabel.TabIndex = 14;
            this.CarTypeLabel.Text = "Car Type";
            this.CarTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CarTypeSelect
            // 
            this.CarTypeSelect.FormattingEnabled = true;
            this.CarTypeSelect.Items.AddRange(new object[] {
            "----",
            "RBR",
            "QBR",
            "GSR",
            "HCR",
            "FBR"});
            this.CarTypeSelect.Location = new System.Drawing.Point(117, 31);
            this.CarTypeSelect.Name = "CarTypeSelect";
            this.CarTypeSelect.Size = new System.Drawing.Size(121, 24);
            this.CarTypeSelect.TabIndex = 15;
            this.CarTypeSelect.SelectedIndexChanged += new System.EventHandler(this.CarTypeSelect_SelectedIndexChanged);
            // 
            // DateLabel
            // 
            this.DateLabel.BackColor = System.Drawing.SystemColors.HighlightText;
            this.DateLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DateLabel.Location = new System.Drawing.Point(244, 31);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(82, 24);
            this.DateLabel.TabIndex = 16;
            this.DateLabel.Text = "Date";
            this.DateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker.Location = new System.Drawing.Point(332, 33);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(200, 22);
            this.dateTimePicker.TabIndex = 17;
            this.dateTimePicker.ValueChanged += new System.EventHandler(this.dateTimepPicker_ValueChanged);
            // 
            // InquiryButton
            // 
            this.InquiryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.InquiryButton.Location = new System.Drawing.Point(538, 32);
            this.InquiryButton.Name = "InquiryButton";
            this.InquiryButton.Size = new System.Drawing.Size(174, 32);
            this.InquiryButton.TabIndex = 18;
            this.InquiryButton.Text = "Inquiry";
            this.InquiryButton.UseVisualStyleBackColor = true;
            this.InquiryButton.Click += new System.EventHandler(this.InquiryButton_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(2, 65);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowTemplate.Height = 24;
            this.dataGridView.Size = new System.Drawing.Size(1104, 641);
            this.dataGridView.TabIndex = 19;
            // 
            // barcodeRwtHistorianBindingSource
            // 
            this.barcodeRwtHistorianBindingSource.DataSource = typeof(GeissBarcodeTest.BarcodeRwtHistorian);
            // 
            // BarcodeRwtHistorian
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.InquiryButton);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.DateLabel);
            this.Controls.Add(this.CarTypeSelect);
            this.Controls.Add(this.CarTypeLabel);
            this.Name = "BarcodeRwtHistorian";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BarcodeRwtHistorian";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barcodeRwtHistorianBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label CarTypeLabel;
        private System.Windows.Forms.ComboBox CarTypeSelect;
        private System.Windows.Forms.Label DateLabel;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Button InquiryButton;
        private System.Windows.Forms.BindingSource barcodeRwtHistorianBindingSource;
        private System.Windows.Forms.DataGridView dataGridView;
    }
}
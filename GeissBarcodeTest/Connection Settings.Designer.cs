namespace GeissBarcodeTest
{
    partial class Connection_Settings
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
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.OK_Button = new System.Windows.Forms.Button();
            this.WritePort = new System.Windows.Forms.TextBox();
            this.WritePort_Label = new System.Windows.Forms.Label();
            this.ReadPort = new System.Windows.Forms.TextBox();
            this.ReadPort_Label = new System.Windows.Forms.Label();
            this.IP_4 = new System.Windows.Forms.TextBox();
            this.IP_3 = new System.Windows.Forms.TextBox();
            this.IP_2 = new System.Windows.Forms.TextBox();
            this.IP_1 = new System.Windows.Forms.TextBox();
            this.IP_Label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Location = new System.Drawing.Point(207, 289);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(88, 38);
            this.Cancel_Button.TabIndex = 32;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            // 
            // OK_Button
            // 
            this.OK_Button.Location = new System.Drawing.Point(103, 289);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(88, 38);
            this.OK_Button.TabIndex = 31;
            this.OK_Button.Text = "OK";
            this.OK_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // WritePort
            // 
            this.WritePort.Location = new System.Drawing.Point(103, 95);
            this.WritePort.Name = "WritePort";
            this.WritePort.Size = new System.Drawing.Size(44, 22);
            this.WritePort.TabIndex = 30;
            this.WritePort.TextChanged += new System.EventHandler(this.WritePort_TextChanged);
            // 
            // WritePort_Label
            // 
            this.WritePort_Label.AutoSize = true;
            this.WritePort_Label.Location = new System.Drawing.Point(28, 99);
            this.WritePort_Label.Name = "WritePort_Label";
            this.WritePort_Label.Size = new System.Drawing.Size(71, 17);
            this.WritePort_Label.TabIndex = 29;
            this.WritePort_Label.Text = "Write Port";
            // 
            // ReadPort
            // 
            this.ReadPort.Location = new System.Drawing.Point(103, 68);
            this.ReadPort.Name = "ReadPort";
            this.ReadPort.Size = new System.Drawing.Size(44, 22);
            this.ReadPort.TabIndex = 28;
            // 
            // ReadPort_Label
            // 
            this.ReadPort_Label.AutoSize = true;
            this.ReadPort_Label.Location = new System.Drawing.Point(28, 71);
            this.ReadPort_Label.Name = "ReadPort_Label";
            this.ReadPort_Label.Size = new System.Drawing.Size(72, 17);
            this.ReadPort_Label.TabIndex = 27;
            this.ReadPort_Label.Text = "Read Port";
            // 
            // IP_4
            // 
            this.IP_4.Location = new System.Drawing.Point(287, 40);
            this.IP_4.Name = "IP_4";
            this.IP_4.Size = new System.Drawing.Size(44, 22);
            this.IP_4.TabIndex = 26;
            this.IP_4.TextChanged += new System.EventHandler(this.IP_4_TextChanged);
            this.IP_4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IP_4_Key_Press);
            // 
            // IP_3
            // 
            this.IP_3.Location = new System.Drawing.Point(237, 40);
            this.IP_3.Name = "IP_3";
            this.IP_3.Size = new System.Drawing.Size(44, 22);
            this.IP_3.TabIndex = 25;
            this.IP_3.TextChanged += new System.EventHandler(this.IP_3_TextChanged);
            this.IP_3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IP_3_Key_Press);
            // 
            // IP_2
            // 
            this.IP_2.Location = new System.Drawing.Point(187, 40);
            this.IP_2.Name = "IP_2";
            this.IP_2.Size = new System.Drawing.Size(44, 22);
            this.IP_2.TabIndex = 24;
            this.IP_2.TextChanged += new System.EventHandler(this.IP_2_TextChanged);
            this.IP_2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IP_2_Key_Press);
            // 
            // IP_1
            // 
            this.IP_1.Location = new System.Drawing.Point(137, 40);
            this.IP_1.Name = "IP_1";
            this.IP_1.Size = new System.Drawing.Size(44, 22);
            this.IP_1.TabIndex = 23;
            this.IP_1.TextChanged += new System.EventHandler(this.IP_1_TextChanged);
            this.IP_1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IP_1_Key_Press);
            // 
            // IP_Label
            // 
            this.IP_Label.AutoSize = true;
            this.IP_Label.Location = new System.Drawing.Point(28, 41);
            this.IP_Label.Name = "IP_Label";
            this.IP_Label.Size = new System.Drawing.Size(106, 17);
            this.IP_Label.TabIndex = 22;
            this.IP_Label.Text = "PLC IP Address";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(29, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(302, 24);
            this.label1.TabIndex = 33;
            this.label1.Text = "Provider PLC Connection Settings:";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(29, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(302, 24);
            this.label2.TabIndex = 34;
            this.label2.Text = "SQL Server Connection Settings:";
            // 
            // Connection_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 355);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.OK_Button);
            this.Controls.Add(this.WritePort);
            this.Controls.Add(this.WritePort_Label);
            this.Controls.Add(this.ReadPort);
            this.Controls.Add(this.ReadPort_Label);
            this.Controls.Add(this.IP_4);
            this.Controls.Add(this.IP_3);
            this.Controls.Add(this.IP_2);
            this.Controls.Add(this.IP_1);
            this.Controls.Add(this.IP_Label);
            this.Name = "Connection_Settings";
            this.Text = "Connection Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Cancel_Button;
        private System.Windows.Forms.Button OK_Button;
        private System.Windows.Forms.TextBox WritePort;
        private System.Windows.Forms.Label WritePort_Label;
        private System.Windows.Forms.TextBox ReadPort;
        private System.Windows.Forms.Label ReadPort_Label;
        public System.Windows.Forms.TextBox IP_4;
        public System.Windows.Forms.TextBox IP_3;
        public System.Windows.Forms.TextBox IP_2;
        public System.Windows.Forms.TextBox IP_1;
        private System.Windows.Forms.Label IP_Label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
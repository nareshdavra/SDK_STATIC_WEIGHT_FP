namespace testPro
{
    partial class ScanForm
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
            this.btnScan = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.lblConnection = new System.Windows.Forms.Label();
            this.btnOpenNewForm = new System.Windows.Forms.Button();
            this.scanWeight = new System.Windows.Forms.Button();
            this.chkAutoWeight = new System.Windows.Forms.CheckBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.cmbDevice = new System.Windows.Forms.ComboBox();
            this.chkWaitmode = new System.Windows.Forms.CheckBox();
            this.btnStopScan = new System.Windows.Forms.Button();
            this.lstControls = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnEnrol = new System.Windows.Forms.Button();
            this.txtfname = new System.Windows.Forms.TextBox();
            this.txtlname = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblfpstatus = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(255, 78);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 0;
            this.btnScan.Text = "Scan ";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(47, 147);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(49, 13);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "message";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(47, 195);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(34, 13);
            this.lblCount.TabIndex = 2;
            this.lblCount.Text = "count";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(47, 170);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(71, 13);
            this.lblError.TabIndex = 3;
            this.lblError.Text = "errorMessage";
            // 
            // lblConnection
            // 
            this.lblConnection.AutoSize = true;
            this.lblConnection.Location = new System.Drawing.Point(47, 245);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(90, 13);
            this.lblConnection.TabIndex = 4;
            this.lblConnection.Text = "connectionStatus";
            // 
            // btnOpenNewForm
            // 
            this.btnOpenNewForm.Location = new System.Drawing.Point(21, 392);
            this.btnOpenNewForm.Name = "btnOpenNewForm";
            this.btnOpenNewForm.Size = new System.Drawing.Size(75, 23);
            this.btnOpenNewForm.TabIndex = 5;
            this.btnOpenNewForm.Text = "Open Form";
            this.btnOpenNewForm.UseVisualStyleBackColor = true;
            this.btnOpenNewForm.Click += new System.EventHandler(this.btnOpenNewForm_Click);
            // 
            // scanWeight
            // 
            this.scanWeight.Location = new System.Drawing.Point(255, 147);
            this.scanWeight.Name = "scanWeight";
            this.scanWeight.Size = new System.Drawing.Size(96, 23);
            this.scanWeight.TabIndex = 6;
            this.scanWeight.Text = "Scan N Weight";
            this.scanWeight.UseVisualStyleBackColor = true;
            this.scanWeight.Click += new System.EventHandler(this.scanWeight_Click);
            // 
            // chkAutoWeight
            // 
            this.chkAutoWeight.AutoSize = true;
            this.chkAutoWeight.Location = new System.Drawing.Point(255, 176);
            this.chkAutoWeight.Name = "chkAutoWeight";
            this.chkAutoWeight.Size = new System.Drawing.Size(121, 17);
            this.chkAutoWeight.TabIndex = 7;
            this.chkAutoWeight.Text = "Enable Auto Weight";
            this.chkAutoWeight.UseVisualStyleBackColor = true;
            this.chkAutoWeight.CheckedChanged += new System.EventHandler(this.chkAutoWeight_CheckedChanged);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(336, 12);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 38);
            this.btnDisconnect.TabIndex = 8;
            this.btnDisconnect.Text = "Disconnect device";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(255, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 38);
            this.btnConnect.TabIndex = 9;
            this.btnConnect.Text = "Connect device";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // cmbDevice
            // 
            this.cmbDevice.FormattingEnabled = true;
            this.cmbDevice.Location = new System.Drawing.Point(50, 12);
            this.cmbDevice.Name = "cmbDevice";
            this.cmbDevice.Size = new System.Drawing.Size(121, 21);
            this.cmbDevice.TabIndex = 10;
            // 
            // chkWaitmode
            // 
            this.chkWaitmode.AutoSize = true;
            this.chkWaitmode.Location = new System.Drawing.Point(255, 117);
            this.chkWaitmode.Name = "chkWaitmode";
            this.chkWaitmode.Size = new System.Drawing.Size(112, 17);
            this.chkWaitmode.TabIndex = 11;
            this.chkWaitmode.Text = "Enable Auto Scan";
            this.chkWaitmode.UseVisualStyleBackColor = true;
            this.chkWaitmode.CheckedChanged += new System.EventHandler(this.chkWaitmode_CheckedChanged);
            // 
            // btnStopScan
            // 
            this.btnStopScan.Location = new System.Drawing.Point(336, 78);
            this.btnStopScan.Name = "btnStopScan";
            this.btnStopScan.Size = new System.Drawing.Size(75, 23);
            this.btnStopScan.TabIndex = 12;
            this.btnStopScan.Text = "StopScan ";
            this.btnStopScan.UseVisualStyleBackColor = true;
            this.btnStopScan.Click += new System.EventHandler(this.btnStopScan_Click);
            // 
            // lstControls
            // 
            this.lstControls.FormattingEnabled = true;
            this.lstControls.Location = new System.Drawing.Point(50, 52);
            this.lstControls.Name = "lstControls";
            this.lstControls.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstControls.Size = new System.Drawing.Size(120, 82);
            this.lstControls.TabIndex = 13;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnEnrol);
            this.groupBox1.Controls.Add(this.txtfname);
            this.groupBox1.Controls.Add(this.txtlname);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(21, 271);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(460, 115);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Control";
            // 
            // btnEnrol
            // 
            this.btnEnrol.Location = new System.Drawing.Point(53, 86);
            this.btnEnrol.Name = "btnEnrol";
            this.btnEnrol.Size = new System.Drawing.Size(75, 23);
            this.btnEnrol.TabIndex = 3;
            this.btnEnrol.Text = "Enrol";
            this.btnEnrol.UseVisualStyleBackColor = true;
            this.btnEnrol.Click += new System.EventHandler(this.btnEnrol_Click);
            // 
            // txtfname
            // 
            this.txtfname.Location = new System.Drawing.Point(76, 28);
            this.txtfname.Name = "txtfname";
            this.txtfname.Size = new System.Drawing.Size(100, 20);
            this.txtfname.TabIndex = 1;
            // 
            // txtlname
            // 
            this.txtlname.Location = new System.Drawing.Point(76, 53);
            this.txtlname.Name = "txtlname";
            this.txtlname.Size = new System.Drawing.Size(100, 20);
            this.txtlname.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "LastName";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "FirstName";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblfpstatus
            // 
            this.lblfpstatus.AutoSize = true;
            this.lblfpstatus.Location = new System.Drawing.Point(47, 222);
            this.lblfpstatus.Name = "lblfpstatus";
            this.lblfpstatus.Size = new System.Drawing.Size(48, 13);
            this.lblfpstatus.TabIndex = 17;
            this.lblfpstatus.Text = "FPstatus";
            // 
            // ScanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 427);
            this.Controls.Add(this.lblfpstatus);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lstControls);
            this.Controls.Add(this.btnStopScan);
            this.Controls.Add(this.chkWaitmode);
            this.Controls.Add(this.cmbDevice);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.chkAutoWeight);
            this.Controls.Add(this.scanWeight);
            this.Controls.Add(this.btnOpenNewForm);
            this.Controls.Add(this.lblConnection);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnScan);
            this.Name = "ScanForm";
            this.Text = "ScanForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScanForm_FormClosing);
            this.Load += new System.EventHandler(this.ScanForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label lblConnection;
        private System.Windows.Forms.Button btnOpenNewForm;
        private System.Windows.Forms.Button scanWeight;
        private System.Windows.Forms.CheckBox chkAutoWeight;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox cmbDevice;
        private System.Windows.Forms.CheckBox chkWaitmode;
        private System.Windows.Forms.Button btnStopScan;
        private System.Windows.Forms.ListBox lstControls;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEnrol;
        private System.Windows.Forms.TextBox txtfname;
        private System.Windows.Forms.TextBox txtlname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblfpstatus;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RFIDLib;
using RFIDLibWeight;



namespace testPro
{
    public partial class ScanForm : Form
    {
        string[] devList;
        public ScanForm()
        {
            InitializeComponent();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            
            RFID.messageDelegate = msgAdd;
            RFID.errorDelegate = errorAdd;
            RFID.deviceStatusDelegate = DevStAdd;
            RFID.ScanDevice();
        }

        private void ScanForm_Load(object sender, EventArgs e)
        {
            RFID.tagsDelegate = tagAdd;
            RFID.messageDelegate = msgAdd;
            RFID.errorDelegate = errorAdd;
            RFID.deviceStatusDelegate = DevStAdd;
            RFID.fpstatusDelegate = fpDevStAdd;

            RFID.IsFPdevice = true;
            devList = RFID.FindDevice();
            
            if (devList != null) { cmbDevice.DataSource = devList; cmbDevice.SelectedIndex = 0; }
        }

        private void btnOpenNewForm_Click(object sender, EventArgs e)
        {
            new ScanForm2().Show();
        }

        private void ScanForm_Activated(object sender, EventArgs e)
        {
            RFID.tagsDelegate = tagAdd;
            RFID.messageDelegate = msgAdd;
            RFID.errorDelegate = errorAdd;
            RFID.deviceStatusDelegate = DevStAdd;
            
        }

        public void tagAdd(string a)
        {
            this.Invoke((MethodInvoker)delegate
            {                
                lblCount.Text = RFID.tags.Count.ToString();
            });

        }
        public void msgAdd(string a)
        {
            lstControls.Invoke((MethodInvoker)delegate
            {
                lstControls.DataSource = null;
                lblCount.Text = "Count";
            });

            this.Invoke((MethodInvoker)delegate
            {


                if (RFID.tags.Count > 0 && a.Equals("Info: Scan Completed"))
                {
                    lstControls.DataSource = RFID.tags;
                    //RFID.LightUpTags(RFID.tags);
                    //MessageBox.Show("Press ok to turn offf");
                    //RFID.LightOffTags();
                }
                lblMessage.Text = a;
            });
            

        }
        public void errorAdd(string a)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblError.Text = a;
            });

        }

        public void DevStAdd(string a)
        {
            this.Invoke((MethodInvoker)delegate
            {
                if (a.Equals("Info : Device Connected"))
                {
                    btnConnect.Enabled = false;
                }
                else if (a.Equals("Info : Device Disconnected"))
                {
                    btnConnect.Enabled = true;
                }
                lblConnection.Text = a;
            });

        }

        public void fpDevStAdd(string a)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblfpstatus.Text = a;
            });

        }

        public void weightAdd(string a,string b)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblCount.Text = a+" : "+b;
            });
        }

        private void scanWeight_Click(object sender, EventArgs e)
        {
            RFID.weightDelegate = weightAdd;
            RFID.ScanWithWeight();            
        }

        private void chkAutoWeight_CheckedChanged(object sender, EventArgs e)
        {
            RFID.weightDelegate = weightAdd;
            if (chkAutoWeight.Checked)
            {
                RFID.EnableAutoWeight();
            }
            else
            {
                RFID.DisableAutoWeight();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (devList != null) { RFID.ConnectDeviceScale(devList[0], ScaleType.SAT); }
            //if (devList != null)
            //{
            //    if (!RFID.ConnectDevice(devList[0]))
            //    {
            //        devList = RFID.FindDevice();
            //        RFID.ConnectDevice(devList[0]);
            //    }
            //}
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            RFID.Dispose();
          
            lstControls.Invoke((MethodInvoker)delegate
            {
                lstControls.DataSource = null;
                lblCount.Text = "Count";
            });
        }

        private void chkWaitmode_CheckedChanged(object sender, EventArgs e)
        {            
            if (chkWaitmode.Checked)
            {
                RFID.EnableWait();
            }
            else
            {
                RFID.DisbleWait();
            }
        }

        private void btnStopScan_Click(object sender, EventArgs e)
        {
            RFID.StopScan();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RFID.Dispose();
        }

        private void ScanForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            RFID.Dispose();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnEnrol_Click(object sender, EventArgs e)
        {
            RFID.enrollUserFingerprint(txtfname.Text, txtlname.Text);
            txtlname.Text = "";
            txtfname.Text = "";
        }


    }
}

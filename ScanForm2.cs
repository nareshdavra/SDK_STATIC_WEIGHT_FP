using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RFIDLib;

namespace testPro
{
    public partial class ScanForm2 : Form
    {
        public ScanForm2()
        {
            InitializeComponent();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            RFID.tagsDelegate = tagAdd;
            RFID.messageDelegate = msgAdd;
            RFID.errorDelegate = errorAdd;
            RFID.deviceStatusDelegate = DevStAdd;
            RFID.ScanDevice();
        }

        private void ScanForm2_Load(object sender, EventArgs e)
        {

        }

        private void ScanForm2_Activated(object sender, EventArgs e)
        {

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
            this.Invoke((MethodInvoker)delegate
            {
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
                lblConnection.Text = a;
            });

        }
    }
}

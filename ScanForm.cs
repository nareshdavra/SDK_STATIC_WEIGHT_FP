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
            //RFID.templateToLoad.Add("AAEAAAD/////AQAAAAAAAAAMAgAAAE1TREtfU0NfRmluZ2VycHJpbnQsIFZlcnNpb249MjAxMi4zLjAuNjcsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbAUBAAAAHFNES19TQ19GaW5nZXJwcmludC5Vc2VyQ2xhc3MEAAAACWZpcnN0TmFtZQhsYXN0TmFtZQ5zdHJGaW5nZXJwcmludBNFbnJvbGxlZEZpbmdlcnNNYXNrAQEGAAgCAAAABgMAAAABdgYEAAAAAWYJBQAAACAAAAARBQAAAAoAAAAGBgAAAIARQVBoL0FjZ3E0M05jd0VFM0NhdHhjTDhVVlpLL2ZqUkg1aFRLRVhnMFhCVnlIVThvcHE0WmhodnNub0NsWXF6Z21DYmlJUWZDdWNXeTlSdVNFZlp4cWdBdEN2K0lPRlIzTmdQRmRHS3FVZ2haQkQ2RlF3aUtVcXNBQVVOdFdkejlsUTExQ3g2WFhCcUJSeUFKSUF3RmlFK241NjJnbk4vUk5WRVhZTEhEQkpzVnRYT2dLaDVRWGQ2RTQwK1JwM2RURUhJV3EwY21lblJ4anZkRnFaUFVtQkJiTmhnRlJ0MFY3R0FRbVBJcGtzMHBEQ2VZdS9PaUlpNGxqNE1zdm0vZmxOV2ZIeWIzTXVwVEMxN1c5Nm82MFhtUjNkcXJ4NVkzRHpPdEE5Szh0L3NDS1Qvd2ZHR3FFVDh2VEg2cy94N3pkajVFTEJwemwrdG1VczRwSGxzT0R6d1BRWnZYaWVoVmdCSHRmcWFRbGtwZDdYZFdUOUI3ZS9uOS9DNTRXTTd4YklCZkZqbVdzNzM0WW9CZnRjYnllU3lTaXVFZUxyL0hhNFBKbnp4bW41cVE3SlBGbmVHcmlFeHZ2bzRNRmw2a3VESVc4My90NTJwTDhJQnFrNlZ0cjE3clRLaVdKbmxUb3A4a0VOU21reXo5SkowWk9DSUM2NWZXaVd0eE9VRVREWGUwYndENGdRSElLdU56WE1CQk53bXJjYkMvRkZXU3Exc0lNd1NFRDByVXJWZXU4cVROdDRyOEtTclNVcFVuam93aFg5ZDVOZ0lYc2tSZWtzRjIvZnFMQXhKU3dqMTYvVVBqTnVXV0FmWjZ2M1ZKdE0xenB1NVhpK3VLNkNpc0FCZncvMS9EaStkc3J2T3pudk41bER2bkU4cDk4T0pxQ0xDbXQwanpKMER3RjNWVm5CRHMrT1U0ay9UbkhJSHZ1THZrbXlHQXE2c1o4cHBtem1IZk1DSzlmNnRObytKb2p5alRiemtBUEFrb3RudXdPOTl5OHV1c3B0YkI4cFJjSUdiZDJXeEhnOVlWMmpXMzluNXU3c2VGTkplVGtwcElDUmRMVlQzbW1tdmRqeGg4Q1VXSys2U1cxa0Jvc3ZQOGtTZnM5aG5hbisvVWY0Z2kyZE5leUIxNXd0SlkyK1RHWEYvZzBUMTQ0NWFXSXM3cXF3cGRTTlZCTE5SUUJpVUxrSDFndGp0Z1AyN2JDT3RsajIxeVFiRWQyd2h4dElOSlV3MW1ualpTcVhMbXB2MkdNM3ZFTmV4a0ZTU28zbjI5ZUVrdGJLTVRER2phRUNmNXE2YVJpcTVmM29rSEphamplVk4vdnBOZldNcDd2Y0pPN3QrSHg3cGhwWThwQlRUVkxxNkEzMkJQUFFTTGJ3RDRnUUhJS3VOelhNQkJOd21yY2JDMUZGV1NlZFlmVXB4MWcxT2IwZktJWUtCMmQvVDl6TU00Y3lPNU9uNjhrMEdFR2pCRzlUVlVIb1ovS1NVamR6YksyZ0E3WnNQTDluSW1mTC95MDk0VnRqTm5oNkcxT0lrRGlLOXVxN1BUZjJTcmIvTjVpaVNUeVRzM1VKVkFyQ0dhODk3WWYxSENhbmsxYzdrbVUvRGtjVlJkN0JkUHVoR3NhZkxlK3Bjb1FoOWZtSG5lc2haU3UreE9lWXBORTVPdTYxRWNUQ3hzeExTM1NQVEFtSjloZUxyZkc4M25xbGY0c0R0ZHhxQ1dzOXpnd2RVQjlWd3J5V3Q5Z3lSYU5JUkRFMW1ha2FhaTh0RHhNVFBTZU04NGtzbVZlbjZXd0hwZy8xRDhVUWQ4bFRDSHJYZ2IyVEVDQUJveVdWL2YzeFFQeDZvSmkvS3lITEZpTUsyQmVsN0Y5NWJpUUJ1c2taWWVlZ0hBSGRLV2E3cFZSNThwZ2lXUHh2NHFOTWZ6THRpYVBUYkkzQ1FZaXBjdHhNNk44RFEyanRKWlBKMnZZRFZpSFdSUDFqdENNUTd4dm1yTDhEMytQRTRPejNtazlHQlBlYStJUlVHVytVS0MwMkFsS1VBR2Y5MnpWS0VTYzgxalBKMzR2RjFNb2ZBcDk5ZzRid0RvZmdISUt1TnpYTUJCTndtcmNUQ2FGRldTcFZoeTlTQnJhQVByT040UUdJczJkK2libkc3aFkxb2ZiTjdXN1JZbE9SM2R4TGJZa01tV040R1hycXhTUnhzRXZlMHVQS3BuZC9VNURrNjN1SmgxM1huRjdscnQxUk12c01xWkpkL3oyZFdSaXVjRkh5QytJOTk0QmovZWdhR3FycTVJWGU4MnBCblhZUzAyYkNFdTdqMkJPN1BLZ0dDVUNFMWkwb21ydEVhM3pKM2RHcjVIUUlsZzFZQWFzaFdqRDg4Z1RvcjBDeUhLdzBCR243MHhwWkdsaG5ua2lEMnFYYjRwK1RKbG5BUVVBUHFBVUpCN2RtMXZ1bTM5YVc4TDJBd05oRG02Q1lva0gwL0JENHdqVm1UVVArdnVGM2xlWXQ0YXdVNDJmSGVTekp1YlkrQyswMnVqQXlEb2k3UElvT2hkRkRJVk40RUtYWFpMSFA1RGJyK0ErSXl4cVJTcHdRMUFzcWNpR3pISzlnRlpNTUJrR1l4Tm9UR20ya2RtdDJTY0tQTXhIM0tzemcwQi95bC8vcHJvdm5pdGNYajdqQnlSVXpiT040MGFlZzdpN3NMMXhBc2ltYVlaajR4RC9iZlZjQkhXUW8ydjkydHRZYTE1c05La3BsT3F2SS8xNC9nL1VwaExid0FBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQQ0JCw==");
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
            string fptemplate = RFID.enrollUserFingerprint(txtfname.Text, txtlname.Text);
            
            txtlname.Text = "";
            txtfname.Text = "";
        }


    }
}

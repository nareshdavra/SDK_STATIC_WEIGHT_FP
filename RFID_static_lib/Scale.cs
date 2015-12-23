using System;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.ComponentModel;
using System.Collections.Generic;

using System.Threading;
using System.IO;
using System.Text.RegularExpressions;



namespace RFIDLibWeight
{
    public delegate void WeightScaleDelegate(string info);

    public enum ScaleType
    {
        SAT,
        MAT,
        CTZ
    }

    public class Scale
    {
        public static ScaleType Scaletype = ScaleType.SAT;
        //public static event WeightScaleDelegate NotifyWeightEvent;
        private static AutoResetEvent mWait = new AutoResetEvent(false);
        private static SerialPort serialPort = new SerialPort();
        public static string lastScaledWeight = "";
        public static string ErrorMsg = "";
        public static bool IsConnected = false;

        public static string baudrate = "";
        public static string databit = "";
        public static string parity = "";
        public static string stopbit = "";
        public static string scale = "";

        private static bool IsConnectionCheck = false;

        ~Scale()
        {
            if (serialPort != null)
                closePort();
        }


        private static void comport_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            // Show the state of the pins
            //UpdatePinState(); reconnect device
        }

        public static void comport_Error(object sender, SerialErrorReceivedEventArgs e)
        {
            // Show the state of the pins
            //UpdatePinState(); reconnect device
        }


        private static void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // If the com port has been closed, do nothing
            if (!serialPort.IsOpen) return;
            if (IsConnectionCheck) { Thread.Sleep(100); }
            lastScaledWeight = "";

            //while (serialPort.BytesToRead > 0)
            //{
            lastScaledWeight = serialPort.ReadTo("\n");
            //}
            
            lastScaledWeight.Trim();
            lastScaledWeight = Regex.Match(lastScaledWeight, @"[0-9]+(\.)[0-9]+").Value;
            lastScaledWeight.Trim();


            //Thread.Sleep(75);
            //if (lastScaledWeight == "") { getWeight(); return; }
            if ( lastScaledWeight!=null && lastScaledWeight != String.Empty && lastScaledWeight.Length >= 4) { mWait.Set(); }
        }


        private static bool openPort(string strCom)
        {

            if (serialPort != null)
            {
                if (serialPort.IsOpen)
                    serialPort.Close();
                serialPort = null;
            }
            try
            {
                int baudRate = Convert.ToInt32(Scale.baudrate);
                int databits = Convert.ToInt32(Scale.databit);
                Parity parity = (Parity)Enum.Parse(typeof(Parity), Scale.parity);
                StopBits stopbits = (StopBits)Enum.Parse(typeof(Parity), Scale.stopbit);


                serialPort = new SerialPort(strCom, baudRate, parity, databits, stopbits);
                serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(comport_Error);
                serialPort.PinChanged += new SerialPinChangedEventHandler(comport_PinChanged);
                                
                serialPort.Open();
                if (serialPort.IsOpen)
                {
                    GC.SuppressFinalize(serialPort.BaseStream);
                }
                //if(S)

                if (Scaletype == ScaleType.MAT)
                {
                    serialPort.DtrEnable = false;
                    serialPort.RtsEnable = false;
                    serialPort.Handshake = Handshake.None;
                    IsConnectionCheck = true;
                    getWeight();                   
                    mWait.WaitOne(5000);
                    IsConnectionCheck = false;
                    
                    return (lastScaledWeight != string.Empty && lastScaledWeight !=null) ? true : false;
                }
                else
                {
                    serialPort.DtrEnable = true;
                    serialPort.RtsEnable = true;
                    //serialPort.
                    serialPort.Handshake = Handshake.RequestToSendXOnXOff;
                    IsConnectionCheck = true;
                    getWeight();
                    mWait.WaitOne(9000);
                    IsConnectionCheck = false;
                    return (lastScaledWeight != string.Empty && lastScaledWeight != null) ? true : false;
                }
            }
            catch (Exception exp)
            {
                ErrorMsg = exp.Message;
                return false;
            }
        }

        public static void closePort()
        {
            if (serialPort != null)
            {
                if (serialPort.IsOpen)
                {
                    serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                    //serialPort.ErrorReceived -= new SerialErrorReceivedEventHandler(OnErrorReceived);
                    serialPort.PinChanged -= new SerialPinChangedEventHandler(comport_PinChanged);

                    GC.ReRegisterForFinalize(serialPort.BaseStream);
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    serialPort.Close();
                }
                serialPort.Dispose();
                serialPort = null;
            }
        }

        private static void getWeight()
        {
            lastScaledWeight = null;
            if (Scale.Scaletype == ScaleType.SAT)
            {

                byte[] buffer;
                buffer = new byte[2];
                buffer[0] = 0x1B;
                buffer[1] = 0x50;
                serialPort.Write(buffer, 0, buffer.Length);
            }
            else if (Scale.Scaletype == ScaleType.MAT)
            {
                serialPort.Write("S\r\n");
            }
            //Thread.Sleep(90);

        }

        public static string weighScale()
        {
            if (baudrate == "" || databit == "" || stopbit == "" || parity == "")
            {
                ErrorMsg = "PLEASE SET SCALE PARAMETERS";
                return "";
            }
            //if (NotifyWeightEvent == null)
            //{
            //    ErrorMsg = "PLEASE ASSIGN SCALE EVENT PARAMETERS";
            //    return "";
            //}


            if (!IsConnected)
            {
                if (!FindAndConnectScale())
                {
                    ErrorMsg = "NO SCALE FOUND";
                }
            }

            if (serialPort == null)
            {
                ErrorMsg = "NO SCALE FOUND";
            }
            if (!serialPort.IsOpen)
            {
                ErrorMsg = "PORT CLOSED";
            }
            getWeight();
            mWait.Reset();
            mWait.WaitOne();
            return lastScaledWeight;
        }

        public static bool FindAndConnectScale()
        {
            List<string> list = new List<string>(SerialPort.GetPortNames());
            using (List<string>.Enumerator enumerator = list.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (openPort(enumerator.Current))                    
                    {
                        IsConnected = true;
                        return true;
                    }
                }
                closePort();
                return false;
            }
        }
        public static bool FindAndConnectScale(string port)
        {
            if (openPort(port))
            {
                IsConnected = true;
                return true;
            }
            else
            {
                closePort();
                return false;
            }
        }

    }

}

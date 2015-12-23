
using DataClass;
using SDK_SC_RFID_Devices;
using SDK_SC_RfidReader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SDK_SC_Fingerprint;

using RFIDLibWeight;
using System.Threading;
using System.IO.Ports;
using System.IO;

namespace RFIDLib
{
    public class RFID
    {
        private static rfidPluggedInfo[] arrayOfPluggedDevice = null;
        private static string[] fpDevArray = null;
        private static int selectedDevice = -1;
        private static int selectedFP = 0;
        //private static string[] listOfFPDevice;
        private static ArrayList templateToLoad = new ArrayList();

        private static RFID_Device device = null;
        public static bool IsDeviceConnected = false;
        public static bool IsFPdevice = false;

        public static bool IsInScan = false;

        public static ArrayList tags = new ArrayList();
        private static bool IsWaitMode = false;
        private static bool IsScanWithWeight = false;
        private static bool IsWeighing = false;
        private static bool IsLightOn = false;
        private static bool IsAutoWeightOn = false;
        
        public  delegate void msgDel(string str);
        public  delegate void tagDel(string str);
        public  delegate void devStatDel(string str);
        public  delegate void errDel(string str);
        public delegate void weightDel(string tag,string weight);
       
        public delegate void FPdevStatDel(string str);

        public static msgDel messageDelegate;
        public static tagDel tagsDelegate;
        public static devStatDel deviceStatusDelegate;
        public static FPdevStatDel fpstatusDelegate;
        public static errDel errorDelegate;
        public static weightDel weightDelegate;


        private const int nbMaxUser = 100;
        private static int indexUser = 0;
       public static UserClassTemplate[] userArray = new UserClassTemplate[nbMaxUser];

        public static string[] FindDevice()
        {
            try
            {
                string[] listOfDevice;

                RFID.arrayOfPluggedDevice = null;
                RFID_Device device1 = new RFID_Device();
            
                RFID.arrayOfPluggedDevice = device1.getRFIDpluggedDevice(true);

                if (RFID.IsFPdevice == true)
                {
                    RFID.fpDevArray = device1.getFingerprintPluggedGUID();
                }
                device1.ReleaseDevice();

                if (RFID.arrayOfPluggedDevice == null)
                {
                    return null;
                }
                else
                {
                    listOfDevice = new string[arrayOfPluggedDevice.Length];
                    
                    
                    for (int i = 0; i < arrayOfPluggedDevice.Length; i++)
                    {
                        if (!listOfDevice.Contains(arrayOfPluggedDevice[i].SerialRFID.ToString()))
                        { listOfDevice[i] = arrayOfPluggedDevice[i].SerialRFID.ToString(); }
                    }

                    messageDelegate("Device Found");
                    return listOfDevice;
                  }
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
                return null;
            }
        }

        public static bool ConnectDevice(string serial)
        {
            Dispose();
            try
            {
                if (RFID.IsDeviceConnected) { return true; }

                selectedDevice = Array.FindIndex(arrayOfPluggedDevice, row => row.SerialRFID == serial);
                
                if (RFID.device != null)
                {
                    if (IsWaitMode)
                    {
                        device.DisableWaitMode();
                    }

                    if (RFID.device.ConnectionStatus == ConnectionStatus.CS_Connected)
                    {
                        RFID.device.ReleaseDevice();
                    }
                }
                RFID.device = new RFID_Device();

                RFID.device.NotifyRFIDEvent += new NotifyHandlerRFIDDelegate(RFID.rfidDev_NotifyRFIDEvent);
                
                bool result;

                if (IsFPdevice == true && fpDevArray != null)
                {
                    RFID.device.NotifyFPEvent += new NotifyHandlerFPDelegate(RFID.rfidDev_NotifyFPEvent);
                    if (RFID.device.Create_1FP_Device(arrayOfPluggedDevice[selectedDevice].SerialRFID, arrayOfPluggedDevice[selectedDevice].portCom, fpDevArray[selectedFP], false))
                    {
                        //raiseMsgEvent("Device created", null);
                        device.get_RFID_Device.DebugReader = true;
                        loadUserTemplate();
                        messageDelegate("Device with FP Connected");
                        result = true;
                    }
                    else
                    {
                        //raiseMsgEvent("Device not created", null);
                        result = false;
                    }
                    return result;
                }
                else
                {
                    //if (RFID.arrayOfPluggedDevice[RFID.selectedDevice].deviceType == DeviceType.DT_SBR || RFID.arrayOfPluggedDevice[RFID.selectedDevice].deviceType == DeviceType.DT_STR || RFID.arrayOfPluggedDevice[RFID.selectedDevice].deviceType == DeviceType.DT_SFR)
                    if (RFID.device.Create_NoFP_Device(RFID.arrayOfPluggedDevice[RFID.selectedDevice].SerialRFID, RFID.arrayOfPluggedDevice[RFID.selectedDevice].portCom))
                    {
                        //raiseMsgEvent("Device created", null);
                        device.get_RFID_Device.DebugReader = true;
                        messageDelegate("Device Connected");
                        result = true;
                    }
                    else
                    {
                        //raiseMsgEvent("Device not created", null);
                        result = false;
                    }
                    return result;
                }
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
                return false;
            }
        }

        public static bool ConnectDeviceScale(string serial, ScaleType t)
        {
            try
            {

                if (ConnectDevice(serial))
                {
                    if (t == ScaleType.SAT)
                    { 
                        return RFID.connectScale("1200", "1", "7", "1", t);
                    }
                    else if (t == ScaleType.MAT)
                    {
                        return RFID.connectScale("2400", "2", "7", "1", t);
                    }
                    else
                    {
                        errorDelegate("scale not connected");
                        return false;
                    }
                }
                else
                {
                    errorDelegate("Device not connected");
                    return false;
                }
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
                return false;
            }
        }

        public static bool ConnectDeviceScale(string serial,string buad, string par, string databit, string stbit,ScaleType t)
        {
            try
            {

                if (ConnectDevice(serial))
                {
                    if (!RFID.connectScale(buad, par, databit, stbit, t))
                    {
                        errorDelegate("scale not connected");
                        return false;
                    }
                    else
                    { return true; }
                }
                else
                {
                    errorDelegate("Device not connected");
                    return false;
                }
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
                return false;
            }
        }

        public static bool ConnectDeviceScale(string serial, string buad, string par, string databit, string stbit, ScaleType t,string port)
        {
            try
            {
                if (ConnectDevice(serial))
                {
                    if (!RFID.connectScale(buad, par, databit, stbit, t, port))
                    {
                        errorDelegate("scale not connected");
                        return false;
                    }
                    else
                    { return true; }
                }
                else
                {
                    errorDelegate("Device not connected");
                    return false;
                }
            }
            catch(Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
                return false;
            }
        }

        private static void DisposeDevice()
        {
            if (RFID.device != null)
            {
                if (IsWaitMode)
                {
                    if (RFID.device.DeviceStatus == DeviceStatus.DS_InScan)
                    {
                        device.StopScan();
                    }
                    device.DisableWaitMode();
                }
                RFID.device.ReleaseDevice();
            }
        }

        private static void rfidDev_NotifyFPEvent(object sender, SDK_SC_Fingerprint.FingerArgs args)
        {
            switch (args.RN_Value)
            {
                case SDK_SC_Fingerprint.FingerArgs.FingerNotify.RN_FingerprintConnect:
                    fpstatusDelegate("FP Connected");
                  //  FPstatusDelegate("FP Connected");
                    break;

                case SDK_SC_Fingerprint.FingerArgs.FingerNotify.RN_FingerprintDisconnect:
                    //FPstatusDelegate("FP DisConnected");
                    fpstatusDelegate("FP DisConnected");
                    break;
                case SDK_SC_Fingerprint.FingerArgs.FingerNotify.RN_FingerTouch:
                   // FPstatusDelegate("FP Device Touch");
                    fpstatusDelegate("FP Device Touch");
                    break;

                case SDK_SC_Fingerprint.FingerArgs.FingerNotify.RN_FingerGone:
                   // FPstatusDelegate("FP FingerGone");
                    fpstatusDelegate("FP fingerGone");
                    break;
                case SDK_SC_Fingerprint.FingerArgs.FingerNotify.RN_FingerUserUnknown:
                   // FPstatusDelegate("FP touch By Unknown");
                    fpstatusDelegate("FP touch By Unknown");
                    break;
                case SDK_SC_Fingerprint.FingerArgs.FingerNotify.RN_AuthentificationCompleted:

                    string[] strUser = args.Message.Split(';');
                    string type = strUser[0];
                    string empid = strUser[1];
                    SDK_SC_Fingerprint.FingerIndexValue fingerUsed = (SDK_SC_Fingerprint.FingerIndexValue)int.Parse(strUser[2]);
                    string userInfo = type + " " + empid + "(" + fingerUsed.ToString() + ")";
                    if (Scale.IsConnected) 
                    {
                        ScanWithWeight();
                    }
                    else
                    {
                        ScanDevice();
                    }
                    fpstatusDelegate("Info FP : User"+userInfo);
                   // FPstatusDelegate("Info FP : User"+userInfo");
                    break;
            }
        }


        private static void rfidDev_NotifyRFIDEvent(object sender, rfidReaderArgs args)
        {
            try
            {
                switch (args.RN_Value)
                {
                    case rfidReaderArgs.ReaderNotify.RN_FailedToConnect:
                        errorDelegate("Info : Failed to Connect");                        
                        break;

                    case rfidReaderArgs.ReaderNotify.RN_Connected:
                        RFID.IsDeviceConnected = true;
                        deviceStatusDelegate("Info : Device Connected");
                        break;

                    case rfidReaderArgs.ReaderNotify.RN_Disconnected:
                        RFID.IsDeviceConnected = false;
                        deviceStatusDelegate("Info : Device Disconnected");                        
                        break;

                    case rfidReaderArgs.ReaderNotify.RN_ScanStarted:
                        messageDelegate("Info : Scan Started");
                        break;

                    case rfidReaderArgs.ReaderNotify.RN_TagAdded:
                        
                        if (!RFID.tags.Contains(args.Message))
                        {
                            RFID.tags.Add(args.Message);

                            bool MG = (IsScanWithWeight == true || IsAutoWeightOn == true);
                            if (MG == false)
                            {
                                tagsDelegate(args.Message);
                            }
                            if (IsAutoWeightOn && IsScanWithWeight == false)
                            {
                                IsScanWithWeight = true;
                            }
                        }
                        break;

                    case rfidReaderArgs.ReaderNotify.RN_ScanCompleted:
                        if (IsScanWithWeight && Scale.IsConnected)
                        {
                            if (device.LastScanResult.listTagAll.Count == 1)
                            {
                                String weight = Scale.weighScale();
                                if (IsFPdevice == true)
                                {
                                    weightDelegate(device.LastScanResult.listTagAll[0].ToString(), weight);
                                }
                                else
                                {
                                    weightDelegate(device.LastScanResult.listTagAll[0].ToString(), weight);
                                }
                                    IsScanWithWeight = false;
                            }
                            else if (device.LastScanResult.listTagAll.Count > 1)
                            {
                                errorDelegate("More than one tags detected");
                            }
                            else
                            {
                                errorDelegate("No tags detected");
                            }
                        }

                        IsInScan = false;
                        if (IsFPdevice == true)
                        {
                            messageDelegate("Info: Scan Completed@" + device.LastScanResult.userFirstName + "_" + device.LastScanResult.userLastName);
                        }
                        else
                        {
                            messageDelegate("Info: Scan Completed");
                        }


                        if (RFID.IsWaitMode)
                        {
                            RFID.device.EnableWaitMode();
                        }
                        break;

                    case rfidReaderArgs.ReaderNotify.RN_ReaderFailToStartScan:

                        device.DisableWaitMode();
                        if (IsWaitMode) { device.EnableWaitMode(); }
                        ScanDevice();
                        //errorDelegate("Info : Fail to start Scan");
                        break;

                    case rfidReaderArgs.ReaderNotify.RN_ReaderScanTimeout:
                    case rfidReaderArgs.ReaderNotify.RN_ErrorDuringScan:
                        device.StopScan();
                        //errorDelegate("Info : Scan has error");
                        break;

                    case rfidReaderArgs.ReaderNotify.RN_ScanCancelByHost:
                        device.DisableWaitMode();
                        if (IsWaitMode) { device.EnableWaitMode(); }
                        errorDelegate("Info : Scan cancel by host");
                        break;

                    case rfidReaderArgs.ReaderNotify.RN_TagPresenceDetected:
                        RFID.device.DisableWaitMode();
                        RFID.Scan();
                        break;

                    case rfidReaderArgs.ReaderNotify.RN_Power_OFF:
                    case rfidReaderArgs.ReaderNotify.RN_UsbCableUnplug:
                        IsDeviceConnected = false;
                        Dispose();
                        deviceStatusDelegate("Info : Device Disconnected");
                        break;

                }
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n"+DateTime.Now.ToLongTimeString()+":Expection Message:"+exp.Message+ "stack trace:"+exp.StackTrace);
                MessageBox.Show(exp.Message);
            }
            Application.DoEvents();
        }


        private static void Scan()
        {
            if (!RFID.IsWaitMode)
            {
                RFID.tags.Clear();
            }
            if (RFID.device != null)
            {
                if (RFID.device.DeviceStatus == DeviceStatus.DS_WaitTag) { device.DisableWaitMode(); }
                if (RFID.device.ConnectionStatus == ConnectionStatus.CS_Connected && RFID.device.DeviceStatus == DeviceStatus.DS_Ready)
                {
                    RFID.device.ScanDevice(true,true); IsInScan = true;
                }
            }
        }

        public static void StopScan()
        {
            try
            {
                if (RFID.device != null)
                {
                    RFID.IsInScan = false;
                    if (RFID.device.DeviceStatus == DeviceStatus.DS_InScan)
                    {
                        RFID.device.StopScan();                        
                    }
                    DisableAutoWeight();
                }
            }
            catch(Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
            }
        }

        public static string ScanDevice()
        {
            if (IsWaitMode) return "No Scan allowed in waitmode";
            if (IsLightOn) return "No Scan allowed while Lighting";
            if (tagsDelegate == null || messageDelegate == null || errorDelegate == null || deviceStatusDelegate == null) return "Please assign call back functions";

            string result;
            if (RFID.IsDeviceConnected)
            {
                try
                {
                    RFID.Scan();
                    result = "Success";
                }
                catch (Exception exp)
                {
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
                    result = "Exception In Scan";
                }
            }
            else
            {
                result = "Device Not Connected";
            }                            
            return result;
        }

        public static void EnableWait()
        {
            try
            {
                if (device == null) return;
                RFID.IsWaitMode = true;
                tags.Clear();
                RFID.device.EnableWaitMode();
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
            }
        }

        public static void DisbleWait()
        {
            try
            {
                if (device == null) return;
                RFID.tags.Clear();
                RFID.IsWaitMode = false;
                if (IsScanWithWeight) IsScanWithWeight = false;
                if (IsDeviceConnected == false) return;

                if (device.DeviceStatus == DeviceStatus.DS_InScan)
                {
                    StopScan();
                    IsInScan = false;
                }
                else
                {
                    device.DisableWaitMode();
                }
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
            }
        }

        public static void LightUpTags(ArrayList tags)
        {
            try
            {
                if (tags == null || device == null) return;
                if (!IsLightOn)
                {
                    List<string> list = tags.Cast<string>().ToList();
                    RFID.device.TestLighting(list);
                    IsLightOn = true;
                    Thread.Sleep(200);
                }
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
            }
        }

        public static void LightOffTags()
        {
            try
            {

                if (device == null) return;
                IsLightOn = false;
                RFID.device.StopLightingLeds();
                Thread.Sleep(200);
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
            }
        }

        public static void Dispose()
        {
            try
            {
                if (Scale.IsConnected)
                {
                    Scale.closePort();
                }
                DisposeDevice();
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
            }
        }

        public static string ScanWithWeight()
        {
            try
            {
                if (tagsDelegate == null || messageDelegate == null || errorDelegate == null || deviceStatusDelegate == null || weightDelegate == null) return "Please assign call back functions";
                string result;
                IsScanWithWeight = true;

                if (RFID.IsDeviceConnected)
                {
                    RFID.Scan();
                    result = "Success";
                }
                else
                {
                    result = "Device Not Connected";
                }
                return result;
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
                return "Error";
            }
        }

        public static bool connectScale(string BaudRate, string Parity, string DataBit, string StopBit, string scaleComP)
        {
            try
            {

                Scale.baudrate = BaudRate;//"1200";
                Scale.parity = Parity;//"1";
                Scale.databit = DataBit;// "7";
                Scale.stopbit = StopBit;// "1";
                Scale.Scaletype = (ScaleType)Enum.Parse(typeof(ScaleType), scaleComP);
                return Scale.FindAndConnectScale();
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
                return false;
            }
        }

        public static bool connectScale(string BaudRate, string Parity, string DataBit, string StopBit, ScaleType t)
        {
            try
            {
                Scale.baudrate = BaudRate;//"1200";
                Scale.parity = Parity;//"1";
                Scale.databit = DataBit;// "7";
                Scale.stopbit = StopBit;// "1";
                Scale.Scaletype = t;
                return Scale.FindAndConnectScale();
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
                return false;
            }
        }

        public static bool connectScale(string BaudRate, string Parity, string DataBit, string StopBit, ScaleType scaleComP, string comport)
        {
            try
            {
                Scale.baudrate = BaudRate;//"1200";
                Scale.parity = Parity;//"1";
                Scale.databit = DataBit;// "7";
                Scale.stopbit = StopBit;// "1";
                Scale.Scaletype = scaleComP;
                return Scale.FindAndConnectScale(comport);
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
                return false;
            }
        }


        public static void discinnectScale()
        {
            Scale.closePort();
        }


        public static void EnableAutoWeight()
        {
            try
            {
                if (IsAutoWeightOn) { return; }
                if (device == null) return;
                IsScanWithWeight = IsDeviceConnected && Scale.IsConnected;
                RFID.IsAutoWeightOn = IsScanWithWeight;
                RFID.IsWaitMode = true;
                tags.Clear();
                RFID.device.EnableWaitMode();
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
            }        
        }

        public static void DisableAutoWeight()
        {
            try
            {
                if (device == null) return;
                if (IsAutoWeightOn) { IsAutoWeightOn = false; }
                RFID.tags.Clear();
                RFID.IsWaitMode = false;
                if (IsScanWithWeight) { IsScanWithWeight = false; }
                if (IsDeviceConnected == false) return;

                if (device.DeviceStatus == DeviceStatus.DS_InScan)
                {
                    StopScan();
                }
                device.DisableWaitMode();
            }
            catch (Exception exp)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//logs.rf", "\n" + DateTime.Now.ToLongTimeString() + ":Expection Message:" + exp.Message + "stack trace:" + exp.StackTrace);
            }
        }

        public static bool ReScanWeight(string tag)
        {
            if(!IsAutoWeightOn) return false;

            if (tag.Length == 10)
            {
                int ind = tags.IndexOf(tag);                
                tags.RemoveAt(ind);
                return true;
            }
            return false;
        }

        public static void weightDetail(string weight)
        {
            IsScanWithWeight = false;
        }

        public static void loadUserTemplate()
        {
            if (templateToLoad.Count > 0 && IsFPdevice == true)
            {
                device.LoadFPTemplate((string[])templateToLoad.ToArray(typeof(string)), device.get_FP_Master);
            }
        }

        public static string enrollUserFingerprint(string fname, string lname)
        {
            if (IsFPdevice != true){return ""; }
           
            string template = device.EnrollUser(null, fname, lname, null);
            templateToLoad.Add(template);
            loadUserTemplate();
            return template;
        }

        public static string modifyUserFingerprint(string fname, string lname, string fpTemplate)
        {
            if (IsFPdevice != true) { return ""; }

            string template = device.EnrollUser(null, fname, lname, fpTemplate);
            templateToLoad.Remove(fpTemplate);
            templateToLoad.Add(template);
            loadUserTemplate();
            return template;
        }
    }
}

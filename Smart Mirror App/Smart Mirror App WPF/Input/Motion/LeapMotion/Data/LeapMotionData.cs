using System;
using System.Diagnostics;
using System.Windows.Documents;
using Leap;

namespace Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data
{
    class LeapMotionData : IMotionData
    {
        public Hands Hands { get; set; }
        public Fingers Fingers { get; set; }
        public Tools Tools { get; set; }
        public Gestures Gestures { get; set; }

        public LeapMotionData()
        {
            this.Hands = new Hands();
            this.Fingers = new Fingers();
            this.Tools = new Tools();
            this.Gestures = new Gestures();
        }
        
        public bool UpdateData(Controller controller)
        {
            Frame frame = controller.Frame();
      
            HandList hands = frame.Hands;
            FingerList fingers = frame.Fingers;
            ToolList tools = frame.Tools;
            GestureList gestures = frame.Gestures();

            //float fps = frame.CurrentFramesPerSecond;
            //long timestamp = frame.Timestamp;
            //BugReport bugReport = controller.BugReport;
            //Config config = controller.Config;
            //DeviceList devices = controller.Devices;
            //ImageList images = controller.Images;
            //bool hasFocus = controller.HasFocus;
            //bool isConnected = controller.IsConnected;

            try
            {
                this.Hands.UpdateData(hands);
                this.Fingers.UpdateData(fingers);
                this.Tools.UpdateData(tools);
                this.Gestures.UpdateData(gestures);

                return true;
            }
            catch (Exception e)
            {
                
                Debug.WriteLine("LeapMotionData.UpdateData: " + e);
                return false;
            }
        }
    
    }
}

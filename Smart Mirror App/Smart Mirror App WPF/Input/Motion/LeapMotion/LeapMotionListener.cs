using System.Diagnostics;
using Leap;
using Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data;

namespace Smart_Mirror_App_WPF.Input.Motion.LeapMotion
{
    class LeapMotionListener : Listener
    {
        private readonly LeapMotionData _data;

        public LeapMotionListener(LeapMotionData data)
        {
            _data = data;
        }

        public override void OnInit(Controller controller)
        {
            Debug.WriteLine("Initialized");
        }

        public override void OnConnect(Controller controller)
        {
            // SOURCE: LeapMotion Sample Example
            controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
            controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
            controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
            controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
        }

        public override void OnDisconnect(Controller controller)
        {
            Debug.WriteLine("Disconnected");
        }

        public override void OnExit(Controller controller)
        {
            Debug.WriteLine("Exited");
        }

        public override void OnFrame(Controller controller)
        {
            _data.UpdateData(controller);
        }
    }
}

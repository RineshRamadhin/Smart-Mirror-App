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

        /// <summary>
        /// Sets the settings for the leapMotion on first connection
        /// </summary>
        /// <param name="controller"></param>
        public override void OnConnect(Leap.Controller controller)
        {
            // SOURCE: LeapMotion Sample Example
            controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
            controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
            controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
            controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
        }

        /// <summary>
        /// Get's called by the leapMotion with each frame
        /// </summary>
        /// <param name="controller">The Controller data at that specific frames</param>
        public override void OnFrame(Leap.Controller controller)
        {
            _data.UpdateData(controller);
        }
    }
}

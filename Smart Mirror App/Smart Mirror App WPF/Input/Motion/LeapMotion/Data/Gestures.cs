using System;
using System.Diagnostics;
using Leap;

namespace Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data
{
    class Gestures : IMotionData
    {
        public string Prop { get; set; }

        public Gestures()
        {
            Prop = "TODO";
        }

        public bool UpdateData(GestureList gestures)
        {
            if (gestures == null) throw new ArgumentNullException(nameof(gestures));

            for (int i = 0; i < gestures.Count; i++)
            {
                Gesture gesture = gestures[i];


                switch (gesture.Type)
                {
                    case Gesture.GestureType.TYPE_CIRCLE:
                        CircleGesture circle = new CircleGesture(gesture);

                        var clockwiseness = circle.Pointable.Direction.AngleTo(circle.Normal) <= Math.PI / 2 ? "clockwise" : "counterclockwise";

                        float sweptAngle = 0;

                        Debug.WriteLine("  Circle id: " + circle.Id
                                        + ", " + circle.State
                                        + ", progress: " + circle.Progress
                                        + ", radius: " + circle.Radius
                                        + ", angle: " + sweptAngle
                                        + ", " + clockwiseness);
                        break;
                    case Gesture.GestureType.TYPE_SWIPE:
                        SwipeGesture swipe = new SwipeGesture(gesture);
                        Debug.WriteLine("  Swipe id: " + swipe.Id
                                        + ", " + swipe.State
                                        + ", position: " + swipe.Position
                                        + ", direction: " + swipe.Direction
                                        + ", speed: " + swipe.Speed);
                        break;
                    case Gesture.GestureType.TYPE_KEY_TAP:
                        KeyTapGesture keytap = new KeyTapGesture(gesture);
                        Debug.WriteLine("  Tap id: " + keytap.Id
                                        + ", " + keytap.State
                                        + ", position: " + keytap.Position
                                        + ", direction: " + keytap.Direction);
                        break;
                    case Gesture.GestureType.TYPE_SCREEN_TAP:
                        ScreenTapGesture screentap = new ScreenTapGesture(gesture);
                        Debug.WriteLine("  Tap id: " + screentap.Id
                                        + ", " + screentap.State
                                        + ", position: " + screentap.Position
                                        + ", direction: " + screentap.Direction);
                        break;
                    default:
                        Debug.WriteLine("  Unknown gesture type.");
                        break;
                }
            }

            try
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

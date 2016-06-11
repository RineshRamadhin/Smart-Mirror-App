using System;
using System.Diagnostics;
using Leap;

namespace Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data
{
    class Gestures : IMotionData
    {
        private bool _activeGesture;
        private Gesture.GestureType _currentGesture;

        public Gestures()
        {
            _activeGesture = false;
        }

        public bool UpdateData(GestureList gestures)
        {
            if (gestures == null) throw new ArgumentNullException(nameof(gestures));

            if (gestures.Count == 0)
                _activeGesture = false;

            try
            {
                foreach (Gesture gesture in gestures)
                {
                    _currentGesture = gesture.Type;

                    switch (gesture.Type)
                    {
                        case Gesture.GestureType.TYPE_CIRCLE:
                            OnCircle(gesture);
                            break;
                        case Gesture.GestureType.TYPE_SWIPE:
                            OnSwipe(gesture);
                            break;
                        case Gesture.GestureType.TYPE_KEY_TAP:
                            OnKeyTap(gesture);
                            break;
                        case Gesture.GestureType.TYPE_SCREEN_TAP:
                            OnScreenTap(gesture);
                            break;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void OnKeyTap(Gesture gesture)
        {
            KeyTapGesture keytap = new KeyTapGesture(gesture);

            if (!_activeGesture)
            {
                _activeGesture = true;
            }


            Debug.WriteLine("  Tap id: " + keytap.Id
                            + ", " + keytap.State
                            + ", position: " + keytap.Position
                            + ", direction: " + keytap.Direction);
        }

        private void OnScreenTap(Gesture gesture)
        {
            ScreenTapGesture screentap = new ScreenTapGesture(gesture);

            Debug.WriteLine("  Tap id: " + screentap.Id
                            + ", " + screentap.State
                            + ", position: " + screentap.Position
                            + ", direction: " + screentap.Direction);
        }

        private void OnSwipe(Gesture gesture)
        {
            SwipeGesture swipe = new SwipeGesture(gesture);

            Debug.WriteLine("  Swipe id: " + swipe.Id
                            + ", " + swipe.State
                            + ", position: " + swipe.Position
                            + ", direction: " + swipe.Direction
                            + ", speed: " + swipe.Speed);
        }

        private void OnCircle(Gesture gesture)
        {
            CircleGesture circle = new CircleGesture(gesture);

            if (!_activeGesture)
            {
                _activeGesture = true;
            }


            var clockwiseness = circle.Pointable.Direction.AngleTo(circle.Normal) <= Math.PI / 2 ? "clockwise" : "counterclockwise";

            float sweptAngle = 0;

            Debug.WriteLine("  Circle id: " + circle.Id
                            + ", " + circle.State
                            + ", progress: " + circle.Progress
                            + ", radius: " + circle.Radius
                            + ", angle: " + sweptAngle
                            + ", " + clockwiseness);
        }

    }
}

using System;
using System.Diagnostics;
using Leap;

namespace Smart_Mirror_App_WPF.Input.Motion.LeapMotion.Data
{
    class Gestures : IMotionData
    {
        private bool _activeGesture;
        private Action _onTap;
        private Action _onScreenTap;
        private Action<Vector> _onSwipe;
        private Action<bool> _onCircle;

        /// <summary>
        /// Initializes Gestures with callback's
        /// </summary>
        /// <param name="onTap"></param>
        /// <param name="onScreenTap"></param>
        /// <param name="onSwipe"></param>
        /// <param name="onCircle"></param>
        public Gestures(Action onTap, Action onScreenTap, Action<Vector> onSwipe, Action<bool> onCircle)
        {
            _activeGesture = false;
            this._onTap = onTap;
            this._onScreenTap = onScreenTap;
            this._onSwipe = onSwipe;
            this._onCircle = onCircle;
        }

        /// <summary>
        /// Updates gestures with data from receiving gestureList
        /// </summary>
        /// <param name="gestures">list of recognized gestures</param>
        /// <returns></returns>
        public bool UpdateData(GestureList gestures)
        {
            if (gestures == null) throw new ArgumentNullException(nameof(gestures));

            if (gestures.Count == 0)
                _activeGesture = false;

            try
            {
                foreach (Gesture gesture in gestures)
                {
                    switch (gesture.Type)
                    {
                        case Gesture.GestureType.TYPE_CIRCLE:
                            PreCircle(gesture);
                            break;
                        case Gesture.GestureType.TYPE_SWIPE:
                            PreSwipe(gesture);
                            break;
                        case Gesture.GestureType.TYPE_KEY_TAP:
                            PreKeyTap(gesture);
                            break;
                        case Gesture.GestureType.TYPE_SCREEN_TAP:
                            PreScreenTap(gesture);
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

        /// <summary>
        /// performs pre actions before calling callback function
        /// </summary>
        /// <param name="gesture">Key Tap gesture</param>
        private void PreKeyTap(Gesture gesture)
        {
            KeyTapGesture keytap = new KeyTapGesture(gesture);

            if (!_activeGesture)
            {
                _activeGesture = true;
            }

            if (_onTap != null) _onTap();
        }

        /// <summary>
        /// performs pre actions before calling callback function
        /// </summary>
        /// <param name="gesture">Screen Tap gesture</param>
        private void PreScreenTap(Gesture gesture)
        {
            ScreenTapGesture screentap = new ScreenTapGesture(gesture);

            if (_onScreenTap != null) _onScreenTap();
        }

        /// <summary>
        /// performs pre actions before calling callback function
        /// </summary>
        /// <param name="gesture">Swipe gesture</param>
        private void PreSwipe(Gesture gesture)
        {
            SwipeGesture swipe = new SwipeGesture(gesture);

            var direction = swipe.Direction;

            if (_onSwipe != null) _onSwipe(direction);
        }

        /// <summary>
        /// performs pre actions before calling callback function
        /// </summary>
        /// <param name="gesture">Circle gesture</param>
        private void PreCircle(Gesture gesture)
        {
            CircleGesture circle = new CircleGesture(gesture);

            if (!_activeGesture)
            {
                _activeGesture = true;
            }


            Boolean clockwise = circle.Pointable.Direction.AngleTo(circle.Normal) <= Math.PI / 2 ? true : false;

            if (_onCircle != null) _onCircle(clockwise);
        }

    }
}

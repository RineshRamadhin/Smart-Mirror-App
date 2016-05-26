using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace Smart_Mirror_App.InputControls.Motion.LeapMotion
{
    class LeapMotionListener : Listener
    {
        // TODO: overwrite default leapmotion listener functions

        public override void OnInit(Controller controller)
        {
            Debug.WriteLine("Initialized");
        }

        public override void OnConnect(Controller controller)
        {
            Debug.WriteLine("Connected");
            controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
            controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
            controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
            controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
        }

        public override void OnDisconnect(Controller controller)
        {
            //Note: not dispatched when running in a debugger.
            Debug.WriteLine("Disconnected");
        }

        public override void OnExit(Controller controller)
        {
            Debug.WriteLine("Exited");
        }

        public override void OnFrame(Controller controller)
        {
            // Get the most recent frame and report some basic information
            Frame frame = controller.Frame();

            Debug.WriteLine("Frame id: " + frame.Id
                            + ", timestamp: " + frame.Timestamp
                            + ", hands: " + frame.Hands.Count
                            + ", fingers: " + frame.Fingers.Count
                            + ", tools: " + frame.Tools.Count
                            + ", gestures: " + frame.Gestures().Count);

            foreach (Hand hand in frame.Hands)
            {
                Debug.WriteLine("  Hand id: " + hand.Id
                                + ", palm position: " + hand.PalmPosition);
                // Get the hand's normal vector and direction
                Vector normal = hand.PalmNormal;
                Vector direction = hand.Direction;

                // Calculate the hand's pitch, roll, and yaw angles
                Debug.WriteLine("  Hand pitch: " + direction.Pitch*180.0f/(float) Math.PI + " degrees, "
                                + "roll: " + normal.Roll*180.0f/(float) Math.PI + " degrees, "
                                + "yaw: " + direction.Yaw*180.0f/(float) Math.PI + " degrees");

                // Get the Arm bone
                Arm arm = hand.Arm;
                Debug.WriteLine("  Arm direction: " + arm.Direction
                                + ", wrist position: " + arm.WristPosition
                                + ", elbow position: " + arm.ElbowPosition);

                // Get fingers
                foreach (Finger finger in hand.Fingers)
                {
                    Debug.WriteLine("    Finger id: " + finger.Id
                                    + ", " + finger.Type.ToString()
                                    + ", length: " + finger.Length
                                    + "mm, width: " + finger.Width + "mm");

                    // Get finger bones
                    Bone bone;
                    foreach (Bone.BoneType boneType in (Bone.BoneType[]) Enum.GetValues(typeof(Bone.BoneType)))
                    {
                        bone = finger.Bone(boneType);
                        Debug.WriteLine("      Bone: " + boneType
                                        + ", start: " + bone.PrevJoint
                                        + ", end: " + bone.NextJoint
                                        + ", direction: " + bone.Direction);
                    }
                }

            }

            // Get tools
            foreach (Tool tool in frame.Tools)
            {
                Debug.WriteLine("  Tool id: " + tool.Id
                                + ", position: " + tool.TipPosition
                                + ", direction " + tool.Direction);
            }

            // Get gestures
            GestureList gestures = frame.Gestures();
            for (int i = 0; i < gestures.Count; i++)
            {
                Gesture gesture = gestures[i];

                switch (gesture.Type)
                {
                    case Gesture.GestureType.TYPE_CIRCLE:
                        CircleGesture circle = new CircleGesture(gesture);

                        // Calculate clock direction using the angle between circle normal and pointable
                        String clockwiseness;
                        if (circle.Pointable.Direction.AngleTo(circle.Normal) <= Math.PI/2)
                        {
                            //Clockwise if angle is less than 90 degrees
                            clockwiseness = "clockwise";
                        }
                        else
                        {
                            clockwiseness = "counterclockwise";
                        }

                        float sweptAngle = 0;

                        // Calculate angle swept since last frame
                        if (circle.State != Gesture.GestureState.STATE_START)
                        {
                            CircleGesture previousUpdate = new CircleGesture(controller.Frame(1).Gesture(circle.Id));
                            sweptAngle = (circle.Progress - previousUpdate.Progress)*360;
                        }

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

            if (!frame.Hands.IsEmpty || !frame.Gestures().IsEmpty)
            {
                Debug.WriteLine("");
            }


        }
    }
}

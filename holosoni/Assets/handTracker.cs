using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;

namespace HoloLensHandTracking
{
    /// <summary>
    /// HandsManager determines if the hand is currently detected or not.
    /// </summary>
    public class handTracker : MonoBehaviour
    {
        /// <summary>
        /// HandDetected tracks the hand detected state.
        /// Returns true if the list of tracked hands is not empty.
        /// </summary>
        public bool HandDetected
        {
            get { return trackedHands.Count > 0; }
        }


        public GameObject mainCamera;
        private Transform originalParent;
        public GameObject TrackingObject;

        //public GameObject TrackingObject2;

        public uint idHand1;//, idHand2;

        public Text hand1Text;//, hand2Text;

       // public static bool holdingHand1;//, holdingHand2;

        public Text debugText;

        private HashSet<uint> trackedHands = new HashSet<uint>();
        //  private Dictionary<uint, GameObject> trackingObject = new Dictionary<uint, GameObject>();
        private GestureRecognizer gestureRecognizer;
        private uint activeId;

        public bool mapIsHandGuided = true;

        private void Start()
        {
            //originalParent = TrackingObject.transform.parent;
            //holdingHand1 = false;
           // holdingHand2 = false;
        }

        void Awake()
        {
            InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
            InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
            InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;

            gestureRecognizer = new GestureRecognizer();
            gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold);
            gestureRecognizer.Tapped += GestureRecognizerTapped;
          /*  gestureRecognizer.HoldStarted += GestureRecognizer_HoldStarted;
            gestureRecognizer.HoldCompleted += GestureRecognizer_HoldCompleted;
            gestureRecognizer.HoldCanceled += GestureRecognizer_HoldCanceled;*/
            gestureRecognizer.StartCapturingGestures();
           // StatusText.text = "READY";
        }

        /* void ChangeObjectColor(GameObject obj, Color color)
         {
             var rend = obj.GetComponentInChildren<Renderer>();
             if (rend)
             {
                 rend.material.color = color;
                 Debug.LogFormat("Color Change: {0}", color.ToString());
             }
         }*/


      /*  private void GestureRecognizer_HoldStarted(HoldStartedEventArgs args)
        {
            // uint id = args.source.id;

            Debug.Log("\nid " + args.source.id + "held on");

            if (holdingHand1)
                holdingHand2 = true;
            else
                holdingHand1 = true;



        }*/

       /* private void GestureRecognizer_HoldCompleted(HoldCompletedEventArgs args)
        {
            Debug.Log("\nid " + args.source.id + "hold completed");

            if (holdingHand2)
                holdingHand2 = false;
          else
                holdingHand1 = false;


        }*/

       /* private void GestureRecognizer_HoldCanceled(HoldCanceledEventArgs args)     //not sure when this happens. releasing a hold gesture makes a "holdCompleted" event happen
        {
            Debug.Log("\nid " + args.source.id + "hold canceled");       

            if (holdingHand2)
                holdingHand2 = false;
            else
                holdingHand1 = false;

        }*/

        private void GestureRecognizerTapped(TappedEventArgs args)
        {


          /*  if (args.source.id == 0)        //hand 1 id
            {
                if (hand1Text.text.Contains("hand"))
                    hand1Text.text = "HAND#1 handedness:" + args.source.handedness + " kind: " + args.source.kind;
                else
                    hand1Text.text = "hand #1 " + args.source.handedness + " kind: " + args.source.kind;
            }
            else if (args.source.id == 1)       //hand 2 id
            {
                if (hand2Text.text.Contains("hand"))
                    hand2Text.text = "HAND#2 " + args.source.handedness + " kind: " + args.source.kind;
                else
                    hand2Text.text = "hand #2 " + args.source.handedness + " kind: " + args.source.kind;
            }
            else
            {
                hand1Text.text = "que mao é essa mddc: " + args.source.handedness + " kind: " + args.source.kind;

            }*/



        }

        void Update()
        {


          /*  if (holdingHand1)
                hand1Text.text = "hand #1 HOLDING";
            else
                hand1Text.text = "hand #1 released";

            if (holdingHand2)
                hand2Text.text = "hand #2 HOLDING";
            else
                hand2Text.text = "hand #2 released";*/

            
        }



        /* private void changeMapMode()
         {
             if (!mapMode)
             {
                 //atualiza mapa conforme camera
             }
         }*/


        private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs args)
        {

            if (trackedHands.Count == 0)
            { 

                idHand1 = args.state.source.id;
                // Check to see that the source is a hand.
                if (args.state.source.kind != InteractionSourceKind.Hand)
                {
                    return;
                }
                trackedHands.Add(idHand1);
                activeId = idHand1;

                // var obj = Instantiate(TrackingObject) as GameObject;
                Vector3 pos;

                if (args.state.sourcePose.TryGetPosition(out pos))
                {
                    TrackingObject.transform.position = pos;
                }


            }

          /*  else { 

                idHand2 = args.state.source.id;
                // Check to see that the source is a hand.
                if (args.state.source.kind != InteractionSourceKind.Hand)
                {
                    return;
                }
                trackedHands.Add(idHand2);
                activeId = idHand2;

                Vector3 pos2;

                if (args.state.sourcePose.TryGetPosition(out pos2))
                {
                    TrackingObject2.transform.position = pos2;
                }

            }*/
        }

        private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs args)
        {

            if (args.state.source.id == idHand1)
            { 
                //idHand1 = args.state.source.id;
                Vector3 pos;
                Quaternion rot;

                if (args.state.source.kind == InteractionSourceKind.Hand)
                {
                    if (args.state.sourcePose.TryGetPosition(out pos))
                    {
                        TrackingObject.transform.position = pos;
                    }

                    if (args.state.sourcePose.TryGetRotation(out rot))
                    {
                        TrackingObject.transform.rotation = rot;
                    }
                }

            }
          /*  else if (args.state.source.id == idHand2)
            {
                //idHand2 = args.state.source.id;
                Vector3 pos;
                Quaternion rot;

                if (args.state.source.kind == InteractionSourceKind.Hand)
                {
                    if (args.state.sourcePose.TryGetPosition(out pos))
                    {
                        TrackingObject2.transform.position = pos;
                    }

                    if (args.state.sourcePose.TryGetRotation(out rot))
                    {
                        TrackingObject2.transform.rotation = rot;
                    }
                }
            }*/
       
        }

        private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs args)
        {
            uint id = args.state.source.id;
            // Check to see that the source is a hand.
            if (args.state.source.kind != InteractionSourceKind.Hand)
            {
                return;
            }

            if (trackedHands.Contains(id))
            {
                trackedHands.Remove(id);
            }

            /* if (trackingObject.ContainsKey(id))
                {
                    var obj = trackingObject[id];
                    trackingObject.Remove(id);
                    Destroy(obj);
                }*/
            if (trackedHands.Count > 0)
            {
                activeId = trackedHands.First();
            }

        }

        void OnDestroy()
        {
            InteractionManager.InteractionSourceDetected -= InteractionManager_InteractionSourceDetected;
            InteractionManager.InteractionSourceUpdated -= InteractionManager_InteractionSourceUpdated;
            InteractionManager.InteractionSourceLost -= InteractionManager_InteractionSourceLost;

            gestureRecognizer.Tapped -= GestureRecognizerTapped;
         /*   gestureRecognizer.HoldStarted -= GestureRecognizer_HoldStarted;
            gestureRecognizer.HoldCompleted -= GestureRecognizer_HoldCompleted;
            gestureRecognizer.HoldCanceled -= GestureRecognizer_HoldCanceled;*/
            gestureRecognizer.StopCapturingGestures();
        }
    }
}
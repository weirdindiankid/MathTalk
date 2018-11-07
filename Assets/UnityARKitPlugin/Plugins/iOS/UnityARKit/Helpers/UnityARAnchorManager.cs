using System;
using System.Collections.Generic;
using System.Linq;
using Collections.Hybrid.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace UnityEngine.XR.iOS
{
	public class UnityARAnchorManager 
	{

		private LinkedListDictionary<string, ARPlaneAnchorGameObject> planeAnchorMap;

        public Stack<ARPlaneAnchorGameObject> confirmedPlanesStack = new Stack<ARPlaneAnchorGameObject>();

        public ARPlaneAnchor newestAnchor;
        public ARPlaneAnchor newestConfirmedPlane;

        public UnityARAnchorManager ()
		{
			planeAnchorMap = new LinkedListDictionary<string,ARPlaneAnchorGameObject> ();
			UnityARSessionNativeInterface.ARAnchorAddedEvent += AddAnchor;
			UnityARSessionNativeInterface.ARAnchorUpdatedEvent += UpdateAnchor;
			UnityARSessionNativeInterface.ARAnchorRemovedEvent += RemoveAnchor;

		}

        //WHAT HAPPENS WHEN A POTENTIAL PLANE IS DETECTED:
		public void AddAnchor(ARPlaneAnchor arPlaneAnchor)
		{
            //show confirmation button

            if(UnityARGeneratePlane.NewestPotentialPlaneChosen || !UnityARGeneratePlane.FirstPotentialPlaneFound){ //the previous potential plane was chosen, so do not delete its anchor
                newestAnchor = arPlaneAnchor;
                UnityARGeneratePlane.NewestPotentialPlaneChosen = false;
                UnityARGeneratePlane.NewAnchorDetected = true;
            }
            else{
                //RemoveAnchor(newestAnchor); //A new potential plane was found before the previous potential plane was confirmed, so delete the previous one's anchor
                //You can't remove the newest Anchor because it was never added
                newestAnchor = arPlaneAnchor;
                UnityARGeneratePlane.NewestPotentialPlaneChosen = false;
                UnityARGeneratePlane.NewAnchorDetected = true;
            }

		}


        //confirmation button calls this
        public void ConfirmNewestAnchor(){

            ARPlaneAnchor arPlaneAnchor = newestAnchor; 
            GameObject go = UnityARUtility.CreatePlaneInScene(arPlaneAnchor);
            go.AddComponent<DontDestroyOnLoad>();  //this is so these GOs persist across scene loads
            ARPlaneAnchorGameObject arpag = new ARPlaneAnchorGameObject();
            arpag.planeAnchor = arPlaneAnchor;
            arpag.gameObject = go;
            planeAnchorMap.Add(arPlaneAnchor.identifier, arpag);
            confirmedPlanesStack.Push(arpag);

            UnityARGeneratePlane.NewAnchorDetected = false;
            UnityARGeneratePlane.NewestPotentialPlaneChosen = true;
            if (!UnityARGeneratePlane.FirstPlaneChosen) UnityARGeneratePlane.FirstPlaneChosen = true;
        }

		public void RemoveAnchor(ARPlaneAnchor arPlaneAnchor)
		{
			if (planeAnchorMap.ContainsKey (arPlaneAnchor.identifier)) {
				ARPlaneAnchorGameObject arpag = planeAnchorMap [arPlaneAnchor.identifier];
				GameObject.Destroy (arpag.gameObject);
				planeAnchorMap.Remove (arPlaneAnchor.identifier);
			}
		}

        public void DeleteNewestAnchor(){
            //RemoveAnchor(newestAnchor);
            UnityARGeneratePlane.NewAnchorDetected = false;
            UnityARGeneratePlane.NewestPotentialPlaneChosen = true;
        }

		public void UpdateAnchor(ARPlaneAnchor arPlaneAnchor)
		{
			if (planeAnchorMap.ContainsKey (arPlaneAnchor.identifier)) {
				ARPlaneAnchorGameObject arpag = planeAnchorMap [arPlaneAnchor.identifier];
				UnityARUtility.UpdatePlaneWithAnchorTransform (arpag.gameObject, arPlaneAnchor);
				arpag.planeAnchor = arPlaneAnchor;
				planeAnchorMap [arPlaneAnchor.identifier] = arpag;
			}
		}

        public void UnsubscribeEvents()
        {
            UnityARSessionNativeInterface.ARAnchorAddedEvent -= AddAnchor;
            UnityARSessionNativeInterface.ARAnchorUpdatedEvent -= UpdateAnchor;
            UnityARSessionNativeInterface.ARAnchorRemovedEvent -= RemoveAnchor;
        }

        public void Destroy()
        {
            foreach (ARPlaneAnchorGameObject arpag in GetCurrentPlaneAnchors()) {
                GameObject.Destroy (arpag.gameObject);
            }

            planeAnchorMap.Clear ();
            UnsubscribeEvents();
        }

		public LinkedList<ARPlaneAnchorGameObject> GetCurrentPlaneAnchors()
		{
			return planeAnchorMap.Values;
		}

        public void DeleteMostRecentPlane(){
            ARPlaneAnchorGameObject mostRecentPlane = confirmedPlanesStack.Pop();
            GameObject.Destroy(mostRecentPlane.gameObject);
        }
	}
}


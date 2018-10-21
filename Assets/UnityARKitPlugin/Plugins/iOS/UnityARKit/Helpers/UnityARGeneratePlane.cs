using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.XR.iOS
{
	public class UnityARGeneratePlane : MonoBehaviour
	{
		public GameObject planePrefab;
        public GameObject potentialPlanePrefab;
        private GameObject PotentialPlane;

        public Button ConfirmationButton;
        public Button DenyButton;
        public Text SearchingMessage;

        private UnityARAnchorManager unityARAnchorManager;
        public static bool NewAnchorDetected;
        public static bool NewestPotentialPlaneChosen;
        public static bool FirstPlaneChosen;
        public static bool FirstPotentialPlaneFound;

		// Use this for initialization
		void Start () {
            unityARAnchorManager = new UnityARAnchorManager();
            UnityARUtility.InitializePlanePrefab(planePrefab);

            NewAnchorDetected = false;
            NewestPotentialPlaneChosen = false;
            FirstPotentialPlaneFound = false;
            FirstPlaneChosen = false;
		}

        void OnDestroy()
        {
            unityARAnchorManager.Destroy ();
        }

        void OnGUI()
        {
			IEnumerable<ARPlaneAnchorGameObject> arpags = unityARAnchorManager.GetCurrentPlaneAnchors ();
			foreach(var planeAnchor in arpags)
			{
                //ARPlaneAnchor ap = planeAnchor;
                //GUI.Box (new Rect (100, 100, 800, 60), string.Format ("Center: x:{0}, y:{1}, z:{2}", ap.center.x, ap.center.y, ap.center.z));
                //GUI.Box(new Rect(100, 200, 800, 60), string.Format ("Extent: x:{0}, y:{1}, z:{2}", ap.extent.x, ap.extent.y, ap.extent.z));
            }
        }

        void Update()
        {
            ConfirmationButton.gameObject.SetActive(NewAnchorDetected);
            DenyButton.gameObject.SetActive(NewAnchorDetected);
            SearchingMessage.gameObject.SetActive(!FirstPlaneChosen && !NewAnchorDetected);

            //Initialize Potential plane when first currentAnchor is found
            if (!FirstPotentialPlaneFound && unityARAnchorManager.newestAnchor != null)
            {
                PotentialPlane = Instantiate(potentialPlanePrefab);
                FirstPotentialPlaneFound = true;
            }

            PotentialPlane.gameObject.SetActive(unityARAnchorManager.newestAnchor != null && !NewestPotentialPlaneChosen && NewAnchorDetected);

            //Update the position and rotation of the potential plane object to always be equal to the position and rotation of the current anchor
            if (unityARAnchorManager.newestAnchor != null)
            {
                PotentialPlane.transform.position = UnityARMatrixOps.GetPosition(unityARAnchorManager.newestAnchor.transform);
                PotentialPlane.transform.rotation = UnityARMatrixOps.GetRotation(unityARAnchorManager.newestAnchor.transform);
            }
        }

        public void ConfirmNewestPotentialPlane(){
            unityARAnchorManager.ConfirmNewestAnchor();
        }

        public void DenyNewestPotentialPlane(){
            unityARAnchorManager.DeleteNewestAnchor();
        }
    }
}


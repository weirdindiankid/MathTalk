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
        private bool FirstPotentialPlaneFound;

        public Button ConfirmationButton;
        public Button DenyButton;
        public Text SearchingMessage;

        private UnityARAnchorManager unityARAnchorManager;
        public static bool AnchorDetected;
        public static bool PlaneChosen;

		// Use this for initialization
		void Start () {
            unityARAnchorManager = new UnityARAnchorManager();
            //ConfirmationButton.gameObject.SetActive(true);
            UnityARUtility.InitializePlanePrefab(planePrefab);
            AnchorDetected = false;
            PlaneChosen = false;
            FirstPotentialPlaneFound = false;
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
            ConfirmationButton.gameObject.SetActive(AnchorDetected);
            DenyButton.gameObject.SetActive(AnchorDetected);
            SearchingMessage.gameObject.SetActive(!AnchorDetected && !PlaneChosen);

            //Initialize Potential plane when first currentAnchor is found
            if (!FirstPotentialPlaneFound && unityARAnchorManager.currentAnchor != null)
            {
                PotentialPlane = Instantiate(potentialPlanePrefab);
                FirstPotentialPlaneFound = true;
            }

            PotentialPlane.gameObject.SetActive(unityARAnchorManager.currentAnchor != null && !PlaneChosen && AnchorDetected);

            //Update the position and rotation of the potential plane object to always be equal to the position and rotation of the current anchor
            if (unityARAnchorManager.currentAnchor != null)
            {
                PotentialPlane.transform.position = UnityARMatrixOps.GetPosition(unityARAnchorManager.currentAnchor.transform);
                PotentialPlane.transform.rotation = UnityARMatrixOps.GetRotation(unityARAnchorManager.currentAnchor.transform);
            }
        }

        public void ConfirmPlane(){
            unityARAnchorManager.ConfirmCurrentAnchor();
        }

        public void DenyPlane(){
            unityARAnchorManager.DeleteCurrentAnchor();
        }
    }
}


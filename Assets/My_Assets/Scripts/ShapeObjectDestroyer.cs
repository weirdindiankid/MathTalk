using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class ShapeObjectDestroyer : MonoBehaviour {

    public GameObject planeAttachedTo = null;  //the plane object that this object is attached to
    public float maxDistanceBelowPlane = 0.15f;
    public GameObject DestroyEffect;
    public int ARPlaneLayer = 10; //arbitrarily defined in the editor

    private GameObject planeAttachedToFollower = null; //follows the plane object that this object is attached to

    private bool planeAttachedToDeleted;

	// Use this for initialization
    void Start () {
        planeAttachedToDeleted = false;
    }
	
	// Update is called once per frame
	void Update () {
        
        if(planeAttachedTo == null && planeAttachedToFollower != null){
            //if the plane's follower object still exists but the actual plane object does not exist, we can deduce that the plane was created then deleted
            planeAttachedToDeleted = true;
        }

        if (planeAttachedTo != null && planeAttachedToFollower != null)
        {
            //this empty game object will follow the plane as a reference for the plane's position. persists in the same spot even after plane is destroyed
            planeAttachedToFollower.transform.position = planeAttachedTo.transform.position; 
        }

        //HANDLES OBJECT DELETION WHEN THE ENTIRE PLANE IS DELETED (so all of the objects "attached"/associated with it are deleted as well
        if (planeAttachedToDeleted)
        {
            GameObject destroyEffect = Instantiate(DestroyEffect); //spawn the destroy effect at the position of this shape
            destroyEffect.transform.position = gameObject.transform.position;
            Destroy(gameObject);
            Destroy(planeAttachedToFollower);
        }

        //HANDLES OBJECT DELETION WHEN A SINGLE OBJECT FALLS OFF OF THE PLANE, AND THE PLANE ITSELF IS NOT DELETED
        if(planeAttachedToFollower != null){
            if (gameObject.transform.position.y + maxDistanceBelowPlane < UnityARGeneratePlane.LowestPlaneHeight)
            {
                //object is too far below the plane so delete it
                GameObject destroyEffect = Instantiate(DestroyEffect); //spawn the destroy effect at the position of this shape
                destroyEffect.transform.position = gameObject.transform.position;
                Destroy(gameObject);
                Destroy(planeAttachedToFollower);
            }
        }
	}

    void OnCollisionEnter(Collision col)
    {
        if (!planeAttachedToDeleted && col.gameObject.layer == ARPlaneLayer)
        {
            
            if(col.collider.tag == "ARPlane"){ //collider is a plane itself
                planeAttachedTo = col.collider.gameObject;
                planeAttachedToFollower = new GameObject();
            }

            else{ //collider is another block which is attached to a plane

                //this object has not yet been attached to a plane while the other object is already attached to a plane
                if ((col.collider.GetComponent<ShapeObjectDestroyer>().planeAttachedTo != null) && (planeAttachedTo == null)) //in other words, the block it is being placed on is currently set on a plane 
                { 
                    planeAttachedTo = col.collider.GetComponent<ShapeObjectDestroyer>().planeAttachedTo; 
                    planeAttachedToFollower = new GameObject();
                }

                //this object is already attached to a plane, and the other object is already attached to a different plane
                else if(col.collider.GetComponent<ShapeObjectDestroyer>().planeAttachedTo.GetInstanceID() != GetComponent<ShapeObjectDestroyer>().planeAttachedTo.GetInstanceID()){
                    if(Mathf.Abs(col.collider.GetComponent<Rigidbody>().velocity.y) < Mathf.Abs(GetComponent<Rigidbody>().velocity.y)){
                        planeAttachedTo = col.collider.GetComponent<ShapeObjectDestroyer>().planeAttachedTo; //arbitrary way to decide which object gets the new plane --> this chooses the one that is moving faster in the y direction (i.e. being dropped or falling on top of)
                        planeAttachedToFollower = new GameObject();
                    }
                }

            }
        }
    }
}


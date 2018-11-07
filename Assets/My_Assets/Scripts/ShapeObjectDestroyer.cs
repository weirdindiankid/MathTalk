using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeObjectDestroyer : MonoBehaviour {

    private GameObject planeAttachedTo = null;
    public float maxDistanceBelowPlane = 0.15f;
    public GameObject DestroyEffect;
    public int ARPlaneLayer = 10; //arbitrarily defined in the editor

    private GameObject planeAttachedToFollower = null;

	// Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (planeAttachedTo != null && planeAttachedToFollower != null)
        {
            planeAttachedToFollower.transform.position = planeAttachedTo.transform.position; //this empty game object will follow the plane as a reference for the plane's position. persists in the same spot even after plane is destroyed
        }
        if(planeAttachedToFollower != null){
            if (gameObject.transform.position.y + maxDistanceBelowPlane < planeAttachedToFollower.transform.position.y)
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
        if (planeAttachedTo == null && col.gameObject.layer == ARPlaneLayer)
        {
            planeAttachedTo = col.gameObject; //assigned to the first plane this object collides with
            planeAttachedToFollower = new GameObject();
        }
    }
}

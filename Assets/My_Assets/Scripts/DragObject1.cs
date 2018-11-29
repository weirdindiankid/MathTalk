using UnityEngine;
using System.Collections;

public class DragObject1 : MonoBehaviour
{

    Vector3 dist;
    float posX;
    float posY;

    void OnMouseDown()
    {
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;

        GetComponent<SelectTracker>().activateHighlight();
    }

    void OnMouseDrag()
    {
        GetComponent<SelectTracker>().isBeingTranslated = true;

        Vector3 curPos =
         new Vector3(Input.mousePosition.x - posX,
                     Input.mousePosition.y - posY, dist.z);

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
        transform.position = worldPos;
    }

    void OnMouseUp()
    {
        GetComponent<SelectTracker>().isBeingTranslated = false;

        if(!GetComponent<SelectTracker>().isBeingScaled){
            GetComponent<SelectTracker>().deactivateHighlight();
        }
    }
}
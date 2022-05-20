using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDestruction : MonoBehaviour
{
    public GameObject cutout;

    public void CreateVisalDestruction(Polygon polygon)
    {

        GameObject newObject = Instantiate<GameObject>(cutout, polygon.center, Quaternion.identity);
        if (newObject != null) 
        {
            newObject.transform.localScale = new Vector2(polygon.radius * 0.78125f, polygon.radius * 0.78125f);
            Cutout newCutout = newObject.GetComponent<Cutout>();
            if (newCutout != null)
            {
                newCutout.Set(polygon.lines);
            }
        }
    }
}

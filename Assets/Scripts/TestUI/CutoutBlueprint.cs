using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutoutBlueprint : MonoBehaviour
{
    public GameObject cutout;
    public Slider lineSlider;
    public Slider radiusSlider;
    public Camera camera;

    public void CreateBlueprint()
    {
        Polygon polygon = new Polygon(new List<Vector2>(), camera.ScreenToWorldPoint(Input.mousePosition), Mathf.FloorToInt(lineSlider.value), radiusSlider.value/10);
        GameObject newObject = Instantiate<GameObject>(cutout, polygon.center, Quaternion.identity);
        if (newObject != null)
        {
            newObject.transform.localScale = new Vector2(polygon.radius * 0.78125f, polygon.radius * 0.78125f);
            Cutout newCutout = newObject.GetComponent<Cutout>();
            if (newCutout != null)
            {
                newCutout.Set(polygon.lines);
            }
            BlueprintDestruction blueprintDestruction = newObject.GetComponent<BlueprintDestruction>();
            if (blueprintDestruction != null)
            {
                blueprintDestruction.lines = Mathf.RoundToInt(lineSlider.value);
                blueprintDestruction.radius = radiusSlider.value/10;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePolygone : MonoBehaviour
{
    public List<Vector2> GetPolygon(int lines, float radius, Vector2 pivot)
    {
        List<Vector2> points = new List<Vector2>();
        Vector2 circle = new Vector2(0, radius);
        for (int i = 0; i < lines; i++) 
        {
            points.Add(circle + pivot);
            circle = rotate(circle, Mathf.Deg2Rad * 360 / lines);
            Debug.DrawLine(circle + pivot, points[points.Count - 1], Color.green, 20, false);
        }

        return points;
    }

    public static Vector2 rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

}

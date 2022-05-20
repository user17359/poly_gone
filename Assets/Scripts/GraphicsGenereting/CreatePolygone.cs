using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePolygone : MonoBehaviour
{
    public Polygon GetPolygon(int lines, float radius, Vector2 pivot)
    {
        List<Vector2> points = new List<Vector2>();
        Vector2 circle = new Vector2(0, radius);
        for (int i = 0; i < lines; i++) 
        {
            points.Add(circle + pivot);
            circle = rotate(circle, Mathf.Deg2Rad * 360 / lines);
            Debug.DrawLine(circle + pivot, points[points.Count - 1], Color.green, 20, false);
        }

        return new Polygon(points, pivot, lines, radius);
    }

    public static Vector2 rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

}

public class Polygon
{
    public List<Vector2> corners;
    public Vector2 center;
    public int lines;
    public float radius;

    public Polygon(List<Vector2> corners, Vector2 center, int lines, float radius)
    {
        this.corners = corners;
        this.center = center;
        this.lines = lines;
        this.radius = radius;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.Linq;

[RequireComponent(typeof(CreatePolygone))]

public class TerainDestroying : MonoBehaviour
{
    public int lines;
    public float radius;
    CreatePolygone createPolygone;
    public PolygonCollider2D spirteShapeController;
    public int at;

    void Start()
    {
        createPolygone = gameObject.GetComponent<CreatePolygone>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CalculateDestruction(createPolygone.GetPolygon(lines, radius, transform.position), spirteShapeController);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
        }
    }


    void CalculateDestruction(List<Vector2> polygon, PolygonCollider2D terrain)
    {
        List<Vector2> currentPath = terrain.points.ToList();
        List<bool> isInside = new List<bool>();
        for (int i = 0; i < polygon.Count; i++)
        {
            if (terrain.OverlapPoint(polygon[i]))
            {
                isInside.Add(true);
            }
            else
            {
                isInside.Add(false);
            }
        }
        for (int i = 0; i < polygon.Count; i++)
        {
            int nextID = (i != polygon.Count - 1) ? i + 1 : 0;
            int previousID = (i != 0) ? i - 1 : polygon.Count - 1;

            if (isInside[i]) {
                currentPath.Add(ConvertToColliderPos(polygon[i], terrain));
            }
            else
            {
                if(isInside[nextID])
                {
                    RaycastHit2D hit = Physics2D.Raycast(polygon[i], -(polygon[i] - polygon[nextID]).normalized);
                    Debug.Log("Next hit at: " + hit.point);
                    currentPath.Add(ConvertToColliderPos(hit.point, terrain));
                }
                if (isInside[previousID])
                {
                    RaycastHit2D hit = Physics2D.Raycast(polygon[i], -(polygon[i] - polygon[previousID]).normalized);
                    Debug.Log("Before hit at: " + hit.point);
                    currentPath.Add(ConvertToColliderPos(hit.point, terrain));
                }
            }
        }
        terrain.SetPath(0, currentPath);
    }

    Vector2 ConvertToColliderPos(Vector2 pos, PolygonCollider2D collider2D)
    {
        Vector2 returnPos;
        returnPos = pos;
        returnPos -= new Vector2(collider2D.bounds.center.x, collider2D.bounds.center.y);
        returnPos.x /= collider2D.bounds.extents.x*2;
        returnPos.y /= collider2D.bounds.extents.y*2;
        return (returnPos);
    }
}

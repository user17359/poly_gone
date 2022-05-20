using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.Linq;

[RequireComponent(typeof(CreatePolygone))]

public class TerainDestroying : MonoBehaviour
{
    public PolygonCollider2D spirteShapeController;
    private int at;
    public LayerMask raycastMask;
    public VisualDestruction visualDestruction;

    void Start()
    {
        //Debug.Log(Inline(new Vector2(2,1), new Vector2(2, 5), new Vector2(2,0)));
        //createPolygone = gameObject.GetComponent<CreatePolygone>();
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            Polygon polygon = createPolygone.GetPolygon(lines, radius, transform.position);
            CalculateDestruction(polygon.corners, spirteShapeController);
            visualDestruction.CreateVisalDestruction(polygon);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
        }*/
    }


    public void CalculateDestruction(List<Vector2> polygon, PolygonCollider2D terrain, float radius)
    {
        List<Vector2> currentPath = terrain.points.ToList();
        List<bool> isInside = new List<bool>();
        int firstID = 0;
        int nextToFirstID = 0;
        int lastID = 0;
        int nextToLastID = 0;


        //calculating which parts will be inside terrain
        for (int i = 0; i < polygon.Count; i++)
        {      
            bool overlap = terrain.OverlapPoint(polygon[i]);
            if (overlap)
            {
                isInside.Add(true);
            }
            else
            {
                isInside.Add(false);
            }


        }

        //finding first and last
        for (int i = 0; i < polygon.Count; i++)
        {
            
            int nextID = (i != polygon.Count - 1) ? i + 1 : 0;
            int previousID = (i != 0) ? i - 1 : polygon.Count - 1;
            if (!isInside[i] && isInside[previousID])
            {
                firstID = i;
                nextToFirstID = previousID;
            }
            if (!isInside[i] && isInside[nextID])
            {
                lastID = i;
                nextToLastID = nextID;
            }
        }

        //raycasting to find points where hole starts and ends
        RaycastHit2D hit2 = Physics2D.Raycast(polygon[lastID], -(polygon[lastID] - polygon[nextToLastID]).normalized, radius * 10, raycastMask);
        Debug.DrawLine(polygon[lastID], hit2.point, Color.red, 20, false);
        Vector2 firstPos = ConvertToColliderPos(hit2.point, terrain);

        RaycastHit2D hit = Physics2D.Raycast(polygon[firstID], -(polygon[firstID] - polygon[nextToFirstID]).normalized, radius * 10, raycastMask);
        Debug.DrawLine(polygon[firstID], hit.point, Color.blue, 20, false);
        Vector2 lastPos = ConvertToColliderPos(hit.point, terrain);


        //print("FirstID: " + firstID + " Next to firstID: " + nextToFirstID + " LastID: " + lastID + " Next to lastID: " + nextToLastID);

        //calculating point where new corners should be added
        int firstCollision = -1;
        int lastCollision = -2;
        for (int i = 0; i < currentPath.Count; i++)
        {
            int nextID = (i != currentPath.Count - 1) ? i + 1 : 0;
            if (Inline(firstPos, currentPath[i], currentPath[nextID]))
            {
                //Debug.Log("First point is between: " + currentPath[i] + " and " + currentPath[nextID]);
                firstCollision = nextID;
            }
            if (Inline(lastPos, currentPath[i], currentPath[nextID]))
            {
                //Debug.Log("Last point is between: " + currentPath[i] + " and " + currentPath[nextID]);
                lastCollision = nextID;
            }
        }

        //setting at
        if(firstCollision == lastCollision)
        {
            at = firstCollision;
        }
        else
        {
            Debug.Log("to skomplikowane: first: " + firstCollision + " last: " + lastCollision);
            for(int i = Mathf.Min(firstCollision, lastCollision); i < Mathf.Max(firstCollision, lastCollision); i++)
            {
                currentPath.RemoveAt(Mathf.Min(firstCollision, lastCollision));
            }
            at = Mathf.Min(firstCollision, lastCollision);
        }

        //adding corners to path
        currentPath.Insert(at, firstPos);

        for (int i = 0; i < polygon.Count; i++)
        {
            if (isInside[i]) {
                currentPath.Insert(at, ConvertToColliderPos(polygon[i], terrain));
            }
        }


        currentPath.Insert(at, lastPos);

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

    bool Inline(Vector2 tested, Vector2 firstBound, Vector2 secondBound)
    {
        float episilon = 0.0001f;
        float diff;
        if (Mathf.Abs(firstBound.x - secondBound.x) > episilon) {
            float a = (firstBound.y - secondBound.y) / (firstBound.x - secondBound.x);
            float b = firstBound.y - a * firstBound.x;

           //Debug.Log("y = " + a + "x +" + b);

            diff = tested.y - a * tested.x - b;
        }
        else
        {
            //Debug.Log("x = " + firstBound.x);
            diff = tested.x - firstBound.x;
        }


        if(Mathf.Abs(diff) < episilon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

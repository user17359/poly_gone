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
        List<Vector2> convertedPolygon = ConvertToColliderPos(polygon, terrain);
        int firstID = 0;
        int nextToFirstID = 0;
        int lastID = 0;
        int nextToLastID = 0;
        bool any = false;

        //calculating which parts will be inside terrain
        for (int i = 0; i < polygon.Count; i++)
        {
            bool overlap = terrain.OverlapPoint(polygon[i]);
            if (overlap)
            {
                isInside.Add(true);
                any = true;
            }
            else
            {
                isInside.Add(false);
            }


        }
        if (any) //double check if destruction is inside terrain
        {
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
            if (firstCollision == lastCollision)
            {
                at = firstCollision;
            }
            else
            {

                /*for(int i = Mathf.Min(firstCollision, lastCollision); i < Mathf.Max(firstCollision, lastCollision); i++)
                {
                    currentPath.RemoveAt(Mathf.Min(firstCollision, lastCollision));
                }
                at = Mathf.Min(firstCollision, lastCollision);*/
                for (int i = 0; i < currentPath.Count; i++) //deleting points inside polygon
                {
                    Debug.Log("Checking: " + currentPath[i].x + ", " + currentPath[i].y);
                    if (PointInPolygon((currentPath[i]), (convertedPolygon)))
                    {
                        Debug.Log("Deleting at: " + currentPath[i].x + ", " + currentPath[i].y);
                        currentPath.Remove(currentPath[i]);
                    }
                }
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
    }

    Vector2 ConvertToColliderPos(Vector2 pos, PolygonCollider2D collider2D)
    {
        Vector2 returnPos;
        returnPos = pos;
        returnPos -= new Vector2(collider2D.bounds.center.x, collider2D.bounds.center.y);
        returnPos.x /= collider2D.bounds.extents.x * 2;
        returnPos.y /= collider2D.bounds.extents.y * 2;
        return (returnPos);
    }

    List<Vector2> ConvertToColliderPos(List<Vector2> pos, PolygonCollider2D collider2D)
    {
        List<Vector2> returnPos = new List<Vector2>();
        foreach (Vector2 point in pos)
        {
            returnPos.Add(ConvertToColliderPos(point, collider2D));
        }
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


        if (Mathf.Abs(diff) < episilon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool PointInPolygon(Vector2 point, List<Vector2> polygon)
    {
        int collisions = 0;
        Halfline halfline = new Halfline(point, point+Vector2.right);
        Debug.Log("Halfline from: " + halfline.startPoint.x + ", " + halfline.startPoint.y + " with a: " + halfline.a + " b: " + halfline.b);
        for (int i = 0; i < polygon.Count; i++)
        {
            int lastID = (i != 0) ? i - 1 : polygon.Count - 1;
            Section section = new Section(polygon[i], polygon[lastID]);
            Debug.Log("Checking against: " + section.startPoint.x + ", " + section.startPoint.y + " point2: " + section.endPoint.x + ", " + section.endPoint.y);
            if (CheckRayToRight(halfline, section))
            {
                collisions++;
            }
        }
        if(collisions%2 == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    bool CheckRayToRight(Halfline halfline, Section section)
    {
        float episilon = 0.0001f;
        if (Mathf.Abs(section.a) < episilon) //checking if lines are not colaterall
        {
            Debug.Log("colaterall");
            return false;
        }
        Vector2 intersection; //calculating intersection
        intersection.y = halfline.b;
        intersection.x = (intersection.y - section.b) / section.a;
        Debug.Log("Intersection at: " + intersection.x + ", " + intersection.y);
        if(intersection.x < halfline.startPoint.x) //check against ray to right
        {
            Debug.Log("left to halfline");
            return false;
        }
        if(intersection.x > Mathf.Max(section.startPoint.x, section.endPoint.x)) //four checks against section
        {
            Debug.Log("outside bounds 1");
            return false;
        }
        if (intersection.x < Mathf.Min(section.startPoint.x, section.endPoint.x))
        {
            Debug.Log("outside bounds 2");
            return false;
        }
        if (intersection.y > Mathf.Max(section.startPoint.y, section.endPoint.y))
        {
            Debug.Log("outside bounds 3");
            return false;
        }
        if (intersection.y < Mathf.Min(section.startPoint.y, section.endPoint.y))
        {
            Debug.Log("outside bounds 4");
            return false;
        }
        Debug.Log("Intersection is true");
        return true;

    }

    Vector3 XYToXZ(Vector2 input)
    {
        return new Vector3(input.x, 0, input.y);
    }

    /*List<Vector3> XYToXZ(List<Vector2> input)
    {
        List<Vector3> result = new List<Vector3>();
        foreach (Vector2 point in input)
        {
            result.Add(new Vector3(point.x, 0, point.y));
        }
        return result;
    }*/

    public class Halfline
    {
        Vector2 StartPoint;
        float A;
        float B;

        public float a { get => A; set => A = value; }
        public float b { get => B; set => B = value; }
        public Vector2 startPoint { get => StartPoint; set => StartPoint = value; }

        public Halfline(Vector2 pointA, Vector2 pointB)
        {
            float episilon = 0.0001f;
            if (Mathf.Abs(pointA.x - pointB.x) > episilon)
            {
                a = (pointA.y - pointB.y) / (pointA.x - pointB.x);
                b = pointA.y - a * pointA.x;
            }
            else
            {
                a = 0;
                b = pointA.x;
            }
            startPoint = pointA;
        }
    }

    public class Section
    {
        Vector2 StartPoint;
        Vector2 EndPoint;
        float A;
        float B;

        public float a { get => A; set => A = value; }
        public float b { get => B; set => B = value; }
        public Vector2 startPoint { get => StartPoint; set => StartPoint = value; }
        public Vector2 endPoint { get => EndPoint; set => EndPoint = value; }

        public Section(Vector2 pointA, Vector2 pointB)
        {
            float episilon = 0.0001f;
            if (Mathf.Abs(pointA.x - pointB.x) > episilon)
            {
                a = (pointA.y - pointB.y) / (pointA.x - pointB.x);
                b = pointA.y - a * pointA.x;
            }
            else
            {
                a = 0;
                b = pointA.x;
            }
            startPoint = pointA;
            endPoint = pointB;
        }
    }
}



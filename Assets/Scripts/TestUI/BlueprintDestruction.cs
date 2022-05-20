using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintDestruction : MonoBehaviour
{
    public int lines;
    public float radius;
    public CreatePolygone createPolygone;
    public TerainDestroying terainDestroying;
    public VisualDestruction visualDestruction;
    public PolygonCollider2D spirteShapeController;

    private void Start()
    {
        spirteShapeController = GameObject.FindGameObjectWithTag("Destructable").GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Polygon polygon = createPolygone.GetPolygon(lines, radius, new Vector3(transform.position.x, transform.position.y, 0));

            terainDestroying.CalculateDestruction(polygon.corners, spirteShapeController, polygon.radius);
            visualDestruction.CreateVisalDestruction(polygon);
            Destroy(gameObject);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SpriteMask))]
public class Cutout : MonoBehaviour
{
    SpriteMask spriteMask;
    SpriteRenderer spriteRenderer;
    public List<Sprite> polygons;

    public void Set(int numberOfCorners)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteMask = gameObject.GetComponent<SpriteMask>();
        if (spriteMask != null)
        {
            spriteMask.sprite = polygons[numberOfCorners - 3];
        }
        spriteRenderer.sprite = polygons[numberOfCorners - 3];
    }
}

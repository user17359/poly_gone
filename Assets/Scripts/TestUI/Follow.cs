using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    private Camera converter;

    void Update()
    {
        if(converter == null)
        {
            converter = GameObject.FindGameObjectWithTag("Camera").GetComponent<Camera>();

        }
        if (converter != null)
        {
            transform.position = converter.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}

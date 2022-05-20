using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberUpdate : MonoBehaviour
{
    public TextMeshProUGUI number;
    public Slider slider;
    public float coeff;

    private void Start()
    {
        number.text = slider.value * coeff + "";
    }

    // Update is called once per frame
    public void Set()
    {
        number.text = slider.value*coeff + "";
    }
}

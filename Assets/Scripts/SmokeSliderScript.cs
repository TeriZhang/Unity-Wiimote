using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmokeSliderScript : MonoBehaviour
{
    public float smokeLevel;
    public float smokeSpeed;
    public float maxSmoke;
    public Slider mySlider;

    // Start is called before the first frame update
    void Start()
    {
        smokeLevel = mySlider.minValue;
        maxSmoke = mySlider.maxValue;
        mySlider = GetComponent<Slider>();
        mySlider.maxValue = 500f;


    }

    public void Update()
    {
        mySlider.value = smokeLevel;
    }
}

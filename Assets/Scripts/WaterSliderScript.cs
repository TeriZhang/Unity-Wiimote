using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterSliderScript : MonoBehaviour
{
    public float remainingWater;
    public Slider mySlider;
    public Slider myTwin;

    public void Start()
    {
        remainingWater = 100;
    }

    // Update is called once per frame
    void Update()
    {
        mySlider.value = remainingWater;
        myTwin.value = remainingWater;

        if(remainingWater > 100)
        {
            remainingWater = 100;
        }
    }
}

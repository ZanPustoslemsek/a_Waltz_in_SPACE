using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSensitivitySlider : MonoBehaviour
{
    Slider slider;

    private void Start() {
        slider = GetComponent<Slider>();
        Konst.baseScrollK = Konst.scrollK;
        SetSensitivity();
    }

    public void SetSensitivity()
    {
        Konst.scrollK = Konst.baseScrollK * slider.value;
    }
}

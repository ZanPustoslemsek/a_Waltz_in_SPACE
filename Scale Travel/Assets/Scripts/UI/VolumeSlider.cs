using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    Slider slider;

    private void Start() {
        slider = GetComponent<Slider>();
        slider.value = AudioManager.Instance.globalVolume;
        SetVolume();
    }

    public void SetVolume()
    {
        AudioManager.Instance.SetVolume(slider.value);
    }
}

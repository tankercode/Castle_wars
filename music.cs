using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class music : MonoBehaviour
{
    [SerializeField]
    AudioSource audioS;

    public Slider mainSlider;

    public void SetVolue() {
        audioS.volume = mainSlider.value;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyncToggleSlider : MonoBehaviour
{
    Toggle tg;
    public Slider sl;
    int volume = 0;

    void Start()
    {
        tg = GetComponent<Toggle>();

        //Debug.Log("Getting game Vol at start : " + PlayerPrefs.GetFloat("GameVolume"));
        sl.value = PlayerPrefs.GetFloat("GameVolume");
        if (sl.value == 0f)
        {
            tg.isOn = true;
        }


        tg.onValueChanged.AddListener(delegate
        {

            print("Toggled " + tg.isOn);

            if (tg.isOn)
            {
                volume = 0;
                sl.value = volume;
            }
            else
            {
                volume = 1;
                sl.value = volume;
            }


        });

        sl.onValueChanged.AddListener(delegate { ChangesliderToggle(); });

    }

    void ChangesliderToggle()
    {
        if (sl.value != sl.minValue)
            tg.isOn = false;
        else
            tg.isOn = true;

    }
    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("GameVolume", sl.value);
        //Debug.Log("Setting Game Vol on dESTROY: " + PlayerPrefs.GetFloat("GameVolume"));

    }
}

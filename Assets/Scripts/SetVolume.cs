using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    private void Start()
    {
        if (gameObject.name == "SliderMusic")
        {
            gameObject.GetComponent<Slider>().value = GameManager.instance.musicLevel;
            SetLevel(GameManager.instance.musicLevel);
        }
        else
        {
            gameObject.GetComponent<Slider>().value = GameManager.instance.sfxLevel;
            //SetLevelSFX(GameManager.instance.sfxLevel);
        }
    }
    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        GameManager.instance.musicLevel = sliderValue;
    }

    //public void SetLevelSFX(float sliderValue)
    //{
    //    mixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
    //    GameManager.instance.sfxLevel = sliderValue;
    //}
}

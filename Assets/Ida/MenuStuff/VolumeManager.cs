using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public List<VolumeController> volumeControllers;
    private float MainVolume = 1f;
    [SerializeField] Slider volumeSlider;

    void OnEnable()
    {

        MainVolume = PlayerPrefs.GetFloat("mainVolume", MainVolume);
        if (volumeSlider != null)
        {
            StartCoroutine(changeSliderWhenActive());
        }
    }

    void OnDisable()
    {

        PlayerPrefs.SetFloat("mainVolume", MainVolume);

    }

    public void changeMainVolume()
    {
        MainVolume = Mathf.Clamp(volumeSlider.value,0f,1f);
        foreach (VolumeController controller in volumeControllers)
        {
            if (controller != null && controller.isActiveAndEnabled)
            {
                controller.adjustVolume(MainVolume);
            }
        }
    }


    public float getVolume()
    {
        return MainVolume;
    }

    private IEnumerator changeSliderWhenActive()
    {
        yield return new WaitUntil(() => volumeSlider.isActiveAndEnabled);
        volumeSlider.SetValueWithoutNotify(MainVolume);
        changeMainVolume();
    }
}

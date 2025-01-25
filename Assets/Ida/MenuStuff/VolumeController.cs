using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VolumeController : MonoBehaviour
{

    private VolumeManager volumeManager;
    private AudioSource audioSource;
    [SerializeField] private float audioSourceMaxVolume = 1f;


    private void Awake()
    {
        volumeManager = FindFirstObjectByType<VolumeManager>();
        if (volumeManager == null)
        {
            Debug.LogWarning("Could not find Volume Manager!");
        }
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        volumeManager?.volumeControllers.Add(this);
        adjustVolume(volumeManager.getVolume());
    }

    private void OnDisable()
    {
        volumeManager?.volumeControllers.Remove(this);
    }


    public void adjustVolume(float volume)
    {
        audioSource.volume = getRelativeVolume(volume);
    }

    private float getRelativeVolume(float input_volume)
    {
        return audioSourceMaxVolume * input_volume;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTimeScaler : MonoBehaviour
{

    AudioSource audioSrc;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        audioSrc.pitch = Time.timeScale;
    }
}

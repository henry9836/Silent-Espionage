using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    public List<AudioClip> tracks = new List<AudioClip>();

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!audio.isPlaying)
        {
            audio.clip = tracks[Random.Range(0, tracks.Count)];
            audio.Play();
        }
    }
}

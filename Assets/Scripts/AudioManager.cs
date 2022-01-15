using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip flap;
    public AudioClip die;
    public AudioClip point;

    private AudioSource source;

    private void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlayFlap()
    {
        source.PlayOneShot(flap);
    }

    public void PlayDie()
    {
        source.PlayOneShot(die);
    }

    public void PlayPoint()
    {
        source.PlayOneShot(point);
    }
}

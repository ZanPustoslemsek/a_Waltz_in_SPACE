using UnityEngine.Audio;
using UnityEngine;
using Unity.VisualScripting;

[System.Serializable]
public class Sound
{   
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;
    public bool playOnAwake;

    [HideInInspector]
    public AudioSource source;
}

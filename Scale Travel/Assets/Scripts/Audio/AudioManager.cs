using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] sounds;

    public float globalVolume = 0.8f;

    private void Awake() 
    {
        if(Instance){Destroy(this.gameObject);return;}
        DontDestroyOnLoad(this);
        Instance = this;

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
            
            if(s.source.playOnAwake)s.source.Play();            
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) {Debug.Log(name + ": not found"); return;}
        s.source.Play();
    }

    public void PlayNotForced(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) {Debug.Log(name + ": not found"); return;}
        if(!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) {Debug.Log(name + ": not found"); return;}
        s.source.Stop();
    }

    public void PlayDynamicly(string name, float time)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) {Debug.Log(name + ": not found"); return;}
        StartCoroutine(PlayDynamiclyCoroutine(s,time));
    }
    IEnumerator PlayDynamiclyCoroutine(Sound s, float time)
    {
        float volume = s.volume*globalVolume;
        float pitch = s.pitch;

        s.source.volume = 0;
        s.source.pitch = 0;

        float timeLerped = 0;

        s.source.Play();
        while(true)
        {
            timeLerped += Time.deltaTime;
            s.source.volume = Mathf.Lerp(s.source.volume, volume,timeLerped / time);
            s.source.pitch = Mathf.Lerp(s.source.pitch, pitch,timeLerped / time);

            if(s.source.volume >= volume){s.source.volume = volume*globalVolume; s.source.pitch = pitch;break;}
            yield return new WaitForSeconds(0);
        }
    }

    public void StopDynamicly(string name, float time, bool pit = true)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) {Debug.Log(name + ": not found"); return;}
        StartCoroutine(StopDynamiclyCoroutine(s,time,pit));
    }
    IEnumerator StopDynamiclyCoroutine(Sound s, float time, bool pit = true)
    {
        float volume = 0;
        float pitch = 0;

        float timeLerped = 0;

        while(true)
        {
            timeLerped += Time.unscaledDeltaTime;
            s.source.volume = Mathf.Lerp(s.source.volume, volume,timeLerped / time);
            if(pit)s.source.pitch = Mathf.Lerp(s.source.pitch, pitch,timeLerped / time);

            if(s.source.volume <= volume){s.source.volume = volume; s.source.pitch = pitch;break;}
            yield return new WaitForSecondsRealtime(0);
        }
        s.source.Stop();
        s.source.volume = s.volume * globalVolume;
        s.source.pitch = s.pitch;
    }

    public void SetVolume(float volume)
    {
        globalVolume = volume;
        foreach(Sound s in sounds)
        s.source.volume = s.volume * volume; 
    }

    public void PlayRandom(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) {Debug.Log(name + ": not found"); return;}
        s.source.pitch = UnityEngine.Random.Range(s.pitch*0.95f, s.pitch*1.05f);
        s.source.volume = UnityEngine.Random.Range(s.volume*0.95f, s.volume*1.05f) * globalVolume;
        s.source.Play();
    }
}

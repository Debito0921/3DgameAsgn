using UnityEngine.Audio;
using System;
using UnityEngine;

public class Audios : MonoBehaviour
{
    public Sound[] sounds;

    public static Audios instance;
    public AudioSource[] audioSources;
    // Start is called before the first frame update
    void Awake()
    {

        //If there is another audio manager, delete that one, else this will be the audiomanager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        /*
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;
        }
        */
        audioSources = new AudioSource[sounds.Length];

        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].source = gameObject.AddComponent<AudioSource>();
            sounds[i].source.clip = sounds[i].clip;

            sounds[i].source.volume = sounds[i].volume;
            sounds[i].source.pitch = sounds[i].pitch;

            sounds[i].source.loop = sounds[i].loop;

            audioSources[i] = sounds[i].source;
        }

    }

    public void PlaySound (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + "not found");
            return;
        }
        s.source.Play();
    }

    public void StopAllSounds()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Stop();
        }
    }
}

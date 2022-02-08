using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioMixerGroup sfx;
    public AudioMixerGroup music;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;

            if (s.name == "Theme")
            {
                s.source.outputAudioMixerGroup = music;
            }
            else
            {
                s.source.outputAudioMixerGroup = sfx;
            }

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Theme");
    }


    public void Play(string name, Vector3 soundPos = new Vector3())
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        if (name == "Body" || name == "Wall" || name == "Boom" || name == "Alarm")
        {
            AudioSource.PlayClipAtPoint(s.source.clip, soundPos);
        }
        s.source.Play();
    }
    
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Stop();
        }
    }

}

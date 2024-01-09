using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;
    public List<AudioClip> SoundEffects;

    void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void Awake()
    {
        CreateSingleton();
    }

    public void PlaySoundEffect(string soundEffect)
    {
        int index = SoundEffects.FindIndex(i => i.name == soundEffect);
        if (index == -1)
            return;

        AudioClip clip = SoundEffects[index];

        audioSource.PlayOneShot(clip);
    }

    public void PlayHitEffect()
    {
        int i = Random.Range(0, 5);
        i++;
        PlaySoundEffect("Hit" + i.ToString());
    }
}

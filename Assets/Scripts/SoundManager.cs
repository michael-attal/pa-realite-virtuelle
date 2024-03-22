using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private AudioSource ambientAudioSource;
    [SerializeField] private AudioSource punchAudioSource;
    
    public bool playAmbientSound = false;
    public AmbientSounds currentAmbientSound = AmbientSounds.RockyTheme;
    
    void Start()
    {
        PlayAmbientSound(currentAmbientSound, playAmbientSound);
    }
    
    void Update()
    {
        // TODO: Manage sound on punch collision
        if (false)
        {
            
        }
    }

    public void SwitchStatePlayAmbientSound()
    {
        playAmbientSound = !playAmbientSound;
        PlayAmbientSound(currentAmbientSound, playAmbientSound);
    }
    
    public void SwitchAmbientSound(int sound)
    {
        currentAmbientSound = (AmbientSounds)sound;
        PlayAmbientSound(currentAmbientSound, playAmbientSound);
    }

    public void UpdateVolume(float volume)
    {
        ambientAudioSource.volume = volume;
        punchAudioSource.volume = volume;
    }
    
    void PlayAmbientSound(AmbientSounds sound, bool playSound)
    {
        AudioClip clip = sounds[(int)sound];
        ambientAudioSource.clip = clip;
        ambientAudioSource.loop = true;
        if (playSound) {
            ambientAudioSource.Play();
        } else {
            ambientAudioSource.Stop();
        }
    }

}

public enum AmbientSounds
{
    RockyTheme = 0,
    Sunset = 1
}

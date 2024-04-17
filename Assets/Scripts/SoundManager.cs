using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private AudioSource ambientAudioSource;
    [SerializeField] private AudioSource punchAudioSource;
    [SerializeField] private Toggle toggleAmbientSoundUI;
    [SerializeField] private Toggle togglePunchSoundUI;

    public bool enableAmbientSound = true;
    public bool enablePunchSound = true;
    public AmbientSounds currentAmbientSound = AmbientSounds.Sunset;

    private void Start()
    {
        toggleAmbientSoundUI.onValueChanged.AddListener(SwitchStatePlayAmbientSound);
        togglePunchSoundUI.onValueChanged.AddListener(SwitchStatePlayPunchSound);
        toggleAmbientSoundUI.isOn = enableAmbientSound;
        togglePunchSoundUI.isOn = enablePunchSound;
    }

    public void SwitchStatePlayAmbientSound(bool isOn)
    {
        enableAmbientSound = isOn;
        toggleAmbientSoundUI.transform.Find("Image").gameObject.transform.Find("On Background").gameObject.SetActive(isOn);
        PlayAmbientSound();
    }

    public void SwitchStatePlayPunchSound(bool isOn)
    {
        enablePunchSound = isOn;
        togglePunchSoundUI.transform.Find("Image").gameObject.transform.Find("On Background").gameObject.SetActive(isOn);
        PlayPunchSound();
    }

    public void ChangeAmbientSound(int sound)
    {
        currentAmbientSound = (AmbientSounds)sound;
    }

    public void UpdateVolume(float volume)
    {
        ambientAudioSource.volume = volume;
        punchAudioSource.volume = volume;
    }

    public void PlayAmbientSound()
    {
        var clip = sounds[(int)currentAmbientSound];
        ambientAudioSource.clip = clip;
        ambientAudioSource.loop = true;
        if (enableAmbientSound)
        {
            ambientAudioSource.Play();
        }
        else
        {
            ambientAudioSource.Stop();
        }
    }

    public void PlayPunchSound(PunchType punchType = PunchType.Normal)
    {
        if (enablePunchSound)
        {
            var punchSound = punchType switch
            {
                PunchType.Normal => (PunchSounds)Random.Range(0, 4),
                PunchType.Perfect => PunchSounds.PunchPerfect,
                PunchType.Miss => PunchSounds.PunchMiss,
                _ => throw new ArgumentOutOfRangeException(nameof(punchType), punchType, null)
            };

            var clip = sounds[(int)punchSound + 2]; // NOTE: Punch sound starts at index 2 in the sounds array
            punchAudioSource.clip = clip;
            punchAudioSource.PlayOneShot(clip);
        }
    }
}

public enum AmbientSounds
{
    Sunset = 0,
    RockyTheme = 1
}

public enum PunchSounds
{
    PunchOne = 0,
    PunchTwo = 1,
    PunchThree = 2,
    PunchFour = 3,
    PunchPerfect = 4,
    PunchMiss = 5
}

public enum PunchType
{
    Normal,
    Perfect,
    Miss
}
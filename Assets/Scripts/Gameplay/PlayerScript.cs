using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private SoundManager soundManager;

    private void Start()
    {
        var soundManagerObject = GameObject.Find("SoundManager");
        soundManager = soundManagerObject.GetComponent<SoundManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        // NOTE: If punch doesn't hit target, then play miss punch sounds
        if (other.gameObject.CompareTag("PunchingBag"))
        {
            soundManager.PlayPunchSound(PunchType.Miss);
        }
    }
}
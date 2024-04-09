using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private HandChirality hand;
    private XRController leftController;
    private XRController rightController;
    private SoundManager soundManager;

    private void Start()
    {
        var soundManagerObject = GameObject.Find("SoundManager");
        soundManager = soundManagerObject.GetComponent<SoundManager>();
        rightController = InputSystem.GetDevice<XRController>(CommonUsages.RightHand);
        leftController = InputSystem.GetDevice<XRController>(CommonUsages.LeftHand);
    }

    private void OnCollisionEnter(Collision other)
    {
        // NOTE: If punch doesn't hit target, then play miss punch sounds
        if (other.gameObject.CompareTag("PunchingBag"))
        {
            soundManager.PlayPunchSound(PunchType.Miss);
            SendHapticImpulse(hand);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target") || other.gameObject.CompareTag("Wood"))
        {
            SendHapticImpulse(hand);
        }
    }

    private void SendHapticImpulse(HandChirality handToGetHapticImpulse = HandChirality.both)
    {
        var command = SendHapticImpulseCommand.Create(0, 0.7f, 0.1f);
        if (handToGetHapticImpulse is HandChirality.right or HandChirality.both)
        {
            rightController.ExecuteCommand(ref command);
        }

        if (handToGetHapticImpulse is HandChirality.left or HandChirality.both)
        {
            leftController.ExecuteCommand(ref command);
        }
    }
}

public enum HandChirality
{
    left,
    right,
    both
}
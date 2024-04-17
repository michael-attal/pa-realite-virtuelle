using System.Collections;
using UnityEngine;

public class WoodenStickScript : MonoBehaviour
{
    [SerializeField] private int dodgeSuccessPoint = 100;
    [SerializeField] private int dodgeFailedPenalityPoint = 100;
    [SerializeField] private float speedStickOut = 3.0f;
    [SerializeField] private float rangeX = 0.3f;
    [SerializeField] private float rangeY = 0.5f;
    [SerializeField] private int maximumTimeBeforeNextPunch = 3;
    [SerializeField] private GameObject woodenPlank;

    private bool isCoroutineRunning;
    private bool isStickDangerous;
    private SoundManager soundManager;

    private void Start()
    {
        var soundManagerObject = GameObject.Find("SoundManager");
        soundManager = soundManagerObject.GetComponent<SoundManager>();
    }

    private void Update()
    {
        if (!isStickDangerous)
        {
            StartCoroutine(ThrowStick());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isStickDangerous)
        {
            soundManager.PlayPunchSound();
            ScoreManager.Instance.Score -= dodgeFailedPenalityPoint;
        }
    }

    private IEnumerator ThrowStick()
    {
        if (isCoroutineRunning)
        {
            yield break; // NOTE: Exit if there's already a coroutine running
        }

        isCoroutineRunning = true;

        // NOTE: Wait before the next punch (set maximumTimeBeforeNextPunch to 0 for no wait time)
        if (maximumTimeBeforeNextPunch > 0)
        {
            yield return new WaitForSeconds(Random.Range(0, maximumTimeBeforeNextPunch));
        }

        isStickDangerous = true;

        // NOTE: Simulate the stick being thrown out
        // TODO: Export the wooden stick with apply transform to not have to add -0.3f or (rangeX * 3f)
        var startPosition = woodenPlank.transform.position + new Vector3(-0.3f, 0, -1f); // Start behind the wooden plank
        var endPosition = woodenPlank.transform.position + new Vector3(Random.Range(-(rangeX * 3f), rangeX), Random.Range(-rangeY, rangeY - 0.2f), 0);

        var time = 0f;

        while (time < 1f)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            time += Time.deltaTime * speedStickOut;
            yield return null;
        }

        // NOTE: Wait for some time before moving the stick back
        yield return new WaitForSeconds(1f);

        // NOTE: Simulate the stick moving back behind the wooden plank
        time = 0f;
        while (time < 1f)
        {
            transform.position = Vector3.Lerp(endPosition, startPosition, time);
            time += Time.deltaTime * speedStickOut;
            yield return null;
        }

        isStickDangerous = false;
        var playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
        HandleSuccessfulDodge(playerCollider);

        isCoroutineRunning = false;
    }

    private void HandleSuccessfulDodge(Collider playerCollider)
    {
        // NOTE: Check if the stick is no longer dangerous and the player is within two meters of the wooden plank to not give free point
        if (!isStickDangerous && Vector3.Distance(playerCollider.transform.position, woodenPlank.transform.position) <= 2f)
        {
            // NOTE: Player successfully dodged the stick and is close enough to the plank
            // soundManager.PlayPunchSound(PunchType.Miss); // TODO: Find a better sound for dodge success
            ScoreManager.Instance.Score += dodgeSuccessPoint;
        }
    }
}
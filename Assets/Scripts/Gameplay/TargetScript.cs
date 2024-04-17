using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TargetScript : MonoBehaviour
{
    private const int PrecisionBonus = 500;
    private const int PowerBonus = 500;
    private const int TimingBonus = 500;
    private const int PerfectHitThreshhold = 1100;

    private const float MinimumSize = 0.25f;
    
    private SoundManager soundManager;

    [SerializeField] private Image timingCircle;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [SerializeField] private Transform idealHitPoint;
    [SerializeField] private ParticleSystem[] onHitParticleSystems;

    [SerializeField] private float hitDistanceTolerance;

    private float timingFactor;
    private bool waitingForHit;

    private void Start()
    {
        GameObject soundManagerObject = GameObject.Find("SoundManager");
        soundManager = soundManagerObject.GetComponent<SoundManager>();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Rigidbody otherBody))
        {
            // Check power of hit
            var powerFactor = Mathf.Clamp01(-Mathf.Cos(Vector3.Angle(transform.forward, otherBody.velocity)));
            if (powerFactor <= .1)
                return;
            
            // Check precision of hit
            var closest = other.ClosestPoint(idealHitPoint.position);
            var precisionFactor = 1 - Mathf.Clamp(Vector3.Distance(closest, idealHitPoint.position), 0, hitDistanceTolerance) / hitDistanceTolerance;
            
            var totalPoints = (int)(PrecisionBonus * precisionFactor + PowerBonus * powerFactor + TimingBonus * timingFactor);
            if (totalPoints >= PerfectHitThreshhold)
                foreach (var particles in onHitParticleSystems)
                    particles.Play();


            soundManager.PlayPunchSound(totalPoints >= PerfectHitThreshhold ? PunchType.Perfect : PunchType.Normal);
            ScoreManager.Instance.Score += totalPoints;
            
            gameObject.SetActive(false);
        }
    }

    public void StartCountdown(float seconds)
    {
        gameObject.SetActive(true);
        StartCoroutine(CountdownHit(seconds));
    }

    private IEnumerator CountdownHit(float seconds)
    {
        float timing = 0;
        while (timing <= seconds)
        {
            // TimingFactor is also used to calculate the points given at a given time
            timingFactor = timing / seconds;
            timingCircle.color = Color.Lerp(startColor, endColor, timingFactor);
            timingCircle.rectTransform.localScale = Vector3.one * Mathf.Lerp(MinimumSize, 1f, 1 - timingFactor);
            
            timing += Time.deltaTime;
            yield return null;
        }

        timingFactor = 0f;
        gameObject.SetActive(false);
    }
}

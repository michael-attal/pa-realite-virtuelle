using System;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    private const int PrecisionBonus = 500;
    private const int PowerBonus = 500;
    private const int PerfectHitThreshhold = 800;
    private SoundManager soundManager;
    
    [SerializeField] private Transform idealHitPoint;
    [SerializeField] private ParticleSystem[] onHitParticleSystems;

    [SerializeField] private float hitDistanceTolerance;

    private void Start()
    {
        GameObject soundManagerObject = GameObject.Find("SoundManager");
        soundManager = soundManagerObject.GetComponent<SoundManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Rigidbody otherBody))
        {
            var otherTransform = other.transform;

            // Check power of hit
            var powerFactor = Mathf.Clamp01(-Mathf.Cos(Vector3.Angle(transform.forward, otherBody.velocity)));
            if (powerFactor <= .1)
                return;
            
            // Check precision of hit
            var closest = other.ClosestPoint(idealHitPoint.position);
            var precisionFactor = 1 - Mathf.Clamp(Vector3.Distance(closest, idealHitPoint.position), 0, hitDistanceTolerance) / hitDistanceTolerance;

            var totalPoints = (int)(PrecisionBonus * precisionFactor + PowerBonus * powerFactor);
            if (totalPoints >= PerfectHitThreshhold)
                foreach (var particles in onHitParticleSystems)
                    particles.Play();

            soundManager.PlayPunchSound(totalPoints >= PerfectHitThreshhold ? PunchType.Perfect : PunchType.Normal);
            ScoreManager.Instance.Score += totalPoints;
        }
    }
}

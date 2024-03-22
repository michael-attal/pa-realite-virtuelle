using UnityEngine;

public class TargetScript : MonoBehaviour
{
    private const int PrecisionBonus = 500;
    private const int PowerBonus = 500;
    private const int PerfectHitThreshhold = 900;
    
    [SerializeField] private Transform idealHitPoint;

    [SerializeField] private float hitTolerance;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Rigidbody otherBody))
        {
            var otherTransform = other.transform;
            
            // Check precision of hit
            var closest = other.ClosestPoint(idealHitPoint.position);
            var precisionFactor = 1 - Vector3.Distance(closest, idealHitPoint.position) / hitTolerance;
            
            // Check power of hit
            var powerFactor = Mathf.Clamp01(-Mathf.Cos(Vector3.Angle(transform.forward, otherBody.velocity)));

            var totalPoints = (int)(PrecisionBonus * precisionFactor + PowerBonus * powerFactor);
            if (totalPoints >= PerfectHitThreshhold)
                Debug.Log("BAM !!!");

            ScoreManager.Instance.Score += totalPoints;
        }
    }
}

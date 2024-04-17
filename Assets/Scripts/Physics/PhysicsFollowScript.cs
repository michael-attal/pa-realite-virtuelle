using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsFollowScript : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private Transform followedObject;

    [SerializeField] private float followSpeed = 1000f;
    [SerializeField] private float followRotationSpeed = 4000f;
    [SerializeField] private float distanceToReset = 1f;

    private void FixedUpdate()
    {
        var currentPosition = transform.position;
        var followedPosition = followedObject.position;
        var dist = Vector3.Distance(currentPosition, followedPosition);
        
        if (dist > distanceToReset)
        {
            // If hitbox is too far from current position, reset current position
            // Have to be careful with this, it could let players go through walls...
            transform.SetPositionAndRotation(followedObject.position, followedObject.rotation);
        }
        else
        {
            // Track Position
            body.velocity = (followedPosition - currentPosition).normalized * (followSpeed * dist * Time.fixedDeltaTime);

            // Track Rotation
            var rotation = followedObject.rotation * Quaternion.Inverse(body.rotation);
            rotation.ToAngleAxis(out float angle, out Vector3 axis);
            body.angularVelocity = axis * (angle * Mathf.Deg2Rad * followRotationSpeed * Time.fixedDeltaTime);
        }
    }
}

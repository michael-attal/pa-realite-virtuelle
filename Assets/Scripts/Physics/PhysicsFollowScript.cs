using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsFollowScript : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private Transform followedObject;

    [SerializeField] private float followSpeed = 1000f;
    [SerializeField] private float followRotationSpeed = 4000f;

    private void Start()
    {
        transform.SetPositionAndRotation(followedObject.position, followedObject.rotation);
    }

    private void FixedUpdate()
    {
        // Track Position
        var currentPosition = transform.position;
        var followedPosition = followedObject.position;
        var dist = Vector3.Distance(currentPosition, followedPosition);
        body.velocity = (followedPosition - currentPosition).normalized * (followSpeed * dist * Time.fixedDeltaTime);
        
        // Track Rotation
        var rotation = followedObject.rotation * Quaternion.Inverse(body.rotation);
        rotation.ToAngleAxis(out float angle, out Vector3 axis);
        body.angularVelocity = axis * (angle * Mathf.Deg2Rad * followRotationSpeed * Time.fixedDeltaTime);
    }
}

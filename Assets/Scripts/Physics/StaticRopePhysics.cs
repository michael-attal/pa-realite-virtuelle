using UnityEngine;

public class StaticRopePhysics : MonoBehaviour
{
    [SerializeField] private Transform attachPoint;
    [SerializeField] private Transform bodyAttachPoint;
    [SerializeField] private Rigidbody attachedBody;

    private float ropeLength;

    private void Start()
    {
        ropeLength = Vector3.Distance(attachPoint.position, bodyAttachPoint.position);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var fixedDeltaTime = Time.fixedDeltaTime;

        var upwardsVector = attachPoint.position - bodyAttachPoint.position;
        transform.rotation = Quaternion.LookRotation(Vector3.Cross(upwardsVector, -transform.right), upwardsVector);

        var futurePos = bodyAttachPoint.position + attachedBody.velocity * fixedDeltaTime;
        if (Vector3.Distance(futurePos, attachPoint.position) > ropeLength)
        {
            var newFuturePos = attachPoint.position + (futurePos - attachPoint.position).normalized * ropeLength;
            var newVelocity = (newFuturePos - bodyAttachPoint.position) / fixedDeltaTime;

            attachedBody.velocity = newVelocity;
        }
    }
}
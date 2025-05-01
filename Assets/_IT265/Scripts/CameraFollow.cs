using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow Timing")]
    public float snapSpeed = 10f;   // speed when focusing at turn start
    public float followSpeed = 3f;  // speed while following during turn
    public bool followRotation = false;

    [Header("Offset Settings")]
    public Vector3 positionOffset = new Vector3(0, 5, -7);
    public Vector3 rotationOffset = Vector3.zero;

    private bool isSnapping = false;

    public void FocusOnTarget(Transform newTarget)
    {
        target = newTarget;
        isSnapping = true;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + positionOffset;
        float speed = isSnapping ? snapSpeed : followSpeed;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * speed);

        if (followRotation)
        {
            Quaternion desiredRotation = Quaternion.Euler(target.eulerAngles + rotationOffset);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * speed);
        }

        if (isSnapping && Vector3.Distance(transform.position, desiredPosition) < 0.01f)
        {
            isSnapping = false; // Finished snapping
        }
    }
}
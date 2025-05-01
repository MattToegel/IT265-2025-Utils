using UnityEngine;

[ExecuteAlways]
public class Billboard : MonoBehaviour
{
    [Header("Billboard Settings")]
    public Camera targetCamera;

    [Range(0f, 1f)]
    public float lookStrength = 1f;

    [Tooltip("Lock rotation on axis (e.g., lock Y to keep upright)")]
    public bool lockX = false;
    public bool lockY = false;
    public bool lockZ = false;

    private void LateUpdate()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
            if (targetCamera == null) return;
        }

        Vector3 camForward = targetCamera.transform.forward;
        Vector3 lookDirection = transform.position - targetCamera.transform.position;

        // Prevent zero direction
        if (lookDirection.sqrMagnitude < 0.0001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        Vector3 euler = targetRotation.eulerAngles;

        if (lockX) euler.x = transform.rotation.eulerAngles.x;
        if (lockY) euler.y = transform.rotation.eulerAngles.y;
        if (lockZ) euler.z = transform.rotation.eulerAngles.z;

        Quaternion finalRotation = Quaternion.Euler(euler);

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, lookStrength);
    }
}
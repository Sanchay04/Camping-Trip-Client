//Sanchay Ravindiran 2020

/*
    This script controls the camera by making it
    move smoothly towards a provided target transform.
*/

using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static Transform targetTransform;

    private void Start()
    {
        if (!targetTransform)
        {
            targetTransform = transform;
        }
    }

    private void FixedUpdate()
    {
        if (!targetTransform)
        {
            targetTransform = transform;
        }

        Vector2 currentVelocity = Vector2.zero;
        Vector2 adjustedTarget = targetTransform.position;

        adjustedTarget.y = Mathf.Clamp(adjustedTarget.y, -20, -.5f);
        adjustedTarget.x = Mathf.Clamp(adjustedTarget.x, -35, 131);

        transform.position = Vector2.SmoothDamp(transform.position, adjustedTarget, ref currentVelocity, Time.deltaTime * 3.75f);
    }

    public static void Target(Transform target)
    {
        targetTransform = target;
    }
}

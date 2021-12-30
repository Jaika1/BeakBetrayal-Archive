using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterCamera : MonoBehaviour
{
    [HideInInspector]
    public GameObject ObjectToFollow;
    public float DistanceFromTop = 13.0f;
    public float CameraAngle = 18.0f;
    public Camera AttachedCamera;

    private float cameraAngleRadians;
    private float defaultFOV;
    private void Start()
    {
        cameraAngleRadians = CameraAngle * (Mathf.PI / 180.0f);
        AttachedCamera = GetComponent<Camera>();
        defaultFOV = AttachedCamera.fieldOfView;
    }

    public void SetFOV(float fov)
    {
        AttachedCamera.fieldOfView = fov;
    }

    public void ResetFOV()
    {
        AttachedCamera.fieldOfView = defaultFOV;
    }


    private void LateUpdate()
    {
        // adj * tan (angle)
        transform.position = new Vector3(
            ObjectToFollow.transform.position.x,
            ObjectToFollow.transform.position.y + DistanceFromTop,
            ObjectToFollow.transform.position.z - (DistanceFromTop * Mathf.Tan(cameraAngleRadians))
            );

        transform.LookAt(ObjectToFollow.transform);
    }
}

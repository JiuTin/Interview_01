using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCameraWithObject : MonoBehaviour
{
    Transform cameraTransform;
    Vector3 direction;
    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    private void LateUpdate()
    {
        FaceCamera();
    }

    private void FaceCamera()
    {
        direction = (cameraTransform.position - transform.position).normalized;
        transform.forward = direction;
    }
}

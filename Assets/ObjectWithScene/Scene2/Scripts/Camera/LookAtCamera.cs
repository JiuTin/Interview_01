using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform cameraPos;
    Transform hpTransform;
    public Transform parent;
    Vector3 direction;
    private void Start()
    {
        hpTransform = GetComponent<Transform>();
        cameraPos = Camera.main.transform;
    }
    private void Update()
    {
       // SetPos();
    }
    private void LateUpdate()
    {
        SetHHpLookAtCamera();
    }
    void SetHHpLookAtCamera()
    {
        direction = cameraPos.position - hpTransform.position;
        hpTransform.forward = direction;
    }
    void SetPos()
    {
        hpTransform.position = new Vector3(parent.position.x, parent.position.y +2.3f, parent.position.z);
    }
}

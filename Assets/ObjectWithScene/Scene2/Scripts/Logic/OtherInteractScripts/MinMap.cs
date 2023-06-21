using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMap : MonoBehaviour
{
    public Transform playerPos;
    void Update()
    {

        transform.position = new Vector3(playerPos.position.x, transform.position.y, playerPos.position.z);
    }
}

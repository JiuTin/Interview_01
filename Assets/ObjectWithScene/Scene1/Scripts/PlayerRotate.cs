using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate :MonoBehaviour
{ 
    public void OnRotateLeft()
    {
        transform.Rotate(0, 2, 0);
    }
    public void OnRotateRight()
    {
        transform.Rotate(0, -2, 0);
    }
}

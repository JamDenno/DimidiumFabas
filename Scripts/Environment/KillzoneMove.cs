using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillzoneMove : MonoBehaviour
{
    public float moveSpeed = 60;
    private float time = 0.0f;

    void Update()
    {
        time += Time.deltaTime;
        if ((time % 60) >= 1 && moveSpeed < 150f)
        {
            //moveSpeed += 0.05f;
            time = 0f;
        }
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.World);
    }
}

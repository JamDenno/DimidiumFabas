using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatMove : MonoBehaviour
{
    public float moveSpeed = 5;
    private bool moveLeft = false;
    private float baseX;
    public float platformScale = 0.3f;
    void Start()
    {
        baseX = gameObject.transform.position.x;
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + Random.Range(-21.5f, 21.5f)* platformScale, gameObject.transform.position.y, gameObject.transform.position.z);
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z - Random.Range(gameObject.transform.localScale.z / 3, gameObject.transform.localScale.z / 2));
        moveLeft = Random.Range(0, 2) > 0;
    }


    void Update()
    {
        if (moveLeft == false)
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed, Space.World);
            if (gameObject.transform.position.x >= baseX + 21.5* platformScale)
                moveLeft = true;
        }
        else
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed, Space.World);
            if (gameObject.transform.position.x <= baseX - 21.5* platformScale)
                moveLeft = false;
        }
    }
}

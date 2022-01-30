using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool moveUp = false;

    void Start()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, Random.Range(-9.5f, 10.6f), gameObject.transform.position.z);
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, 0.7f);
        moveUp = Random.Range(0, 2) > 0;
    }

    void Update()
    {
        if(moveUp == false)
		{
            transform.Translate(Vector3.down * Time.deltaTime * moveSpeed, Space.World);
            if (gameObject.transform.position.y <= -9.5)
                moveUp = true;
		}
        else
		{
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed, Space.World);
            if (gameObject.transform.position.y >= 10.5)
                moveUp = false;
        }
    }
}

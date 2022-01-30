using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarRandFast : MonoBehaviour
{
    public float moveSpeed;
    private bool moveLeft = false;
    private float baseX;
    public float pillarScale = 0.3f;
    void Start()
    {
        moveSpeed = Random.Range(10, 30) * pillarScale;
        baseX = gameObject.transform.position.x;
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + Random.Range(-25, 26) * pillarScale, gameObject.transform.position.y, gameObject.transform.position.z);
        gameObject.transform.localScale = new Vector3(7, 40, 7);
        moveLeft = Random.Range(0, 2) > 0;
    }


    void Update()
    {
        if (moveLeft == false)
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed, Space.World);
            if (gameObject.transform.position.x >= baseX + 28 * pillarScale)
			{
                moveSpeed = Random.Range(10, 30) * pillarScale;
                moveLeft = true;
            }
        }
        else
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed, Space.World);
            if (gameObject.transform.position.x <= baseX - 28 * pillarScale)
			{
                moveSpeed = Random.Range(10, 30) * pillarScale;
                moveLeft = false;
            }
        }
    }
}

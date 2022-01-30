using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierRandSpawn : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, Random.Range(-5f, 10.6f), gameObject.transform.position.z);
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, 0.7f);
    }
}

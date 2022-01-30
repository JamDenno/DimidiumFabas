using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatRandSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    public float platformScale = 0.3f;
    void Start()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + Random.Range(-22.5f, 21.5f)*platformScale, gameObject.transform.position.y, gameObject.transform.position.z);
        gameObject.transform.localScale = new Vector3(Random.Range(4, 10), gameObject.transform.localScale.y, gameObject.transform.localScale.z - Random.Range(gameObject.transform.localScale.z / 3, gameObject.transform.localScale.z / 2));
    }
}

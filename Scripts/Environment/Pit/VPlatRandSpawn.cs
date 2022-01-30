using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VPlatRandSpawn : MonoBehaviour
{
    public float platformScale = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + Random.Range(-22.5f, 21.5f)* platformScale, gameObject.transform.position.y, gameObject.transform.position.z);
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }
}

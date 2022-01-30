using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarRandSpawn : MonoBehaviour
{
    public float pillarScale = 0.3f;
    void Start()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + Random.Range(-25, 26) * pillarScale, gameObject.transform.position.y, gameObject.transform.position.z);
        gameObject.transform.localScale = new Vector3(7, 40, 7f);
    }
}

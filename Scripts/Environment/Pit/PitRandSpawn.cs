using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitRandSpawn : MonoBehaviour
{
    public float scaleModifier = 0.3f;
    public AdjustFloor[] floors;
    public Transform[] trans;

    public float platformScale = 0.3f;
    void Start()
    {
        scaleModifier = Random.Range(0, 0.5f * gameObject.transform.localScale.z);
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z + scaleModifier);

        for(int i = 0; i < floors.Length; i++)
		{
            floors[i].AdjustFloorPos(trans[i].position);
		}
    }
}

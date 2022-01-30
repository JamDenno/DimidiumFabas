using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustFloor : MonoBehaviour
{
    public float zPos;
    public float zScale = 0.3f;
    void Start()
    {
        
    }


    public void AdjustFloorPos(Vector3 Target) //float scaleModifier)
	{
        //Debug.Log("Adjuster: " + scaleModifier);
        //gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z - scaleModifier);
        transform.position = Target + new Vector3(0,0,zPos * zScale);
    }
}

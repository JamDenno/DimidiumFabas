using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public PlayerMovement moveScript;
    public Camera playerCam;

    float startPos = 0;
    float duration = 0;

    private float cbob;
    public float bobSpeed = 12f;
    public float sprint_bobSpeed = 15f;

    private float camount;
    public float amount = 0.08f;
    public float sprint_amount = 0.1f;

    [SerializeField] float landTimer = 0;
    public float landDuration = 1;
    public float landDepth = 0.2f;

    [SerializeField] private float landRecovertime = 0.15f;
    [SerializeField] private float recoverTimer = 0.15f;

    public float returnSpeed = 6f; // Speed that the camera returns to the startpos with

    void Start()
    {
        landTimer = landDuration;
        moveScript = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveScript.IsSprinting) { cbob = sprint_bobSpeed; camount = sprint_amount; }
        else { cbob = bobSpeed; camount = amount; }

        if (recoverTimer < landRecovertime) { recoverTimer += Time.deltaTime; }

        if (landTimer < landDuration)
        {
            landTimer += Time.deltaTime;
            if (landTimer >= landDuration)
            {
                recoverTimer = 0;
            }
            playerCam.transform.localPosition = new Vector3(playerCam.transform.localPosition.x, Mathf.Lerp(playerCam.transform.localPosition.y, playerCam.transform.localPosition.y-landDepth, Time.deltaTime*10), playerCam.transform.localPosition.z);
        }
        else if ((Mathf.Abs(Input.GetAxisRaw("Horizontal"))>0.1f || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f) && recoverTimer >= landRecovertime)
        {
            duration += Time.deltaTime * cbob;

            playerCam.transform.localPosition = new Vector3(playerCam.transform.localPosition.x, startPos + Mathf.Sin(duration) * camount, playerCam.transform.localPosition.z);
        }
        else
        {
            duration = 0;
            playerCam.transform.localPosition = new Vector3(playerCam.transform.localPosition.x, Mathf.Lerp(playerCam.transform.localPosition.y, startPos, Time.deltaTime * returnSpeed), playerCam.transform.localPosition.z);
        }
    }
    public void LandBob()
    {
        landTimer = 0;
    }
}

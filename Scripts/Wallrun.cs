using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallrun : MonoBehaviour
{
    PlayerMovement mov;
    public LayerMask Wall;
    public GameObject cameradolly = null;
    public bool leftWall, rightWall;
    public float wr_speed, wr_duration;
    private float timer = 0;
    public float maxtilt, tilt;
    public bool isWallrunning = false;

    public float boostForce = 5;
    public float boostCooldown = 1;
    private float boostTimer = 1;

    private Rigidbody rb;

    private void Start()
    {
        mov = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (boostTimer < boostCooldown) { boostTimer += Time.deltaTime; }
        CheckWall();
        if (Input.GetAxisRaw("Vertical") > 0 && rightWall && !mov.Grounded && timer<wr_duration) { Start_Wallrun(); }
        else if (Input.GetAxisRaw("Vertical") > 0 && leftWall && !mov.Grounded && timer < wr_duration) { Start_Wallrun(); }
        else if (isWallrunning) { Stop_Wallrun(); }
        if (isWallrunning) { timer += Time.deltaTime; GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, Mathf.Lerp(GetComponent<Rigidbody>().velocity.y,0,Time.deltaTime*2), GetComponent<Rigidbody>().velocity.z); 
            if (Input.GetButtonDown("Jump") && boostTimer>= boostCooldown) { WallBoost(); }
        }

        cameradolly.transform.localRotation = Quaternion.Euler(0, 0, tilt);

        if (Mathf.Abs(tilt)<maxtilt && isWallrunning)
        {
            if (rightWall)
            {
                tilt = Mathf.Lerp(tilt, maxtilt, Time.deltaTime * 8);
            }
            else if (leftWall)
            {
                tilt = Mathf.Lerp(tilt, -maxtilt, Time.deltaTime * 8);
            }
        }

        if (!rightWall && !leftWall)
        {
            if (tilt > 0)
            {
                tilt = Mathf.Lerp(tilt, 0, Time.deltaTime * 8);
            }
            if (tilt < 0)
            {
                tilt = Mathf.Lerp(tilt, 0, Time.deltaTime * 8);
            }
        }
    }
    public void Start_Wallrun()
    {
        //GetComponent<Rigidbody>().velocity = new Vector3 (GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.z);
        mov.Player_GravityEnabled = false;
        isWallrunning = true;
    }
    public void Stop_Wallrun()
    {
        mov.Player_GravityEnabled = true;
        isWallrunning = false;
    }
    public void CheckWall()
    {
        rightWall = Physics.Raycast(transform.position, transform.right, 1f, Wall);
        leftWall = Physics.Raycast(transform.position, -transform.right, 1f, Wall);
    }
    public void RefreshWallrun()
    {
        timer = 0;
    }
    public void WallBoost()
    {
        Debug.Log("Wall Boost");
        Stop_Wallrun();
        rb.AddForce(Camera.main.transform.forward * boostForce);
        boostTimer = 0;
    }
}

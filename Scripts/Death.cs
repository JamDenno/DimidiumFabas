using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    public Camera DeathCam;
    public Rigidbody rb;
    private bool active;
    public GameObject arms;
    public float deathduration = 5;
    public SwitchTrack switchscript;
    private float timer = 5;
    private void Update()
    {
        if (timer < deathduration) { timer += Time.deltaTime; if (timer >= deathduration) { SceneManager.LoadScene("MainMenu"); } }
        if (active) { DeathCam.transform.parent.transform.position = transform.position; }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Deadly" && !active)
        {
            DeathCam.gameObject.SetActive(true);
            GetComponent<PlayerMovement>().AbleToJump = false; GetComponent<PlayerMovement>().AbleToWalk = false;
            GetComponent<CameraController>().enabled = false;
            Camera.main.gameObject.tag = "Untagged";
            DeathCam.gameObject.tag = "MainCamera";
            DeathCam.depth = 5;
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
            active = true;
            DeathCam.transform.parent.transform.parent = null;
            arms.transform.parent = null; arms.GetComponent<MeshCollider>().enabled = true; arms.AddComponent<Rigidbody>();
            timer = 0;
            switchscript.AbleToSwitch = false;
        }
    }
}

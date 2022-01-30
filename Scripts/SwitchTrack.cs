using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTrack : MonoBehaviour
{
    public bool type;
    public bool Track = true; // True for light, False for dark
    public float Trackoffset = 100;

    public Camera playercam;
    public Camera altcam;

    public Color LightSkyBox;
    public Color DarkSkyBox;

    public Material SwitchMat;
    public GameObject SwitchUI;
    public float SwitchDuration;
    private float switchtimer;
    public float transitionspeed = 1.5f;

    public Texture rendertex;

    [SerializeField] private bool switching = false;
    [SerializeField] private bool reversing = false;

    [SerializeField] LayerMask LightCameraMask;
    [SerializeField] LayerMask DarkCameraMask;

    public Score score;
    public Score scoreinvert;
    public bool AbleToSwitch = true;
    private void Start()
    {
        if (Track) { playercam.cullingMask = LightCameraMask; altcam.cullingMask = DarkCameraMask; }
        else { playercam.cullingMask = DarkCameraMask; altcam.cullingMask = LightCameraMask; }
        SwitchMat.SetFloat("Timer", SwitchDuration); switchtimer = SwitchDuration;
        SwitchSky(Track);
        SwitchMat.SetFloat("Type", System.Convert.ToSingle(type));
    }
    void Update()
    {
        if (score != null) { score.Track = Track; }
        if (scoreinvert != null) { scoreinvert.Track = Track; }
        if (Input.GetButtonDown("Toggle Type")) { type = !type; SwitchMat.SetFloat("Type", System.Convert.ToSingle(type)); }
        SwitchMat.SetTexture("Texture",rendertex);
        if (switchtimer < SwitchDuration && !reversing) { switchtimer+=Time.deltaTime * transitionspeed; /*SwitchMat.SetFloat("Timer", switchtimer);*/SwitchMat.SetFloat("Timer", switchtimer); }
        else if (reversing) { switchtimer -= Time.deltaTime * transitionspeed; if (switchtimer <= 0) { switchtimer = 0; switching = false; reversing = false; SwitchMat.SetFloat("Timer", switchtimer); switchtimer = SwitchDuration; } else { SwitchMat.SetFloat("Timer", switchtimer); } }
        else if (switching) { Switch(); }
        else { SwitchUI.SetActive(false); }
        altcam.transform.position = Track ? new Vector3(playercam.transform.position.x - Trackoffset, playercam.transform.position.y, playercam.transform.position.z) : new Vector3(playercam.transform.position.x + Trackoffset, playercam.transform.position.y, playercam.transform.position.z);
        altcam.transform.rotation = playercam.transform.rotation;
        if (Input.GetButtonDown("Fire2")&&AbleToSwitch)
        {
            if (switching && !reversing) { reversing = true; }
            else if (reversing) { reversing = false; }
            else
            {
                SwitchUI.SetActive(true);
                SwitchMat.SetFloat("Timer", 0); switchtimer = 0;
                switching = true;
            }
        }
    }
    public void Switch()
    {
        if (Track) { transform.position -= new Vector3(Trackoffset, 0, 0); Track = !Track; SwitchSky(Track); playercam.cullingMask = DarkCameraMask; altcam.cullingMask = LightCameraMask; }
        else { transform.position += new Vector3(Trackoffset, 0, 0); Track = !Track; SwitchSky(Track); playercam.cullingMask = LightCameraMask; altcam.cullingMask = DarkCameraMask; }
        switching = false;
        SwitchUI.SetActive(false);
    }
    public void SwitchSky(bool tr)
    {
        if (playercam != null)
        {
            if (tr)
            {
                playercam.backgroundColor = LightSkyBox; altcam.backgroundColor = DarkSkyBox;
            }
            else
            {
                playercam.backgroundColor = DarkSkyBox; altcam.backgroundColor = LightSkyBox;
            }
        }
    }
}

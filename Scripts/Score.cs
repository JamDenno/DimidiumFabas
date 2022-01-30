using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Score : MonoBehaviour
{
    public bool invert = false;
    public bool Track;
    Text score;
    int scr = 0;
    float timer;
    public Color Light, Dark;
    private void Start()
    {
        score = GetComponent<Text>();
    }
    void Update()
    {
        if (!invert)
        {
            if (!Track) { score.color = Light; } else { score.color = Dark; }
        }
        else
        {
            if (Track) { score.color = Light; } else { score.color = Dark; }
        }
        
        timer += Time.deltaTime;
        scr += (int)timer * 1;
        score.text = ("SCORE : "+scr);
    }
}

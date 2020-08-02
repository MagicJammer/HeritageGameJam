using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip JumpSound;
    public AudioClip LandingSound;
    public AudioClip FootstepRight;
    public AudioClip FootstepLeft;
    bool stepRight;
    AudioSource a;
    void Start()
    {
        a = GetComponent<AudioSource>();
    }
    public void Step()
    {
        if (stepRight)
            a.clip = FootstepRight;
        else
            a.clip = FootstepLeft;
        stepRight = !stepRight;
        a.Play();
    }
    public void Jump()
    {
        a.clip = JumpSound;
        a.Play();
    }    
    public void Landing()
    {
        a.clip = LandingSound;
        a.Play();
    }
}

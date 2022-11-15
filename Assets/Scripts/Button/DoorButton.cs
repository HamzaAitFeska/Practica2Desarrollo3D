using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    public Animation m_Animation;
    public AnimationClip m_DoorOpeningClip;
    public AnimationClip m_DoorClosingClip;

    public AudioSource doorButtonOpening, doorButtonClosing, doorMechanism;

    public GameObject m_Button;
    Button button;

    bool doorIsOpen = false;
    private void Start()
    {
        button = m_Button.GetComponent<Button>();
    }
    private void Update()
    {
        if (button.m_ButtonIsPressed && !doorIsOpen)
        {
            doorIsOpen = true;
            SetDoorOpeningAnimation();
            AudioController.instance.PlayOneShot(doorButtonOpening);
            AudioController.instance.PlayOneShot(doorMechanism);
        }
        if (!button.m_ButtonIsPressed && doorIsOpen)
        {
            doorIsOpen = false;
            SetDoorClosingAnimation();    
            AudioController.instance.PlayOneShot(doorButtonClosing);
        }
    }
    void SetDoorOpeningAnimation()
    {
        m_Animation.CrossFadeQueued(m_DoorOpeningClip.name);
        
    }
    void SetDoorClosingAnimation()
    {
        m_Animation.CrossFadeQueued(m_DoorClosingClip.name);
       
    }
}

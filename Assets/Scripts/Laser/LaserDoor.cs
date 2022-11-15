using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDoor : MonoBehaviour
{
    public Animation m_Animation;
    public AnimationClip m_DoorOpeningClip;
    public AnimationClip m_DoorClosingClip;

    public AudioSource doorButtonOpening, doorButtonClosing, doorMechanism;

    public GameObject m_Laser;
    LaserEmitter laserEmitter;

    bool doorIsOpen = false;
    private void Start()
    {
        laserEmitter = m_Laser.GetComponent<LaserEmitter>();
    }
    private void Update()
    {
        if (laserEmitter.ReceptorIsActive && !doorIsOpen)
        {
            doorIsOpen = true;
            SetDoorOpeningAnimation();
            AudioController.instance.PlayOneShot(doorButtonOpening);
            AudioController.instance.PlayOneShot(doorMechanism);
        }
        if (!laserEmitter.ReceptorIsActive && doorIsOpen)
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

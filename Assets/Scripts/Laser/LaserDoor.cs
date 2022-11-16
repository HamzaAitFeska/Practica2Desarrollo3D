using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDoor : MonoBehaviour
{
    public Animation m_Animation;
    public AnimationClip m_DoorOpeningClip;
    public AnimationClip m_DoorClosingClip;

    public AudioSource doorButtonOpening, doorButtonClosing, doorMechanism;

    public GameObject m_LaserReceptor;
    LaserReceptor laser;

    bool doorIsOpen = false;
    private void Start()
    {
        laser = m_LaserReceptor.GetComponent<LaserReceptor>();
    }
    private void Update()
    {
        if (laser.laserReceptorIsActive && !doorIsOpen)
        {
            doorIsOpen = true;
            SetDoorOpeningAnimation();
            AudioController.instance.PlayOneShot(doorButtonOpening);
            AudioController.instance.PlayOneShot(doorMechanism);
        }
        if (!laser.laserReceptorIsActive && doorIsOpen)
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

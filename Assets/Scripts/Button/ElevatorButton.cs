using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    public Animation m_Animation;
    public AnimationClip m_ElevatorDownClip;
    public AnimationClip m_ElevatorUpClip;

    public AudioSource elevatorButtonDown, elevatorButtonUp, elevatorMechanism;

    public GameObject m_Button;
    public GameObject m_BluePortal;
    public GameObject m_OrangePortal;
    Button button;

    bool elevatorIsDown = false;
    private void Start()
    {
        button = m_Button.GetComponent<Button>();
    }
    private void Update()
    {
        if (button.m_ButtonIsPressed && !elevatorIsDown)
        {
            elevatorIsDown = true;
            SetElevatorDownAnimation();
            AudioController.instance.PlayOneShot(elevatorButtonDown);
            AudioController.instance.PlayOneShot(elevatorMechanism);
        }
        if (!button.m_ButtonIsPressed && elevatorIsDown)
        {
            elevatorIsDown = false;
            SetElevatorUpAnimation();
            AudioController.instance.PlayOneShot(elevatorButtonUp);
            StartCoroutine(ElevatorMoving());
        }
    }
    void SetElevatorDownAnimation()
    {
        m_Animation.CrossFadeQueued(m_ElevatorDownClip.name);
    }
    void SetElevatorUpAnimation()
    {
        m_Animation.CrossFadeQueued(m_ElevatorUpClip.name);
    }
    public IEnumerator ElevatorMoving()
    {
        yield return new WaitForSeconds(0.5f);
        m_BluePortal.SetActive(false);
        m_OrangePortal.SetActive(false);
    }
}

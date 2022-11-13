using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Animation m_Animation;
    public AnimationClip m_ButtonOn;
    public AnimationClip m_ButtonOff;
    public GameObject buttonLight;

    bool l_ButtonChangingState = false;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player" || other.tag == "CompanionCube") && !l_ButtonChangingState)
        {
            SetButtonOnAnimation();
            Debug.Log("ON");
            l_ButtonChangingState = true;
            StartCoroutine(ButtonIsPressed());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Player" || other.tag =="CompanionCube") && !l_ButtonChangingState)
        {
            SetButtonOffAnimation();
            buttonLight.SetActive(false);
            AudioController.instance.PlayOneShot(AudioController.instance.buttonNegative);
        }
    }
    public IEnumerator ButtonIsPressed()
    {
        yield return new WaitForSeconds(m_ButtonOn.length);
        buttonLight.SetActive(true);
        AudioController.instance.PlayOneShot(AudioController.instance.buttonPositive);
        l_ButtonChangingState = false;
    }
    void SetButtonOnAnimation()
    {
        m_Animation.CrossFade(m_ButtonOn.name);
    }
    void SetButtonOffAnimation()
    {
        m_Animation.CrossFade(m_ButtonOff.name);
    }
}

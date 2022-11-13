using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Animation m_Animation;
    public AnimationClip m_ButtonOn;
    public AnimationClip m_ButtonOff;
    public GameObject buttonLight;

    bool l_ButtonOnAnimation = false;
    bool l_EntityIsOnButton = false;
    bool l_ButtonActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player" || other.tag == "CompanionCube") && !l_ButtonOnAnimation)
        {
            l_EntityIsOnButton = true;
            SetButtonOnAnimation();
            Debug.Log("ON");
            l_ButtonOnAnimation = true;
            StartCoroutine(ButtonIsPressed());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Player" || other.tag =="CompanionCube"))
        {
            l_EntityIsOnButton = false;
            SetButtonOffAnimation();
            buttonLight.SetActive(false);
            if (l_ButtonActive)
            {
                AudioController.instance.PlayOneShot(AudioController.instance.buttonNegative);
            }
            l_ButtonActive = false;
            l_ButtonOnAnimation = false; ;
        }
    }
    public IEnumerator ButtonIsPressed()
    {
        yield return new WaitForSeconds(m_ButtonOn.length);
        if (l_EntityIsOnButton)
        {
            buttonLight.SetActive(true);
            AudioController.instance.PlayOneShot(AudioController.instance.buttonPositive);
            l_ButtonOnAnimation = false;
            l_ButtonActive = true;
        } 
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

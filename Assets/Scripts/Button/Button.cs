using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Animation m_Animation;
    public AnimationClip m_ButtonOn;
    public AnimationClip m_ButtonOff;
    public GameObject buttonLight;
    public AudioSource buttonPositive, buttonNegative;

    int numberOfEntities = 0;
    bool l_ButtonOnAnimation = false;
    bool l_EntityIsOnButton = false;
    bool l_ButtonActive = false;
    public bool m_ButtonIsPressed = false;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player" || other.tag == "CompanionCube") && !l_ButtonOnAnimation)
        {  
            if (numberOfEntities <= 0)
            {
                l_EntityIsOnButton = true;
                SetButtonOnAnimation();
                l_ButtonOnAnimation = true;
                StartCoroutine(ButtonIsPressed());
                
            }
            numberOfEntities++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Player" || other.tag =="CompanionCube"))
        {
            if (numberOfEntities > 0)
            {
                numberOfEntities--;
            }
            if (numberOfEntities <= 0)
            {
                l_EntityIsOnButton = false;
                SetButtonOffAnimation();
                buttonLight.SetActive(false);
                if (l_ButtonActive)
                {
                    AudioController.instance.PlayOneShot(buttonNegative);
                    m_ButtonIsPressed = false;
                }
                l_ButtonActive = false;
                l_ButtonOnAnimation = false;
            }
            
        } 
    }
    public IEnumerator ButtonIsPressed()
    {
        yield return new WaitForSeconds(m_ButtonOn.length);
        if (l_EntityIsOnButton)
        {
            buttonLight.SetActive(true);
            AudioController.instance.PlayOneShot(buttonPositive);
            l_ButtonOnAnimation = false;
            l_ButtonActive = true;
            m_ButtonIsPressed = true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceptor : MonoBehaviour
{
    public Animation m_Animation;
    public AnimationClip m_LaserReceptorStartingClip;
    public AnimationClip m_LaserReceptorActiveClip;

    public AudioSource laserReceptorIdle;

    bool laserReceptorIsActive = false;

    public bool m_ReceptorIsActive;

    private void Update()
    {
        if (m_ReceptorIsActive && !laserReceptorIsActive)
        {
            laserReceptorIsActive = true;
            SetLaserReceptorStartingAnimation();
            AudioController.instance.Play(laserReceptorIdle);
            StartCoroutine(LaserReceptorIsActive());
        }
        if (!m_ReceptorIsActive && laserReceptorIsActive)
        {
            laserReceptorIsActive = false;
            StopLaserReceptorActiveAnimation();
            AudioController.instance.Stop(laserReceptorIdle);
        }
        m_ReceptorIsActive = false;
    }
    public IEnumerator LaserReceptorIsActive()
    {
        yield return new WaitForSeconds(m_LaserReceptorStartingClip.length);
        if (laserReceptorIsActive)
        {
            SetLaserReceptorActiveAnimation();
        }
    }
    void SetLaserReceptorStartingAnimation()
    {
        m_Animation.CrossFadeQueued(m_LaserReceptorStartingClip.name);
    }
    void SetLaserReceptorActiveAnimation()
    {
        m_Animation.CrossFadeQueued(m_LaserReceptorActiveClip.name);
    }
    void StopLaserReceptorActiveAnimation()
    {
        m_Animation.Stop(m_LaserReceptorActiveClip.name);
    }
}

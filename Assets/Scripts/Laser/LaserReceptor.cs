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

    public bool ReceptorIsActive;

    private void Update()
    {
        if (ReceptorIsActive && !laserReceptorIsActive)
        {
            laserReceptorIsActive = true;
            SetLaserReceptorStartingAnimation();
            AudioController.instance.Play(laserReceptorIdle);
        }
        if (!ReceptorIsActive && laserReceptorIsActive)
        {
            laserReceptorIsActive = false;
            StopLaserReceptorActiveAnimation();
            SetLaserReceptorActiveAnimation();
            AudioController.instance.Stop(laserReceptorIdle);
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceptor : MonoBehaviour
{
    public Animation m_Animation;
    public AnimationClip m_LaserReceptorStartingClip;
    public AnimationClip m_LaserReceptorActiveClip;

    public AudioSource laserReceptorIdle;

    public GameObject m_Laser;
    LaserEmitter laserEmitter;

    bool laserReceptorIsActive = false;
    private void Start()
    {
        laserEmitter = m_Laser.GetComponent<LaserEmitter>();
    }
    private void Update()
    {
        if (laserEmitter.ReceptorIsActive && !laserReceptorIsActive)
        {
            laserReceptorIsActive = true;
            SetLaserReceptorStartingAnimation();
            AudioController.instance.Play(laserReceptorIdle);
        }
        if (!laserEmitter.ReceptorIsActive && laserReceptorIsActive)
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

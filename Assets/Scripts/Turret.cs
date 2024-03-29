using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Turret : MonoBehaviour
{
    // Start is called before the first frame update
    public Laser m_Laser;
    public float m_AngleLaserActive;
    public Rigidbody m_Rigidbody;
    bool m_IsAttached = false;
    public float m_OffsetPortal = 1.5f;
    Portal m_ExitPortal = null;
    public static Turret instance;

    public AudioSource turretAlert, turretDisabled, idleLoop;
    public AudioSource[] turretAlarmVoice;
    public AudioSource[] turretPickupVoice;
    public AudioSource[] turretDeadVoice;

    public bool m_TurretIsAlive = true;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        instance = this;
    }
    void Update()
    {
        if (m_TurretIsAlive) TurretLaserRange();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal") && !m_IsAttached)
        {
            Portal l_Portal = other.GetComponent<Portal>();
            if (l_Portal != m_ExitPortal)
            {
                Teleport(l_Portal);

            }
        }
        if (other.CompareTag("Laser") && !m_IsAttached)
        {
            Portal l_Portal = other.GetComponent<Portal>();
            if (l_Portal != m_ExitPortal)
            {
                Teleport(l_Portal);

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Portal")
        {
            if (other.GetComponent<Portal>() == m_ExitPortal)
            {
                m_ExitPortal = null;
            }
        }
    }
    public void SetAttached(bool Attached)
    {
        m_IsAttached = Attached;
    }
    void TurretLaserRange()
    {
        bool l_LaserAlive = Vector3.Dot(transform.up, Vector3.up) > Mathf.Cos(m_AngleLaserActive * Mathf.Deg2Rad); 
        if (l_LaserAlive || m_IsAttached)
        {
            m_Laser.Shoot();
            m_Laser.m_LineRenderer.gameObject.SetActive(true);
        }
        else if (!m_IsAttached)
        {
            TurretDeath();
        }          
    }
    public void TurretDeath()
    {
        m_Laser.m_LineRenderer.gameObject.SetActive(false);
        m_TurretIsAlive = false;
        TurretDeathSound();
    }

    public void Teleport(Portal _Portal)
    {
        Vector3 l_Position = _Portal.m_OtherPortalTransform.InverseTransformPoint(transform.position);
        Vector3 l_Direction = _Portal.m_OtherPortalTransform.InverseTransformDirection(transform.forward);
        Vector3 l_LocalVelocity = _Portal.m_OtherPortalTransform.InverseTransformDirection(m_Rigidbody.velocity);
        Vector3 l_WorldVelocity = _Portal.m_MirrorPortal.transform.TransformDirection(l_LocalVelocity);
        m_Rigidbody.isKinematic = true;
        transform.forward = _Portal.m_MirrorPortal.transform.TransformDirection(l_Direction);
        transform.position = _Portal.m_MirrorPortal.transform.TransformPoint(l_Position) + l_WorldVelocity * m_OffsetPortal;
        //transform.position = _Portal.m_MirrorPortal.transform.TransformPoint(l_Position) + l_WorldVelocity + transform.forward * m_OffsetPortal;
        m_Rigidbody.isKinematic = false;
        transform.localScale *= (_Portal.m_MirrorPortal.transform.localScale.x / _Portal.transform.localScale.x);
        m_Rigidbody.velocity = l_WorldVelocity;
        m_ExitPortal = _Portal.m_MirrorPortal;
    }
    public void TurretAlarmSound()
    {
        AudioController.instance.PlayOneShot(turretAlert);
        AudioController.instance.PlayOneShot(turretAlarmVoice[Random.Range(0, turretAlarmVoice.Length)]);
    }
    public void TurretPickupSound()
    {
        if (m_TurretIsAlive)
        {
            AudioController.instance.PlayOneShot(turretPickupVoice[Random.Range(0, turretPickupVoice.Length)]);
        }
    }     
    public void TurretDeathSound()
    {
        AudioController.instance.Stop(idleLoop);
        AudioController.instance.PlayOneShot(turretDisabled); 
        AudioController.instance.PlayOneShot(turretDeadVoice[Random.Range(0, turretDeadVoice.Length)]);
    }
}
    
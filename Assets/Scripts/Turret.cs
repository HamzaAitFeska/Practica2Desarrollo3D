using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Turret : MonoBehaviour
{
    // Start is called before the first frame update
    public Laser m_Laser;
    public float m_AngleLaserActive;
    [SerializeField] public Rigidbody m_Rigidbody;
    bool m_IsAttached = false;
    public float m_OffsetPortal = 1.5f;
    Portal m_ExitPortal = null;
    public static Turret instance;

    public AudioSource turretAlert, turretDead;
    public AudioSource[] turretAlarmVoice;
    public AudioSource[] turretPickupVoice;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        instance = this;
    }

    public void SetAttached(bool Attached)
    {
        m_IsAttached = Attached;
    }

    // Update is called once per frame
    void Update()
    {
        TurretLaserRange();        
        
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


    void TurretLaserRange()
    {
        bool l_LaserAlive = Vector3.Dot(transform.up, Vector3.up) > Mathf.Cos(m_AngleLaserActive * Mathf.Deg2Rad);
        m_Laser.m_LineRenderer.gameObject.SetActive(l_LaserAlive);
        if (l_LaserAlive)
        {
            m_Laser.Shoot();
        }
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
        AudioController.instance.PlayOneShot(turretPickupVoice[Random.Range(0, turretPickupVoice.Length)]);
    }
}
    
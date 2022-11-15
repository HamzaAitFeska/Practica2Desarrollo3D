﻿using UnityEngine;
public class RefractionVube : MonoBehaviour
{
    bool m_RefractionEnable = false;
    public Laser m_Laser;
    bool m_IsAttached = false;
    public Rigidbody m_Rigidbody;
    public float m_OffsetPortal = 1.5f;
    Portal m_ExitPortal = null;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    public void SetAttached(bool Attached)
    {
        m_IsAttached = Attached;
    }
    private void Update()
    {
        m_Laser.m_LineRenderer.gameObject.SetActive(m_RefractionEnable);
        m_RefractionEnable = false;
    }
    public void Createfraction()
    {
        if (m_RefractionEnable)
        {
            return;
        }
        m_RefractionEnable = true;
        m_Laser.Shoot();
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

}
    
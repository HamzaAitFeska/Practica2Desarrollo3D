﻿using UnityEngine;
using System.Collections.Generic;

public class Portal : MonoBehaviour
{
    public Camera m_Camera;
    public Transform m_OtherPortalTransform;
    public Portal m_MirrorPortal;
    public FPSPlayerController m_Player;
    public float m_OffsetNearPlayer;
    public List<Transform> m_ValidPoints;
    public float m_MinValidDistance;
    public float m_MaxValidDistance;
    public float m_MinDotValidAngle;
    public AudioSource[] portalOpening;
    bool m_RefractionEnable = false;
    public Laser m_Laser;

    private void Update()
    {
        m_Laser.m_LineRenderer.gameObject.SetActive(m_RefractionEnable);
        m_RefractionEnable = false;
    }
    private void LateUpdate()
    {
        Vector3 l_WorldPosiition = m_Player.m_Camera.transform.position;
        Vector3 l_localPosition = m_OtherPortalTransform.InverseTransformPoint(l_WorldPosiition);
        m_MirrorPortal.m_Camera.transform.position = m_MirrorPortal.transform.TransformPoint(l_localPosition);

        Vector3 l_WorldDirection = m_Player.m_Camera.transform.forward;
        Vector3 l_localDirection = m_OtherPortalTransform.InverseTransformDirection(l_WorldDirection);
        m_MirrorPortal.m_Camera.transform.forward = m_MirrorPortal.transform.TransformDirection(l_localDirection);

        float l_Distance = Vector3.Distance(m_MirrorPortal.m_Camera.transform.position, m_MirrorPortal.transform.position);
        m_MirrorPortal.m_Camera.nearClipPlane = l_Distance + m_OffsetNearPlayer;
    }
    public bool IsValidPosition(Vector3 StartPosition, Vector3 forward, float MaxDistance, LayerMask PortalLayerMask, out Vector3 Position, out Vector3 Normal)
    {
        Ray l_Ray = new Ray(StartPosition, forward);
        RaycastHit l_RaycastHit;
        bool l_Valid = false;
        Position = Vector3.zero;
        Normal = Vector3.forward;

        Debug.DrawRay(StartPosition, forward * 5.0f, Color.red, 5.0f);

        if (Physics.Raycast(l_Ray, out l_RaycastHit, MaxDistance, PortalLayerMask.value))
        {
           
            if (l_RaycastHit.collider.tag == "DrawableWall")
            {
                
                l_Valid = true;
                Normal = l_RaycastHit.normal;
                Position = l_RaycastHit.point;
                transform.position = Position;
                transform.rotation = Quaternion.LookRotation(Normal);
                if (FPSPlayerController.instance.BlueTexturee.activeInHierarchy)
                {
                    transform.localScale = FPSPlayerController.instance.BlueTexturee.transform.localScale;
                }
                else if(FPSPlayerController.instance.OrangeTexturee.activeInHierarchy)
                {
                    transform.localScale = FPSPlayerController.instance.OrangeTexturee.transform.localScale;
                }

                for (int i = 0; i < m_ValidPoints.Count; i++)
                {
                    Vector3 l_Direction = m_ValidPoints[i].position - StartPosition;
                    l_Direction.Normalize();
                    l_Ray = new Ray(StartPosition, l_Direction);
                    if (Physics.Raycast(l_Ray, out l_RaycastHit, MaxDistance, PortalLayerMask.value))
                    {
                        
                        if (l_RaycastHit.collider.tag == "DrawableWall")
                        {

                            float l_Distance = Vector3.Distance(Position, l_RaycastHit.point);
                            float l_DotAngle = Vector3.Dot(Normal, l_RaycastHit.normal);
                            ;
                            if (!(l_Distance >= m_MinValidDistance && l_Distance <= m_MaxValidDistance && l_DotAngle > m_MinDotValidAngle))
                            {
                                l_Valid = false;
                            }
                        }
                        else
                            l_Valid = false;
                    }
                    else
                    {
                        l_Valid = false;
                    }
                }

            }

        }
        if (l_Valid) RandomizeOpenSound();
        return l_Valid;
    }
    void RandomizeOpenSound()
    {
        AudioController.instance.PlayOneShot(portalOpening[Random.Range(0, portalOpening.Length)]);
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
}

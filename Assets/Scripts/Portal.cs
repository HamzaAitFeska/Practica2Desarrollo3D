using UnityEngine;
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
    public GameObject Offset;

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
            Debug.Log("BB " + l_RaycastHit.collider.name + " - " + l_RaycastHit.collider.tag);
            if (l_RaycastHit.collider.tag == "DrawableWall")
            {
                l_Valid = true;
                Normal = l_RaycastHit.normal;
                Position = l_RaycastHit.point;
                transform.position = Position;
                transform.rotation = Quaternion.LookRotation(Normal);

                for (int i = 0; i < m_ValidPoints.Count; i++)
                {
                    Vector3 l_Direction = m_ValidPoints[i].position - StartPosition;
                    l_Direction.Normalize();
                    l_Ray = new Ray(StartPosition, l_Direction);
                    if (Physics.Raycast(l_Ray, out l_RaycastHit, MaxDistance, PortalLayerMask.value))
                    {
                        if (l_RaycastHit.collider.tag == "DrawableWall")
                        {

                            float l_Distance = Vector3.Distance(transform.position, l_RaycastHit.point);
                            float l_DotAngle = Vector3.Dot(Normal, l_RaycastHit.normal);
                            Debug.Log("rs "+l_Distance+" | "+l_DotAngle);
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
        return l_Valid;
    }


    public void Teleport()
    {
        Vector3 l_Position = m_OtherPortalTransform.InverseTransformPoint(m_Player.transform.position);
        Vector3 l_Direction = m_OtherPortalTransform.InverseTransformDirection(-m_Player.transform.forward);

        m_Player.transform.position = m_MirrorPortal.transform.TransformPoint(l_Position);
        m_Player.transform.forward = m_MirrorPortal.transform.TransformDirection(l_Direction);
        //PlayerLife.instance.transform.position = Vector3.MoveTowards(PlayerLife.instance.transform.position, Offset.transform.position, 1);

    }
}

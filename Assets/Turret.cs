using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // Start is called before the first frame update
    public LineRenderer m_LineRenderer;
    public LayerMask m_CollisionLayerMask;
    public float m_MaxDistance;
    public float m_AngleLaserActive;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TurretRangeLaser();
    }

    void TurretRangeLaser()
    {
        RaycastHit l_RaycastHit;
        if(Physics.Raycast(new Ray(m_LineRenderer.transform.position, m_LineRenderer.transform.forward), out l_RaycastHit, m_MaxDistance, m_CollisionLayerMask.value))
        {
            Vector3 l_EndRaycastPosition;
            if(l_RaycastHit.collider)
            {
                l_EndRaycastPosition = Vector3.forward * l_RaycastHit.distance;
            }
            else
            {
                l_EndRaycastPosition = Vector3.forward * m_MaxDistance;
            }

            m_LineRenderer.SetPosition(1, l_EndRaycastPosition);
        }

    }

}
    
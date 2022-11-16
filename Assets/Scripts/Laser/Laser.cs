using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer m_LineRenderer;
    public LayerMask m_CollisionLayerMask;
    public float m_MaxDistance;
    

    public GameObject m_Turret;
    Turret turret;

    public void Start()
    {
        turret = m_Turret.GetComponent<Turret>();
    }
    public void Shoot()
    {
        float l_laserDistance = m_MaxDistance;
        RaycastHit l_RayvastHit;
        if (Physics.Raycast(new Ray(m_LineRenderer.transform.position, m_LineRenderer.transform.forward), out l_RayvastHit, m_MaxDistance, m_CollisionLayerMask.value) && !PlayerLife.instance.m_IsDead)
        {
            l_laserDistance = Vector3.Distance(m_LineRenderer.transform.position, l_RayvastHit.point);
            if (l_RayvastHit.collider.tag == "RefractionCube")
            {
                l_RayvastHit.collider.GetComponent<RefractionVube>().Createfraction();
            }
            if (l_RayvastHit.collider.tag == "Portal" && l_RayvastHit.collider.GetComponent<Portal>() == FPSPlayerController.instance.m_BluePortal && Turret.instance.m_Laser.m_LineRenderer.gameObject.activeInHierarchy)
            {
                FPSPlayerController.instance.m_OrangePortal.GetComponent<Portal>().Createfraction();
            }
            if (l_RayvastHit.collider.tag == "Portal" && l_RayvastHit.collider.GetComponent<Portal>() == FPSPlayerController.instance.m_OrangePortal && Turret.instance.m_Laser.m_LineRenderer.gameObject.activeInHierarchy)
            {
                FPSPlayerController.instance.m_BluePortal.GetComponent<Portal>().Createfraction();
            }
            if (l_RayvastHit.collider.tag == "Player")
            {
                PlayerLife.instance.currentLife = 0;
                turret.TurretAlarmSound();
            }
            if(l_RayvastHit.collider.tag == "Turret" && l_RayvastHit.collider.GetComponent<Turret>().m_TurretIsAlive)
            {
                l_RayvastHit.collider.GetComponent<Turret>().TurretDeath();
            }


        }
        m_LineRenderer.SetPosition(1, new Vector3(0, 0, l_laserDistance));
    }

    void Teleport()
    {

    }
}
    
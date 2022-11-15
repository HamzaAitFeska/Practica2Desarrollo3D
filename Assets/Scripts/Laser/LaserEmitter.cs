using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    public LineRenderer m_LineRenderer;
    public LayerMask m_CollisionLayerMask;
    public float m_MaxDistance;

    public bool ReceptorIsActive;
    public void Shoot()
    {
        float l_laserDistance = m_MaxDistance;
        RaycastHit l_RayvastHit;
        if (Physics.Raycast(new Ray(m_LineRenderer.transform.position, m_LineRenderer.transform.forward), out l_RayvastHit, m_MaxDistance, m_CollisionLayerMask.value))
        {
            l_laserDistance = Vector3.Distance(m_LineRenderer.transform.position, l_RayvastHit.point);
            if (l_RayvastHit.collider.tag == "RefractionCube")
            {
                l_RayvastHit.collider.GetComponent<RefractionVube>().Createfraction();
            }
            if (l_RayvastHit.collider.tag == "Portal" && l_RayvastHit.collider.GetComponent<Portal>() == FPSPlayerController.instance.m_BluePortal)
            {
                FPSPlayerController.instance.m_OrangePortal.GetComponent<Portal>().Createfraction();
            }
            if (l_RayvastHit.collider.tag == "Portal" && l_RayvastHit.collider.GetComponent<Portal>() == FPSPlayerController.instance.m_OrangePortal)
            {
                FPSPlayerController.instance.m_BluePortal.GetComponent<Portal>().Createfraction();
            }
            if (l_RayvastHit.collider.tag == "LaserReceptor")
            {
                ReceptorIsActive = true;
            }
            else ReceptorIsActive = false;
        }
        m_LineRenderer.SetPosition(1, new Vector3(0, 0, l_laserDistance));
    }
}

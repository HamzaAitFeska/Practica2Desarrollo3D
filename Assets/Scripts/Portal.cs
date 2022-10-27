using UnityEngine;

public class Portal : MonoBehaviour
{
    public Camera m_Camera;
    public Transform m_OtherPortaktransform;
    public Portal m_MirrorPortal;
    public FPSPlayerController m_Player;

    private void LateUpdate()
    {
        Vector3 l_WorldPosiition = m_Player.m_Camera.transform.position;
        Vector3 l_localPosition = m_OtherPortaktransform.InverseTransformPoint(l_WorldPosiition);
        m_MirrorPortal.m_Camera.transform.position = m_MirrorPortal.transform.TransformPoint(l_localPosition);

        Vector3 l_WorldDirection = m_Player.m_Camera.transform.forward;
        Vector3 l_localDirection = m_OtherPortaktransform.InverseTransformDirection(l_WorldDirection);
        m_MirrorPortal.m_Camera.transform.forward = m_MirrorPortal.transform.TransformDirection(l_localDirection);
    }
}

using UnityEngine;
public class RefractionVube : MonoBehaviour
{
    bool m_RefractionEnable = false;
    public Laser m_Laser;

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
    
}
    
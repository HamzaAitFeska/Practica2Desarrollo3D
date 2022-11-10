using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Turret : MonoBehaviour
{
    // Start is called before the first frame update
    public Laser m_Laser;
    public float m_AngleLaserActive;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TurretLaserRange();        
        
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

}
    
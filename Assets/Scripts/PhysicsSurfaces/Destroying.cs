using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroying : MonoBehaviour
{
    // Start is called before the first frame update
    public Companion m_companion;
    public Turret m_Turret;
    public RefractionVube m_refractionVube;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("CompanionCube"))
        {
            Destroy(m_companion.gameObject);
        }

        if (collision.collider.CompareTag("Turret"))
        {
            Destroy(m_Turret.gameObject);
        }

        if (collision.collider.CompareTag("RefractionCube"))
        {
            Destroy(m_refractionVube.gameObject);
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CompanionCube")
        {
            Destroy(Companion.instance.gameObject, 0.1f);
        }

        if(other.tag == "Turret")
        {
            Destroy(Turret.instance.gameObject, 0.1f);
        }
    }*/
}

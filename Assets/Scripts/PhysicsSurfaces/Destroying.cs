using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroying : MonoBehaviour
{
    // Start is called before the first frame update

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("CompanionCube"))
        {
            Destroy(Companion.instance.m_Rigidbody.gameObject, 0.1f);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CompanionCube")
        {
            Destroy(Companion.instance.m_Rigidbody.gameObject, 0.1f);
        }
    }
}

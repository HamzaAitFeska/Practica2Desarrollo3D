using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncing : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float jumpAmount = 100f;

    private void Start()
    {
        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "CompanionCube")
        {
            Companion.instance.m_Rigidbody.AddForce(transform.forward * jumpAmount, ForceMode.Impulse);
        }

        if(collision.collider.tag == "Turret")
        {
            Turret.instance.m_Rigidbody.AddForce(transform.forward * jumpAmount, ForceMode.Impulse);
        }
    }
}

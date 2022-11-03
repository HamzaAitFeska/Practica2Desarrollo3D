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
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CompanionCube")
        {
            Companion.instance.m_Rigidbody.AddForce(transform.forward * jumpAmount, ForceMode.Impulse);
        }
    }
}

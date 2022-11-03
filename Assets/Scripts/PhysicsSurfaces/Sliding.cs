using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "CompanionCube")
        {
            Companion.instance.m_Rigidbody.AddForce(-transform.right * 10, ForceMode.Impulse);
            Debug.Log("IMIN");
        }
    
    }
}

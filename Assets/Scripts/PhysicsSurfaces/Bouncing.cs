using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncing : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public float jumpAmount = 10f;

    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
          
        if (other.CompareTag("Player"))
        {
            rb.AddForce(transform.up * jumpAmount, ForceMode.Impulse);
        }
    }
}

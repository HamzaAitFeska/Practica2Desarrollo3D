using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncing : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public float jumpAmount = 100f;

    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Cube")
        {
            rb.AddForce(transform.up * jumpAmount, ForceMode.Impulse);
            Debug.Log("IM IN");
        }
    }
}

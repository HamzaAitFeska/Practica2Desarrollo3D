using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroying : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Destroy(rb.gameObject,0.2f);
        }
    }
}

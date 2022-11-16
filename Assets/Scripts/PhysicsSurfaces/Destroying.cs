using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroying : MonoBehaviour
{
    // Start is called before the first frame update
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("CompanionCube"))
        {
            Destroy(collision.collider.GetComponent<Companion>().gameObject);
        }

        if (collision.collider.CompareTag("Turret"))
        {
            Destroy(collision.collider.GetComponent<Turret>().gameObject);
        }

        if (collision.collider.CompareTag("RefractionCube"))
        {
            Destroy(collision.collider.GetComponent<RefractionVube>().gameObject);
        }
    }

    
}

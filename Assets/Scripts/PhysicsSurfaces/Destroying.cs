using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroying : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("CompanionCube"))
        {
            Destroy(Companion.instance.m_Rigidbody.gameObject,0.2f);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionSpawner : MonoBehaviour
{
    public Companion m_prefabCompanion;
    public Transform m_SpawnPosition;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameObject.Instantiate(m_prefabCompanion, m_SpawnPosition.position, m_SpawnPosition.rotation);
        }
    }
}

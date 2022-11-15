using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionSpawner : MonoBehaviour
{
    public Companion m_prefabCompanion;
    public Transform m_SpawnPosition;
    public GameObject m_Button;
    Button button;
    // Start is called before the first frame update

    bool doorIsOpen = false;
    private void Start()
    {
        button = m_Button.GetComponent<Button>();
    }
    private void Update()
    {
        if (button.m_ButtonIsPressed)
        {
            GameObject.Instantiate(m_prefabCompanion, m_SpawnPosition.position, m_SpawnPosition.rotation);
        }
    }
}

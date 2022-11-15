using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionSpawner : MonoBehaviour
{
    public Companion m_prefabCompanion;
    public Transform m_SpawnPosition;
    public GameObject m_Button;
    Button button;

    bool buttonIsActive = false;

    private void Start()
    {
        button = m_Button.GetComponent<Button>();
    }
    private void Update()
    {
        if (button != null)
        {
            if (button.m_ButtonIsPressed && !buttonIsActive)
            {
                GameObject.Instantiate(m_prefabCompanion, m_SpawnPosition.position, m_SpawnPosition.rotation);
                buttonIsActive = true;
            }
            if (!button.m_ButtonIsPressed && buttonIsActive)
            {
                buttonIsActive = false;
            }
        }
        
    }
}

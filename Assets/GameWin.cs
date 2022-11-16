using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWin : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject UIWIN;
    public GameObject UI;
    void Start()
    {
        UIWIN.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIWIN.SetActive(true);
            FPSPlayerController.instance.m_Shooting = true;
            FPSPlayerController.instance.m_AngleLocked = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            FPSPlayerController.instance.m_characterController.enabled = false;
            FPSPlayerController.instance.ThrowAttachedObject(0.0f);
            UI.SetActive(false);
        }
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(0);
    }
}

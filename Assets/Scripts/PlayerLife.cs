using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerLife instance;
    private readonly int maxLife = 100;
    [NonSerialized]public float currentLife;
    
    public KeyCode damagePlayer;
    public Vector3 CheckpointPosition;
    public float CheckpoinPitch;
    public float CheckPointYaw;
    public bool m_IsDead;
    public bool m_PlayedOnce;
    public bool m_IsCreated;
    [Header("GameOver")]
    public GameObject GameOver;
    public GameObject UI;
    
    void Start()
    {
        instance = this;
        currentLife = maxLife;
        m_IsCreated = false;
        m_IsDead = false;
        m_PlayedOnce = false;
        GameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentLife <= 0)
        {
            currentLife = 0;
            GameOver.SetActive(true);
            FPSPlayerController.instance.m_Shooting = true;
            FPSPlayerController.instance.m_AngleLocked = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            FPSPlayerController.instance.m_characterController.enabled = false;
            UI.SetActive(false);
            m_IsDead = true;
            FPSPlayerController.instance.ThrowAttachedObject(0.0f);
        }

              
    }

    private void LateUpdate()
    {
        if (m_IsDead && !m_PlayedOnce)
        {
            AudioController.instance.PlayOneShot(AudioController.instance.playerDeath);
            m_PlayedOnce = true;
        }
    }

    public void DamagePlayer()
    {
        currentLife--;
    }

    public void Death()
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        transform.position = CheckpointPosition;
        FPSPlayerController.instance.ProhibitedTexture.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        GameOver.SetActive(false);
        UI.SetActive(true);
        FPSPlayerController.instance.m_Shooting = false;
        FPSPlayerController.instance.m_Yaw = CheckPointYaw;
        FPSPlayerController.instance.m_Pitch = CheckpoinPitch;
        FPSPlayerController.instance.m_characterController.enabled = true;
        FPSPlayerController.instance.m_AngleLocked = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_IsCreated = true;
        m_IsDead = false;
        m_PlayedOnce = false;
        currentLife = maxLife;
    }
}

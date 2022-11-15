using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerLife.instance.CheckpointPosition = transform.position;
            PlayerLife.instance.CheckpoinPitch = FPSPlayerController.instance.m_Pitch;
            PlayerLife.instance.CheckPointYaw = FPSPlayerController.instance.m_Yaw;
            AudioController.instance.PlayOneShot(AudioController.instance.checkpointReached);
            Destroy(gameObject);
        }
    }
}

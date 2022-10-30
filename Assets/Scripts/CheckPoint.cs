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
            PlayerLife.instance.CheckpointRotation.x = FPSPlayerController.instance.m_PitchCotroller.rotation.x;
            PlayerLife.instance.CheckpointRotation.y = FPSPlayerController.instance.transform.rotation.y;
            //AudioController.instance.PlayOneShot(AudioController.instance.uiWarning);
            Destroy(gameObject);
        }
    }
}

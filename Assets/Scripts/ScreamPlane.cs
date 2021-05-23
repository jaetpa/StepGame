using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var playerController = other.GetComponent<PlayerController>();
            if (playerController)
            {
                playerController.PlayScreamSound();
                playerController.SetFallState();
            }
        }
    }
}

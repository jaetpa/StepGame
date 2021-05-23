using System;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    public event Action OnPlayerKilled;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(other.gameObject);
            OnPlayerKilled?.Invoke();
        }
    }

}

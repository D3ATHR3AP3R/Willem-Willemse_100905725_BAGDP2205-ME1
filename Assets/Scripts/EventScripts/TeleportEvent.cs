using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportEvent : MonoBehaviour
{
    public UnityEvent Teleport;

    private void Start()
    {
        if(Teleport == null)
        {
            Teleport = new UnityEvent();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Teleport?.Invoke();
        }
    }
}

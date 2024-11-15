using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeleportListener : MonoBehaviour
{
    [SerializeField] private TeleportEvent _Teleport;
    [SerializeField] private Transform _TeleportPoint;
    [SerializeField] private NavMeshAgent _Player; 

    private void Start()
    {
        _Teleport.Teleport.AddListener(Teleport);
    }

    private void Teleport()
    {
        Vector3 telPos = new Vector3(_TeleportPoint.position.x, _TeleportPoint.position.y, _TeleportPoint.position.z);
        _Player.Warp(telPos);
    }
}

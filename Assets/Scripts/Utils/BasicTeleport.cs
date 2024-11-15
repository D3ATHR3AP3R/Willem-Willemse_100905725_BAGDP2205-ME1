using UnityEngine;
using UnityEngine.AI;

public class BasicTeleport : MonoBehaviour
{
    [SerializeField] private Transform _TeleportPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            Vector3 telPos = new Vector3(_TeleportPoint.position.x, _TeleportPoint.position.y, _TeleportPoint.position.z);
            player.GetComponent<NavMeshAgent>().Warp(telPos);
        }
    }
}

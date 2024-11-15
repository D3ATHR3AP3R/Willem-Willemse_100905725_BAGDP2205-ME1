using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavBuilder : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _NavMeshSurface;

    private void OnEnable()
    {
        _NavMeshSurface.BuildNavMesh();
    }
}

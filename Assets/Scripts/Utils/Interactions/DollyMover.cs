using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyMover : MonoBehaviour
{
    [SerializeField] private List<Transform> _Waypoints;

    [SerializeField] private float _SmoothTime = 0.3f;
    [SerializeField] private float _ActiveTime;

    [SerializeField] private GameObject _DollySystem;

    private Transform _Target;
    private Vector3 _Velocity = Vector3.zero;
    private bool _IsMoving = false;

    private void Update()
    {
        if(_IsMoving)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _Target.position, ref _Velocity, _SmoothTime);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i <= _Waypoints.Count - 1; i++)
        {
            if(Vector3.Distance(transform.position, _Waypoints[i].position) <= 0.75f)
            {
                _IsMoving = false;
                _Target = _Waypoints[i + 1];
                StartCoroutine(MovementUpdate());
            }

            if(i == _Waypoints.Count - 1)
            {
                StartCoroutine(ObjDeactivate());
            }
        }
    }

    IEnumerator ObjDeactivate()
    {
        yield return new WaitForSeconds(_ActiveTime);
        PlayerController.instance._PlayerBrain.enabled = false;
        PlayerController.instance.CamResetTrigger();
        _DollySystem.SetActive(false);
    }

    IEnumerator MovementUpdate()
    {
        yield return new WaitForSeconds(0.2f);
        _IsMoving = true;
    }
}

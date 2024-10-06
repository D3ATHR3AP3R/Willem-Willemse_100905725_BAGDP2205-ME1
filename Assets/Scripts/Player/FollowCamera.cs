using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;

    private float _Value;
    private bool _Rotate;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position;

        Rotating();
    }

    public void OnCameraRotateInput(InputAction.CallbackContext context)
    {
        RotateHold(context);

        _Value = context.ReadValue<float>();
    }

    private void RotateHold(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _Rotate = true;
        }
        else if (context.canceled)
        {
            _Rotate = false;
        }
    }

    private void Rotating()
    {
        if (_Rotate)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 50f * _Value);
        }
    }
}

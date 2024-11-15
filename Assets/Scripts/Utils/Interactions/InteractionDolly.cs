using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDolly : InteractionBase
{
    [SerializeField] private GameObject _DollySystem;

    public override void Interact()
    {
        _DollySystem.SetActive(true);
        PlayerController.instance._PlayerBrain.enabled = true;
    }
}

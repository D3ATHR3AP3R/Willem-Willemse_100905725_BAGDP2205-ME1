using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterEffectsSMR : MonoBehaviour
{
    [SerializeField] private VisualEffect effect;
    [SerializeField] private GameObject character;

    private void Start()
    {
        if (character != null)
        {
            NPCController _Character = character.GetComponent<NPCController>();
            effect.SetSkinnedMeshRenderer("CharSkinnedMeshRenderer", character.GetComponentInChildren<SkinnedMeshRenderer>());
            _Character.characterEffectsSMR = this;
            
        }
    }

    public void OnDeath()
    {
        Destroy(this.gameObject);
    }
}

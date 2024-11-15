using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterEffectsMR : MonoBehaviour
{
    [SerializeField] private VisualEffect effect;
    [SerializeField] private GameObject character;

    private void Start()
    {
        if (character != null)
        {
            //NPCController _Character = character.GetComponent<NPCController>();
            effect.SetMesh("ItemMesh", character.GetComponentInChildren<MeshFilter>().mesh);
            //_Character.characterEffectsMR = this;
            
        }
    }

    public void OnDeath()
    {
        Destroy(this.gameObject);
    }
}

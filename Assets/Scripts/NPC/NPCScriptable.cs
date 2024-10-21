using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "NPC Data", menuName = "New NPC Data")]
public class NPCScriptable : ScriptableObject
{
    [Header("Stats")]
    public int maxHealth;
    public float walkSpeed;
    public float runSpeed;

    [Header("AI")]
    public NPCType aiType;
    public NPCState aiState;
    public float detectDistance;
    public float safeDistance;
    public float interactDistance;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;
    public float wanderingWait;

    /*[Header("Combat")]
    public int damage;
    public float attackRate;
    public float lastAttackTime;
    public float detectDistance;
    public float attackDistance;*/

    public float playerDistance;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public NPCScriptable NPCData;

    public NavMeshAgent agent;
    public Animator anim;
    public GameObject model;
    public GameObject player;
    public GameObject modelPrefab;

    public Transform centerPoint;

    [SerializeField] private float _MaxWanderDistance;
    [SerializeField] private GameObject _HitParticle;
    [SerializeField] private Transform _HitParticleSpawnPoint;

    private float _CurHealth;
    private bool _Dead;

    //public RuntimeAnimatorController idleControl;
    //public RuntimeAnimatorController walkingControl;

    void Start()
    {
        if(NPCData.aiType != NPCType.Interactive)
        {
            SetState(NPCState.Wandering);
        }
        else
        {
            SetState(NPCState.Idle);
            anim.SetBool("Interactable", true);
        }
        _CurHealth = NPCData.maxHealth;
        _Dead = false;
        //anim.runtimeAnimatorController = idleControl;
    }

    void Update()
    {
        NPCData.playerDistance = Vector3.Distance(model.transform.position, player.transform.position);

        //NPCData.anim.SetBool("Moving", NPCData.aiState != NPCState.Idle);

        if (!_Dead || NPCData.aiType != NPCType.Interactive)
        {
            switch (NPCData.aiState)
            {
                case NPCState.Idle: PassiveUpdate(); break;
                case NPCState.Wandering: PassiveUpdate(); break;
                //case NPCState.Attacking: AttackingUpdate(); break;
                case NPCState.Fleeing: FleeingUpdate(); break;
            }

            if(Vector3.Distance(transform.position, centerPoint.position) >= _MaxWanderDistance)
            {
                agent.SetDestination(centerPoint.position);
            }

            UpdateAnimator();
        }
        else if(NPCData.aiType == NPCType.Interactive)
        {
            return;
        }
    }

    void PassiveUpdate()
    {
        if (NPCData.aiState == NPCState.Wandering && agent.remainingDistance < 0.1f &&!_Dead)
        {
            SetState(NPCState.Idle);
            //anim.runtimeAnimatorController = idleControl;
            Invoke("WanderToNewLocation", Random.Range(NPCData.minWanderWaitTime, NPCData.maxWanderWaitTime));
        }

        if (NPCData.aiType == NPCType.Aggressive && NPCData.playerDistance < NPCData.detectDistance)
        {
            //SetState(NPCState.Attacking);
            return;
        }
        else if (NPCData.aiType == NPCType.Frightful && NPCData.playerDistance < NPCData.detectDistance)
        {
            SetState(NPCState.Fleeing);
            agent.SetDestination(GetFleeLocation());
        }
    }

    /*void AttackingUpdate()
    {
        if (playerDistance > attackDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(PlayerController.instance.transform.position);
        }
        else
        {
            agent.isStopped = true;

            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                PlayerController.instance.GetComponent<Idamagable>().takeDamage(damage);
                anim.SetTrigger("Attack");
            }
        }
    }*/

    void FleeingUpdate()
    {
        if (NPCData.playerDistance < NPCData.safeDistance && agent.remainingDistance < 0.1f)
        {
            agent.SetDestination(GetFleeLocation());
        }
        else if (NPCData.playerDistance > NPCData.safeDistance)
        {
            SetState(NPCState.Wandering);
        }
    }

    void SetState(NPCState newState)
    {
        NPCData.aiState = newState;

        switch (NPCData.aiState)
        {
            case NPCState.Idle:
                {
                    agent.speed = NPCData.walkSpeed;
                    agent.isStopped = true;
                    break;
                }
            case NPCState.Wandering:
                {
                    agent.speed = NPCData.walkSpeed;
                    agent.isStopped = false;
                    break;
                }
            /*case NPCState.Attacking:
                {
                    agent.speed = runSpeed;
                    agent.isStopped = false;
                    break;
                }*/
            case NPCState.Fleeing:
                {
                    agent.speed = NPCData.runSpeed;
                    agent.isStopped = false;
                    break;
                }
        }
    }

    void WanderToNewLocation()
    {
        if (NPCData.aiState != NPCState.Idle || _Dead)
            return;

        SetState(NPCState.Wandering);
        agent.SetDestination(GetWanderLocation());
        //anim.runtimeAnimatorController = walkingControl;

    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(model.transform.position + (Random.onUnitSphere * Random.Range(NPCData.minWanderDistance, NPCData.maxWanderDistance)), out hit, NPCData.maxWanderDistance, NavMesh.AllAreas);

        int i = 0;

        while (Vector3.Distance(model.transform.position, hit.position) < NPCData.detectDistance)
        {
            NavMesh.SamplePosition(model.transform.position + (Random.onUnitSphere * Random.Range(NPCData.minWanderDistance, NPCData.maxWanderDistance)), out hit, NPCData.maxWanderDistance, NavMesh.AllAreas);

            i++;

            if (i == 30)
                break;
        }

        return hit.position;
    }

    Vector3 GetFleeLocation()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(model.transform.position + (Random.onUnitSphere * NPCData.safeDistance), out hit, NPCData.safeDistance, NavMesh.AllAreas);

        int i = 0;

        while (GetDestinationAngle(hit.position) > 90 || NPCData.playerDistance < NPCData.safeDistance)
        {
            NavMesh.SamplePosition(model.transform.position + (Random.onUnitSphere * NPCData.safeDistance), out hit, NPCData.safeDistance, NavMesh.AllAreas);

            i++;

            if (i == 30)
                break;
        }

        return hit.position;
    }

    float GetDestinationAngle(Vector3 targetPos)
    {
        return Vector3.Angle(model.transform.position - player.transform.position, model.transform.position + targetPos);
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = agent.velocity;

        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        float speed = localVelocity.z;

        anim.SetFloat("ForwardSpeed", speed);
    }

    private void NPCDead()
    {
        _Dead = true;
        Destroy(gameObject, 3f);
    }

    public void TakeDamage(float damage)
    {
        _CurHealth -= damage;
        Destroy(Instantiate(_HitParticle, _HitParticleSpawnPoint.position, transform.rotation), 1.0f);

        if (_CurHealth <= 0)
        {
            PlayerController.instance.OnEnemyDeath();
            anim.SetBool("Dead", true);
            NPCDead();
        }
    }
}

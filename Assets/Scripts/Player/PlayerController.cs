using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public LayerMask groundLayer;

    public NavMeshAgent playerAgent;
    public Camera playerCam;
    public Animator playerAnimator;

    public float attackOffset;

    private bool _OnMoveInputPressed;
    private bool _Attacking;
    [SerializeField] private float _AttackTime;
    [SerializeField] private float _AttackDamage;
    private Transform _Enemy;
    private float _CurAttackCoolTime;
    private bool _AttackCooled;

    public static PlayerController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void onMoveInput(InputAction.CallbackContext context)
    {
        onMoveInputHold(context);

        if (context.phase == InputActionPhase.Performed)
        {
            _Attacking = false;
            playerAnimator.SetBool("Attacking", false);
            Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, groundLayer))
            {
                Vector3 target = hit.point;

                playerAgent.SetDestination(target);
            }
        }
    }

    private void onMoveInputHold(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _OnMoveInputPressed = true;
        }
        else if(context.canceled)
        {
            _OnMoveInputPressed = false;
        }
    }

    private void Locomotion()
    {
        if(_OnMoveInputPressed)
        {
            Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, groundLayer))
            {
                Vector3 target = hit.point;

                playerAgent.stoppingDistance = 0f;
                playerAgent.SetDestination(target);
            }
        }
    }

    public void onAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if(hit.collider.CompareTag("Enemy"))
                {
                    _Enemy = hit.collider.gameObject.transform;
                    Vector3 target = _Enemy.transform.position;

                    playerAgent.stoppingDistance = attackOffset;
                    playerAgent.SetDestination(target);
                    _Attacking = true;
                }
            }
        }
    }

    private void Combat()
    {
        if(_Attacking && _Enemy != null)
        {
            Vector3 target = _Enemy.transform.position;

            playerAgent.stoppingDistance = attackOffset;
            playerAgent.SetDestination(target);

            if (Vector3.Distance(transform.position, target) < attackOffset)
            {
                if(_CurAttackCoolTime > 0)
                {
                    playerAnimator.SetBool("AttackCooled", false);
                    _AttackCooled = false;
                }
                else
                {
                    playerAnimator.SetBool("AttackCooled", true);
                    _AttackCooled = true;
                    _CurAttackCoolTime = _AttackTime;
                }

                playerAnimator.SetBool("Attacking", true);
                
            }
            else
            {
                playerAnimator.SetBool("Attacking", false);
            }
        }
    }

    public void OnAttackHit()
    {
        if(_Enemy != null)
        {
            _Enemy.gameObject.GetComponent<NPCController>().TakeDamage(_AttackDamage);
            Debug.Log(_Enemy.name);
        }
    }

    public void OnEnemyDeath()
    {
        _Attacking = false;
        playerAnimator.SetBool("Attacking", false);
        _Enemy = null;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
        Locomotion();
        Combat();

        if(!_AttackCooled)
        {
            _CurAttackCoolTime -= Time.deltaTime;
        }
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = playerAgent.velocity;

        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        float speed = localVelocity.z;

        playerAnimator.SetFloat("ForwardSpeed", speed);
    }
}

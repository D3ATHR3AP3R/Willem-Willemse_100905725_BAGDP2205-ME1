using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public LayerMask groundLayer;

    public NavMeshAgent playerAgent;
    public Camera playerCam;
    public Animator playerAnimator;

    public CinemachineBrain _PlayerBrain;

    public float attackOffset;

    private bool _OnMoveInputPressed;
    private bool _Attacking;
    [SerializeField] private float _AttackTime;
    [SerializeField] private float _AttackDamage;
    private Transform _Enemy;
    private Transform _InteractionObj;
    private float _CurAttackCoolTime;
    private bool _AttackCooled;
    private bool _InteractingEnem;
    private bool _ObjInteracting;

    private Vector3 _CamPos;
    private Quaternion _CamRot;

    public static PlayerController instance;
    [SerializeField] private float _MaxHealth = 100;
    private float _CurHealth;

    [SerializeField] private GameObject _HitParticle;
    [SerializeField] private GameObject _HitParticleSpawnPoint;

    [SerializeField] AudioSource _AudioSource;

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

    private void Start()
    {
        _CamPos = new Vector3(playerCam.transform.localPosition.x, playerCam.transform.localPosition.y, playerCam.gameObject.transform.localPosition.z);
        _CamRot = new Quaternion(playerCam.transform.localRotation.x, playerCam.transform.localRotation.y, playerCam.transform.localRotation.z, playerCam.transform.localRotation.w);
        
        _PlayerBrain.enabled = false;
        _CurHealth = _MaxHealth;
    }

    public void onMoveInput(InputAction.CallbackContext context)
    {
        onMoveInputHold(context);

        if (context.phase == InputActionPhase.Performed)
        {
            _Attacking = false;
            _InteractingEnem = false;
            _Enemy = null;
            _ObjInteracting = false;
            _InteractionObj = null;

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
                if (hit.collider.CompareTag("Enemy"))
                {
                    _Enemy = hit.collider.gameObject.transform;
                    Vector3 target = _Enemy.transform.position;

                    playerAgent.stoppingDistance = attackOffset;
                    playerAgent.SetDestination(target);
                    _Attacking = true;
                }
                else if (hit.collider.CompareTag("NPC"))
                {
                    _Enemy = hit.collider.gameObject.transform;
                    Vector3 target = _Enemy.transform.position;

                    playerAgent.stoppingDistance = attackOffset;
                    playerAgent.SetDestination(target);
                    _InteractingEnem = true;
                }
                else if(hit.collider.CompareTag("InteractionObj"))
                {
                    _InteractionObj = hit.collider.gameObject.transform;
                    Vector3 target = _InteractionObj.transform.position;

                    playerAgent.stoppingDistance = attackOffset;
                    playerAgent.SetDestination(target);
                    _ObjInteracting = true;
                }
            }
        }
    }

    private void Interacting()
    {
        if(_Enemy != null)
        {

            if (_InteractingEnem)
            {
                Vector3 target = _Enemy.transform.position;

                if (_Enemy.GetComponent<NPCController>().NPCData.interactDistance > Vector3.Distance(transform.position, target))
                {
                    _Enemy.GetComponent<NPCController>().anim.SetBool("Interacted", true);
                }
            }
            else
            {
                _Enemy.GetComponent<NPCController>().anim.SetBool("Interacted", false);
            }
        }
    }

    private void ObjectInteraction()
    {
        if(_InteractionObj != null)
        {
            if(_ObjInteracting)
            {
                if(Vector3.Distance(transform.position, _InteractionObj.transform.position) <= 0.75f)
                {
                    _InteractionObj.GetComponent<InteractionBase>().Interact();
                    _InteractionObj = null;
                    _ObjInteracting = false;
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
            _AudioSource.Play();
        }
    }

    public void OnEnemyDeath()
    {
        _Attacking = false;
        playerAnimator.SetBool("Attacking", false);

        Debug.Log("Enemy Killed");


        _Enemy = null;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
        Locomotion();
        Combat();
        Interacting();
        ObjectInteraction();

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

    public void CamResetTrigger()
    {
        StartCoroutine(CameraPosReset());
    }

    IEnumerator CameraPosReset()
    {
        Debug.Log("Camera Rest Called");
        yield return new WaitForSeconds(1f);
        Debug.Log("Camera Reset Triggered");
        playerCam.gameObject.transform.localPosition = _CamPos;
        playerCam.gameObject.transform.localRotation = _CamRot;
    }

    public void TakeDamage(float damage)
    {
        _CurHealth -= damage;
        Destroy(Instantiate(_HitParticle, _HitParticleSpawnPoint.transform.position, transform.rotation), 1.0f);

        if (_CurHealth <= 0)
        {
            playerAnimator.SetBool("Dead", true);
            ReloadScene();
        }
    }

    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }
}

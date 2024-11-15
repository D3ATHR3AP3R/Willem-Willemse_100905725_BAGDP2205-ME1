using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class NpcNavMeshController : MonoBehaviour
{
    BossFightCollider bossFightController;
    private NavMeshAgent agent;
    [SerializeField] Collider bossFightCollider;

    [SerializeField] Animator animator;
    private const string BossFightTrigger0 = "NinjaBossFight_Crouch_Idle0";
    [SerializeField] bool rotateAgent;
    [SerializeField] float lerpSpeed = 5.0f;
    [SerializeField] int startLerpDistance = 10;
    private Transform currentDestination;

    private bool startBossFight;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if (rotateAgent && agent.remainingDistance <= startLerpDistance)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                currentDestination.rotation,
                lerpSpeed * Time.deltaTime);

            if (!agent.hasPath)
            {
                rotateAgent = false;
                if (animator != null && startBossFight)
                {
                    startBossFight = false;
                    agent.enabled = false;
                    animator.SetTrigger(BossFightTrigger0);
                }
            }
        }
    }
    public void SetDestination(Transform destination)
    {
        currentDestination = destination;
        agent.SetDestination(currentDestination.position);
    }
    public void SetDestinationAndRotation(Transform destination)
    {
        currentDestination = destination;
        agent.SetDestination(currentDestination.position);
        rotateAgent = true;
    }

    public void StartCutScene()
    {
        bossFightController?.StartCutScene();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other == bossFightCollider)
        {
            startBossFight = true;
            bossFightController = other.GetComponent<BossFightCollider>();
            bossFightController?.StartBossFight();
            other.enabled = false;
        }
    }
}

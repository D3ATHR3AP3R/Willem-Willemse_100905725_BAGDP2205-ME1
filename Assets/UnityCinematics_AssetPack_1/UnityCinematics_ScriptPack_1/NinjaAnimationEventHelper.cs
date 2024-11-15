using UnityEngine;
using UnityEngine.Events;

public class NinjaAnimationEventHelper : MonoBehaviour
{
    public UnityEvent OnDestinationReached;
    public UnityEvent OnEnableRightHandFlame;
    public void DestinationReached()
    {
        OnDestinationReached?.Invoke();
    }
    public void EnableRightHandFlame()
    {
        OnEnableRightHandFlame?.Invoke();
    }
}

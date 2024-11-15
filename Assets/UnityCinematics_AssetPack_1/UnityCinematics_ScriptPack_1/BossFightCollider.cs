using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[RequireComponent(typeof(Collider))]
public class BossFightCollider : MonoBehaviour
{
    [SerializeField] private PlayableDirector bossFightCutscene;
    public UnityEvent StartingBossFight;
    public UnityEvent StartingCutScene;
    
    public UnityEvent EndingCutScene;
    void Awake()
    {
        if(bossFightCutscene != null)
        {
            bossFightCutscene.stopped+= EndCutScene;
        }
    }

    private void EndCutScene(PlayableDirector director)
    {
        EndingCutScene?.Invoke();
    }

    public void StartBossFight()
    {
        StartingBossFight?.Invoke(); 
    }
    public void StartCutScene()
    {
        StartingCutScene?.Invoke();
    }
}

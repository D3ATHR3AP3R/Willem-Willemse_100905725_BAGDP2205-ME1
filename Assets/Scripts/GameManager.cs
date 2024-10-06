using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject[] pickUps;
    public Transform[] pickUpSpawns;

    private bool _PickUpSpawned;

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
        _PickUpSpawned = false;

        Application.targetFrameRate = 30;
        Screen.SetResolution(1920, 1080, true);

        int i = 0;
        foreach(GameObject gameObject in pickUps)
        {
            gameObject.GetComponent<PickupItem>().index = i;
            i++;
        }
    }

    private void Update()
    {
        if(!_PickUpSpawned)
        {
            int i = Random.Range(0, pickUps.Length-1);
            int k = Random.Range(0, pickUpSpawns.Length-1);
            pickUps[i].transform.position = pickUpSpawns[k].position;
            pickUps[i].SetActive(true);
            _PickUpSpawned = true;
        }
    }

    public void ItemCollected(int index)
    {
        pickUps[index].SetActive(false);
        _PickUpSpawned = false;
    }
}

public enum NPCType
{
    Passive,
    Frightful,
    Aggressive
}

public enum NPCState
{
    Idle,
    Wandering,
    Fleeing
}

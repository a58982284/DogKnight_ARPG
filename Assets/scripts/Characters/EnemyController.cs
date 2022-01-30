using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyStates { GUARD,PATROL,CHASE,DEAD};
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public EnemyStates enemyStates;
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SwitchStates();
    }

    void SwitchStates()
    {
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                break;
            case EnemyStates.PATROL:
                break;
            case EnemyStates.CHASE:
                break;
            case EnemyStates.DEAD:
                break;
            default:
                break;
        }
    }
}

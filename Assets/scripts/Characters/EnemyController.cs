using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyStates { GUARD,PATROL,CHASE,DEAD};
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private EnemyStates enemyStates;
    private NavMeshAgent agent;
    private Animator anim;

    [Header("Basic Settings")]
    public float sightRadius;   //怪物的可视范围
    public bool isGuard;
    private float speed;

    private GameObject attackTarget;
    public float lookAtTime;
    private float remainLookAtTime;

    [Header("Patrol State")]
    public float patrolRange;
    private Vector3 wayPoint;
    private Vector3 guardPos;
    //bool配合动画
    bool isWalk;
    bool isChase;
    bool isFollow;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        speed = agent.speed;
        guardPos = transform.position;
        remainLookAtTime = lookAtTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        SwitchStates();
        SwitchAnimation();
    }

    void SwitchAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
    }

    void SwitchStates()
    {
        //如果发现player 切换到CHASE追击
        if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
        }
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                break;
            case EnemyStates.PATROL:
                isChase = false;
                agent.speed = speed * 0.5f;
                //判断是否到了随机巡逻点
                if (Vector3.Distance(wayPoint,transform.position)<=agent.stoppingDistance)
                {
                    isWalk = false;
                    if (remainLookAtTime>0)
                    {
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else
                    {
                        GetNewWayPoint();
                    }
                    
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }
               
                break;
            case EnemyStates.CHASE:
                isWalk = false;
                isChase = true;
                agent.speed = speed;

                if (!FoundPlayer())
                {
                    isFollow = false;
                    if (remainLookAtTime>0)
                    {
                        agent.destination = transform.position;
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else if (isGuard)
                    {
                        enemyStates = EnemyStates.GUARD;
                    }
                    else
                    {
                        enemyStates = EnemyStates.PATROL;
                    }

                }
                else
                {
                    isFollow = true;
                    agent.destination = attackTarget.transform.position;    //追击
                }
                break;
            case EnemyStates.DEAD:
                break;
            default:
                break;
        }
    }
    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius); //敌人范围内查找所有的碰撞体
        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }

    void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);
        // wayPoint = randomPoint;
        NavMeshHit hit;
        wayPoint=NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
}

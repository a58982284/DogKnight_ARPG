using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private GameObject attackTarget;
    private float lastAttackTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        mouseManager.Instance.onMouseClicked += MoveToTarget;
        mouseManager.Instance.onEnemyClicked += EventAttack;
    }

    private void EventAttack(GameObject target)
    {
        if (target != null)
        {
            attackTarget = target;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    // Update is called once per frame
    void Update()
    {
        switchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    private void switchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }   

    public void MoveToTarget(Vector3 target)
    {
        agent.isStopped = false;
        agent.destination = target;
    }

    IEnumerator MoveToAttackTarget()
    {
        StopAllCoroutines();
        agent.isStopped = false;
        transform.LookAt(attackTarget.transform);
        //Todo:修改攻击范围参数
        while (Vector3.Distance(attackTarget.transform.position,transform.position)>1)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        agent.isStopped = true;
        //attack
        if (lastAttackTime<0)
        {
            anim.SetTrigger("Attack");
            lastAttackTime = 0.5f;
        }
    }

}

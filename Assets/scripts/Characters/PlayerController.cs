using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private CharacterStats characterStats;
    private GameObject attackTarget;
    private float lastAttackTime;
    private bool isDead;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
    }
    // Start is called before the first frame update
    void Start()
    {
        mouseManager.Instance.onMouseClicked += MoveToTarget;
        mouseManager.Instance.onEnemyClicked += EventAttack;
        //characterStats.MaxHealth = 2;
    }

    private void EventAttack(GameObject target)
    {
        if (target != null)
        {
            attackTarget = target;
            characterStats.isCritical =UnityEngine.Random.value < characterStats.attackData.criticalChance;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    // Update is called once per frame
    void Update()
    {
        isDead = characterStats.CurrentHealth == 0;
        switchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    private void switchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        anim.SetBool("Death", isDead);
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
        while (Vector3.Distance(attackTarget.transform.position,transform.position)>characterStats.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        agent.isStopped = true;
        //attack
        if (lastAttackTime<0)
        {
            anim.SetBool("Critical", characterStats.isCritical);
            anim.SetTrigger("Attack");
            //重置冷却时间
            lastAttackTime = characterStats.attackData.coolDown;
        }
    }

    //Animation Event
    void Hit()
    {
        var targetStats = attackTarget.GetComponent<CharacterStats>();
        targetStats.TakeDamage(characterStats, targetStats);
    }
}

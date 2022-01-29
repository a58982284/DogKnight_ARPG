using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        mouseManager.Instance.onMouseClicked += MoveToTarget;
    }

    // Update is called once per frame
    void Update()
    {
        switchAnimation();
    }

    private void switchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }   

    public void MoveToTarget(Vector3 target)
    {
        agent.destination = target;
    }
}

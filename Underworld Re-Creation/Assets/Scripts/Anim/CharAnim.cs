using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharAnim : MonoBehaviour
{
    public Animator animator;

    public NavMeshAgent navmeshAgent;
    public bool nextRunOn = false;


    public virtual void Start()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
       animator = GetComponent<Animator>();
    }

    public virtual void Update()
    {

      //  animator.SetFloat("Speed Percent", navmeshAgent.velocity.magnitude / navmeshAgent.speed, .1f, Time.deltaTime);
        //if (nextRunOn)
        //{
        //    animator.enabled = false;
        //    animator.SetTrigger("NextRun");
        //    animator.enabled = true;
        //    nextRunOn = false;
        //}

    }

    public virtual void momentOfAttack()
    {
      
    }

    public virtual void momentOfAttacks()
    {

    }
}

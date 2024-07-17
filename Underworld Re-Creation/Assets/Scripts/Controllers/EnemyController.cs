using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/* Makes enemies follow and attack the player */

public class EnemyController : MonoBehaviour {

	public float lookRadius = 10f;

	Transform target;
	NavMeshAgent agent;
	

	Enemy skeleton;
	EnemyAnim enemyAnim;
    void Start()
	{
		target = playerController.instance.transform;
		agent = GetComponent<NavMeshAgent>();
		
        skeleton = GetComponent<Enemy>();
        enemyAnim = GetComponent<EnemyAnim>();
    }

	void Update ()
	{
		// Get the distance to the player
		float distance = Vector3.Distance(target.position, transform.position);
		if (skeleton.StanEnemy )
		{
			


		}
		else
		{
			// If inside the radius
			if (distance <= lookRadius)
			{
				
				
                // Move towards the player
                agent.SetDestination(target.position);
                enemyAnim.animator.SetFloat("IdleOrWalk", 1);//запускаем анимацию бега
                if (distance <= agent.stoppingDistance)
				{
                    enemyAnim.animator.SetFloat("IdleOrWalk", 0);
                    // Attack
                    enemyAnim.AttackDefault();

                    FaceTarget();



				}

			}
		}
	}
  


    // Point towards the player
    void FaceTarget ()
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
	}

   

}

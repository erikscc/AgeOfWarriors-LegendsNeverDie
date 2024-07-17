using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public abstract class MyUnit : MonoBehaviour
{
	public string unitName;
	public int health;
	public float attackRange;
	public int attackDamage;
	public float attackCooldown = 1.0f; // Set this value in the inspector

	protected NavMeshAgent navMeshAgent;
	protected MyUnit currentTarget;
	private StateController stateController;
	private bool canAttack = true;

	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		stateController = GetComponent<StateController>();

		if (navMeshAgent == null)
		{
			Debug.LogError("NavMeshAgent component missing from " + unitName);
		}
		else
		{
			navMeshAgent.stoppingDistance = attackRange; // Set the stopping distance to the attack range
		}

		if (stateController == null)
		{
			Debug.LogError("StateController component missing from " + unitName);
		}
	}

	public virtual void SelectTarget(MyUnit target)
	{
		Debug.Log($"{unitName} selects {target.unitName} as target.");
		currentTarget = target;
		StartCoroutine(UpdateTargetPosition());
	}

	private IEnumerator UpdateTargetPosition()
	{
		while (currentTarget != null && currentTarget.health > 0)
		{
			float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

			if (distanceToTarget <= attackRange)
			{
				if (canAttack)
				{
					Attack(currentTarget);
				}
			}
			else
			{
				navMeshAgent.SetDestination(currentTarget.transform.position);
				stateController.ChangeAnimationState(State.Walk);
			}

			yield return new WaitForSeconds(0.1f); // Adjust this value as needed for performance
		}
		stateController.ChangeAnimationState(State.Idle);
	}

	public virtual void Attack(MyUnit target)
	{
		if (target == null || !canAttack) return;

		float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
		if (distanceToTarget <= attackRange)
		{
			Debug.Log($"{unitName} attacks {target.unitName} with attack for {attackDamage} damage.");
			stateController.ChangeAnimationState(State.Attack);
			target.TakeDamage(attackDamage);
			StartCoroutine(AttackCooldown());
		}
	}

	private IEnumerator AttackCooldown()
	{
		canAttack = false;
		yield return new WaitForSeconds(attackCooldown);
		canAttack = true;

		// After the attack cooldown, set state to idle if not attacking
		stateController.ChangeAnimationState(State.Idle);
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
		Debug.Log($"{unitName} takes {damage} damage. Remaining health: {health}");
		if (health <= 0)
		{
			Die();
		}
	}

	public virtual void Die()
	{
		Debug.Log($"{unitName} has died.");
		StopAllCoroutines(); // Stop updating target position
		Destroy(gameObject);
	}

	public void MoveTo(Vector3 position)
	{
		if (navMeshAgent != null)
		{
			navMeshAgent.SetDestination(position);
			stateController.ChangeAnimationState(State.Walk);
			Debug.Log($"{unitName} moves to {position}.");
		}
	}

	public void Patrol(Vector3[] waypoints)
	{
		// Implement patrol logic using NavMesh
		Debug.Log($"{unitName} starts patrolling.");
		stateController.ChangeAnimationState(State.Walk);
	}
}

using UnityEngine;
using System.Collections;

/// <summary>
/// Creates wandering behaviour for a top-down 2D Rigidbody.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Wander : MonoBehaviour
{
	public float speed = 5;
	public float directionChangeInterval = 1;
	public float maxHeadingChange = 30;

	Rigidbody2D rb;
	float heading;
	float targetHeading;

	void Awake ()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.gravityScale = 0f;

		// Set random initial rotation
		heading = Random.Range(0f, 360f);
		targetHeading = heading;
		transform.rotation = Quaternion.Euler(0f, 0f, heading);

		StartCoroutine(NewHeading());
	}

	void FixedUpdate ()
	{
		heading = Mathf.MoveTowardsAngle(heading, targetHeading, directionChangeInterval * Time.fixedDeltaTime * maxHeadingChange);
		transform.rotation = Quaternion.Euler(0f, 0f, heading);

		float rad = heading * Mathf.Deg2Rad;
		Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
		rb.linearVelocity = direction * speed;
	}

	/// <summary>
	/// Repeatedly calculates a new direction to move towards.
	/// Use this instead of MonoBehaviour.InvokeRepeating so that the interval can be changed at runtime.
	/// </summary>
	IEnumerator NewHeading ()
	{
		while (true) {
			NewHeadingRoutine();
			yield return new WaitForSeconds(directionChangeInterval);
		}
	}

	/// <summary>
	/// Calculates a new direction to move towards.
	/// </summary>
	void NewHeadingRoutine ()
	{
		var floor = heading - maxHeadingChange;
		var ceil  = heading + maxHeadingChange;
		targetHeading = Random.Range(floor, ceil);
	}
}

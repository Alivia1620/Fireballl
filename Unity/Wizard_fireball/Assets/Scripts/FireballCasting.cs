using UnityEngine;

public class FireballCasting : MonoBehaviour
{
	[Header("Cast Settings")]
	public Transform handTransform;
	public Transform headTransform;
	public Transform castOrigin;
	public GameObject fireballPrefab;
	public float fireballSpeed = 15f;
	public float requiredForwardDot = 0.7f;
	public float requiredFlatDot = 0.7f;
	public float poseHoldTime = 0.5f;

	[Header("Runtime")]
	public bool canCast;
	public bool hasCast;
	private float poseTimer;

    void Start()
    {
        handTransform = GameObject.FindGameObjectsWithTag("CastOrigin")[0].transform; // Assuming the first hand found is the correct one
        headTransform = GameObject.FindGameObjectsWithTag("Head")[0].transform; // Assuming the first head found is the correct one
		castOrigin = handTransform; // Set cast origin to hand by default
    }
    void Update()
	{
		if (!canCast || hasCast || handTransform == null || headTransform == null || castOrigin == null || fireballPrefab == null)
			return;

		if (IsHandFlatAndForward())
		{
			poseTimer += Time.deltaTime;
			if (poseTimer >= poseHoldTime)
			{
				CastFireball();
			}
		}
		else
		{
			poseTimer = 0f;
		}
	}

	private bool IsHandFlatAndForward()
	{
		Vector3 headForward = headTransform.forward;
		Vector3 handForward = handTransform.forward;
		Vector3 handUp = handTransform.up;

		float forwardDot = Vector3.Dot(handForward, headForward);
		float flatDot = Mathf.Abs(Vector3.Dot(handUp, Vector3.up));

		return forwardDot >= requiredForwardDot && flatDot >= requiredFlatDot;
	}

	private void CastFireball()
	{
		if (fireballPrefab == null || castOrigin == null)
			return;

		hasCast = true;
		canCast = false;
		poseTimer = 0f;

		GameObject fireball = Instantiate(fireballPrefab, castOrigin.position, castOrigin.rotation);
		Rigidbody rb = fireball.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.linearVelocity = castOrigin.forward * fireballSpeed;
		}
	}

	public void EnableCasting()
	{
		canCast = true;
		hasCast = false;
		poseTimer = 0f;
	}
}

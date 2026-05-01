using UnityEngine;

public class FireballCasting : MonoBehaviour
{
    [Header("References")]
    public Transform handTransform;
    public Transform headTransform;
    public Transform castOrigin;
    public GameObject fireballPrefab;

    [Header("Casting")]
    public float fireballSpeed = 15f;
    public float velocityMultiplier = 2f;
    public float maxVelocityBoost = 3f;

    [Header("Gesture Detection")]
    public float requiredForwardDot = 0.7f;
    public float requiredFlatDot = 0.7f;
    public float velocityThreshold = 1.5f; // how hard you must thrust forward

    [Header("Cooldown")]
    public float castCooldown = 0.5f;

    private float cooldownTimer;

    private Vector3 lastHandPos;
    private Vector3 handVelocity;

    void Start()
    {
        TryFindTransforms();

        if (handTransform != null)
            lastHandPos = handTransform.position;
    }

    void Update()
    {
        if (handTransform == null || headTransform == null)
            TryFindTransforms();

        if (handTransform == null || headTransform == null || fireballPrefab == null)
            return;

        // Update cooldown
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        // Track hand velocity
        handVelocity = (handTransform.position - lastHandPos) / Time.deltaTime;
        lastHandPos = handTransform.position;

        // Check if gesture is valid
        if (cooldownTimer <= 0f && IsHandFlatAndForward() && IsThrustingForward())
        {
            CastFireball();
        }
    }

    private void TryFindTransforms()
    {
        if (handTransform == null)
        {
            var castOrigins = GameObject.FindGameObjectsWithTag("CastOrigin");
            if (castOrigins.Length > 0)
                handTransform = castOrigins[0].transform;
        }

        if (headTransform == null)
        {
            var heads = GameObject.FindGameObjectsWithTag("Head");
            if (heads.Length > 0)
                headTransform = heads[0].transform;
        }

        if (castOrigin == null && handTransform != null)
            castOrigin = handTransform;
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

    private bool IsThrustingForward()
    {
        // Check if hand velocity is going forward relative to head
        float forwardVelocity = Vector3.Dot(handVelocity.normalized, headTransform.forward);

        return handVelocity.magnitude > velocityThreshold && forwardVelocity > 0.7f;
    }

    private void CastFireball()
    {
        if (castOrigin == null) return;

        cooldownTimer = castCooldown;

        GameObject fireball = Instantiate(fireballPrefab, castOrigin.position, castOrigin.rotation);

        // Ensure Rigidbody exists
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = fireball.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }

        // Scale power by hand speed
        float speedBoost = Mathf.Clamp(handVelocity.magnitude * velocityMultiplier, 0f, maxVelocityBoost);

        rb.linearVelocity = castOrigin.forward * (fireballSpeed + speedBoost);

        Destroy(fireball, 5f);
    }
}
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class DrinkinScript : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public float drinkDistance = 0.3f; // Distance to camera to trigger drinking
    public Animator animator;
    public string drinkAnimationBool = "drink";
    public Transform headTransform;
    public FireballCasting fireballCasting;
    private bool hasDrunk;
    public float drinkAnimationDuration = 2f; // Duration of the drinking animation

    void Start()
    {
        animator = GetComponent<Animator>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (headTransform == null)
        {
            if (Camera.main != null)
            {
                headTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning("DrinkinScript: No headTransform assigned and no MainCamera found. Assign the XR head transform in the Inspector.");
            }
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (hasDrunk || grabInteractable == null)
            return;

        if (grabInteractable.isSelected && collision.CompareTag("Head"))
        {
            DrinkPotion();
        }
    }

    private void DrinkPotion()
    {
        // Trigger drinking motion/effect
        Debug.Log("Drinking potion!");
        hasDrunk = true;
        // Play drinking animation
        if (animator != null)
        {
            animator.SetBool(drinkAnimationBool, true);
        }
        if (fireballCasting != null)
        {
            fireballCasting.EnableCasting();
        }
        // Add drinking sound or particle effect here if needed
        // Start coroutine to destroy after animation
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(drinkAnimationDuration);
        if (animator != null)
        {
            animator.SetBool(drinkAnimationBool, false);
        }
        Destroy(gameObject);
    }
}

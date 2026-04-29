using UnityEngine;

public class CauldronColorChange : MonoBehaviour
{
    public Animator animator;

    private void OnTriggerEnter(Collider collision)
    {
        // Check if the object that hit the trigger has the "ingredient" tag
        if (collision.CompareTag("ingredient"))
        {
            // Play the animation
            if (animator != null)
            {
                animator.SetTrigger("IngredientAdded");
            }
        }
    }
}


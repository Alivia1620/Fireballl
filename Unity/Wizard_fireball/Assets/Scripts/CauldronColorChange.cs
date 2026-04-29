using UnityEngine;

public class CauldronColorChange : MonoBehaviour
{
    public Animator animator;
    public GameObject ingredient;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Check if the object that hit the trigger has the "ingredient" tag
        if (collision.CompareTag("ingredient"))
        {
            ingredient = collision.gameObject;
            // Play the animation
            if (animator != null)
            {
                animator.SetBool("IngredientAdded", true);
            }
            Destroy(ingredient);
        }
    }
}


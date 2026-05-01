using UnityEngine;
using System.Collections;

public class CauldronColorChange : MonoBehaviour
{
    public Animator animator;
    public Animator spoonAnimator;
    public GameObject ingredient;
    public float animationDuration = 2f;
    public Material[] materials; // Array of materials to change the cauldron color
    public GameObject vial;
    public float vialSpawnHeight = 2f;
    public GameObject VialSpawnPoint;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Check if the object that hit the trigger has the "Ingredient" tag
        if (collision.CompareTag("Ingredient"))
        {
            ingredient = collision.gameObject;
            // Play the animation on both wizard juice and spoon
            if (animator != null)
            {
                animator.SetBool("IngredientAdded 0", true);
            }
            if (spoonAnimator != null)
            {
                spoonAnimator.SetBool("IngredientAdded 0", true);
            }
            Destroy(ingredient);
            // Stop animation after duration
            StartCoroutine(StopAnimationAfterDuration());
        }
    }

    private IEnumerator StopAnimationAfterDuration()
    {
        yield return new WaitForSeconds(animationDuration);
        
        if (animator != null)
        {
            animator.SetBool("IngredientAdded 0", false);
        }
        if (spoonAnimator != null)
        {
            spoonAnimator.SetBool("IngredientAdded 0", false);
        }
        // Change the material of the cauldron to a random one from the array
        if (materials.Length > 0)
        {
            int randomIndex = Random.Range(0, materials.Length);
            GetComponent<Renderer>().material = materials[randomIndex];
            Instantiate(vial, VialSpawnPoint.transform.position, Quaternion.identity); // Instantiate the vial above the cauldron
        }
    }
}


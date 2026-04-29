using UnityEngine;

public class SpoonAnimationStart : MonoBehaviour
{
    public Animator animator;

    public CauldronColorChange cauldronColorChange;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    //void Update()
    //{
        //if (cauldronColorChange.spoon == true)
        //{
            //AnimationStart();
        //}
    //}

    void AnimationStart() //spoon animation coincides with the wizard juice animation, so it starts when the ingredient is added to the cauldron
    {
        if (animator != null)
        {
            animator.SetBool("IngredientAdded 0", true);
        }
    }

    //void AnimationStop() //spoon animation stops when the wizard juice animation stops, so it stops after a certain time
    //{
        //if (animator != null)
        //{
            //animator.SetBool("IngredientAdded 0", false);
            //cauldronColorChange.StopAnimation();
        //}
    //}
}

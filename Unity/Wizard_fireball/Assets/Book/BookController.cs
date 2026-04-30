using Oculus.Interaction;
using System.Collections;
using UnityEngine;

public class BookController : MonoBehaviour
{
    public Animator animator;

    private int closeBookAnim = 0;
    private int openBookAnim = 1;

    public bool isBookOpen;

    public GameObject UI;

    public bool useLocalTransform = true;
    public float closeMoveDuration = 0.5f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private Coroutine closeCoroutine;

    // Meta SDK component
    public Grabbable grabbable;

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (grabbable == null)
            grabbable = GetComponent<Grabbable>();

        // Store original transform
        if (useLocalTransform)
        {
            originalPosition = transform.localPosition;
            originalRotation = transform.localRotation;
        }
        else
        {
            originalPosition = transform.position;
            originalRotation = transform.rotation;
        }
    }

    void OnEnable()
    {
        if (grabbable != null)
        {
            grabbable.WhenPointerEventRaised += HandlePointerEvent;
        }
    }

    void OnDisable()
    {
        if (grabbable != null)
        {
            grabbable.WhenPointerEventRaised -= HandlePointerEvent;
        }
    }

    private void HandlePointerEvent(PointerEvent evt)
    {
        // Grab start
        if (evt.Type == PointerEventType.Select)
        {
            OnGrab();
        }
        // Grab end
        else if (evt.Type == PointerEventType.Unselect)
        {
            OnRelease();
        }
    }

    private void OnGrab()
    {
        // Stop return animation if grabbing again
        if (closeCoroutine != null)
        {
            StopCoroutine(closeCoroutine);
            closeCoroutine = null;
        }

        isBookOpen = true;

        // Play open animation immediately
        animator.SetInteger("Anim", openBookAnim);
        //UI.SetActive(true);
    }

    private void OnRelease()
    {
        isBookOpen = false;

        animator.SetInteger("Anim", closeBookAnim);
        //UI.SetActive(false);

        if (closeCoroutine == null)
        {
            closeCoroutine = StartCoroutine(MoveToOriginal(closeMoveDuration));
        }
    }

    private IEnumerator MoveToOriginal(float duration)
    {
        if (duration <= 0f)
        {
            ApplyTransform(originalPosition, originalRotation);
            closeCoroutine = null;
            yield break;
        }

        float elapsed = 0f;

        Vector3 startPos = useLocalTransform ? transform.localPosition : transform.position;
        Quaternion startRot = useLocalTransform ? transform.localRotation : transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            Vector3 pos = Vector3.Lerp(startPos, originalPosition, t);
            Quaternion rot = Quaternion.Slerp(startRot, originalRotation, t);

            ApplyTransform(pos, rot);

            yield return null;
        }

        ApplyTransform(originalPosition, originalRotation);
        closeCoroutine = null;
    }

    private void ApplyTransform(Vector3 pos, Quaternion rot)
    {
        if (useLocalTransform)
        {
            transform.localPosition = pos;
            transform.localRotation = rot;
        }
        else
        {
            transform.position = pos;
            transform.rotation = rot;
        }
    }
}
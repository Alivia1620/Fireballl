using UnityEngine;

public class ToggleScript : MonoBehaviour
{
    public GameObject target;

    public void ToggleActive()
    {
        target.SetActive(!target.activeSelf);
    }
}
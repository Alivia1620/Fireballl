using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject prefab;

    [Header("Spawn Settings")]
    public Transform spawnPoint; // optional
    public bool useSpawnerTransform = true;

    [Header("Parent (optional)")]
    public Transform parent;

    public void Spawn()
    {
        if (prefab == null)
        {
            Debug.LogWarning("No prefab assigned!");
            return;
        }

        Vector3 pos;
        Quaternion rot;

        if (spawnPoint != null)
        {
            pos = spawnPoint.position;
            rot = spawnPoint.rotation;
        }
        else if (useSpawnerTransform)
        {
            pos = transform.position;
            rot = transform.rotation;
        }
        else
        {
            pos = Vector3.zero;
            rot = Quaternion.identity;
        }

        GameObject obj = Instantiate(prefab, pos, rot);

        if (parent != null)
        {
            obj.transform.SetParent(parent);
        }
    }
}
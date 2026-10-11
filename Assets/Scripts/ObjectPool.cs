using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 10;

    private readonly Queue<GameObject> available = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            available.Enqueue(CreateInstance());
        }
    }
    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject instance = available.Count > 0 ? available.Dequeue() : CreateInstance();
        instance.transform.SetPositionAndRotation(position, rotation);
        instance.SetActive(true);
        return instance;
    }
    public void Return(GameObject instance)
    {
        if (!instance.activeSelf)
        {
            return;
        }

        instance.SetActive(false);
        available.Enqueue(instance);
    }

    private GameObject CreateInstance()
    {
        GameObject instance = Instantiate(prefab, transform);
        instance.SetActive(false);
        return instance;
    }
}

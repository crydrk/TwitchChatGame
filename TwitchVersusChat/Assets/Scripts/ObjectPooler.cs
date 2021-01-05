using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolableObject
{
    public string Name;
    public GameObject Prefab;
    public int AmountToPool;
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;

    public List<PoolableObject> PoolableObjects = new List<PoolableObject>();
    public List<List<GameObject>> PooledObjects = new List<List<GameObject>>();

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        int count = 0;
        foreach (PoolableObject po in PoolableObjects)
        {
            PooledObjects.Add(new List<GameObject>());
            for (int i = 0; i < po.AmountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(po.Prefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                PooledObjects[count].Add(obj);
            }
            count += 1;
        }
    }

    public GameObject GetPooledObject(int index)
    {
        for (int i = 0; i < PooledObjects[index].Count; i++)
        {
            if (!PooledObjects[index][i].activeInHierarchy)
            {
                return PooledObjects[index][i];
            }
        }

        return null;
    }
}

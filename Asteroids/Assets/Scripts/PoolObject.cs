using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject<T> where T : MonoBehaviour
{
    public T prefab { get; }
    [SerializeField] private Transform storage;
    private bool autoExpand;

    private List<T> pool;

    public PoolObject(T _prefab, Transform _storage, bool _autoExpand, int count)
    {
        prefab = _prefab;
        storage = _storage;
        autoExpand = _autoExpand;
        CreatePool(count);
    }

    private void CreatePool(int count)
    {
        pool = new List<T>();

        for (int i = 0; i < count; i++)
            CreateObject();
    }

    private T CreateObject(bool isActive = false)
    {
        var createdObject = UnityEngine.Object.Instantiate(prefab, storage);
        createdObject.gameObject.SetActive(isActive);
        pool.Add(createdObject);
        return createdObject;
    }

    private bool HasFreeElement(out T element)
    {
        foreach(T obj in pool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                element = obj;
                obj.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public bool HasActiveElement(T element = null)
    {
        foreach (T obj in pool)
        {
            if (obj.gameObject.activeInHierarchy && obj != element)
                return true;
        }

        return false;
    }

    public T GetFreeElement()
    {
        if (HasFreeElement(out var element))
            return element;
        
        if(autoExpand)
            return CreateObject(true);

        throw new Exception("pool is empty"); 
    }

}

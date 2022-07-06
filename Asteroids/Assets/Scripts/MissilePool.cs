using UnityEngine;

public class MissilePool : MonoBehaviour
{
    [SerializeField] private int poolCount;
    [SerializeField] private bool autoExpand;
    [SerializeField] private Missile prefab;

    private PoolObject<Missile> pool;

    private void Awake()
    {
        pool = new PoolObject<Missile>(prefab, transform, autoExpand, poolCount);
    }

    public Missile GetFromPool()
    {
        return pool.GetFreeElement();
    }
}

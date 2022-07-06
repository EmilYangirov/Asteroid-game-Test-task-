
using UnityEngine;

public interface IHit
{
    public abstract void OnHit(int attackerLayer);

    public abstract void OnDie();
}

using System.Collections;
using UnityEngine;

public abstract class Attacker : SpaceObject
{
    [SerializeField] protected Vector3 missileOffset;
    [SerializeField] protected MissilePool missilePool;
    [SerializeField] protected MissileData missileData;
    [SerializeField] protected float reloadTime;
    [SerializeField] protected AudioClip attackSound, moveSound;

    protected SoundPlayer attackPlayer, movePlayer;
    protected bool isReloading;

    private Coroutine reload;

    protected override void Awake()
    {
        base.Awake();

        attackPlayer = new SoundPlayer(gameObject);
        movePlayer = new SoundPlayer(gameObject);
    }
   
    protected virtual void Attack(Vector2 attackDirection)
    {
        if (isReloading)
            return;

        attackPlayer.PlaySound(attackSound);
        Missile newMissile = missilePool.GetFromPool();
        newMissile.ChangeParameters(missileData, attackDirection);
        newMissile.transform.position = transform.position + transform.rotation * missileOffset;

        if(reload != null)
            StopCoroutine(reload);

        reload = StartCoroutine(Reload(reloadTime));
    }

    protected IEnumerator Reload(float reloadTime)
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }
}

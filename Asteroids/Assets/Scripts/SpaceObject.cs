using UnityEngine;

public abstract class SpaceObject : MonoBehaviour, IHit
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected LayerMask hitLayers;
    [SerializeField] protected AudioClip deadAudio;

    protected SoundPlayer dieSoundPlayer;
    protected Vector2 direction;

    protected virtual void Awake()
    {
        dieSoundPlayer = new SoundPlayer(gameObject);
    }
    protected virtual void FixedUpdate()
    {
        Move();
    }
    protected virtual void Move()
    {
        transform.Translate(direction * Speed() * Time.deltaTime);
    }

    protected virtual float Speed()
    {
        return moveSpeed;
    }
    protected virtual void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    public Vector2 GetDirection()
    {
        return direction;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        int thisLayerMask = 1 << other.gameObject.layer;

        if (CheckLayer(thisLayerMask, hitLayers))
        {
            Debug.Log(transform.name);
            IHit hit = other.gameObject.GetComponent<IHit>();
            hit.OnHit(1 << gameObject.layer);
        }
    }

    protected bool CheckLayer(int layer, LayerMask mask)
    {
        if (mask == (mask | layer))
            return true;
        else
            return false;
    }

    public abstract void OnHit(int attackerLayer);

    public virtual void OnDie()
    {
        dieSoundPlayer.PlaySound(deadAudio);
    }
}

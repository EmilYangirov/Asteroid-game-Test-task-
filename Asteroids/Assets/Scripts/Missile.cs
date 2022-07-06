using System.Collections;
using UnityEngine;

public class Missile : SpaceObject
{
    [SerializeField] private float lifeTime;
    private SpriteRenderer image;    

    protected override void Awake()
    {
        image = GetComponent<SpriteRenderer>();
        StartCoroutine(DisableAfterLifeTime());

        SetDirection(transform.up);
    }
    public void ChangeParameters(MissileData data, Vector2 dir)
    {
        moveSpeed = data.moveSpeed;
        image.sprite = data.image;
        hitLayers = data.hitLayers;
        transform.up = dir;
    }
    private IEnumerator DisableAfterLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        int thisLayerMask = 1 << other.gameObject.layer;

        if (CheckLayer(thisLayerMask, hitLayers))
        {
            IHit hit = other.gameObject.GetComponent<IHit>();
            hit.OnHit(1 << gameObject.layer);
            OnHit(1 << gameObject.layer);
        }  
        
        if(!CheckLayer(1 << 6, hitLayers))
        {
            IScore score = null;
            score = other.GetComponent<IScore>();

            if(score!= null)
                score.SetScore();
        }

    }
    public override void OnHit(int attackerLayer)
    {
        OnDie();
    }

    public override void OnDie()
    {
        gameObject.SetActive(false);
    }
}

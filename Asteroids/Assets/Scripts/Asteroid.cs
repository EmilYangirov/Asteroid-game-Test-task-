using UnityEngine;

public class Asteroid : SpaceObject, IScore
{    
    [SerializeField] private CircleCollider2D collider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int newAsteroidCount = 2;
    
    [HideInInspector] public AsteroidSpawner spawner;
    [HideInInspector] public Type type;

    public int value { get; set; }
    public Score score { get; set; }
        
    public void ChangeParameter(AsteroidData data, Vector2 dir, float speed = 0, Score scores = null)
    {
        spriteRenderer.sprite = data.image;
        moveSpeed = speed;

        if (speed == 0)
            moveSpeed = Random.Range(data.minSpeed, data.maxSpeed);        
            

        collider.radius = data.radius;
        transform.localScale = data.scale;
        type = data.type;

        score = scores;
        value = data.score;

        SetDirection(dir);
    }       

    public override void OnHit(int attackerLayer)
    {
        if (CheckLayer(attackerLayer, hitLayers))
        {
            spawner.OnSmallAsteroidDie(this);
            gameObject.SetActive(false);
            return;
        }

        OnDie();
    }

    public override void OnDie()
    {
        spawner.asteroidSoundPlayer.PlaySound(deadAudio);

        if (spawner != null)
        {
            if (type == Type.small)
                spawner.OnSmallAsteroidDie(this);
            else
                spawner.CreateAsteroidsByCount(this, newAsteroidCount);
        }

        gameObject.SetActive(false);
    }

    public void SetScore()
    {
        if (score == null)
            return;

        score.IncreaseScore(value);
    }
}

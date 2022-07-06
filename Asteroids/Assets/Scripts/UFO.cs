using System.Collections;
using UnityEngine;

public class UFO : Attacker, IEvent, IScore
{
    [SerializeField] private float minReloadTime = 2, maxReloadTime = 5;
    [HideInInspector] public GameEvents gameEvents { get; set; }
    [HideInInspector] public Transform eventTransform { get; set; }
    [HideInInspector] public Score score { get; set; }
    public int value { get; set; }

    private MapBorders mapBorders;
    private Transform player;
    private bool dead;

    protected void Awake()
    {
        base.Awake();

        eventTransform = this.transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        var scoreGameObject = GameObject.FindGameObjectWithTag("Score");
        score = scoreGameObject.GetComponent<Score>();

        value = 200;
    }

    protected override void FixedUpdate()
    {
        if (dead)
            return;

        base.FixedUpdate();

        Attack(player.position - transform.position);
    }
    private void Start()
    {
        mapBorders = Camera.main.transform.GetComponent<MapBorders>();
        gameObject.SetActive(false);
    }

    public void StartEvent()
    {
        transform.position = mapBorders.GetSpawnPosition(out direction);
        isReloading = false;
        SetDirection(direction);
    }

    public void StopEvent()
    {
        gameEvents.OnEventStop();
        dead = false;
        gameObject.SetActive(false);
    }

    public override void OnHit(int attackerLayer)
    {
        OnDie();
    }

    public override void OnDie()
    {
        base.OnDie();
        StartCoroutine(DisableGameObject());
    }

    protected override void Attack(Vector2 attackDirection)
    {
        reloadTime = Random.Range(minReloadTime, maxReloadTime);

        base.Attack(attackDirection);
    }

    public void SetScore()
    {
        if (score == null)
            return;

        score.IncreaseScore(value);
    }

    protected override void Move()
    {
        base.Move();
        movePlayer.PlaySound(moveSound);
    }

    private IEnumerator DisableGameObject()
    {
        dead = true;
        yield return new WaitForSeconds(0.5f);
        StopEvent();
    }
}

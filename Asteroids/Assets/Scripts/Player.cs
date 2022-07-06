using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class Player : Attacker
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float acceleration;
        
    private KeyCode fireButton = KeyCode.Mouse0;
    private bool mouseRotation = true, dead;
    private float speed;     

    private Camera camera;
    private Animator animator;
    private PlayerHealth health;

    protected override void Awake()
    {
        base.Awake();
        camera = Camera.main;
        animator = GetComponent<Animator>();
        health = GetComponent<PlayerHealth>();
        SetDirection(transform.up);
        reloadTime = 1f / 3;
    }

    protected override void FixedUpdate()
    {
        if (Input.GetKeyDown(fireButton))
            Attack(transform.up);
        
        base.FixedUpdate();

        Rotate();         
    }  

    private void Rotate()
    {
        if (!mouseRotation && Input.GetAxis("Horizontal") == 0)
            return;

        var direction = RotationDirection();

        float angle = -Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        var newRot = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, rotateSpeed);
    }
    //Get direction of rotation with using two control variants
    private Vector3 RotationDirection()
    {
        Vector3 direction = new Vector3();

        if (mouseRotation)
        {
            Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            direction = mousePosition - transform.position;
        } else
        {
            var forward = Quaternion.Euler(0, 0, 45 * Mathf.Sign(Input.GetAxis("Horizontal")));
            direction = forward * -transform.up;            
        }

        return direction;
    }
    //Speed taking into account acceleration and deceleration
    protected override float Speed()
    {
        int i = -1;

        if (Input.GetAxis("Vertical") > 0)
        {
            movePlayer.PlaySound(moveSound);
            i = 1;
        }

        speed = Mathf.Clamp(speed + acceleration * Time.deltaTime * i, 0, moveSpeed);
        return speed;
    }
    
    public void ChangeControl()
    {
        int i = PlayerPrefs.GetInt("mouseControl");

        if (i == 0)
            mouseRotation = false;
        else
            mouseRotation = true;

        if (mouseRotation)
            fireButton = KeyCode.Mouse0;
        else
            fireButton = KeyCode.Space;
    }

    public override void OnHit(int attackerLayer)
    {
        if (dead)
            return;

        OnDie();
    }

    public override void OnDie()
    {
        base.OnDie();
        transform.position = Vector2.zero;
        health.OnPlayerDie();
        StartCoroutine(SpawnAfterDie());
    }

    private IEnumerator SpawnAfterDie()
    {
        animator.SetBool("dead", true);
        dead = true;
        yield return new WaitForSeconds(3);
        animator.SetBool("dead", false);
        dead = false;
    }

    private void OnEnable()
    {
        CoreLogic.OnChangeControl += ChangeControl;
    }

    private void OnDisable()
    {
        CoreLogic.OnChangeControl -= ChangeControl;
    }

    private void KillEnemy(Vector3 targetPosition)
    {

    }
}

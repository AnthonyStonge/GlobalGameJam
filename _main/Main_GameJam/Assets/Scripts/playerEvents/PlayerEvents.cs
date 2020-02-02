using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.VFX;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class PlayerEvents : CustomEventBehaviour<PlayerEvents.Event>, IFlow
{
    public enum Event
    {
        DASH,
        DIE,
        TRHOW,
        START_MOVING,
        STOP_MOVING,
        FOOT_STEP_LEFT,
        FOOT_STEP_RIGHT
    }

    [Header("Settings")] public float speed = 100;
    public int currentNumOfChick = 0;
    public float currentRepairPoints = 500;
    public float initialRepairPoints = 500;

    [Header("Internal")] public Rigidbody rb;
    public Transform shotSpawn;
    private GameObject bulletPrefab;

    [SerializeField] private CustomEvent onDash;
    [SerializeField] private CustomEvent onDie;
    [SerializeField] private CustomEvent onThrow;
    [SerializeField] private CustomEvent onStartMoving;
    [SerializeField] private CustomEvent onStopMoving;
    [SerializeField] private VisualEffect footStepLeft;
    [SerializeField] private VisualEffect footStepRight;
    [SerializeField] private float delayToShoot;

    public Chick chick;
    private Animator chickAnimator;
    private Animator animator;
    private Vector2 currentInput;
    private bool isMoving = false;
    private int AssID;
    private bool hasControl = true;
    public bool canShoot = true;
    public bool isDead = false;
    [HideInInspector] public bool gameOver = false;

    [HideInInspector] public bool eggCompleted;
    [HideInInspector] public int numberEgg;
    private List<Vector3> spawnPosition;
    private List<Vector3> cannonPosition;
    private ushort nextCannonPosition;

    public void PreInitialize()
    {
        chick = Resources.Load<Chick>("Player/Chick/Chick");
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet/Egg");

        chickAnimator = chick.GetComponent<Animator>();

        eggCompleted = true;
        this.numberEgg = 2;
        this.nextCannonPosition = 0;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        onDash = new CustomEvent();
        onDie = new CustomEvent();
        onThrow = new CustomEvent();
        onStartMoving = new CustomEvent();
        onStopMoving = new CustomEvent();

        currentInput = new Vector2();

        SubscribeCustomEvent(Event.DIE, onDie);
        SubscribeCustomEvent(Event.DASH, onDash);
        SubscribeCustomEvent(Event.TRHOW, onThrow);
        SubscribeCustomEvent(Event.START_MOVING, onStartMoving);
        SubscribeCustomEvent(Event.STOP_MOVING, onStopMoving);

        AddAction(Event.DIE, Die);
        AddAction(Event.DASH, Dash);
        AddAction(Event.TRHOW, Throw);
        AddAction(Event.START_MOVING, StartMoving);
        AddAction(Event.STOP_MOVING, StopMoving);
        AddAction(Event.FOOT_STEP_LEFT, Foot_Step_Left);
        AddAction(Event.FOOT_STEP_RIGHT, Foot_Step_Right);
    }

    public void Initialize()
    {
        this.spawnPosition = new List<Vector3>();
        this.cannonPosition = new List<Vector3>();

        this.spawnPosition.Add(new Vector3(Random.Range(360, 450), 135, Random.Range(580, 670)));
        this.spawnPosition.Add(new Vector3(Random.Range(360, 450), 135, Random.Range(580, 670)));
        this.spawnPosition.Add(new Vector3(Random.Range(360, 450), 135, Random.Range(580, 670)));
        this.spawnPosition.Add(new Vector3(Random.Range(360, 450), 135, Random.Range(580, 670)));
        this.spawnPosition.Add(new Vector3(Random.Range(360, 450), 135, Random.Range(580, 670)));
        this.spawnPosition.Add(new Vector3(Random.Range(360, 450), 135, Random.Range(580, 670)));

        if (this.AssID == 10)
            this.cannonPosition.Add(new Vector3(141, 1, 8));
        else
            this.cannonPosition.Add(new Vector3(161, 1, 8));

    }

    public void Refresh()
    {
        if (currentNumOfChick > 0 && !gameOver)
        {
            currentRepairPoints -= currentNumOfChick * Time.deltaTime;
            if (currentRepairPoints <= 0)
            {
                Game.Instance.gameState = Game.GameState.EndGame;
                gameOver = true;
            }
        }
        //Debug.Log(this + ", repairPoints : " + currentRepairPoints);

        //DEBUG
        if (Input.GetKeyDown(KeyCode.P))
        {
            Game.Instance.gameState = Game.GameState.EndGame;
        }
    }

    public void PhysicsRefresh()
    {
    }

    public void LateRefresh()
    {
    }

    public void EndFlow()
    {
    }

    private void OnDestroy()
    {
        onDash.RemoveAllListeners();
        onDie.RemoveAllListeners();
        onThrow.RemoveAllListeners();
    }

    public void ResetValues()
    {
        currentRepairPoints = initialRepairPoints;
        currentNumOfChick = 0;
    }

    public void SmackThatChick()
    {
        currentNumOfChick++;
        Chick chicky = Instantiate(chick, new Vector3(0, 5, 0), Quaternion.identity);
        chicky.Initialize();
        if (nextCannonPosition == this.cannonPosition.Count)
            nextCannonPosition = 0;
        chicky.SetCannonPosition(this.cannonPosition[nextCannonPosition++]);
        PlayerManager.Instance.chicksssss.Add(chicky);
    }

    public void Move(float horizontal, float vertical)
    {
        //Block movement if player not really pushing the joystick.
        if (hasControl && !isDead)
        {
            if ((horizontal < 0.01f && vertical < 0.01f) && (horizontal > -0.01f && vertical > -0.01f))
            {
                if (isMoving)
                {
                    OnAction(Event.STOP_MOVING);
                    isMoving = false;
                }
            }
            else
            {
                if (!isMoving)
                {
                    OnAction(Event.START_MOVING);
                    isMoving = true;
                }

                var newDirection = Quaternion.LookRotation(new Vector3(-vertical, 0,horizontal )).eulerAngles;

                newDirection.x = 0;
                newDirection.z = 0;
                transform.rotation = Quaternion.Euler(newDirection);

                rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
            }
        }
    }

    public void StartMoving()
    {
        animator.SetBool("Run", true);
    }

    public void StopMoving()
    {
        animator.SetBool("Run", false);
    }

    public void Foot_Step_Left()
    {
        this.footStepLeft.Play();
    }

    public void Foot_Step_Right()
    {
        this.footStepRight.Play();
    }

    public void PlayHitSound()
    {
        SoundManager.Instance.PlayOnce(gameObject, 0);
    }

    public void Die()
    {
        hasControl = false;
        isDead = true;
        Debug.Log("In Die");
        animator.SetTrigger("Die");
        TimeManager.Instance.AddTimedAction(new TimedAction(
            () => { StartCoroutine(Dissolve(3)); }
            , 2));
    }

    public void Dash()
    {
        Debug.Log("In Dash");
    }

    public void Throw()
    {
        if (eggCompleted && canShoot)
        {
            animator.SetTrigger("Throw");

            TimeManager.Instance.AddTimedAction(new TimedAction(SpawnBullet, this.delayToShoot));
            TimeManager.Instance.AddTimedAction(new TimedAction(() => { hasControl = true; },
                this.delayToShoot + 0.1f));
            hasControl = false;
            canShoot = false;
        }
    }

    private void BlockControls()
    {
    }

    private void SpawnBullet()
    {
        GameObject shot = GameObject.Instantiate(bulletPrefab, shotSpawn.position, shotSpawn.rotation);
        Bullet bullet = shot.GetComponent<Bullet>();
        bullet.Initialize(AssID);
        bullet.Launch(transform);
        eggCompleted = false;
        this.numberEgg = 0;
    }

    private IEnumerator Dissolve(float TransitionTime)
    {
        float ElapsedTime = 0.0f;
        float accumulateur = 0;
        float ok = GetComponentInChildren<SkinnedMeshRenderer>().material.GetFloat("_DissolveValue");
        while (ok < 1)
        {
            accumulateur += (1 / TransitionTime) * Time.deltaTime;
            ElapsedTime += Time.deltaTime;
            ok += accumulateur;

            foreach (SkinnedMeshRenderer renderer in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                renderer.material.SetFloat("_DissolveValue",
                    renderer.material.GetFloat("_DissolveValue") + accumulateur);
            }

            yield return null;
        }

        Respawn();
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator Resolve(float TransitionTime)
    {
        float ElapsedTime = 0.0f;
        float accumulateur = 0;
        float ok = GetComponentInChildren<SkinnedMeshRenderer>().material.GetFloat("_DissolveValue");
        while (ok > -1)
        {
            accumulateur += (1 / TransitionTime) * Time.deltaTime;
            ElapsedTime += Time.deltaTime;
            ok -= accumulateur;

            foreach (SkinnedMeshRenderer renderer in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                renderer.material.SetFloat("_DissolveValue",
                    renderer.material.GetFloat("_DissolveValue") - accumulateur);
            }

            yield return null;
        }

        gameObject.tag = this.AssID.ToString();
        hasControl = true;

        yield return new WaitForEndOfFrame();
    }

    public void Respawn()
    {
        transform.position = this.spawnPosition[Random.Range(0, this.spawnPosition.Count - 1)];
        animator.SetTrigger("Respawn");
        gameObject.tag = "IRON ASS";

        TimeManager.Instance.AddTimedAction(new TimedAction(
            () => { StartCoroutine(Resolve(3)); }, 2f
        ));

        isDead = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.forward * 5;
        Gizmos.DrawRay(transform.position, direction);
    }

    public void SetAssID(int id)
    {
        AssID = id;
        transform.tag = id.ToString();
    }
}
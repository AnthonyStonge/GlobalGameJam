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
    public Transform lookAt1;
    public Transform lookAt2;
    public int currentNumOfChick = 0;
    public float currentRepairPoints = 200;
    public float initialRepairPoints = 200;

    [Header("Internal")] public Rigidbody rb;
    public Transform shotSpawn;
    private GameObject bulletPrefab;

    [SerializeField] private CustomEvent onDash;
    [SerializeField] private CustomEvent onDie;
    [SerializeField] private CustomEvent onThrow;
    [SerializeField] private CustomEvent onStartMoving;
    [SerializeField] private CustomEvent onStopMoving;
    //[SerializeField] private VisualEffect footStepLeft;
    [SerializeField] private VisualEffect footStepRight;
    [SerializeField] private float delayToShoot;
    [SerializeField] private SkinnedMeshRenderer HighlightMaterial;

    public VisualEffect plumePOWER;
    public VisualEffect smokeCANCER;
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
    private List<GameObject> cannonPrefabs;
    public Transform actuallyTheCannonPosition1;
    public Transform actuallyTheCannonPosition2;
    private Transform actuallyTheCannonPosition;
    private ushort nextCannonPrefab;
    private ushort nextCannonPosition;
    private GameObject actualCannonPrefab;
    private GameObject stepsParticuleSystem;
    public VisualEffect deathVisual;
    public int multipleP1 = 0;
    public int multipleP2 = 0;

    private float factor;
    public void PreInitialize()
    {
        float intensity = 2;
        factor = Mathf.Pow(2, intensity);
        lookAt1 = GameObject.FindWithTag("LookAt1").transform;
        lookAt2 = GameObject.FindWithTag("LookAt2").transform;
        chick = Resources.Load<Chick>("Player/Chick/Chick");
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet/Egg");
        this.stepsParticuleSystem = Resources.Load<GameObject>("VFX/Steps");


        chickAnimator = chick.GetComponent<Animator>();

        eggCompleted = true;
        this.numberEgg = 2;
        this.nextCannonPosition = 0;
        this.nextCannonPrefab = 0;
        this.cannonPrefabs = new List<GameObject>();

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

        PlayerGlow();
    }

    public void Initialize()
    {
        LoadCannonPrefab();
        this.actuallyTheCannonPosition1 = GameObject.Instantiate(this.actuallyTheCannonPosition1);
        this.actuallyTheCannonPosition2 = GameObject.Instantiate(this.actuallyTheCannonPosition2);

        if (this.AssID == 10)
            this.actuallyTheCannonPosition = this.actuallyTheCannonPosition2;
        else
            this.actuallyTheCannonPosition = this.actuallyTheCannonPosition1;
        this.actualCannonPrefab = GameObject.Instantiate<GameObject>(this.cannonPrefabs[this.nextCannonPrefab],
            this.actuallyTheCannonPosition);

        this.spawnPosition = new List<Vector3>();
        this.cannonPosition = new List<Vector3>();

        this.spawnPosition.Add(new Vector3(Random.Range(215, 277), 134, Random.Range(385, 442)));
        this.spawnPosition.Add(new Vector3(Random.Range(215, 277), 134, Random.Range(385, 442)));
        this.spawnPosition.Add(new Vector3(Random.Range(215, 277), 134, Random.Range(385, 442)));
        this.spawnPosition.Add(new Vector3(Random.Range(215, 277), 134, Random.Range(385, 442)));
        this.spawnPosition.Add(new Vector3(Random.Range(215, 277), 134, Random.Range(385, 442)));
        this.spawnPosition.Add(new Vector3(Random.Range(215, 277), 134, Random.Range(385, 442)));
        this.spawnPosition.Add(new Vector3(Random.Range(215, 277), 134, Random.Range(385, 442)));
        this.spawnPosition.Add(new Vector3(Random.Range(215, 277), 134, Random.Range(385, 442)));
        this.spawnPosition.Add(new Vector3(Random.Range(215, 277), 134, Random.Range(385, 442)));

        //Position on cannon for Chickss
        if (this.AssID == 10)
            this.cannonPosition.Add(new Vector3(254, 133, 533));
        else
            this.cannonPosition.Add(new Vector3(254, 133, 341));
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

    public void PlayerGlow()
    {
        if (HighlightMaterial)
        {
            Color col = HighlightMaterial.material.color;
            Color color = new Color(col.r * factor, col.g * factor, col.b * factor);
            HighlightMaterial.material.color = color;
        }
    }

    public void PlayerShootingGlow()
    {
        if (HighlightMaterial)
        {
            Color col = HighlightMaterial.material.color;
            Color color = new Color(col.r / factor, col.g / factor, col.b / factor);
            HighlightMaterial.material.color = color;
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

    private void LoadCannonPrefab()
    {
        if (this.AssID == 10)
        {
            this.cannonPrefabs.Add(Resources.Load<GameObject>("Prefabs/Cannon/Cannon_State_0_0"));
            this.cannonPrefabs.Add(Resources.Load<GameObject>("Prefabs/Cannon/Cannon_State_1_0"));
            this.cannonPrefabs.Add(Resources.Load<GameObject>("Prefabs/Cannon/Cannon_State_2_0"));
            this.cannonPrefabs.Add(Resources.Load<GameObject>("Prefabs/Cannon/Cannon_State_3_0"));
            this.cannonPrefabs.Add(Resources.Load<GameObject>("Prefabs/Cannon/Cannon_State_4_0"));
        }
        else
        {
            this.cannonPrefabs.Add(Resources.Load<GameObject>("Prefabs/Cannon/Cannon_State_0_1"));
            this.cannonPrefabs.Add(Resources.Load<GameObject>("Prefabs/Cannon/Cannon_State_1_1"));
            this.cannonPrefabs.Add(Resources.Load<GameObject>("Prefabs/Cannon/Cannon_State_2_1"));
            this.cannonPrefabs.Add(Resources.Load<GameObject>("Prefabs/Cannon/Cannon_State_3_1"));
            this.cannonPrefabs.Add(Resources.Load<GameObject>("Prefabs/Cannon/Cannon_State_4_1"));
        }
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
        foreach (var i in PlayerManager.Instance.chicksssss)
        {
            Destroy(i.gameObject);
        }
        PlayerManager.Instance.chicksssss.Clear();
    }

    public void SmackThatChick()
    {
        currentNumOfChick++;
        Chick chicky = Instantiate(chick, new Vector3(254, 133, 439), Quaternion.identity);
        chicky.Initialize();
        if (AssID == 10)
        {
            chicky.firstPlayer = true;
            //chicky.transform.LookAt(lookAt1);
        }
        else
        {
            chicky.firstPlayer = false;
            //chicky.transform.LookAt(lookAt2);
        }

        if (nextCannonPosition == this.cannonPosition.Count)
            nextCannonPosition = 0;

        if (AssID == 10)
        {
            //chicky.transform.LookAt(lookAt1);
            chicky.SetCannonPosition(this.cannonPosition[nextCannonPosition++] +
                                     new Vector3(-19f, 13f, ((7 * multipleP1) - 3)));
            multipleP1 += 1;
        }
        else
        {
            chicky.SetCannonPosition(this.cannonPosition[nextCannonPosition++] -
                                     new Vector3((11 * multipleP2) + 36f, 0, (-3 * multipleP2) + 24f));
            multipleP2 += 1;
        }

        PlayerManager.Instance.chicksssss.Add(chicky);
        SwapCannonPrefab();

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

                var newDirection = Quaternion.LookRotation(new Vector3(-vertical, 0, horizontal)).eulerAngles;

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
        //this.footStepLeft.Play();
    }

    public void Foot_Step_Right()
    {
        //this.footStepRight.Play();
        // GameObject thatShitIsGoingDown = Instantiate(this.stepsParticuleSystem, footStepRight.transform.position, Quaternion.identity);
        //thatShitIsGoingDown.GetComponent<VisualEffect>().Play();
        // Destroy(thatShitIsGoingDown, 3f);
    }

    public void PlayHitSound()
    {
        SoundManager.Instance.PlayOnce(gameObject, 0);
    }

    public void Die()
    {
        hasControl = false;
        isDead = true;
        //Debug.Log("In Die");
        animator.SetTrigger("Die");
        SoundManager.Instance.PlayOnce(gameObject, 3);
        gameObject.tag = "IRON ASS";
        Main.Instance.StartShake(2f, 0.2f, 0.5f);
        GameObject LOL = Instantiate(deathVisual.gameObject, deathVisual.transform.position, Quaternion.identity, null);
        LOL.transform.localScale = transform.localScale;
        Destroy(LOL, 2.5f);

        TimeManager.Instance.AddTimedAction(new TimedAction(
            () => { StartCoroutine(Dissolve(3)); }
            , .5f));
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

            PlayerShootingGlow();

            TimeManager.Instance.AddTimedAction(new TimedAction(SpawnBullet, this.delayToShoot));
            TimeManager.Instance.AddTimedAction(new TimedAction(() => { hasControl = true; },
                this.delayToShoot + 0.1f));
            hasControl = false;
            canShoot = false;
        }
    }

    private void SpawnBullet()
    {
        GameObject shot = GameObject.Instantiate(bulletPrefab, shotSpawn.position, shotSpawn.rotation);
        Bullet bullet = shot.GetComponent<Bullet>();
        bullet.Initialize(AssID);
        bullet.Launch(transform);
        
        GameObject LOL = Instantiate(plumePOWER.gameObject, plumePOWER.transform.position, Quaternion.identity, null);
        LOL.SetActive(true);
        LOL.transform.up = -transform.forward;
        LOL.transform.localScale = transform.localScale;
        Destroy(LOL, 1.5f);

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
        

        TimeManager.Instance.AddTimedAction(new TimedAction(
            () => { StartCoroutine(Resolve(3)); }, 2f
        ));

        isDead = false;
    }

    private void SwapCannonPrefab()
    {
        Destroy(actualCannonPrefab);
        //Go to next prefab
        this.nextCannonPrefab++;
        if (nextCannonPrefab < 5)
        {
            this.actualCannonPrefab = GameObject.Instantiate<GameObject>(this.cannonPrefabs[this.nextCannonPrefab],
                this.actuallyTheCannonPosition);
        }
        else
        {

            if (AssID == 10)
            {
                Game.Instance.playerOneWin = true;
            }
            else
            {
                Game.Instance.playerOneWin = false;
            }

            this.nextCannonPrefab = 0;
            Game.Instance.gameState = Game.GameState.EndGame;
        }
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
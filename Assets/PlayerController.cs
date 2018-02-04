using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    GameController gControl;
    [SerializeField]
    float moveSpeed;
    public int health;
    public float jumpForce;
    [SerializeField]
    int maxJump = 1;
    [SerializeField]
    int currentJump;
    public float fallMultiplier = 3.5f;
    public float lowJumpMultiplier = 2f;
    public int fallStart = 0;
    public int fallDamage = 0;
    //checks
    bool facingRight;
    bool attack;
    [SerializeField]
    bool grounded;
    [SerializeField]
    bool wallAttached;
    [SerializeField]
    bool canWallJump;
    [SerializeField]
    bool onWall;
    [SerializeField]
    bool DamageGround;
    /// <summary>
    /// air and falling bools
    /// </summary>
    bool isFalling;
    bool TakeFallDamage;
    bool haveTakenFallDamage;
    //checks when doublejump has happened - sets damage back to 0 and returns false again when falling
    [SerializeField]
    bool fallDamageReset;
    public bool doubleJEnabled;
    bool DoubleJUpgraded;
    public bool buttonCheck;
    //public checks (must be accessible)
    public bool alive;
    //whether or not they are slipper
    //damage list for falling
    private Dictionary<int, int> fallDamageMap = new Dictionary<int, int>();
    public LayerMask groundLayer, wallLayer, dmgGroundLayer;

    /// <summary>
    /// Respawn
    /// </summary>
    [SerializeField]
    bool checkPointReached = false;
    public Vector3 respawnPosition;

    private void Awake()
    {
        InitializeFallDamageMap();
    }

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        //pAnimator = GetComponent<Animator>();
        facingRight = true;
        isFalling = false;
        alive = true;
        TakeFallDamage = false;
        fallDamageReset = false;
        currentJump = maxJump;
        doubleJEnabled = true;
        DoubleJUpgraded = false;
        health = 100;

        respawnPosition = transform.position;
    }

  

    private void FixedUpdate()
    {


    }

    void Update()
    {
        //if alive, all these functions are valid

        if (alive)
        {
            AttackInput();
            PMove();
           
            Jump();
            WallJump();
            Attacks();
            Falling();
            ResetValues();
            PhysicsCheck();
            GroundDamage();
        }

        if (!alive)
        {
            print("you died");
        }

        if (health <= 0)
        {
            health = 0;
            alive = false;
        }
    }

    void PMove()
    {
        float h = Input.GetAxis("Horizontal");
        PHandleMovement(h);
        Flip(h);
    }

    void PHandleMovement(float horizontal)
    {
        var DeltaPosition = moveSpeed * Time.deltaTime;
        rb.velocity = new Vector2(horizontal * DeltaPosition, rb.velocity.y);

    }

    void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 tScale = transform.localScale;

            tScale.x *= -1;

            transform.localScale = tScale;
        }

    }

    void PhysicsCheck()
    {
        grounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.09f, transform.position.y - 0.09f),
            new Vector2(transform.position.x + 0.09f, transform.position.y + 0.09f), groundLayer);
        onWall = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.09f, transform.position.y - 0.09f),
            new Vector2(transform.position.x + 0.09f, transform.position.y + 0.09f), wallLayer);
        DamageGround = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.09f, transform.position.y - 0.09f),
            new Vector2(transform.position.x + 0.09f, transform.position.y + 0.09f), dmgGroundLayer);
    }

    void GroundDamage ()
    {
        if (DamageGround)
        {
            StartCoroutine(DmgGroundReduceHealth());
        }
    }

    private IEnumerator DmgGroundReduceHealth()
    {
        if (Time.frameCount % 6 == 0 && health >= 0)
        {
            health--;
        }

        yield return null;
    }

    void Jump()
    {
        var DeltaJump = jumpForce * Time.fixedDeltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && currentJump > 0)
        {
            //rb.AddForce(transform.up * DeltaJump, ForceMode2D.Impulse);
            rb.velocity = new Vector3(0, DeltaJump, 0);
            currentJump--;
            fallDamage = 0;
        }

        if (doubleJEnabled)
        {
            maxJump = 2;
            DoubleJUpgraded = true;
        }

        if (grounded)
        {
           //fallDamageReset = false;
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        if (rb.velocity.y == 0)
        {
            currentJump = maxJump;
        }

    }

    void WallJump()
    {

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            buttonCheck = true;
        }
        else
        {
            buttonCheck = false;
        }

        if (onWall && buttonCheck)
        {
            wallAttached = true;
        }
        else
        {
            wallAttached = false;
        }
    }

    void Falling()
    {

        if (rb.velocity.y < 0)
        {
            isFalling = true;
        }
        else if (rb.velocity.y >= 0)
        {
            isFalling = false;
        }


        //fallstart measure our fall count
        if (isFalling)
        {
            fallStart++;
            rb.gravityScale = 0.1f;
        }
        //fallstart resets if no longer falling
        else if (!isFalling)
        {
            rb.gravityScale = 1;
            fallStart = 0;
        }


        foreach (int k in fallDamageMap.Keys)
        {
            if (fallStart >= k)
            {
                fallDamage = fallDamageMap[k];
            }
        }

        if (wallAttached)
        {
            fallDamage = 0;
        }

        if (fallDamage > 0)
        {
            TakeFallDamage = true;
            if (TakeFallDamage && grounded)
            {
                health -= fallDamage;
                fallDamage = 0;
                TakeFallDamage = false;
            } 
        }


    }

    private void InitializeFallDamageMap()
    {
        fallDamageMap.Add(75, 20);
        fallDamageMap.Add(90, 35);
        fallDamageMap.Add(100, 60);
        fallDamageMap.Add(110, 85);
        fallDamageMap.Add(120, 110);
        fallDamageMap.Add(130, 140);
        fallDamageMap.Add(140, 170);
        fallDamageMap.Add(150, 200);
        fallDamageMap.Add(160, 240);
        fallDamageMap.Add(170, 280);

    }

    void Attacks()
    {
        //if attack and our animation is no longer attack, then we can attack again
        if (attack /*&& !this.pAnimator.GetCurrentAnimatorStateInfo(0).IsTag("attack)*/)
        {
            //pAnimator.SetTrigger ("attack");
            //if player running, and we attack as we are running, set speed to 1/8
            //thus moving ever so slightly forward as we attack.

        }
    }

    void AttackInput ()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            attack = true;
        }
    }

    void ResetValues()
    {
        attack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CheckPoint")
        {
            checkPointReached = true;
            respawnPosition = collision.transform.position;
        }
    }

}


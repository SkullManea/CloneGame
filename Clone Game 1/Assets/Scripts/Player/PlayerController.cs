using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    public bool canMove = true;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = .2f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float fallMultiplier = 2f;

    [Header("Stamina")]
    public float currentStamina;
    public float maxStamina;
    public float jumpCost;
    public float chargeRate;
    private Coroutine recharge;
    public UnityEngine.UI.Image staminaBar;
    public float dashCost;

    public float burnoutSpeed = 1f;
    public float burnoutJump = 0.5f;
    private bool inBurnout;
    public ParticleSystem burnout;

    [Header("Attacking")]
    public float attackCost;
    public Transform attackOrigin;
    public float attackRadius = 1f;
    public LayerMask enemyMask;
    public int attackDamage;
    public int comboDamage;
    public float cooldownTime = .5f;
    public float cooldownTimer = 0f;


    [Header("KnockBack")]
    public float KBForce;
    public bool KnockFromRight;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rigidBody;
    private bool isGrounded;

    [Header("Dashing")]
    public float dashPower;
    private bool isDashing;
    private bool canDash = true;
    public float dashingTime = 1f;


    private void Awake()
    {
        playerControls = new PlayerControls();
        rigidBody = GetComponent<Rigidbody2D>();
        playerControls.Movement.Jump.performed += OnJump;
        playerControls.Movement.Attack.performed += OnAttack;
        playerControls.Movement.Dash.performed += OnDash;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        CheckIsGrounded();
        HandleBetterFall();

        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;

        Move();
    }

    private void CheckIsGrounded()
    {
        bool notGrounded = isGrounded;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!notGrounded && isGrounded)
        {
            canMove = true;
        }
    }

    private void Move()
    {
        if (!canMove)
            return;

        rigidBody.linearVelocity = new Vector2(movement.x * moveSpeed, rigidBody.linearVelocity.y);

        if (rigidBody.linearVelocityX < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        if (rigidBody.linearVelocityX > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    public void KnockBack(bool fromRight)
    {
        canMove = false;

        float direction = fromRight ? -1 : 1f;
        rigidBody.linearVelocity = new Vector2(direction * KBForce, KBForce);
    }

    private void OnJump(InputAction.CallbackContext contex)
    {
        if (isGrounded)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);

            currentStamina -= jumpCost;
            Staminacharge();

        }
    }

    private void HandleBetterFall()
    {
        if (rigidBody.linearVelocity.y < 0)
        {
            rigidBody.linearVelocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }
    }

    private void OnAttack(InputAction.CallbackContext contex)
    {
        if (!inBurnout)
        {

            if (cooldownTimer <= 0)
            {
                {
                    Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, enemyMask);
                    foreach (var enemy in enemiesInRange)
                    {
                        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                        if (canDash)
                        {
                            enemyHealth.TakeDamage(attackDamage);
                            currentStamina -= attackCost;
                            Staminacharge();
                        }
                        else if (!canDash)
                        {
                            enemyHealth.TakeDamage(comboDamage);
                            Debug.Log("Combo");
                        }

                    }

                    cooldownTimer = cooldownTime; //resets timer

                }
            }
        }

        else
        {
            return;
        }

    }

    void Staminacharge()
    {
        if (currentStamina < 0)
        {
            currentStamina = 0;
            Burnout();
        }
        staminaBar.fillAmount = currentStamina / maxStamina;
        if (recharge != null) StopCoroutine(recharge);
        recharge = StartCoroutine(RechargeStamina());

    }

    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(3f);

        while (currentStamina < maxStamina)
        {
            currentStamina += chargeRate / 10f;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
            staminaBar.fillAmount = currentStamina / maxStamina;
            yield return new WaitForSeconds(.1f);
        }
    }

    private void OnDrawGizmos() //for attacking
    {
        Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
    }

    private void Burnout()
    {
        moveSpeed = burnoutSpeed;
        jumpForce = burnoutJump;
        StopCoroutine(RechargeStamina());
        StartCoroutine(BurnoutTimer());
        inBurnout = true;
        burnout.Play();
    }

    private IEnumerator BurnoutTimer()
    {
        yield return new WaitForSeconds(7f);

        StartCoroutine(RechargeStamina());
        StopCoroutine(BurnoutTimer());
        moveSpeed = 7f;
        jumpForce = 10f;
        inBurnout = false;
        burnout.Stop();
    }

    private void OnDash(InputAction.CallbackContext contex)
    {
        if (!inBurnout && canDash)
        {
            StartCoroutine(Dash());
            currentStamina -= dashCost;
            Staminacharge();
        }
        else
        {
            return;
        }

    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        float originalGravity = rigidBody.gravityScale;
        rigidBody.gravityScale = 0;

        rigidBody.linearVelocity = new Vector2(transform.localScale.x * dashPower, 0f);

        yield return new WaitForSeconds(dashingTime);
        rigidBody.gravityScale = originalGravity;

        isDashing = false;

        yield return new WaitForSeconds(1f);
        canDash = true;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;

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

<<<<<<< HEAD:Clone Game 1/Assets/Scripts/Player/PlayerController.cs
    [Header("Knock Back")]
    public float KBForce;
    public float KBCounter;
    public float KBTotalTime;

    public bool KnockFromRight;
=======
    [Header ("Attacking")]
    public float attackCost;
>>>>>>> Attack:Clone Game 1/Assets/Scripts/PlayerController.cs

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rigidBody;
    private bool isGrounded;


    private void Awake()
    {
        playerControls = new PlayerControls();
        rigidBody = GetComponent<Rigidbody2D>();
        playerControls.Movement.Jump.performed += OnJump;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    private void Update()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        CheckIsGrounded();
        HandleBetterFall();
    }

    private void FixedUpdate()
    {
        if (KBCounter <= 0)
        {
            Move();
        }
        else
        {
            if (KnockFromRight == true)
            {
                rigidBody.linearVelocity = new Vector2(-KBForce, KBForce);
            }
            if (KnockFromRight == false)
            {
                rigidBody.linearVelocity = new Vector2(KBForce, KBForce);
            }

            KBCounter -= Time.deltaTime;
        }
        Debug.Log(currentStamina);
    }

    private void CheckIsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Move()
    {
        rigidBody.linearVelocity = new Vector2(movement.x * moveSpeed, rigidBody.linearVelocity.y);
        //flip the character on the x-axis 1 to -1 when moving directions
    }

    private void OnJump(InputAction.CallbackContext contex)
    {
        if (isGrounded)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);

<<<<<<< HEAD:Clone Game 1/Assets/Scripts/Player/PlayerController.cs
            currentStamina -= jumpCost;//*Time.deltaTime;
            Staminacharge();

        }
=======
        currentStamina -= jumpCost;
        Staminacharge();
       
        } 
>>>>>>> Attack:Clone Game 1/Assets/Scripts/PlayerController.cs
    }

    private void HandleBetterFall()
    {
        if (rigidBody.linearVelocity.y < 0)
        {
            rigidBody.linearVelocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }
    }

<<<<<<< HEAD:Clone Game 1/Assets/Scripts/Player/PlayerController.cs
    private void StaminaHandle()
    {
        if (hasJumped)
        {
            // currentStamina -= jumpCost;//*Time.deltaTime;
            // Staminacharge();
        }

    }
=======
    // private void OnAttack(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //     {
        
    //     currentStamina -= attackCost;
    //     Staminacharge();  
    //     }
        
    // }
>>>>>>> Attack:Clone Game 1/Assets/Scripts/PlayerController.cs

    void Staminacharge()
    {
        if (currentStamina < 0)
        {
            currentStamina = 0;
            //insert burnout
        }
        staminaBar.fillAmount = currentStamina / maxStamina;
        if (recharge != null) StopCoroutine(recharge);
        recharge = StartCoroutine(RechargeStamina());

    }

    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while (currentStamina < maxStamina)
        {
            currentStamina += chargeRate / 10f;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
            staminaBar.fillAmount = currentStamina / maxStamina;
            yield return new WaitForSeconds(.1f);
        }
    }


}

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

    [Header ("Stamina")]
    public float currentStamina;
    public float maxStamina;
    public float jumpCost;
    public float chargeRate;
    private Coroutine recharge;
    private bool hasJumped = false;
    public UnityEngine.UI.Image staminaBar;



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
        StaminaHandle();
    }

    private void FixedUpdate()
    {
        Move();
        Debug.Log(currentStamina);
    }

    private void CheckIsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Move()
    {
        rigidBody.linearVelocity = new Vector2(movement.x * moveSpeed, rigidBody.linearVelocity.y);

    }

    private void OnJump(InputAction.CallbackContext contex)
    {
        if (isGrounded) 
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
            hasJumped = true;
            
        }
       
    }

    private void HandleBetterFall()
    {
        if (rigidBody.linearVelocity.y < 0)
        {
            rigidBody.linearVelocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }
    }

    private void StaminaHandle()
    {
        if (!isGrounded && hasJumped)
        {
            currentStamina -= jumpCost * Time.deltaTime;
            Staminacharge();
        }
       
    }

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
           currentStamina += chargeRate /10f;
           if (currentStamina < maxStamina) currentStamina = maxStamina;
           staminaBar.fillAmount = currentStamina / maxStamina;
           yield return new WaitForSeconds(.1f);
        }
    }


}

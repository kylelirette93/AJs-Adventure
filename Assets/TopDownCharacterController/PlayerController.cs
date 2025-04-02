using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;


public class PlayerController : MonoBehaviour
{
    // References and variables.
    Rigidbody2D rb2D;
    Camera mainCamera;
    Animator animator;
    Vector2 moveVector;
    Vector2 lastMoveVector;
    bool isFacingRight = true;
    bool shiftHeld = false;

    public LayerMask layersForRaycast;

    // Movement settings for the player.
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] float newSpeed;
    [SerializeField] float runSpeed = 0f;
    [SerializeField] bool isRunning = false;
    [SerializeField] bool wasRunning = false;
    [SerializeField] bool isGrounded = false;
    public bool IsGrounded => isGrounded;
    // Jump settings for the player.
    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 15f;
    [SerializeField] bool isJumping = false;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    [SerializeField] float maxJumpTime = 0.2f;
    [SerializeField] float jumpTime = 0f;
    [SerializeField] float groundCheckDistance = 0.2f;

    [Header("Dash Settings")]
    [SerializeField] bool isDashing = false;
    [SerializeField] Vector2 dashDirection;
    [SerializeField] float dashSpeed = 20.0f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashTrailDuration = 0.2f;
    [SerializeField] float dashTrailFadeDuration = 0.1f;
    [SerializeField] int dashTrailSegments = 10;
    [SerializeField] float dashRotationAmount = 45f;
    [SerializeField] Ease dashEase = Ease.OutQuad;
    [SerializeField] Ease rotationEase = Ease.OutBack;
    [SerializeField] Ease scaleEase = Ease.OutBack;
    private float customDashTimer = 0f;

    [Header("Stamina Settings")]
    public float maxEnergy = 100f;
    public float currentEnergy;
    public float energyCost = 30f;
    public float energyRegenRate = 10f;
    public Image energyBar;
    public TextMeshProUGUI energyText;

    float lastTimeJumped = -10.0f;

    public HealthSystem healthSystem = new HealthSystem(100);
    public ParticleSystem dustParticles;

    Vector2 startingPosition;

    private void Awake()
    {
        energyBar.enabled = true;
        currentEnergy = maxEnergy;
        startingPosition = transform.position;
        // Get references.
        rb2D = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        animator = GetComponentInChildren<Animator>();

        // Subscribe to the events.
        Actions.MoveEvent += GetInputVector;
        Actions.DashEvent += Dash;
        Actions.SpaceKeyPressed += OnSpaceKeyPressed;
        Actions.SpaceKeyReleased += () => isJumping = false;

        Actions.ShiftKeyPressed += OnShiftKeyPressed;
        Actions.ShiftKeyReleased += OnShiftKeyReleased;

        newSpeed = moveSpeed;
    }

    private void OnDisable()
    {
        if (Actions.MoveEvent != null)
        {
            Actions.MoveEvent -= GetInputVector;
        }
        if (Actions.DashEvent != null)
        {
            Actions.DashEvent -= Dash;
        }
        Actions.SpaceKeyPressed -= OnSpaceKeyPressed;
        Actions.SpaceKeyReleased -= () => isJumping = false;

        Actions.ShiftKeyPressed -= OnShiftKeyPressed;
        Actions.ShiftKeyReleased -= OnShiftKeyReleased;
    }
    float lastTimeMovedPlayer;

    private void FixedUpdate()
    {
       // Handle movement in fixed update for physics calculations.
       MovePlayer();
       HandleJump();
    }

    private void HandleJump()
    {
        if (isJumping)
        {
            if (jumpTime < maxJumpTime)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
                jumpTime += Time.fixedDeltaTime;
            }
            else
            {
                isJumping = false;
                jumpTime = 0f;
            }
        }
    }

    public void EnableInput()
    {
        // Subscribe to the events.
        Actions.MoveEvent += GetInputVector;
        Actions.DashEvent += Dash;
        Actions.SpaceKeyPressed += OnSpaceKeyPressed;
        Actions.SpaceKeyReleased += () => isJumping = false;

        Actions.ShiftKeyPressed += OnShiftKeyPressed;
        Actions.ShiftKeyReleased += OnShiftKeyReleased;
    }
    public void DisableInput()
    {
        // Subscribe to the events.
        Actions.MoveEvent -= GetInputVector;
        Actions.DashEvent -= Dash;
        Actions.SpaceKeyPressed -= OnSpaceKeyPressed;
        Actions.SpaceKeyReleased += () => isJumping = false;

        Actions.ShiftKeyPressed -= OnShiftKeyPressed;
        Actions.ShiftKeyReleased += OnShiftKeyReleased;
    }
   

    private void Update()
    {
        isGrounded = GroundCheck();

        RegenerateStamina();
        UpdateStaminaUI();

        if (customDashTimer > 0.0f)
        {
            customDashTimer -= Time.unscaledDeltaTime; // Use unscaledDeltaTime
            if (customDashTimer <= 0)
            {
                lowJumpMultiplier = 2.5f;
                newSpeed = moveSpeed;
            }
            else
            {
                lowJumpMultiplier = 25f;
            }
        }

        if (rb2D.velocity.y < 0)
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;  
        }
        else if (rb2D.velocity.y > 0 && !isJumping)
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        
        if (moveVector != Vector2.zero)
        {
            // Only handle flip if player is moving.
            Flip();
        }

        if (shiftHeld && moveVector != Vector2.zero && !isRunning)
        {
            StartRun();
        }
        else if (isRunning && shiftHeld && moveVector == Vector2.zero)
        {
            StopRun();
        }

        UpdateAnimator();

        if (transform.position.y <= -6f)
        {
            GameManager.instance.audioManager.PlayOneShot(GameManager.instance.audioManager.deathSFX);
            ResetPlayer();
        }
    }

    private void UpdateAnimator()
    {
        animator.SetBool("IsRunning", isRunning && !isJumping);
        animator.SetBool("IsMoving", moveVector != Vector2.zero);
        animator.SetBool("IsJumping", isJumping);      
        animator.SetBool("IsGrounded", isGrounded); 
    }

    private void OnShiftKeyPressed()
    {
        shiftHeld = true;
        if (isGrounded && !isJumping)
        {
            StartRun();
        }
    }

    private void OnShiftKeyReleased()
    {
        shiftHeld = false;
        StopRun();
    }

    private void OnSpaceKeyPressed()
    {
        Jump();
    }

    void StartRun()
    {
        if (shiftHeld && moveVector != Vector2.zero)
        {
            isRunning = true;
            runSpeed = moveSpeed * 2f;
        }
    }

    public void ResetPlayer()
    {
        Camera camera = Camera.main;
        camera.transform.DOMove(new Vector3(startingPosition.x, startingPosition.y + 5f, -10f), 0.7f).SetEase(Ease.OutExpo);
        transform.DOMove(startingPosition, 0.7f).SetEase(Ease.OutExpo);
        foreach (GameObject cheese in GameObject.FindGameObjectsWithTag("Cheese"))
        {
            GameManager.instance.scoreManager.Reset();
            cheese.GetComponent<Cheese>().IsCollected = false;
            cheese.GetComponent<Cheese>().EnableCheese();
        }
    }

    void StopRun()
    {
        isRunning = false;
        runSpeed = 0f;
    }
    void Jump()
    {
        if (isGrounded && !isJumping)
        {
            // Perform a jump.
            isJumping = true;
            GameManager.instance.audioManager.PlayOneShot(GameManager.instance.audioManager.jumpSFX);
            jumpTime = 0f;
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            lastTimeJumped = Time.time;       
        }
    }

    void Dash()
    {
        if (currentEnergy < energyCost) return;

        isDashing = true;
        GameManager.instance.audioManager.PlayOneShot(GameManager.instance.audioManager.dashSFX);
        customDashTimer = dashDuration;

        currentEnergy -= energyCost;
        UpdateStaminaUI();

        if (moveVector != Vector2.zero)
        {
            dashDirection = moveVector.normalized;
        }
        else
        {
            dashDirection = isFacingRight ? Vector2.right : Vector2.left;
        }

        rb2D.velocity = dashDirection * dashSpeed;


        // Dash movement, rotation, and scale.
        transform.DOMove(transform.position + new Vector3(dashDirection.x, 0, 0), dashDuration / 2).SetEase(dashEase);

        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        // Rotate the player in the direction of the dash, then return to original rotation.
        transform.DORotate(new Vector3(0, 0, transform.eulerAngles.z + (dashDirection.x > 0 ? -dashRotationAmount : dashRotationAmount)), dashDuration, RotateMode.FastBeyond360)
            .SetEase(rotationEase)
            .OnComplete(() =>
            {
                transform.DORotate(new Vector3(0, 0, 0), dashDuration * 0.7f).SetEase(rotationEase); // Return to original rotation smoothly after dash.
                isDashing = false;
            });
        // Scale the player up and down during the dash.
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), dashDuration, 10, 1).SetEase(scaleEase);

        CreateDashTrail();
    }

    GameObject hitobject = null;


    void RegenerateStamina()
    {
        if (currentEnergy < maxEnergy && !isDashing)
        {
            currentEnergy += energyRegenRate * Time.deltaTime;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        }
    }

    void UpdateStaminaUI()
    {
        if (energyBar != null && energyText != null)
        {
            energyBar.fillAmount = currentEnergy / maxEnergy;
            energyText.text = "energy";

            float energyPercentage = currentEnergy / maxEnergy;
            Color startColor = Color.red;
            Color endColor = Color.green;
            Color currentColor = Color.Lerp(startColor, endColor, energyPercentage);

            energyText.DOColor(currentColor, 0.2f);
        }
    }

    bool GroundCheck()
    {
        // Get player's collider bounds.
        Bounds colliderBounds = GetComponent<BoxCollider2D>().bounds;

        // Calculate raycast positions.
        Vector2 centerRaycast = new Vector2(colliderBounds.center.x, colliderBounds.min.y);
        Vector2 leftRaycast = new Vector2(colliderBounds.min.x, colliderBounds.min.y);
        Vector2 rightRaycast = new Vector2(colliderBounds.max.x, colliderBounds.min.y);

        // Cast raycasts.
        RaycastHit2D centerHit = Physics2D.Raycast(centerRaycast, Vector2.down, groundCheckDistance, layersForRaycast);
        RaycastHit2D leftHit = Physics2D.Raycast(leftRaycast, Vector2.down, groundCheckDistance, layersForRaycast);
        RaycastHit2D rightHit = Physics2D.Raycast(rightRaycast, Vector2.down, groundCheckDistance, layersForRaycast);

        // Check if any raycast hit.
        if (centerHit.collider != null || leftHit.collider != null || rightHit.collider != null)
        {
            return true;
        }

        return false;
    }
    void CreateDashTrail()
    {
        // This was a pain in the butt... U.U but it works.
        for (int i = 0; i < dashTrailSegments; i++)
        {
            // Create a new trail segment. 
            GameObject trailSegment = new GameObject("DashTrailSegment");
            // Add a sprite renderer to the trail segment and copy the player's sprite.
            SpriteRenderer trailRenderer = trailSegment.AddComponent<SpriteRenderer>();
            SpriteRenderer playerRenderer = GetComponentInChildren<SpriteRenderer>();
            trailRenderer.sprite = playerRenderer.sprite;

            trailRenderer.sortingLayerName = "Player";
            // Set the segment progress based on the current iteration.
            float segmentProgress = (float)i / dashTrailSegments;
            // Set the trail segment's sorting order to be behind the player.
            Vector3 trailPosition = transform.position - (Vector3)(dashDirection * dashSpeed * dashDuration * segmentProgress / dashTrailSegments);

            // Set the segment's position to be behind the player.
            trailSegment.transform.position = trailPosition;

            // Set the segment's scale to be slightly smaller than the player's scale.
            trailSegment.transform.localScale = transform.localScale * (1f - segmentProgress * 0.05f);

            // Flip the trail segment's sprite to match the player's facing direction.
            if (playerRenderer.flipX)
            {
                trailRenderer.flipX = true;
            }
            else
            {
                trailRenderer.flipX = false;
            }

            // Fade out the segment over time and than destroy it.
            Color startColor = trailRenderer.color;
            startColor.a = 1f;
            trailRenderer.color = startColor;

            trailRenderer.DOFade(0f, dashTrailFadeDuration)
                .SetDelay(dashTrailDuration * segmentProgress)
                .OnComplete(() => Destroy(trailSegment));           
        }        
    }

    void MovePlayer()
    {
        if (!isDashing)
        {
            if (moveVector != Vector2.zero)
            {
                float currentSpeed = (isRunning && runSpeed > 0) ? runSpeed : newSpeed;
                rb2D.velocity = new Vector2(moveVector.x * currentSpeed, rb2D.velocity.y);
            }
            else
            {
                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            }
        }
    }

    void GetInputVector(Vector2 inputDirection)
    {
        // If there is movement input, store the new vector. 
        if (inputDirection != Vector2.zero)
        {
            moveVector = inputDirection;
            lastMoveVector = moveVector.normalized;
        }
        else
        {
            // No input, stay idle.
            moveVector = Vector2.zero;
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }
    }

    private void Flip()
    {
        // Check for movement direction and flip the player sprite accordingly.
        if (isDashing) return;
        if (moveVector.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            if (isGrounded)
            {
                dustParticles.Play();
            }
        }
        else if (moveVector.x < 0 && isFacingRight)
        {
            isFacingRight = false;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            if (isGrounded)
            {
                dustParticles.Play();
            }
        }
    }
}
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Rendering.UI;


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
    [SerializeField] Transform groundCheck;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    [SerializeField] float maxJumpTime = 0.2f;
    [SerializeField] float jumpTime = 0f;

    [Header("Dash Settings")]
    [SerializeField] bool isDashing = false;
    [SerializeField] float dashCooldownTime = 2f;
    [SerializeField] float nextDashTime = 0f;
    [SerializeField] Vector2 dashDirection;
    [SerializeField] float dashSpeed = 20.0f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashTime = 0f;
    [SerializeField] float dashTrailDuration = 0.2f; 
    [SerializeField] float dashTrailFadeDuration = 0.1f; 
    [SerializeField] int dashTrailSegments = 10; 
    [SerializeField] float dashRotationAmount = 45f; 
    [SerializeField] Ease dashEase = Ease.OutQuad; 
    [SerializeField] Ease rotationEase = Ease.OutBack; 
    [SerializeField] Ease scaleEase = Ease.OutBack;

    float lastTimeJumped = -10.0f;
    private void Awake()
    {
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
        Actions.MoveEvent -= GetInputVector;
        Actions.DashEvent -= Dash;
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

   

    private void Update()
    {
        isGrounded = GroundCheck();
        if (dashTime > 0.0f)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
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

        if (shiftHeld && moveVector != Vector2.zero && !isRunning && isGrounded)
        {
            StartRun();
        }
        else if (isRunning && shiftHeld && moveVector == Vector2.zero)
        {
            StopRun();
        }

        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        animator.SetBool("IsRunning", isRunning);
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
        if (shiftHeld && moveVector != Vector2.zero && isGrounded)
        {
            isRunning = true;
            runSpeed = moveSpeed * 2f;
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
            jumpTime = 0f;
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            lastTimeJumped = Time.time;       
        }
    }

    void Dash()
    {
        if (Time.time < nextDashTime) return;

        isDashing = true;
        dashTime = dashDuration;
        if (moveVector != Vector2.zero)
        {
            dashDirection = moveVector.normalized;
        }
        else
        {
            dashDirection = isFacingRight ? Vector2.right : Vector2.left;
        }

        newSpeed = dashSpeed;


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

        nextDashTime = Time.time + dashCooldownTime;
    }

    GameObject hitobject = null; 

    bool GroundCheck()
    {
        if (lastTimeJumped + 0.2f > Time.time)
            return false;
        
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.8f, layersForRaycast);
        if (hit.collider != null)
        {
            isJumping = false;
            hitobject = hit.collider.gameObject; 
            return true;
        }
        else
        {
            hitobject = null; 
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = Vector2.down * 0.8f;
        Gizmos.DrawRay(groundCheck.position, direction);
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
        if (moveVector != Vector2.zero)
        {
            float currentSpeed = (runSpeed > 0 && !isDashing) ? runSpeed : newSpeed;
            Vector2 newPosition = rb2D.position + moveVector * currentSpeed * Time.deltaTime;
            lastTimeMovedPlayer = Time.time;
            rb2D.velocity = new Vector2(moveVector.x * currentSpeed, rb2D.velocity.y);
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
        }
        else if (moveVector.x < 0 && isFacingRight)
        {
            isFacingRight = false;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
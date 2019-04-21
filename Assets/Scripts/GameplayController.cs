using UnityEngine;
using System.Collections;

public class GameplayController : MonoBehaviour
{

    public Transform playerTransform = null;
    // public Animator animator = null;
    public Rigidbody2D body = null;
    public LayerMask groundLayerMask = 0;
    public GameObject cutlass = null;

    public string inputNameJump = "Jump";
    public string inputTriggerNameJump = "Jump";
    public string inputNameHorizontal = "Horizontal";
    public string inputNameFire1 = "Fire1";

    public float playerForceMovement = 365.0f;
    public float playerForceJump = 1000.0f;
    public float playerVelocityMax = 5.0f;
    public float playerRadius = 1.0f;

    public GrappleManager grappleManager;

    private bool isJumpRequested = false;
    private bool isCutlassRequested = false;
    private Vector3 playerPositionPrevious = Vector3.zero;
    private bool isPlayerPositionPreviousInitialized = false;

    public int cutlassDurationTicks = 30;
    private int cutlassTicksSinceRequest = 0;

    void Awake()
    {

        // Debug.Assert(animator != null);
        Debug.Assert(body != null);
        Debug.Assert(cutlass != null);
    }

    void Update()
    {
        Vector3 playerGroundPosition;
        playerGroundPosition.x = playerTransform.position.x;
        playerGroundPosition.y = playerTransform.position.y - playerRadius;
        playerGroundPosition.z = playerTransform.position.z;
        bool isGrounded = Physics2D.Linecast(
            playerTransform.position,
            playerGroundPosition,
            groundLayerMask
        );

        if (Input.GetButtonDown(inputNameJump) && isGrounded)
        {
            isJumpRequested = true;
        }
        if (Input.GetKeyDown(KeyCode.F)) {

            grappleManager.ToggleGrapple();
        }
        if (Input.GetButton(inputNameFire1))
        {
            isCutlassRequested = true;
        }
    }

    void FixedUpdate()
    {
        float horizontalFactor = Input.GetAxis(inputNameHorizontal);
        // animator.SetFloat("Speed", Mathf.Abs(horizontalFactor));

        if (horizontalFactor * body.velocity.x < playerVelocityMax)
        {
            body.AddForce(Vector2.right * horizontalFactor * playerForceMovement);
        }


        // Enforce player max velocity
        Vector2 playerVelocity = new Vector2(
            Mathf.Min(body.velocity.x, (body.velocity.x >= 0.0f) ? playerVelocityMax : -playerVelocityMax),
            body.velocity.y
        );

        body.velocity = playerVelocity;

        if (isJumpRequested)
        {
            isJumpRequested = false;
            // animator.SetTrigger(inputTriggerNameJump);
            body.AddForce(new Vector2(0.0f, playerForceJump));
        }

        this.ComputePlayerFacingDirection(horizontalFactor);
        this.ComputePlayerRotation(horizontalFactor);

        float cutlassDirection = (Mathf.Abs(horizontalFactor) > 1e-5f)
            ? horizontalFactor
            : body.velocity.x;
        this.ComputeCutlass(cutlassDirection);
    }

    void ComputePlayerFacingDirection(float inputHorizontalFactor)
    {
        Vector2 localScale = new Vector2((inputHorizontalFactor >= 0.0f)
            ? Mathf.Abs(playerTransform.localScale.x)
            : -Mathf.Abs(playerTransform.localScale.x),
            playerTransform.localScale.y
        );
        playerTransform.localScale = localScale;
    }

    void ComputePlayerRotation(float inputHorizontalFactor)
    {
        playerPositionPrevious = isPlayerPositionPreviousInitialized
            ? playerTransform.position
            : playerPositionPrevious;
        isPlayerPositionPreviousInitialized = true;

        float playerCircumference = 2.0f * Mathf.PI * playerRadius;
        float playerDistanceTraveled = Vector3.Distance(playerPositionPrevious, playerTransform.position);

        Debug.Assert(playerCircumference > 0.0f);
        float playerRotationDelta = 2.0f * Mathf.PI * playerDistanceTraveled / playerCircumference;
        playerRotationDelta = (inputHorizontalFactor >= 0.0f)
            ? -playerRotationDelta
            : playerRotationDelta;
        playerPositionPrevious = playerTransform.position;

        float tiltAroundX = Input.GetAxis("Horizontal") * playerRotationDelta;
        float tiltAroundZ = Input.GetAxis("Vertical") * playerRotationDelta;

        Quaternion target = Quaternion.Euler(0.0f, 0.0f, tiltAroundZ);

        playerTransform.rotation = (playerTransform.rotation * target).normalized;
    }

    void ComputeCutlass(float horizontalFactor)
    {
        Vector3 offset = new Vector3((horizontalFactor >= 0.0f) ? 2.5f : -2.5f, 0.0f, 0.0f);
        cutlass.transform.position = playerTransform.position + offset;
        cutlass.transform.localScale = new Vector2((horizontalFactor >= 0.0f)
            ? Mathf.Abs(cutlass.transform.localScale.x)
            : -Mathf.Abs(cutlass.transform.localScale.x),
            1.0f);

        if (isCutlassRequested)
        {
            isCutlassRequested = false;
            cutlassTicksSinceRequest = 0;
            cutlass.SetActive(true);
        }


        if (cutlassTicksSinceRequest == cutlassDurationTicks)
        {
            cutlass.SetActive(false);
        }

        cutlassTicksSinceRequest = Mathf.Min(cutlassTicksSinceRequest + 1, cutlassDurationTicks);
    }
}
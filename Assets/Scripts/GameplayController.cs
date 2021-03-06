using UnityEngine;
using System.Collections;
using System;

public class GameplayController : MonoBehaviour
{
    private static GameplayController instance = null;
    public Transform playerPhysicsTransform = null;
    public Transform playerRenderingTransform = null;
    // public Animator animator = null;
    public Rigidbody2D body = null;
    public LayerMask groundLayerMask = 0;
    public GameObject cutlass = null;
    public static bool isDead = false;

    public string inputNameJump = "Jump";
    public string inputTriggerNameJump = "Jump";
    public string inputNameHorizontal = "Horizontal";

    public string inputNameFire1 = "Fire1";

    public float playerForceMovement = 365.0f;
    public float playerForceJump = 1000.0f;
    public float playerVelocityMax = 5.0f;
    public float playerGrappleVelocityMin = 1.0f;
    public float playerRadius = 1.0f;

    public GrappleManager grappleManager;
    public AudioController audioController;

    private bool isJumpRequested = false;
    private bool isCutlassRequested = false;
    private bool isCutlassAudioPlaying = false;
    private Vector3 playerPositionPrevious = Vector3.zero;
    private bool isPlayerPositionPreviousInitialized = false;
    public float cutlassOffset = 1.0f;

    public static bool playerIsFacingRight = true;

    void Awake()
    {
        Debug.Assert(GameplayController.instance==null);
        GameplayController.instance = this;

        // Debug.Assert(animator != null);
        Debug.Assert(body != null);
        Debug.Assert(cutlass != null);
        Debug.Assert(playerPhysicsTransform != null);
        Debug.Assert(playerRenderingTransform != null);
    }

    void Update()
    {
        if (Input.GetButtonDown(inputNameJump))
        {
            isJumpRequested = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            grappleManager.ToggleGrapple();
        }
        if (Input.GetMouseButtonUp(1) && grappleManager.isGrappling) {
            grappleManager.ToggleGrapple();
        }
        if (Input.GetButton(inputNameFire1))
        {
            isCutlassRequested = true;
        }
        else
        {
            isCutlassAudioPlaying = false;
        }
    }

    void FixedUpdate()
    {
        float horizontalFactor = Input.GetAxis(inputNameHorizontal);

        // Update player facing direction only on input.
        if (Mathf.Abs(horizontalFactor) > 1e-5f) { playerIsFacingRight = (horizontalFactor >= 0.0f); }

        if (this.grappleManager.isGrappling)
        {
            this.body.simulated = false;
            Vector2 playerPos = new Vector2(this.playerPhysicsTransform.position.x, this.playerPhysicsTransform.position.y);
            Vector2 currentVelocity = this.body.velocity;
            Vector2 newPosition = playerPos + currentVelocity * Time.deltaTime;
            Vector2 grapplePointPosition = this.grappleManager.GetGrapplePointPosition();
            Vector2 ropeDirection = (newPosition - grapplePointPosition).normalized;
            newPosition = grapplePointPosition + ropeDirection * grappleManager.ropeLength;
            this.playerPhysicsTransform.position = new Vector3(newPosition.x, newPosition.y, this.playerPhysicsTransform.position.z);
            grappleManager.UpdateRopeTransform(newPosition);

            var newDirection = new Vector2(-ropeDirection.y, ropeDirection.x);
            if (Vector2.Dot(newDirection, this.body.velocity) < 0)
            {
                newDirection *= -1;
            }
            var newSpeed = this.body.velocity.magnitude;
            if (newSpeed < playerGrappleVelocityMin) {
                newSpeed = playerGrappleVelocityMin;
            }
            var newVelocity =  newDirection * newSpeed;
            this.body.velocity = newVelocity;
        }
        else
        {
            Vector3 playerGroundPosition;
            playerGroundPosition.x = playerPhysicsTransform.position.x;
            playerGroundPosition.y = playerPhysicsTransform.position.y - playerRadius;
            playerGroundPosition.z = playerPhysicsTransform.position.z;
            bool isGrounded = Physics2D.Linecast(
                playerPhysicsTransform.position,
                playerGroundPosition,
                groundLayerMask
            );

            this.body.simulated = true;
            if (Mathf.Abs(horizontalFactor) > 1e-5f && isGrounded)
            {
                body.AddForce(Vector2.right * horizontalFactor * playerForceMovement);
            }

            // Enforce player max velocity only when grounded.
            Vector2 playerVelocity = isGrounded
                ? new Vector2(
                    Mathf.Max(Mathf.Min(body.velocity.x, playerVelocityMax), -playerVelocityMax),
                    body.velocity.y
                )
                : body.velocity;

            body.velocity = playerVelocity;


            if (isJumpRequested && isGrounded) { body.AddForce(new Vector2(0.0f, playerForceJump)); }
            isJumpRequested = false;

            if (playerPhysicsTransform.position.y < -15.0f) {
                GameplayController.isDead = true;
            }
        }

        this.ComputePlayerFacingDirection();
        this.ComputePlayerRotation();
        this.ComputeCutlass();
    }

    void ComputePlayerFacingDirection()
    {
        Vector2 localScale = new Vector2(playerIsFacingRight
            ? Mathf.Abs(playerRenderingTransform.localScale.x)
            : -Mathf.Abs(playerRenderingTransform.localScale.x),
            playerRenderingTransform.localScale.y
        );
        playerRenderingTransform.localScale = localScale;
    }

    void ComputePlayerRotation()
    {
        playerPositionPrevious = isPlayerPositionPreviousInitialized
            ? playerPositionPrevious
            : playerRenderingTransform.position;
        isPlayerPositionPreviousInitialized = true;

        float playerCircumference = 2.0f * Mathf.PI * playerRadius;
        float playerDistanceTraveled = Vector3.Distance(playerPositionPrevious, playerRenderingTransform.position);
        playerPositionPrevious = playerRenderingTransform.position;

        Debug.Assert(playerCircumference > 0.0f);
        float playerRotationDelta = 2.0f * Mathf.PI * playerDistanceTraveled / playerCircumference;

        bool isPlayerVelocityFacingRight = (this.body.velocity.x >= 0.0f);
        playerRotationDelta = isPlayerVelocityFacingRight
            ? -playerRotationDelta
            : playerRotationDelta;

        Quaternion target = Quaternion.Euler(0.0f, 0.0f, Mathf.Rad2Deg * playerRotationDelta);
        playerRenderingTransform.rotation = (playerRenderingTransform.rotation * target).normalized;
    }

    void ComputeCutlass()
    {
        float cutlassOffset = this.playerRadius + 1e-2f;
        Vector3 offset = new Vector3(playerIsFacingRight ? cutlassOffset : -cutlassOffset, 0.0f, 0.0f);
        cutlass.transform.position = playerPhysicsTransform.position + offset;
        cutlass.transform.localScale = new Vector2(playerIsFacingRight
            ? Mathf.Abs(cutlass.transform.localScale.x)
            : -Mathf.Abs(cutlass.transform.localScale.x),
            cutlass.transform.localScale.y
        );


        cutlass.SetActive(isCutlassRequested);
        if (isCutlassRequested && !isCutlassAudioPlaying)
        {
            isCutlassAudioPlaying = true;
            audioController.playCutlass();
        }
        isCutlassRequested = false;
    }

    public static Vector2 GetVelocity()
    {
        return GameplayController.instance.body.velocity;
    }
}
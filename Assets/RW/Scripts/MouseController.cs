using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    public float jetpackForce = 75.0f;
    public float forwardMovementSpeed = 3.0f;
    public Transform groundCheckTransform;
    private bool isGrounded;
    public LayerMask groundCheckLayerMask;
    private Animator mouseAnimator;
    private Rigidbody2D playerRigidbody;
    public ParticleSystem jetpack;
    private bool isDead = false;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        mouseAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        bool jetpackActive = Mouse.current.leftButton.isPressed;
        jetpackActive = jetpackActive && !isDead;

        if (jetpackActive)
        {
            playerRigidbody.AddForce(new Vector2(0, jetpackForce));
        }

        if (!isDead)
        {
            Vector2 newVelocity = playerRigidbody.velocity;
            newVelocity.x = forwardMovementSpeed;
            playerRigidbody.velocity = newVelocity;
        }

        UpdateGroundedStatus();
        AdjustJetpack(jetpackActive);
    }

    void UpdateGroundedStatus()
    {
        //1
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        //2
        mouseAnimator.SetBool("isGrounded", isGrounded);
    }

    void AdjustJetpack(bool jetpackActive)
    {
        var jetpackEmission = jetpack.emission;
        jetpackEmission.enabled = !isGrounded;
        if (jetpackActive)
        {
            jetpackEmission.rateOverTime = 300.0f;
        }
        else
        {
            jetpackEmission.rateOverTime = 75.0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        HitByLaser(collider);
    }

    void HitByLaser(Collider2D laserCollider)
    {
        isDead = true;
        mouseAnimator.SetBool("isDead", true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Controls")]
    public Joystick joystick;

    [Header("Movement")]
    public float horizontalForce;
    public float verticalForce;

    public Vector2 maxVelocity;
    [Range(0.0f, 1.0f)] public float airMoveFactor = 0.5f;

    [Header("Effects")]
    public ParticleSystem dustTrail;
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineBasicMultiChannelPerlin perlin;
    public float shakeIntensity;
    public float maxShakeTime;
    public float shakeTimer;
    public bool isCameraShaking;

    [Header("Ground")]
    public bool isGrounded;
    public Transform groundOrigin;
    public float groundRadius;
    public LayerMask groundLayerMask;

    private Animator animationControler;
    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animationControler = GetComponent<Animator>();

        perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        CheckIfGrounded();

        if (isGrounded && rigidbody.velocity.sqrMagnitude > 0.1f)
            dustTrail.Play();
        else if ((!isGrounded || rigidbody.velocity.sqrMagnitude < 0.1f))
            dustTrail.Stop();

        if (isCameraShaking)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0.0f)
            {
                perlin.m_AmplitudeGain = 0.0f;
                shakeTimer = maxShakeTime;
                isCameraShaking = false;
            }
        }
    }

    private void ShakeCamera()
    {
        perlin.m_AmplitudeGain = shakeIntensity;
        isCameraShaking = true;
    }

    private void Move()
    {
        float x = joystick.Direction.x + Input.GetAxisRaw("Horizontal");

        float horizontalMoveForce = x * horizontalForce * Time.deltaTime;

        float mass = rigidbody.mass * rigidbody.gravityScale;

        if (isGrounded)
        {
            // Keyboard Input
            float jump = (Input.GetAxisRaw("Jump") != 0.0f || UIController.jump ? 1 : 0);

            float jumpMoveForce = jump * verticalForce * Time.deltaTime;

            rigidbody.AddForce(new Vector2(horizontalMoveForce, jumpMoveForce) * mass);
            rigidbody.velocity *= 0.99f; // scaling / stopping hack

            if (rigidbody.velocity.sqrMagnitude > 0.1f)
            {
                animationControler.SetInteger("AnimationState", (int)PlayerAnimationEnum.RUN);
            }
            else
            {
                animationControler.SetInteger("AnimationState", (int)PlayerAnimationEnum.IDLE);
            }

        }
        else
        {
            animationControler.SetInteger("AnimationState", (int)PlayerAnimationEnum.JUMP);

            rigidbody.AddForce(new Vector2(horizontalMoveForce * airMoveFactor, 0.0f) * mass);
        }

        if (x != 0)
        {
            x = FlipAnimation(x);
        }

        rigidbody.velocity = new Vector2(Mathf.Clamp(rigidbody.velocity.x, -maxVelocity.x, maxVelocity.x),
                Mathf.Clamp(rigidbody.velocity.y, -maxVelocity.y, maxVelocity.y));
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.down, groundRadius, groundLayerMask);

        if (!isGrounded && hit)
        {
            ShakeCamera();
        }

        isGrounded = (hit) ? true : false;
    }

    private float FlipAnimation(float x)
    {
        // depending on direction scale across the x-axis either 1 or -1
        x = (x > 0) ? 1 : -1;

        transform.localScale = new Vector3(x, 1.0f);
        return x;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(collision.transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(null);
        }
    }

    // UTILITIES

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundOrigin.position, groundRadius);
    }

}

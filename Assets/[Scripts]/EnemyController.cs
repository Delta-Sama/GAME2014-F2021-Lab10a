using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public Vector2 maxVelocity;

    [Header("Line Casts")]
    public Transform lookAhead;
    public Transform lookInFront;

    [Header("Ground")]
    public bool isGrounded;
    public Transform groundOrigin;
    public float groundRadius;

    [Header("Layers")]
    public LayerMask groundMask;
    public LayerMask wallMask;
    public LayerMask groundLayerMask;

    private Rigidbody2D rigidBody;
    private LOS enemyLOS;

    public bool hasLOS = false;

    private float currentDirection = -1.0f;
    

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        enemyLOS = GetComponent<LOS>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementAI();
    }

    private void FixedUpdate()
    {
        CheckIfGrounded();

        HasLOS();

        MoveEnemy();

        rigidBody.velocity = new Vector2(Mathf.Clamp(rigidBody.velocity.x, -maxVelocity.x, maxVelocity.x),
                Mathf.Clamp(rigidBody.velocity.y, -maxVelocity.y, maxVelocity.y));
    }

    private void MovementAI()
    {
        if (!isGrounded) return;

        LookAhead();
        LookInFront();
    }

    private void LookAhead()
    {
        var hitResult = Physics2D.Linecast(transform.position, lookAhead.position, groundMask);

        if (!hitResult)
        {
            FlipDirection();
        }
    }

    private void LookInFront()
    {
        var hitResult = Physics2D.Linecast(transform.position, lookInFront.position, wallMask);

        if (hitResult)
        {
            FlipDirection();
        }
    }

    private void HasLOS()
    {
        if (enemyLOS.colliderList.Count > 0)
        {
            foreach (var collider in enemyLOS.colliderList)
            {
                if (collider.gameObject.CompareTag("Player"))
                {
                    Vector3 dist = collider.transform.position - transform.position;
                    RaycastHit2D result = Physics2D.Raycast(transform.position, dist.normalized, dist.magnitude, LayerMask.GetMask("Platform", "Wall"));

                    if (result.collider == null)
                    {
                        hasLOS = true;

                        return;
                    }
                }
            }
        }

        hasLOS = false;
    }

    private void MoveEnemy()
    {
        if (hasLOS)
        {
            rigidBody.velocity = new Vector3(0.0f, rigidBody.velocity.y);
            return;
        }

        float mass = rigidBody.mass * rigidBody.gravityScale;

        rigidBody.AddForce(new Vector2(currentDirection * speed * Time.deltaTime, 0.0f) * mass);
        rigidBody.velocity *= 0.99f;

        //transform.position += new Vector3(currentDirection * speed * Time.deltaTime, 0.0f);
    }

    private void FlipDirection()
    {
        currentDirection = -currentDirection;

        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y);
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.down, groundRadius, groundLayerMask);

        isGrounded = (hit) ? true : false;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, lookAhead.position);
        Gizmos.DrawLine(transform.position, lookInFront.position);

        Gizmos.DrawWireSphere(groundOrigin.position, groundRadius);

        if (GameManager.Instance != null)
        {
            if (hasLOS)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, GameManager.Instance.Player.transform.position);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, GameManager.Instance.Player.transform.position);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class LOS : MonoBehaviour
{
    [Header("LOS Settings")]
    public ContactFilter2D contactFilter;
    public List<Collider2D> colliderList;

    [HideInInspector] public Collider2D collidesWith;

    private PolygonCollider2D LOSCollider;

    void Start()
    {
        LOSCollider = GetComponent<PolygonCollider2D>();
    }

    private void FixedUpdate()
    {
        Physics2D.GetContacts(LOSCollider, contactFilter, colliderList);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        collidesWith = other;
    }
}

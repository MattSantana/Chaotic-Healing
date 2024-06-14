using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockback(Transform damageSource, float knockbackThrust)
    {
        Vector2 difference = (transform.position - damageSource.position).normalized * knockbackThrust * rb.mass;
        rb.AddForce(difference, ForceMode2D.Impulse);
    }
}
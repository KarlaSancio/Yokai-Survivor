using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D collectorCollider;
    public float attractSpeed;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        collectorCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        collectorCollider.radius = player.CurrentMagnet;
    }

    // Método que é chamado quando um objeto entra no trigger do coletor
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out InfCollector collector))
        {
            // Adiciona forca ao objeto para atrair ele para o player
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            rb.AddForce(direction * attractSpeed);

            collector.Collect();
        }
    }
}

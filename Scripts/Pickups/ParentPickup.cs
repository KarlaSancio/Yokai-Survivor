using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPickup : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se o objeto que colidiu com o diamante de xp for o player, o diamante eh destruido
        if (collision.CompareTag("Player")) 
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Variáveis de movimento
    [HideInInspector]
    public float lastHorizontalV;
    [HideInInspector]
    public float lastVerticalV;
    [HideInInspector]
    public Vector2 moveDirection;
    [HideInInspector]
    public Vector2 lastMovedVector;

    Rigidbody2D rb;
    PlayerStats player;




    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1, 0f); // Inicializa o vetor de movimento
        
    }

    // Update is called once per frame
    void Update()
    {
        gerenciadorInputs();
    }

    void FixedUpdate()
    {
        MoveHandler();
    }

    void gerenciadorInputs()
    {
        // parar o player se game over == true
        if(GameManager.instance.isGameOver)
        {
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;   

        if (moveDirection.x != 0)
        {
            lastHorizontalV = moveDirection.x;
            lastMovedVector = new Vector2(lastHorizontalV, 0f);
        }
        if (moveDirection.y != 0)
        {
            lastVerticalV = moveDirection.y;
            lastMovedVector = new Vector2(0f, lastVerticalV);
        }

        if(moveDirection.x != 0 && moveDirection.y != 0)
        {
            lastMovedVector = new Vector2(lastHorizontalV, lastVerticalV);
        }
    }

     void MoveHandler()
     {
        if(GameManager.instance.isGameOver)
        {
            return;
        }
        rb.velocity = new Vector2(moveDirection.x * player.CurrentMoveSpeed, moveDirection.y * player.CurrentMoveSpeed);
     }
}

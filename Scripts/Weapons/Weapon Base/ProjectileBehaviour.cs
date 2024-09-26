using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script para controle de projeteis
/// </summary>

public class ProjectileBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponStats;
    protected Vector3 direction;
    public float destroyAfter;

    // Stats atuais
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected float currentPierce;

    void Awake()
    {
        currentDamage = weaponStats.Damage;
        currentSpeed = weaponStats.Speed;
        currentCooldownDuration = weaponStats.CooldownDuration;
        currentPierce = weaponStats.Pierce;
    }

    public float GetCurrentDamage()
    {
        return currentDamage *= FindObjectOfType<PlayerStats>().CurrentMight;
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfter);
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;

        float dirx = direction.x;
        float diry = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if(dirx < 0 && diry == 0) // Esquerda
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if(dirx == 0 && diry < 0) // Baixo
        {
            scale.y = scale.y * -1;
        }
        else if(dirx == 0 && diry > 0) // Cima
        {
            scale.x = scale.x * -1;
        }
        else if(dirx > 0 && dir.y > 0) // Direita cima
        {
            rotation.z = 0f;
        }
        else if(dir.x > 0 && dir.y < 0) // Direita baixo
        {
            rotation.z = -90f;
        }
        else if (dir.x < 0 && dir.y > 0) // Esquerda cima
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = -90f;
        }
        else if (dir.x < 0 && dir.y < 0) // Esquerda baixo
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1; 
            rotation.z = 0f;
        }


        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemy"))
        {
            EnemyData enemy = col.GetComponent<EnemyData>();
            enemy.GripDamage(GetCurrentDamage());
            DecreasePierce();
        }
        else if (col.CompareTag("Prop"))
        {
            if(col.gameObject.TryGetComponent(out DestroyableProps destroyable))
            {
                destroyable.GripDamage(GetCurrentDamage());
                DecreasePierce();
            }
        }
    }

    void DecreasePierce() // Diminui a quantidade de vezes que o projétil pode atravessar inimigos
    {
        currentPierce--;

        if(currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/Character")]
public class CharacterScriptableObject : ScriptableObject
{
    [SerializeField]
    Sprite icon; 
    public Sprite Icon { get => icon; private set => icon = value; } 

    [SerializeField]
    new string name;
    public string Name { get => name; private set => name = value; }

    [SerializeField]
    GameObject startWeapon; 
    public GameObject StartWeapon { get => startWeapon; private set => startWeapon = value; } 

    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }

    [SerializeField]
    float maxH;
    public float MaxH { get => maxH; private set => maxH = value; }

    [SerializeField]
    float recoveryItem;
    public float RecoveryItem { get => recoveryItem; private set => recoveryItem = value; }

    [SerializeField]
    float might;
    public float Might { get => might; private set => might = value; }


    [SerializeField]
    float projectileSpeed;
    public float ProjectileSpeed { get => projectileSpeed; private set => projectileSpeed = value; }

    [SerializeField]
    float magnet;// eh a propriedade que define o raio de atracao do personagem (o xp e itens vao ser atraidos para ele)
    public float Magnet { get => magnet; private set => magnet = value; }
}

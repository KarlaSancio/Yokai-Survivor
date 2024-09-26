using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField]
    GameObject prefab;
    public GameObject Prefab { get => prefab; private set => prefab = value;}

    // Stats base das armas
    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value; }
    [SerializeField]
    float speed;
    public float Speed { get => speed; private set => speed = value; }
    [SerializeField]
    float cooldownDuration;
    public float CooldownDuration { get => cooldownDuration; private set => cooldownDuration = value; }
    [SerializeField]
    int pierce;
    public int Pierce { get => pierce; private set => pierce = value; }

    [SerializeField]
    int level; // MODIFICAR SOMENTE NO EDITOR!!!
    public int Level { get => level; private set => level = value; }
    [SerializeField]
    GameObject nextlevelPrefab; 
    public GameObject NextLevelPrefab { get => nextlevelPrefab; private set => nextlevelPrefab = value; }

    [SerializeField]
    new string name; // Nome da arma
    public string Name { get => name; private set => name = value; }

    [SerializeField]
    string description; // Descricao da arma
    public string Description { get => description; private set => description = value; }

    [SerializeField]
    Sprite icon; // Idem
    public Sprite Icon { get => icon; private set => icon = value; }
}

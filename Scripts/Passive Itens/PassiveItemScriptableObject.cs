using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject ", menuName = "ScriptableObjects/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    float mult;
    public float Mult
    {
        get => mult;
        private set => mult = value;
    }

    [SerializeField]
    int level; // MODIFICAR SOMENTE NO EDITOR!!!
    public int Level 
    { 
        get => level;
        private set => level = value; 
    }
    [SerializeField]
    GameObject nextlevelPrefab; 
    public GameObject NextLevelPrefab 
    { 
        get => nextlevelPrefab; 
        private set => nextlevelPrefab = value; 
    }

    [SerializeField]
    Sprite icon; // Idem
    public Sprite Icon 
    { 
        get => icon; 
        private set => icon = value; 
    }

    [SerializeField]
    new string name; // Nome do item passivo
    public string Name 
    { 
        get => name; 
        private set => name = value; 
    }

    [SerializeField]
    string description; // Descricao do item passivo
    public string Description 
    { 
        get => description; 
        private set => description = value; 
    }
}

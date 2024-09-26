using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterPicker : MonoBehaviour
{

    public static CharacterPicker instance;
    public CharacterScriptableObject characterData;

void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Ja existe um CharacterPicker na cena" + this + " foi destruido");
            Destroy(gameObject);
        }
    }

    public static CharacterScriptableObject GetCharacterData()
    {
        return instance.characterData;
    }

    public void SetCharacter(CharacterScriptableObject character)
    {
        characterData = character;
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}

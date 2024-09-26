using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStats player;
    public PassiveItemScriptableObject passiveItemData;

    protected virtual void ApllyMods()
    {
        // Aplica o valor bonus para a o status correto nas classes filhas
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        ApllyMods();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPassiveItem : PassiveItem 
{
    protected override void ApllyMods()
    {
        player.CurrentMight *= 1 + passiveItemData.Mult / 100f; // Aumenta o dano do jogador ao pegar a flor
    }
}

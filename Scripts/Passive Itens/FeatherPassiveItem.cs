using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherPassiveItem : PassiveItem
{
    protected override void ApllyMods()
    {
        player.CurrentMoveSpeed *= 1 + passiveItemData.Mult / 100f; // Aumenta a velocidade de movimento do jogador ao pegar a pena
    }
}

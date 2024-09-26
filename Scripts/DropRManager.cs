using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string dropName;
        public GameObject itemPrefab;
        public float dropRate;
    }

    public List<Drops> drops;

    void OnDestroy()
    {
        if(!gameObject.scene.isLoaded) return; // Precisei adicionar essa verificacao para evitar erros ao fechar o jogo

        float random = UnityEngine.Random.Range(0f, 100f); // Gera um número aleatório entre 0 e 100
        List<Drops> probableDrops = new List<Drops>();

        foreach (Drops drop in drops)
        {
            if (random <= drop.dropRate)
            {
                probableDrops.Add(drop);
                break;
            }
        }
        // Se houver drops possíveis, escolhe um aleatório e instancia o prefab do item
        if (probableDrops.Count > 0)
        {
            Drops drops = probableDrops[UnityEngine.Random.Range(0, probableDrops.Count)];
            Instantiate(drops.itemPrefab, transform.position, Quaternion.identity);
        }
        
    }
}

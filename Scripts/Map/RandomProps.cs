using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomProps : MonoBehaviour
{
    public List<GameObject> propsPrefab;
    public List<GameObject> propsSpawn;

    // Start is called before the first frame update
    void Start()
    {
        SpawnProps();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnProps(){
        foreach(GameObject sp in propsSpawn){
            int index = Random.Range(0, propsPrefab.Count);
            GameObject prop = Instantiate(propsPrefab[index], sp.transform.position, Quaternion.identity);
            prop.transform.parent = sp.transform;
        }
    }
}

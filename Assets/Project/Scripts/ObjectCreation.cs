using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Items",menuName = "Categoria 1/Categoria 2/Categoria 3")]
public class ObjectCreation : ScriptableObject
{
    List<GameObject> smallObjects = new List<GameObject>();
    List<GameObject> mediumObjects = new List<GameObject>();
    List<GameObject> bigObjects = new List<GameObject>();
    GameObject GetRandomObject(List<GameObject> objectList) 
    { 
        if(objectList.Count == 0)
        {
            Debug.LogWarning("La lista is empty.");
            return null;
        }
        return objectList[Random.Range(0,objectList.Count-1)];
    }
}

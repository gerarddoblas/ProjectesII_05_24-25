using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Items",menuName = "ObjectCreatorManager")]
public class ObjectCreation : ScriptableObject
{
    List<GameObject> smallObjects = new List<GameObject>();
    List<GameObject> mediumObjects = new List<GameObject>();
    List<GameObject> bigObjects = new List<GameObject>();
    GameObject GetRandomSmallObject() { return smallObjects[Random.Range(0, smallObjects.Count - 1)]; }
    GameObject GetRandomMediumObject() { return mediumObjects[Random.Range(0, mediumObjects.Count - 1)]; }
    GameObject GetRandomBigObject() { return bigObjects[Random.Range(0, bigObjects.Count - 1)]; }
}

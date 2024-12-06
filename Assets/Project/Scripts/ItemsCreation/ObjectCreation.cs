using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Items",menuName = "ObjectCreatorManager")]
public class ObjectCreation : ScriptableObject
{
    [SerializeField]List<GameObject> smallObjects = new List<GameObject>();
    [SerializeField] List<GameObject> mediumObjects = new List<GameObject>();
    [SerializeField] List<GameObject> bigObjects = new List<GameObject>();
    public GameObject GetRandomSmallObject() { return smallObjects[Random.Range(0, smallObjects.Count)]; }
    public GameObject GetRandomMediumObject() { return mediumObjects[Random.Range(0, mediumObjects.Count)]; }
    public GameObject GetRandomBigObject() { return bigObjects[Random.Range(0, bigObjects.Count)]; }
}

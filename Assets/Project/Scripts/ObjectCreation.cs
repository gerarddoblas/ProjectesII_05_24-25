using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreation : ScriptableObject
{
    List<GameObject> objects = new List<GameObject>();
    GameObject GetRandomObject() { return objects[Random.Range(0,objects.Count-1)]; }
}

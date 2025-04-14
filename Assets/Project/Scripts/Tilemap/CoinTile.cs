using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEngine.Tilemaps
{

    [Serializable]
    public class CoinTile : PrefabTile
    {
        private void OnEnable()
        {
            spawnInfo = new List<ValueTuple<Vector3Int, string>>();
            SceneManager.sceneLoaded += delegate (Scene loadedScene, LoadSceneMode loadedSceneMode)
            {
                Debug.Log("PrefabSpawn");
                string sceneName = loadedScene.name;
                foreach (var info in spawnInfo)
                {
                    if (sceneName == info.Item2 && GameController.Instance.currentGameMode.GetType().Equals(typeof(CoinCollectGame))) Instantiate(prefab, info.Item1 + Vector3.right * 0.5f + Vector3.down * .5f, Quaternion.identity);
                }
            };
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(CoinTile))]
    public class CoinTileEditor : Editor
    {
        private CoinTile tile { get { return (target as CoinTile); } }

        public void OnEnable()
        {


        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Place sprite.");
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            tile.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", tile.sprite, typeof(Sprite), false, null);
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(tile);

            EditorGUILayout.LabelField("Place prefab.");
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            tile.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", tile.prefab, typeof(GameObject), false, null);
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(tile);
        }

        [MenuItem("Assets/Create/CoinTile")]
        public static void CreatePrefabTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Coin Tile", "New Coin Tile", "Asset", "Save Coin Tile", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CoinTile>(), path);
        }
    }

#endif

}

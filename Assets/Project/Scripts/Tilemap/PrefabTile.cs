using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using static UnityEditor.VersionControl.Asset;
using UnityEngine.SceneManagement;
#endif

namespace UnityEngine.Tilemaps
{
    //Several parts of the code are dependent on the UNITY_EDITOR flag
    //This is to allow for easier use during development

    [Serializable]
    public class PrefabTile : TileBase
    {
        public List<Vector3Int> positions;
        public Sprite sprite;
        public GameObject prefab;
        void OnEnable()
        {
            positions = new List<Vector3Int>();
            SceneManager.sceneLoaded += delegate (Scene loadedScene, LoadSceneMode loadedSceneMode)
            {
                Debug.Log("PrefabSpawn");
                foreach (var position in positions)
                {
                    Instantiate(prefab, position, Quaternion.identity);
                }
            };
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying) tileData.sprite = null;
            else tileData.sprite = sprite;
#else
            tileData.sprite = null;
#endif
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(PrefabTile))]
    public class PrefabTileEditor : Editor
    {
        private PrefabTile tile { get { return (target as PrefabTile); } }

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
            tile.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", tile.sprite, typeof(GameObject), false, null);
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(tile);
        }

        [MenuItem("Assets/Create/PrefabTile")]
        public static void CreatePrefabTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Prefab Tile", "New Prefab Tile", "Asset", "Save Prefab Tile", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<PrefabTile>(), path);
        }
    }

#endif

}
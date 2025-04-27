using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using static UnityEditor.VersionControl.Asset;
#endif

namespace UnityEngine.Tilemaps
{
    //Several parts of the code are dependent on the UNITY_EDITOR flag
    //This is to allow for easier use during development

    [Serializable]
    public class PrefabTile : TileBase
    {
        public Sprite sprite;
        public GameObject prefab;
        public GameObject parent;

        public Dictionary<int, List<Vector3Int>> positions = new Dictionary<int, List<Vector3Int>>();

        public virtual void OnEnable()
        {
            SceneManager.sceneLoaded += delegate (Scene loadedScene, LoadSceneMode loadSceneMode)
            {
                if(!positions.ContainsKey(loadedScene.buildIndex)) positions.Add(loadedScene.buildIndex, new List<Vector3Int>());
                if(parent == null) parent = Instantiate(new GameObject("---" + prefab.name.ToUpper() + "---"));
                foreach (Vector3Int position in positions[loadedScene.buildIndex]) Instantiate(prefab, position, Quaternion.identity, parent.transform);
            };
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            
            int curSceneIndex = SceneManager.GetActiveScene().buildIndex;

            if(!positions.ContainsKey(curSceneIndex)) positions.Add(curSceneIndex, new List<Vector3Int>());
            if (!positions[curSceneIndex].Contains(position)) positions[curSceneIndex].Add(position);

            tileData.colliderType = Tile.ColliderType.None;

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
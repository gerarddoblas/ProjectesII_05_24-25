using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Tilemaps
{
    [Serializable]
    public class ExplodingTile : TileBase
    {
        [SerializeField] public bool started = false;
        [SerializeField] public Sprite[] sprites;

        public override void RefreshTile(Vector3Int pos, ITilemap tilemap)
        {
            Debug.Log("Refreshed at " + pos);
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            Debug.Log("GetData at " + position);
            UpdateTile(position, tilemap, ref tileData);
        }

        private void UpdateTile(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {

#if UNITY_EDITOR
            tileData.sprite = sprites[0];
            tileData.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 0f), Vector3.one);
            tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
            tileData.colliderType = Tile.ColliderType.Sprite;
            started = false;
            Debug.Log("Setup");
            return;
#else
            if (started)
            {
                Debug.Log("boom");
                tileData.sprite = sprites[1];
                tileData.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 0f), Vector3.one);
                tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
                tileData.colliderType = Tile.ColliderType.Sprite;
            } else
            {
                tileData.sprite = sprites[0];
                tileData.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 0f), Vector3.one);
                tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
                tileData.colliderType = Tile.ColliderType.Sprite;
                started = true;
            }
#endif
        }
    }

#if UNITY_EDITOR

[CustomEditor(typeof(ExplodingTile))]
public class ExplodingTileEditor : Editor 
{
    private ExplodingTile tile { get { return (target as ExplodingTile); } }

    public void OnEnable()
    {
        if(tile.sprites == null || tile.sprites.Length != 2) tile.sprites = new Sprite[2];
    }
    
    public override void OnInspectorGUI() 
    {
        EditorGUILayout.LabelField("Place sprites shown based on the state.");
        EditorGUILayout.Space();
          
        EditorGUI.BeginChangeCheck();
        tile.sprites[0] = (Sprite) EditorGUILayout.ObjectField("Before", tile.sprites[0], typeof(Sprite), false, null);
        tile.sprites[1] = (Sprite) EditorGUILayout.ObjectField("After", tile.sprites[1], typeof(Sprite), false, null);
        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(tile);
    }

    [MenuItem("Assets/Create/ExplodingTile")]
    public static void CreateExplodingTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Exploding Tile", "New Exploding Tile", "Asset", "Save Exploding Tile", "Assets");
        if (path == "")
            return;                           
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ExplodingTile>(), path);
    }
}

#endif

}
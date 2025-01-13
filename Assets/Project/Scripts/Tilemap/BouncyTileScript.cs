using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Tilemaps
{
    //Several parts of the code are dependent on the UNITY_EDITOR flag
    //This is to allow for easier use during development

    [Serializable]
    public class BouncyTile : TileBase
    {
        public Sprite sprite;
        void OnEnable()
        {
            
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = sprite;
            tileData.colliderType = Tile.ColliderType.Sprite;
        }

        public void BounceAt(GameObject gameObject, Vector3 contact)
        {
            float x = gameObject.transform.position.x - contact.x;
            float y = gameObject.transform.position.y - contact.y;
            Vector2 dir = (Mathf.Abs(x) > Mathf.Abs(y))
                ? Vector2.right * Mathf.Sign(x)
                : Vector2.up * Mathf.Sign(y);

            gameObject.GetComponent<Rigidbody2D>().AddForce(dir * 1000.0f, ForceMode2D.Force);
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(ExplodingTile))]
    public class BouncyTileEditor : Editor 
    {
        private BouncyTile tile { get { return (target as BouncyTile); } }

        public void OnEnable()
        {


        }
    
        public override void OnInspectorGUI() 
        {
            EditorGUILayout.LabelField("Place sprite.");
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            tile.sprite = (Sprite) EditorGUILayout.ObjectField("Sprite", tile.sprite, typeof(Sprite), false, null);
        }

        [MenuItem("Assets/Create/BouncyTile")]
        public static void CreateBouncyTile()
        {
                string path = EditorUtility.SaveFilePanelInProject("Save Bouncy Tile", "New Bouncy Tile", "Asset", "Save Bouncy Tile", "Assets");
                if (path == "")
                    return;
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BouncyTile>(), path);
            }
    }

#endif

}
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TileClickForcer))]
[CanEditMultipleObjects]
public class TileClickEditor : Editor
{

    // SerializedProperty tile;

    public override void OnInspectorGUI()
    {
        // serializedObject.Update();
        // serializedObject.ApplyModifiedProperties(); 
        GameObject activeObj = Selection.activeGameObject.GetComponentInParent<TileClickTarget>().gameObject;
        // TileClickTarget component = activeObj.GetComponent<TileClickTarget>();
        // Debug.Log(component);
        // if (activeObj = null){
        //     activeObj = Selection.activeGameObject.transform.parent.gameObject.GetComponentInChildren<TileClickTarget>().gameObject;
        // }
       //  Selection.activeGameObject = activeObj;
         
    }

    // private void OnSceneGUI() {
        // Selection.activeGameObject = Selection.activeGameObject.transform.parent.transform.parent.GetComponent<TileClickTarget>().gameObject;
        
    // }
}
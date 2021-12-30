using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

internal sealed class NavmeshMenuItem
{
    [MenuItem("J-Tools/Clear Navigation Static")]
    private static void ClearNavStatic()
    {
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(go);
            if ((flags | StaticEditorFlags.NavigationStatic) == flags)
            {
                flags = flags & ~StaticEditorFlags.NavigationStatic;
                GameObjectUtility.SetStaticEditorFlags(go, flags);
            }
        }
        NavMesh.RemoveAllNavMeshData();
    }
}

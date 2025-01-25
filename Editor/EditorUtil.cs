using BP.PoolIO;
using UnityEditor;
using UnityEngine;

public static class EditorUtil
{

    [MenuItem("GameObject/Pools/Pool", false, 10)]
    public static void CreatePool(MenuCommand menuCommand)
    {
        GameObject go = new("Pool");
        go.AddComponent<Pool>();
        EditorCreeate(menuCommand, go);
    }

    [MenuItem("GameObject/Pools/Group", false, 10)]
    public static void CreateGroup(MenuCommand menuCommand)
    {
        GameObject root = new("Group", typeof(PoolGroup));
        var rootGroup = root.GetComponent<PoolGroup>();
        for (int i = 0; i < 3; i++)
        {
            GameObject child = new($"Pool #{i + 1}", typeof(Pool));
            child.transform.SetParent(root.transform);
            rootGroup.AddPool(child.GetComponent<Pool>());
        }
        EditorCreeate(menuCommand, root);
    }

    private static void EditorCreeate(MenuCommand menuCommand, params GameObject[] gameobjects)
    {
        foreach (var go in gameobjects)
        {
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        }
        Selection.objects = gameobjects;
    }
}

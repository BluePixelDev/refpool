using UnityEditor;
using UnityEngine;

namespace BP.RefPool.Editor
{
    internal static class EditorItems
    {
        [MenuItem("GameObject/RefPool/Pool", false, 10)]
        public static void CreatePool(MenuCommand menuCommand)
        {
            GameObject go = new("Pool");
            go.AddComponent<RefPooler>();
            EditorCreate(menuCommand, go);
        }

        [MenuItem("GameObject/RefPool/Group", false, 10)]
        public static void CreateGroup(MenuCommand menuCommand)
        {
            GameObject root = new("Group", typeof(RefPoolerGroup));
            var rootGroup = root.GetComponent<RefPoolerGroup>();
            for (int i = 0; i < 3; i++)
            {
                GameObject child = new($"Pool #{i + 1}", typeof(RefPooler));
                child.transform.SetParent(root.transform);
                rootGroup.Add(child.GetComponent<RefPooler>());
            }
            EditorCreate(menuCommand, root);
        }


        [MenuItem("GameObject/RefPool/Spawner", false, 10)]
        public static void CreateSpawner(MenuCommand menuCommand)
        {
            GameObject root = new("Spawner", typeof(RefSpawner));
            EditorCreate(menuCommand, root);
        }

        private static void EditorCreate(MenuCommand menuCommand, params GameObject[] gameobjects)
        {
            foreach (var go in gameobjects)
            {
                GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
                Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            }
            Selection.objects = gameobjects;
        }
    }
}
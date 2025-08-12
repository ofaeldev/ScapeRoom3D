using UnityEditor;
using UnityEngine;
using System.IO;

public static class MissingScriptProjectScanner
{
    [MenuItem("Tools/Utilitários/Procurar Missing Scripts em Prefabs do Projeto")]
    public static void ScanProjectPrefabs()
    {
        string[] prefabPaths = Directory.GetFiles("Assets", "*.prefab", SearchOption.AllDirectories);
        int missingCount = 0;

        foreach (string path in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
                continue;

            Component[] components = prefab.GetComponentsInChildren<Component>(true);
            foreach (Component comp in components)
            {
                if (comp == null)
                {
                    missingCount++;
                    Debug.LogWarning($"❌ Missing script encontrado no prefab: {path}", prefab);
                    break; // só reporta uma vez por prefab
                }
            }
        }

        if (missingCount == 0)
        {
            Debug.Log("✅ Nenhum Missing Script encontrado nos prefabs do projeto.");
        }
        else
        {
            Debug.Log($"⚠️ Total de prefabs com Missing Scripts: {missingCount}");
        }
    }
}

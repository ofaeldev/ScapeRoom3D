using UnityEngine;
using UnityEditor;

public class MissingScriptFinder : MonoBehaviour
{
    [MenuItem("Tools/Utilitários/Procurar Missing Scripts na Cena")]
    public static void FindMissingScripts()
    {
        int missingCount = 0;
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            Component[] components = go.GetComponents<Component>();

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    missingCount++;
                    Debug.LogWarning($"Missing script encontrado no objeto: {GetFullPath(go)}", go);
                }
            }
        }

        if (missingCount == 0)
        {
            Debug.Log("✅ Nenhum Missing Script encontrado na cena.");
        }
        else
        {
            Debug.Log($"⚠️ Total de objetos com Missing Script: {missingCount}");
        }
    }

    private static string GetFullPath(GameObject obj)
    {
        string path = obj.name;
        Transform current = obj.transform;

        while (current.parent != null)
        {
            current = current.parent;
            path = current.name + "/" + path;
        }

        return path;
    }
}

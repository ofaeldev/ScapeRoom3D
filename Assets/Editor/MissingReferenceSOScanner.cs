using UnityEditor;
using UnityEngine;
using System.IO;

public static class MissingReferenceSOScanner
{
    [MenuItem("Tools/Utilitários/Procurar Referências Quebradas em ScriptableObjects")]
    public static void ScanScriptableObjects()
    {
        string[] assetPaths = Directory.GetFiles("Assets", "*.asset", SearchOption.AllDirectories);
        int totalMissing = 0;

        foreach (string path in assetPaths)
        {
            ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            if (so == null) continue;

            SerializedObject serializedObject = new SerializedObject(so);
            SerializedProperty property = serializedObject.GetIterator();

            bool hasMissing = false;

            while (property.NextVisible(true))
            {
                if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null && property.objectReferenceInstanceIDValue != 0)
                {
                    Debug.LogWarning($"❌ Referência quebrada em ScriptableObject: {path} → Campo: {property.name}", so);
                    hasMissing = true;
                    totalMissing++;
                }
            }

            if (!hasMissing)
            {
                // opcional: mostrar os assets ok
                // Debug.Log($"✔️ {path} OK");
            }
        }

        if (totalMissing == 0)
        {
            Debug.Log("✅ Nenhuma referência quebrada encontrada nos ScriptableObjects do projeto.");
        }
        else
        {
            Debug.Log($"⚠️ Total de referências quebradas em ScriptableObjects: {totalMissing}");
        }
    }
}

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;

public class CreateCharacterList
{
    [MenuItem("Assets/Create/Character List")]
	public static CharacterList Create()
    {
        CharacterList asset = ScriptableObject.CreateInstance<CharacterList>();

        AssetDatabase.CreateAsset(asset, "Assets/CharacterList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
#endif
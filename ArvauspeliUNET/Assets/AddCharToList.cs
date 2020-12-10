using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;


public class AddCharToList : EditorWindow 
{
    string pathToCharList="Assets/CharacterList.asset";
    int currentCharacter=1;
    int jumpToNumber=1;
    CharacterList charList;

    [MenuItem ("Window/Character Manager")]
    
    public static void ShowWindow ()
    {
        EditorWindow.GetWindow(typeof(AddCharToList));
    }
    
    void OnGUI()
    {
        if (!charList)
        {
            charList = AssetDatabase.LoadAssetAtPath(pathToCharList, typeof(CharacterList)) as CharacterList;
        }
        GUILayout.Label("Character Manager:", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(currentCharacter + " of " + charList.characterList.Count);        
        if (GUILayout.Button("Prev"))
        {
            if(currentCharacter!=1)
            {
                currentCharacter--;                
            }
            else if (currentCharacter == 1)
            {
                currentCharacter = charList.characterList.Count;
            }
            jumpToNumber = currentCharacter;
            GUI.FocusControl("");
        }
        if (GUILayout.Button("Next"))
        {
            if (currentCharacter != charList.characterList.Count)
            {
                currentCharacter++;
            }
            else if (currentCharacter == charList.characterList.Count)
            {
                currentCharacter = 1;
            }
            jumpToNumber = currentCharacter;
            GUI.FocusControl ("");
        }
        EditorGUILayout.EndHorizontal();
        charList.characterList[currentCharacter - 1].name= EditorGUILayout.TextField("Character name: ", charList.characterList[currentCharacter - 1].name);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Character Description: ");
        EditorStyles.textField.wordWrap = true;
        charList.characterList[currentCharacter - 1].description = EditorGUILayout.TextArea(charList.characterList[currentCharacter - 1].description,                                                                                             
                                                                                             GUILayout.Width(300),
                                                                                             GUILayout.Height(100));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Image: ");
        charList.characterList[currentCharacter - 1].image = EditorGUILayout.ObjectField(charList.characterList[currentCharacter - 1].image, typeof(Sprite), false) as Sprite;
        EditorGUILayout.EndHorizontal();
        charList.characterList[currentCharacter - 1].fiction = (bool)EditorGUILayout.Toggle("Fiction: ", charList.characterList[currentCharacter - 1].fiction);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add a new Character"))
        {
            Character newCharacter = new Character();
            newCharacter.name = "Character Name";
            newCharacter.description = "Character Description";
            newCharacter.image = null;
            newCharacter.fiction = false;
            charList.characterList.Add(newCharacter);
            currentCharacter = charList.characterList.Count;
            GUI.FocusControl ("");
            jumpToNumber = currentCharacter;
        }    
        if (GUILayout.Button("Delete Current Character"))
        {
            charList.characterList.RemoveAt(currentCharacter - 1);
            if(currentCharacter>charList.characterList.Count)
            {
                currentCharacter = charList.characterList.Count;                
            }
            jumpToNumber = currentCharacter;
            GUI.FocusControl ("");
        }        
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        jumpToNumber = EditorGUILayout.IntField("Index: ", jumpToNumber);
        if (GUILayout.Button("Jump to"))
        {
            if(jumpToNumber>=1 && jumpToNumber<=charList.characterList.Count)
            {
                currentCharacter = jumpToNumber;
            }
            else
            {
                EditorUtility.DisplayDialog("Invalid Input:", "You can only use values from 1 to " + charList.characterList.Count, "OK");
            }
            GUI.FocusControl ("");
        }
        GUI.SetNextControlName("");
        if (GUI.changed)
        {
            EditorUtility.SetDirty(charList);
        }
    }
}
#endif
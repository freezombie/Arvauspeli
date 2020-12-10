using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GetRandom : MonoBehaviour 
{
    Text character;
    public GameController gameController;

    void OnEnable()
    {
        gameController.setNewRandom();
        character = GetComponent<Text>();
        character.text = gameController.getName();
    }

    void OnDisable()
    {
        character = null;
    }
}

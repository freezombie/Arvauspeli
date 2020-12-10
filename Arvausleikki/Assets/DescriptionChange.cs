using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DescriptionChange : MonoBehaviour 
{
    public GameController gameController;
    Text description;

    void OnEnable()
    {
        description = GetComponent<Text>();
        description.text = gameController.getDescription();
    }

    void OnDisable()
    {
        description = null;
    }
}


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageChange : MonoBehaviour 
{
    public GameController gameController;
    Image picture;

    void OnEnable()
    {
        picture = GetComponent<Image>();
        picture.sprite = gameController.getImage();
    }

    void OnDisable()
    {
        picture = null;
    }
}

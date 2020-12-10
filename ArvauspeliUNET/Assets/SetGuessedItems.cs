using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetGuessedItems : MonoBehaviour 
{
    public GameController gameController;
    public Toggle bothToggle;
    public Toggle realToggle;
    public Toggle fictionalToggle;

    public void SetBoth ()
    {
        if (bothToggle.isOn)
        {
            Debug.Log("SetBoth Called");
            gameController.setBooleans(true, true);
        }        
    }
    
    public void SetReal()
    {
        if (realToggle.isOn)
        {
            Debug.Log("SetReal Called");
            gameController.setBooleans(false, true);
        }        
    }

    public void SetFictional()
    {
        if (fictionalToggle)
        {
            Debug.Log("SetFictional Called");
            gameController.setBooleans(true, false);
        }        
    }
}

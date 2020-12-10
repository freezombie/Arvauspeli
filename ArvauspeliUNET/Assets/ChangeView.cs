using UnityEngine;
using System.Collections;

public class ChangeView : MonoBehaviour 
{
	public void changeview (string aspect)
    {
        if (string.Compare(aspect,"Portrait")==0)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else if (string.Compare(aspect,"Landscape")==0)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            Debug.Log("Screen.orientation: " + Screen.orientation);
            Debug.Log("Input.deviceOrientation: " + Input.deviceOrientation);
        }
        else
        {
            Debug.Log("Wrong value entered into function ChangeView");
        }
    }
}

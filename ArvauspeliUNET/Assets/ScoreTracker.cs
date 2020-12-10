using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreTracker : MonoBehaviour 
{
    int right;
    int total;
    Text score;
    
    void Start()
    {
        right = 0;
        total = 0;
        score = GetComponent<Text>();
    }

    public void UpdateScore(string rightorwrong)
    {
        if (string.Compare(rightorwrong,"Right")==0)
        {
            right++;
            total++;
        }
        else if (string.Compare(rightorwrong,"Wrong")==0)
        {
            total++;
        }
        else
        {
            Debug.Log("Wrong input in editor for UpdateScore");
        }
        Debug.Log((double)right / (double)total);
        if(((double)right/(double)total)>=0.5f)
        {
            score.color = Color.green;
        }
        else
        {
            score.color = Color.red;
        }        
        score.text = right + "/" + total;
    }
}

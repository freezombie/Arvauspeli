using UnityEngine;
using System.Collections;
using System.Linq;

public class GameController : MonoBehaviour 
{
    public CharacterList myCharacterList;
    int random=999;
    int previousRandom=999;
    bool fiction = false;
    bool real = false;

    public void setBooleans (bool fictionalcharacters, bool realcharacters)
    {
        fiction = fictionalcharacters;
        real = realcharacters;
    }

    public void setNewRandom()
    {
        while (random == previousRandom)
        {
            random = Random.Range(0, myCharacterList.characterList.Count);
            if (fiction==false && real==true)
            {
                while ( myCharacterList.characterList[random].fiction==true || random == previousRandom)
                {
                    random = Random.Range(0, myCharacterList.characterList.Count);
                }
            }
            else if (fiction==true && real==false)
            {
                while (myCharacterList.characterList[random].fiction == false || random == previousRandom)
                {
                    random = Random.Range(0, myCharacterList.characterList.Count);
                }
            }            
        }
        previousRandom = random;
    }    

    public Sprite getImage()
    {
        return myCharacterList.characterList[random].image;
    }

    public string getDescription()
    {
        return myCharacterList.characterList[random].description;
    }

    public string getName()
    {
        return myCharacterList.characterList[random].name;
    }
}
using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    public string selectedCharacterName;

     private Dictionary<string, string> characterToGoalMap = new Dictionary<string, string>()
    {
        { "Bunny", "carrot" },
        { "Piglet", "apple" },
        { "Cow", "carrot" },
        { "Penguin", "fish" },
        { "Frog", "worm" },
        { "Elephant", "watermelon" },
        { "Crocodile", "fish" },
        { "Panda", "bamboo" },
        { "Axolotl", "worm" },
        { "Dog", "candy" },
        { "Cat", "fish" },
        { "Fox", "apple" },
        { "Chicken", "corn" },
        { "Bear", "honey" },
        { "Turtle", "carrot" },
        { "Monkey", "banana" },
        { "Mouse", "cheese" },
        { "Unicorn", "candy" },
        { "Mole", "worm" },
        { "Parrot", "carrot" }
    };
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetSelectedCharacter(GameObject characterPrefab) {
        selectedCharacterName = characterPrefab.name;
        Debug.Log("Selected character name: " + selectedCharacterName);
    }

    public string GetGoalNameForSelectedCharacter()
    {
        if (characterToGoalMap.TryGetValue(selectedCharacterName, out string goalName))
        {
            return goalName;
        }
        else
        {
            Debug.LogWarning("No goal mapped for character: " + selectedCharacterName);
            return null;
        }
    }
}
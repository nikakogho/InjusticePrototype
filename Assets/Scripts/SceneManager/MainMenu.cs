using UnityEngine;
using UnityEngine.SceneManagement;
using Injustice.CharacterBlueprints;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public static string accountName { get; private set; }

    public static Collection collection { get; private set; }

    public static CharacterBlueprint[] allBlueprints { get; private set; }

    void Awake()
    {
        instance = this;

        if(accountName == string.Empty)
        {
            Debug.LogError("No Account Found!");
            return;
        }
    }

    public static void LoadAccount(string accountName, CharacterBlueprint[] allBlueprints)
    {
        MainMenu.accountName = accountName;
        MainMenu.allBlueprints = allBlueprints;

        int characterAmount = PlayerPrefs.GetInt(accountName + "Character Amount", 0);

        Character[] characters = new Character[characterAmount];

        for(int i = 0; i < characterAmount; i++)
        {
            string name = PlayerPrefs.GetString("Character Name" + i);
            string label = PlayerPrefs.GetString("Character Label" + i);

            CharacterBlueprint chosenBlueprint = null;

            foreach(var blueprint in allBlueprints)
            {
                if(blueprint.name == name && blueprint.label == label)
                {
                    chosenBlueprint = blueprint;
                    break;
                }
            }

            if(chosenBlueprint == null)
            {
                Debug.LogError("No Character With Name " + name + " And Label " + label);
                return;
            }

            characters[i] = Character.Load(accountName, chosenBlueprint);
        }

        collection = new Collection(characters.ToList());
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

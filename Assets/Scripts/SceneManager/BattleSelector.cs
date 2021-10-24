using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BattleSelector : MonoBehaviour
{
    public string trainingSceneName = "trainZone";
    public string campaignSceneName = "campaignBattleSelector";

    public List<Injustice.CharacterBlueprints.CharacterBlueprint> possibleStartingCharacters;

    public Button trainingButton;
    public Button[] alreadyTrainedButtons;

    Collection collection;

    public int startCharacterAmount = 1;

    void Awake()
    {
        collection = Collection.instance;

        bool trainedAlready = collection.characters.Count > 0;

        trainingButton.interactable = !trainedAlready;

        foreach(var button in alreadyTrainedButtons)
        {
            button.interactable = trainedAlready;
        }
    }

    public void StartTraining()
    {
        var list = new List<Injustice.CharacterBlueprints.CharacterBlueprint>(possibleStartingCharacters);

        for (int i = 0; i < startCharacterAmount; i++)
        {
            int index = Random.Range(0, list.Count);

            collection.AddCharacter(list[index]);

            list.RemoveAt(index);
        }

        TrainingManager.Setup(collection.characters[0]);

        LoadScene(trainingSceneName);
    }

    public void Campaign()
    {
        LoadScene(campaignSceneName);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}

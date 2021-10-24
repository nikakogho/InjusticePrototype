using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public static GameStarter instance;

    public string menuSceneName = "gameMenu";

    public GameObject accountSlotPrefab;
    public Transform accountSlotParent;

    public Injustice.CharacterBlueprints.CharacterBlueprint[] allBlueprints;

    void Awake()
    {
        instance = this;
    }

    public void NewAccount(string name)
    {
        var clone = Instantiate(accountSlotPrefab, accountSlotParent.position, accountSlotParent.rotation, accountSlotParent);

        clone.GetComponent<AccountSlot>().nameText.text = name;
    }

    public void SelectAccount(string accountName)
    {
        MainMenu.LoadAccount(accountName, allBlueprints);
        SceneManager.LoadScene(menuSceneName);
    }
}

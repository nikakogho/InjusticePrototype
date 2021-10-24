using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TrainingManager : MonoBehaviour
{
    public GameObject[] possibleEnvironments;
    public CustomCharacter[] possibleOpponents;

    [Range(1, 10)]
    public float offsetX = 2;

    public string backToSceneName = "battleSelector";

    public Text trainingText;

    public string[] trainingTexts = new string[size - 1];

    public static TrainingManager instance;

    TrainingAI enemy;
    TrainingPlayer player;

    GameObject environmentPrefab;

    Character aiCharacter;
    static Character playerCharacter;

    #region Spawn

    void Awake()
    {
        instance = this;

        environmentPrefab = possibleEnvironments[Random.Range(0, possibleEnvironments.Length)];

        Instantiate(environmentPrefab, transform.position, transform.rotation);

        aiCharacter = possibleOpponents[Random.Range(0, possibleOpponents.Length)].ToCharacter();

        GameObject playerClone = SpawnPlayer();
        GameObject enemyClone = SpawnEnemy();

        player = playerClone.AddComponent<TrainingPlayer>();
        enemy = enemyClone.AddComponent<TrainingAI>();

        player.Init(playerCharacter, enemy);
        enemy.Init(aiCharacter, player);

        Invoke("ApplyDelegates", 0.3f);

        playerClone.SetActive(true);
        enemyClone.SetActive(true);

        playerCharacter.blueprint.prefab.SetActive(true);
        aiCharacter.blueprint.prefab.SetActive(true);
    }

    GameObject SpawnPlayer()
    {
        var prefab = playerCharacter.blueprint.prefab;

        prefab.SetActive(false);

        return Instantiate(prefab, transform.position - Vector3.forward * offsetX, Quaternion.Euler(0, 0, 0));
    }

    GameObject SpawnEnemy()
    {
        var prefab = aiCharacter.blueprint.prefab;

        prefab.SetActive(false);

        return Instantiate(prefab, transform.position + Vector3.forward * offsetX, Quaternion.Euler(0, 180, 0));
    }

    #endregion

    const int size = 5;

    enum TrainingState { None = 0, NormalAttack, HeavyAttack, Block, Special1, Special3 }
    TrainingState trainingState = TrainingState.None;

    bool trained = true;

    void ApplyDelegates()
    {
        player.onNormalAttack += (streak) =>
        {
            if (trainingState == TrainingState.NormalAttack) trained = true;
        };

        player.onHeavyAttack += (streak) =>
        {
            if (trainingState == TrainingState.HeavyAttack) trained = true;
        };

        player.onBlock += (damage, attackType) =>
        {
            if (trainingState == TrainingState.Block) trained = true;
        };

        player.onSpecialFirst += () =>
        {
            if (trainingState == TrainingState.Special1) trained = true;
        };

        player.onSpecialThird += () =>
        {
            if (trainingState == TrainingState.Special3) trained = true;
        };
    }

    void Update()
    {
        if (trained)
        {
            trainingState = (TrainingState)(((int)trainingState + 1) % size);

            if(trainingState == 0)
            {
                CompleteTraining();
                return;
            }

            trainingText.text = trainingTexts[(int)trainingState - 1];

            trained = false;
        }
    }

    void CompleteTraining()
    {
        StartCoroutine(CompleteTrainingRoutine());
    }

    IEnumerator CompleteTrainingRoutine()
    {
        trainingText.text = "Training Complete!";

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(backToSceneName);
    }

    public static void Setup(Character player)
    {
        playerCharacter = player;
    }
}

using UnityEngine;

public class BattleManager : MonoBehaviour
{
    static Character characterPlayer;
    static Character characterEnemy;

    static GameObject environmentPrefab;

    RealPlayer player;
    AIPlayer enemy;

    public static BattleManager instance;

    [Range(0, 10)]
    public float offsetX = 2;

    void Awake()
    {
        instance = this;

        Instantiate(environmentPrefab, transform.position, transform.rotation);

        GameObject playerClone = SpawnPlayer();
        GameObject enemyClone = SpawnEnemy();

        player = playerClone.AddComponent<RealPlayer>();
        enemy = enemyClone.AddComponent<AIPlayer>();

        player.Init(characterPlayer, enemy);
        enemy.Init(characterEnemy, player);
    }

    GameObject SpawnPlayer()
    {
        return Instantiate(characterPlayer.blueprint.prefab, transform.position - Vector3.forward * offsetX, Quaternion.Euler(0, 0, 0));
    }

    GameObject SpawnEnemy()
    {
        return Instantiate(characterEnemy.blueprint.prefab, transform.position + Vector3.forward * offsetX, Quaternion.Euler(0, 180, 0));
    }

    public static void ApplyBattle(Character player, Character enemy, GameObject environment)
    {
        characterPlayer = player;
        characterEnemy = enemy;
        environmentPrefab = environment;
    }
}

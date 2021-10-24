using UnityEngine;
using Injustice.CharacterBlueprints;

public class Character
{
    public static int[] XPToGrow =
    {
        100, 200, 300, 400, 500, 700, 1000, 1400, 2000, 3000, 4000, 5000, 7000, 10000, 14000, 17000, 20000, 30000, 40000, 50000,
        60000, 70000, 80000, 90000, 100000, 110000, 125000, 130000, 150000, 170000, 200000, 200000, 200000, 200000, 2000000, 200000
    };

    public CharacterBlueprint blueprint;

    public int level = 1;
    public int xp = 0;

    public float multiplyHealthOnUpgradeBy = 1.1f;
    public float multiplyDamageOnUpgradeBy = 1.1f;

    public float blockComboPercentage = 0.4f;
    public float blockSpecialPercentage = 0.3f;
    public float blockPowerDraingPercentage = 0.45f;

    public int startHealth, damage;

    public Character(CharacterBlueprint blueprint)
    {
        this.blueprint = blueprint;

        level = 1;
        xp = 0;

        startHealth = blueprint.FirstLevelHealth;
        damage = blueprint.FirstLevelDamage;
    }

    public Character(CharacterBlueprint blueprint, int startHealth, int damage, int level, int xp, float multiplyHealthOnUpgradeBy, float multiplyDamageOnUpgradeBy, float blockComboPercentage = 0.4f, float blockSpecialPercentage = 0.3f, float blockPowerDraingPercentage = 0.45f)
    {
        this.blueprint = blueprint;
        this.startHealth = startHealth;
        this.damage = damage;
        this.level = level;
        this.xp = xp;
        this.multiplyHealthOnUpgradeBy = multiplyHealthOnUpgradeBy;
        this.multiplyDamageOnUpgradeBy = multiplyDamageOnUpgradeBy;
        this.blockComboPercentage = blockComboPercentage;
        this.blockSpecialPercentage = blockSpecialPercentage;
        this.blockPowerDraingPercentage = blockPowerDraingPercentage;
    }

    public static void Save(string accountName, Character character)
    {
        string preName = accountName + character.blueprint.name + character.blueprint.label;

        PlayerPrefs.SetInt(preName + "health", character.startHealth);
        PlayerPrefs.SetInt(preName + "damage", character.damage);
        PlayerPrefs.SetInt(preName + "level", character.level);
        PlayerPrefs.SetInt(preName + "xp", character.xp);
        PlayerPrefs.SetFloat(preName + "multiply health on upgrade by", character.multiplyHealthOnUpgradeBy);
        PlayerPrefs.SetFloat(preName + "multiply damage on upgrade by", character.multiplyDamageOnUpgradeBy);
    }

    public static Character Load(string accountName, CharacterBlueprint blueprint)
    {
        if(blueprint == null)
        {
            Debug.LogError("Blueprint must not be null!");
            return null;
        }

        string preName = accountName + blueprint.name + blueprint.label;

        int startHealth = PlayerPrefs.GetInt(preName + "health", blueprint.FirstLevelHealth);
        int damage = PlayerPrefs.GetInt(preName + "damage", blueprint.FirstLevelDamage);
        int level = PlayerPrefs.GetInt(preName + "level", 1);
        int xp = PlayerPrefs.GetInt(preName + "xp", 0);
        float multiplyHealthOnUpgradeBy = PlayerPrefs.GetFloat(preName + "multiply health on upgrade by", 1.1f);
        float multiplyDamageOnUpgradeBy = PlayerPrefs.GetFloat(preName + "multiply damage on upgrade by", 1.1f);

        return new Character(blueprint, startHealth, damage, level, xp, multiplyHealthOnUpgradeBy, multiplyDamageOnUpgradeBy);
    }

    public void Save(string accountName)
    {
        Save(accountName, this);
    }
}

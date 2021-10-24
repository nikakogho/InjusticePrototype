using UnityEngine;

[System.Serializable]
public class CustomCharacter
{
    public Injustice.CharacterBlueprints.CharacterBlueprint blueprint;
    
    [Range(0, 1)]
    public float blockComboPercentage = 0.4f, blockSpecialPercentage = 0.3f, blockPowerDraingPercentage = 0.45f;

    public int startHealth, damage;

    [Range(1, 100)]
    public int level;

    public Character ToCharacter()
    {
        return new Character(blueprint, startHealth, damage, level, 0, 1, 1, blockComboPercentage, blockSpecialPercentage, blockPowerDraingPercentage);
    }
}

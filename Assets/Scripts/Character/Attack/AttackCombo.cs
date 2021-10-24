using UnityEngine;

[CreateAssetMenu(fileName = "New Combo", menuName = "Character/Attack/Combo")]
public class AttackCombo : ScriptableObject
{
    public AttackPart[] combo;
    public AttackPart ending;

    public int FirstPartSize { get { return combo.Length; } }
    public int FullSize { get { return FirstPartSize + 1; } }

    public AttackPart GetPart(int streak)
    {
        if (streak > FirstPartSize || streak < 0) return null;

        if (streak == FirstPartSize) return ending;

        return combo[streak];
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "New Special Attack", menuName = "Character/Attack/Special")]
public class SpecialAttack : ScriptableObject
{
    new public string name;

    [TextArea(3, 10)]
    public string description;

    public AttackPart[] parts;

    public int MinDamage(int damage)
    {
        float minPercentage = 0;

        foreach(var part in parts)
        {
            minPercentage += part.damagePercentage;
        }

        return (int)(damage * minPercentage / 100);
    }

    public int MaxDamage(int damage)
    {
        return MinDamage(damage) * 2;
    }
    
    public bool unblockable
    {
        get
        {
            foreach(var part in parts)
            {
                if (part.unblockable) return true;
            }

            return false;
        }
    }
}

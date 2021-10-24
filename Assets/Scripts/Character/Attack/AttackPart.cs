using UnityEngine;

[System.Serializable]
public class AttackPart
{
    public enum AttackType
    {
        Normal, PowerDrain, LifeStealBasedOnOpponentHealth, LifeStealBasedOnPlayerHealth, Stun,
        LifeStealBasedOnOpponentDamage, LifeStealBasedOnPlayerDamage, Freeze, Poison, Bleeding, Burning
    }

    public enum HitType { Normal, FallDown, FallAway, LookDown, LookUp, LookLeft, LookRight }

    public AnimationClip anim;

    public AudioClip audio;
    public float audioWait;
    
    public float waitTime;
    
    public float attackAfter;
    
    public bool unblockable;

    public AttackType attackType;
    public HitType hitType;
    
    public float damagePercentage;
    
    [HideInInspector]public float powerDrainBy = 0;
    [HideInInspector]public float lifeStealPercentage = 0;
    [HideInInspector]public float freezeDuration = 0;
    [HideInInspector]public float bleedDuration = 0, bleedByEachBleedPercentage = 0, bleedDelta = 0;
    
    void OnValidate()
    {
        if(attackAfter > waitTime)
        {
            attackAfter = waitTime;
        }
    }

    public void Attack(Player attacker, Player opponent, float percentage)
    {
        float playerDamage = attacker.character.damage * (percentage / 100);

        int damage = (int)(damagePercentage * playerDamage);

        opponent.TakeDamage(damage, unblockable);

        bool lifeSteal = false;
        float totalLifeToSteal = 0;

        #region Switch

        switch (attackType)
        {
            case AttackType.Normal:

                break;

            case AttackType.PowerDrain:

                opponent.DrainPower(powerDrainBy);

                break;

            case AttackType.Bleeding:
            case AttackType.Burning:
            case AttackType.Poison:

                int bleedDamage = (int)(bleedByEachBleedPercentage * playerDamage);

                opponent.Bleed(bleedDamage, bleedDelta, bleedDuration, attackType);

                break;

            case AttackType.Freeze:
            case AttackType.Stun:

                opponent.Freeze(freezeDuration, attackType, unblockable);

                break;

            case AttackType.LifeStealBasedOnOpponentDamage:
                totalLifeToSteal = opponent.damage * lifeStealPercentage;
                lifeSteal = true;
                break;

            case AttackType.LifeStealBasedOnOpponentHealth:
                totalLifeToSteal = opponent.health * lifeStealPercentage;
                lifeSteal = true;
                break;

            case AttackType.LifeStealBasedOnPlayerDamage:
                totalLifeToSteal = attacker.damage * lifeStealPercentage;
                lifeSteal = true;
                break;

            case AttackType.LifeStealBasedOnPlayerHealth:
                totalLifeToSteal = attacker.health * lifeStealPercentage;
                lifeSteal = true;
                break;
        }

        #endregion

        if (lifeSteal)
        {
            totalLifeToSteal = Mathf.Clamp(totalLifeToSteal, 0, Mathf.Min(opponent.health, attacker.character.startHealth - attacker.health));

            int actualLifeToSteal = (int)totalLifeToSteal;

            if(totalLifeToSteal > 0)
            {
                opponent.TakeDamage(actualLifeToSteal, true);
                attacker.GainHealth(actualLifeToSteal);
            }
        }
    }
}

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AttackPart))]
public class AttackPartEditor : PropertyDrawer
{
    const int minFieldAmount = 10;
    static int fieldAmount = minFieldAmount;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        fieldAmount = minFieldAmount;

        var attackType = (AttackPart.AttackType)property.FindPropertyRelative("attackType").enumValueIndex;

        switch (attackType)
        {
            case AttackPart.AttackType.Normal:
                break;

            case AttackPart.AttackType.PowerDrain:
                fieldAmount++;
                break;

            case AttackPart.AttackType.LifeStealBasedOnPlayerDamage:
            case AttackPart.AttackType.LifeStealBasedOnOpponentDamage:
            case AttackPart.AttackType.LifeStealBasedOnPlayerHealth:
                fieldAmount++;
                break;

            case AttackPart.AttackType.Bleeding:
            case AttackPart.AttackType.Burning:
            case AttackPart.AttackType.Poison:

                fieldAmount += 3;

                break;

            case AttackPart.AttackType.Freeze:
            case AttackPart.AttackType.Stun:

                fieldAmount++;

                break;
        }

        return base.GetPropertyHeight(property, label) * fieldAmount + 20;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PrefixLabel(position, label);

        position.width -= 20;
        position.x += 20;
        position.y += 16;
        
        position.height = 16;

        float height = position.height;

        EditorGUI.ObjectField(position, property.FindPropertyRelative("anim"), typeof(AnimationClip));

        position.y += height;

        EditorGUI.Slider(position, property.FindPropertyRelative("waitTime"), 0, 20);

        position.y += height;

        EditorGUI.ObjectField(position, property.FindPropertyRelative("audio"), typeof(AudioClip));

        position.y += height;

        EditorGUI.Slider(position, property.FindPropertyRelative("audioWait"), 0, 20, "Audio Wait");

        position.y += height;

        EditorGUI.Slider(position, property.FindPropertyRelative("attackAfter"), 0, 20, "Attack After");

        position.y += height;

        var unblockableProp = property.FindPropertyRelative("unblockable");

        unblockableProp.boolValue = EditorGUI.Toggle(position, "Unblockable", unblockableProp.boolValue);

        position.y += height;

        var attackTypeProp = property.FindPropertyRelative("attackType");

        attackTypeProp.enumValueIndex = (int)(AttackPart.AttackType)EditorGUI.EnumPopup(position, "Attack Type", (AttackPart.AttackType)attackTypeProp.enumValueIndex);

        position.y += height;

        var hitTypeProp = property.FindPropertyRelative("hitType");

        hitTypeProp.enumValueIndex = (int)(AttackPart.HitType)EditorGUI.EnumPopup(position, "Hit Type", (AttackPart.HitType)hitTypeProp.enumValueIndex);

        position.y += height;

        EditorGUI.Slider(position, property.FindPropertyRelative("damagePercentage"), 0, 100, "Damage Percentage");

        position.y += height;

        float lifeStealMin = 0;
        float lifeStealMax = 0;

        var attackType = (AttackPart.AttackType)attackTypeProp.enumValueIndex;

        switch (attackType)
        {
            case AttackPart.AttackType.Normal:
                break;

            case AttackPart.AttackType.PowerDrain:
                EditorGUI.Slider(position, property.FindPropertyRelative("powerDrainBy"), 0, 3, "Power Drain By");

                position.y += height;
                break;

            case AttackPart.AttackType.LifeStealBasedOnPlayerDamage:
            case AttackPart.AttackType.LifeStealBasedOnOpponentDamage:
                lifeStealMin = 0;
                lifeStealMax = 80;
                break;

            case AttackPart.AttackType.LifeStealBasedOnOpponentHealth:
                lifeStealMin = 0;
                lifeStealMax = 100;
                break;

            case AttackPart.AttackType.LifeStealBasedOnPlayerHealth:
                lifeStealMin = 0;
                lifeStealMax = 300;
                break;

            case AttackPart.AttackType.Bleeding:
            case AttackPart.AttackType.Burning:
            case AttackPart.AttackType.Poison:

                EditorGUI.Slider(position, property.FindPropertyRelative("bleedByEachBleedPercentage"), 0, 100, "Bleed By Each Bleed Percentage");

                position.y += height;

                EditorGUI.Slider(position, property.FindPropertyRelative("bleedDelta"), 0.1f, 3, "Bleed Delta");

                position.y += height;

                EditorGUI.Slider(position, property.FindPropertyRelative("bleedDuration"), 1, 8, "Bleed Duration");

                position.y += height;

                break;

            case AttackPart.AttackType.Freeze:
            case AttackPart.AttackType.Stun:

                EditorGUI.Slider(position, property.FindPropertyRelative("freezeDuration"), 0, 8, "Freeze Duration");

                position.y += height;

                break;
        }

        if (lifeStealMax > 0)
        {
            EditorGUI.Slider(position, property.FindPropertyRelative("lifeStealPercentage"), lifeStealMin, lifeStealMax, "Life Steal Percentage");
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

namespace Injustice.CharacterBlueprints
{
    public enum Category { Bronze, Silver, Gold }
    public enum Sex { Male, Female, None }

    public abstract class CharacterBlueprint : ScriptableObject
    {
        new public abstract string name { get; }
        public abstract string skillDescription { get; }
        public abstract string label { get; }

        public abstract Sex sex { get; }
        public abstract Category category { get; }

        public abstract int FirstLevelHealth { get; }
        public abstract int FirstLevelDamage { get; }

        public abstract AttackCombo NormalCombo { get; }
        public abstract AttackCombo HeavyCombo { get; }

        public abstract SpecialAttack SpecialFirst { get; }
        public abstract SpecialAttack SpecialSecond { get; }
        public abstract SpecialAttack SpecialThird { get; }

        public abstract GameObject prefab { get; }
        public abstract Sprite icon { get; }

        public abstract AnimationClip idleClip { get; }
        public abstract AnimationClip blockClip { get; }

        public abstract AnimationClip[] GetHitClips { get; }

        public abstract SkillEvent skill { get; }
    }
}

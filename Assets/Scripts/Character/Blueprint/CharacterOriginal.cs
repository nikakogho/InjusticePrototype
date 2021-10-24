using UnityEngine;

namespace Injustice.CharacterBlueprints
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Character/Blueprint/Original")]
    public sealed class CharacterOriginal : CharacterBlueprint
    {
        #region Fields

        [SerializeField]
        string _name;

        [TextArea(3, 10)]
        [SerializeField]
        string _skillDescription;

        [SerializeField]
        Sex _sex;

        [SerializeField]
        Category _category;

        [SerializeField]
        int _damage;

        [SerializeField]
        int _health;

        [SerializeField]
        AttackCombo _normalCombo;

        [SerializeField]
        AttackCombo _heavyCombo;

        [SerializeField]
        SpecialAttack _specialFirst;

        [SerializeField]
        SpecialAttack _specialSecond;

        [SerializeField]
        SpecialAttack _specialThird;

        [SerializeField]
        GameObject _prefab;

        [SerializeField]
        Sprite _icon;

        [SerializeField]
        AnimationClip _idleClip;

        [SerializeField]
        AnimationClip _blockClip;

        [SerializeField]
        AnimationClip[] getHitClips = new AnimationClip[System.Enum.GetNames(typeof(AttackPart.HitType)).Length];

        [SerializeField]
        SkillEvent _skill;

        #endregion

        #region Overrides

        public override string name { get { return _name; } }
        public override string skillDescription { get { return _skillDescription; } }
        public override string label { get { return string.Empty; } }

        public override Category category { get { return _category; } }
        public override Sex sex { get { return _sex; } }

        public override int FirstLevelDamage { get { return _damage; } }
        public override int FirstLevelHealth { get { return _health; } }

        public override AttackCombo NormalCombo { get { return _normalCombo; } }
        public override AttackCombo HeavyCombo { get { return _heavyCombo; } }

        public override SpecialAttack SpecialFirst { get { return _specialFirst; } }
        public override SpecialAttack SpecialSecond { get { return _specialSecond; } }
        public override SpecialAttack SpecialThird { get { return _specialThird; } }

        public override GameObject prefab { get { return _prefab; } }
        public override Sprite icon { get { return _icon; } }

        public override AnimationClip idleClip { get { return _idleClip; } }
        public override AnimationClip blockClip { get { return _blockClip; } }

        public override AnimationClip[] GetHitClips { get { return getHitClips; } }

        public override SkillEvent skill { get { return _skill; } }

        #endregion
    }
}

using UnityEngine;

namespace Injustice.CharacterBlueprints
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Character/Blueprint/Override")]
    public sealed class CharacterOverride : CharacterBlueprint
    {
        #region Fields

        [SerializeField]
        CharacterOriginal _original;

        [TextArea(3, 10)]
        [SerializeField]
        string _skillDescription;

        [SerializeField]
        string _label;

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

        public override string name { get { return _original.name; } }
        public override string skillDescription { get { return _skillDescription; } }
        public override string label { get { return _label; } }

        public override Category category { get { return _category; } }
        public override Sex sex { get { return _original.sex; } }

        public override int FirstLevelHealth { get { return _health; } }
        public override int FirstLevelDamage { get { return _damage; } }

        public override AttackCombo NormalCombo { get { return _normalCombo; } }
        public override AttackCombo HeavyCombo { get { return _heavyCombo; } }

        public override SpecialAttack SpecialFirst { get { return _specialFirst; } }
        public override SpecialAttack SpecialSecond { get { return _specialSecond; } }
        public override SpecialAttack SpecialThird { get { return _original.SpecialThird; } }

        public override GameObject prefab { get { return _prefab; } }
        public override Sprite icon { get { return _icon; } }

        public override AnimationClip idleClip { get { return _idleClip; } }
        public override AnimationClip blockClip { get { return _blockClip; } }

        public override AnimationClip[] GetHitClips { get { return getHitClips; } }

        public override SkillEvent skill { get { return _skill; } }

        #endregion

        void OnValidate()
        {
            if (_health == 0) _health = _original.FirstLevelHealth;
            if (_damage == 0) _damage = _original.FirstLevelDamage;

            if (_normalCombo == null) _normalCombo = _original.NormalCombo;
            if (_heavyCombo == null) _heavyCombo = _original.HeavyCombo;

            if (_specialFirst == null) _specialFirst = _original.SpecialFirst;
            if (_specialSecond == null) _specialSecond = _original.SpecialSecond;

            if (_idleClip == null) _idleClip = _original.idleClip;
            if (_blockClip == null) _blockClip = _original.blockClip;
        }
    }
}
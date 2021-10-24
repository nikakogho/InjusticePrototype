using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(AudioSource))]
public abstract class Player : MonoBehaviour
{
    public Character character { get; private set; }

    #region Delegates

    public delegate void OnTakeDamage(int damage);
    public OnTakeDamage onTakeDamage;

    public delegate void OnDie(int whichTime);
    public OnDie onDie;

    public delegate void OnPowerDrain(float amount);
    public OnPowerDrain onPowerDrain;

    public delegate void Action();
    public Action onAwake, onStart;

    public delegate void OnComboAttack(int streak);
    public OnComboAttack onNormalAttack, onHeavyAttack;

    public delegate void OnBlock(int damage, AttackPart.AttackType attackType);
    public OnBlock onBlock;

    public delegate void OnSpecialAttack();

    public OnSpecialAttack onSpecialFirst, onSpecialSecond, onSpecialThird;

    #endregion

    #region Callbacks

    void Awake()
    {
        health = character.startHealth;
        damage = character.damage;

        anim = GetComponent<Animation>();
        source = GetComponent<AudioSource>();

        AwakeStuff();

        if (onAwake != null) onAwake.Invoke();
    }

    void Start()
    {
        StartStuff();

        if (onStart != null) onStart.Invoke();
    }

    void Update()
    {
        UpdateStuff();
    }

    void FixedUpdate()
    {
        FixedUpdateStuff();
    }

    #endregion

    #region Callback Stuff

    protected virtual void AwakeStuff()
    {
        onTakeDamage += DefaultOnTakeDamage;
        onDie += DefaultOnDie;
        onPowerDrain += DefaultOnPowerDrain;
        onNormalAttack += DefaultOnComboAttack;
        onHeavyAttack += DefaultOnComboAttack;

        onSpecialFirst += DefaultOnSpecialAttack;
        onSpecialSecond += DefaultOnSpecialAttack;
        onSpecialThird += DefaultOnSpecialAttack;

        character.blueprint.skill.Invoke(this);
    }

    protected virtual void StartStuff()
    {

    }

    protected virtual void UpdateStuff()
    {

    }

    protected virtual void FixedUpdateStuff()
    {
        
    }

    #endregion

    #region Properties

    Animation anim;
    AudioSource source;

    protected int deathTime = 0;

    protected int startHealth, startDamage;

    public int health { get; private set; }
    public int damage { get; private set; }

    public float HealthPercentage { get { return (float)health / character.startHealth * 100; } }

    protected Player opponent;

    public float power { get; private set; }

    public bool blocking = false;

    #endregion

    public virtual void Init(Character character, Player opponent)
    {
        this.character = character;
        this.opponent = opponent;
    }

    #region Default Delegates

    public void DefaultOnTakeDamage(int damage)
    {
        health -= damage;
    }

    public void DefaultOnDie(int whichTime)
    {
        //actually die

        gameObject.SetActive(false);
    }

    public void DefaultOnPowerDrain(float amount)
    {
        power -= amount;
    }

    public void DefaultOnComboAttack(int streak)
    {

    }

    public void DefaultOnSpecialAttack()
    {

    }

    public void DefaultOnBlock(bool isNormal, int damage, AttackPart.AttackType attackType)
    {
        health -= (int)(damage * (1 - character.blockComboPercentage));
    }

    #endregion

    #region Get Attaccked Routines

    IEnumerator BleedRoutine(int damage, float delta, float duration)
    {
        do
        {
            yield return new WaitForSeconds(delta);

            health -= damage;
            duration -= delta;

        } while (duration > 0);
    }

    IEnumerator FreezeRoutine(float duration)
    {
        //freeze

        yield return new WaitForSeconds(duration);

        //unfreeze
    }

    #endregion

    #region Public Delegate Calls

    public void TakeDamage(int damage, bool unblockable)
    {
        if (blocking && !unblockable)
            onBlock.Invoke(damage, AttackPart.AttackType.Normal);
        else
            onTakeDamage.Invoke(damage);

        if (health <= 0)
        {
            health = 0;

            onDie.Invoke(deathTime);

            deathTime++;
        }
    }

    public void DrainPower(float amount)
    {
        onPowerDrain.Invoke(amount);
    }
    
    public void Bleed(int damage, float delta, float duration, AttackPart.AttackType bleedType)
    {
        StartCoroutine(BleedRoutine(damage, delta, duration));
    }

    public void Freeze(float duration, AttackPart.AttackType freezeType, bool unblockable)
    {
        if(blocking && !unblockable)
        StartCoroutine(FreezeRoutine(duration));
    }

    public void GainHealth(int value)
    {
        health += value;

        if(health > character.startHealth)
        {
            health = character.startHealth;
        }
    }

    #endregion

    #region Attack

    #region Part

    IEnumerator AttackRoutine(AttackPart part)
    {
        yield return new WaitForSeconds(part.attackAfter);

        part.Attack(this, opponent, 100);
    }

    IEnumerator AudioRoutine(AttackPart part)
    {
        yield return new WaitForSeconds(part.audioWait);

        source.clip = part.audio;
        source.Play();
    }

    void UseAttackPart(AttackPart part)
    {
        StartCoroutine(WaitForAttackPartToFinish(part));

        if(anim.GetClip(part.anim.name) == null)
        anim.AddClip(part.anim, part.anim.name);

        anim.clip = part.anim;
        anim.Play();

        StartCoroutine(AttackRoutine(part));
        StartCoroutine(AudioRoutine(part));
    }

    #endregion

    #region Combo

    enum ComboMode { None, Normal, Heavy }
    ComboMode comboMode = ComboMode.None;

    int comboStreak = 0;
    bool streakOver = true;

    IEnumerator StopCombo(AttackPart part)
    {
        yield return new WaitForSeconds(anim.clip.length);

        anim.CrossFade(character.blueprint.idleClip.name, 2);

        yield return new WaitForSeconds(part.waitTime);

        comboMode = ComboMode.None;
        comboStreak = 0;

        stackedHit = StackedHit.None;
        stackedAmount = 0;
    }

    protected bool waitingToFinishAttackPart = false;

    IEnumerator WaitForAttackPartToFinish(AttackPart part)
    {
        yield return new WaitForSeconds(part.anim.length);

        waitingToFinishAttackPart = false;

        if(stackedAmount > 0)
        {
            switch (stackedHit)
            {
                case StackedHit.Normal:
                    NormalHit();
                    break;

                case StackedHit.Heavy:
                    HeavyHit();
                    break;

                default:
                    Debug.Log("Stacked Amount Should Be 0 If There Is No stackedHit!");
                    break;
            }

            stackedAmount--;

            if(stackedAmount == 0)
            {
                stackedHit = StackedHit.None;
            }
        }
    }
    
    void ActualNormalHit()
    {
        var combo = character.blueprint.NormalCombo;

        onNormalAttack.Invoke(comboStreak);

        if (comboStreak == combo.FirstPartSize)
        {
            UseAttackPart(combo.ending);

            streakOver = true;
        }
        else
        {
            UseAttackPart(combo.combo[comboStreak]);
        }
    }

    void ActualHeavyHit()
    {
        var combo = character.blueprint.HeavyCombo;

        onHeavyAttack.Invoke(comboStreak);

        if (comboStreak == combo.FirstPartSize)
        {
            UseAttackPart(combo.ending);

            streakOver = true;
        }
        else
        {
            UseAttackPart(combo.combo[comboStreak]);
        }
    }

    Coroutine stopCombo = null;

    enum StackedHit { None, Normal, Heavy }
    StackedHit stackedHit = StackedHit.None;
    int stackedAmount = 0;

    protected void NormalHit()
    {
        if (waitingToFinishAttackPart)
        {
            if (comboMode != ComboMode.Normal)
            {
                stackedHit = StackedHit.None;
                stackedAmount = 0;
            }
            else
            {
                stackedHit = StackedHit.Normal;

                stackedAmount = Mathf.Clamp(stackedAmount + 1, 0, character.blueprint.NormalCombo.FirstPartSize - comboStreak);
            }

            return;
        }

        waitingToFinishAttackPart = true;

        streakOver = false;

        ActualNormalHit();

        if (comboMode == ComboMode.Normal)
        {
            if (streakOver)
            {
                comboStreak = 0;
                comboMode = ComboMode.None;
            }
            else
            {
                comboStreak++;
            }

            if(stackedAmount == 0)
            {
                StopCoroutine(stopCombo);

                stopCombo = StartCoroutine(StopCombo(character.blueprint.NormalCombo.GetPart(comboStreak)));
            }
        }
        else
        {
            if (comboMode == ComboMode.Heavy) StopCoroutine(stopCombo);

            comboMode = ComboMode.Normal;
            comboStreak = 1;

            stopCombo = StartCoroutine(StopCombo(character.blueprint.NormalCombo.combo[0]));
        }
    }

    protected void HeavyHit()
    {
        if (waitingToFinishAttackPart)
        {
            if (comboMode != ComboMode.Heavy)
            {
                stackedHit = StackedHit.None;
                stackedAmount = 0;
            }
            else
            {
                stackedHit = StackedHit.Heavy;

                stackedAmount = Mathf.Clamp(stackedAmount + 1, 0, character.blueprint.HeavyCombo.FirstPartSize - comboStreak);
            }

            return;
        }

        streakOver = false;

        ActualHeavyHit();

        if (comboMode == ComboMode.Heavy)
        {
            if (streakOver)
            {
                comboStreak = 0;
                comboMode = ComboMode.None;
            }
            else
            {
                comboStreak++;
            }

            if (stackedAmount == 0)
            {
                StopCoroutine(stopCombo);

                stopCombo = StartCoroutine(StopCombo(character.blueprint.HeavyCombo.GetPart(comboStreak)));
            }
        }
        else
        {
            StopCoroutine(stopCombo);

            comboMode = ComboMode.Heavy;
            comboStreak = 1;

            stopCombo = StartCoroutine(StopCombo(character.blueprint.HeavyCombo.combo[0]));
        }
    }

    #endregion

    #region Special

    IEnumerator UnleashSpecialAttack(SpecialAttack attack)
    {
        foreach(var part in attack.parts)
        {
            UseAttackPart(part);

            yield return new WaitForSeconds(part.waitTime);
        }
    }

    protected void SpecialAttack(int index)
    {
        if (waitingToFinishAttackPart) return;

        if (index < 1 || index > 3)
        {
            Debug.LogError("There Is No Special Attack Number " + index + " !!!");
            return;
        }

        if (power < index) return;

        comboMode = ComboMode.None;
        comboStreak = 0;

        StopCoroutine("StopCombo");

        SpecialAttack attack = null;

        switch (index)
        {
            case 1:
                attack = character.blueprint.SpecialFirst;

                onSpecialFirst.Invoke();
                break;
            case 2:
                attack = character.blueprint.SpecialSecond;

                onSpecialSecond.Invoke();
                break;
            case 3:
                attack = character.blueprint.SpecialThird;

                onSpecialThird.Invoke();
                break;
        }

        power -= index;

        StartCoroutine(UnleashSpecialAttack(attack));
    }

    #endregion

    #endregion

    #region Set Stats

    public void SetStat(string statName, int value)
    {
        switch (statName)
        {
            case "health":
                health = value;
                break;

            case "damage":
                damage = value;
                break;
        }
    }

    public void SetStat(string statName, float value)
    {
        switch (statName)
        {
            case "power":
                power = value;
                break;
        }
    }

    #endregion

    public void ToggleBlocking()
    {
        blocking = !blocking;

        anim.clip = blocking ? character.blueprint.blockClip : character.blueprint.idleClip;
        anim.Play();
    }
}

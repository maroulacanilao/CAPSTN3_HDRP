using System.Collections;
using System.Collections.Generic;
using BattleSystem.BattleState;
using CustomEvent;
using UnityEngine;
using Character;
using Character.CharacterComponents;
using CustomHelpers;
using Managers;
using NaughtyAttributes;
using ObjectPool;
using UnityEngine.VFX;

public abstract class StatusEffectBase : MonoBehaviour
{
    [field: BoxGroup("Base Properties")]
    [field: SerializeField] public int ID { get; private set; }

    [field: BoxGroup("Base Properties")]
    [field: SerializeField] public string EffectName { get; private set; }
    
    [field: BoxGroup("Base Properties")] [field: ResizableTextArea]
    [field: SerializeField] public string Description { get; private set; }
    
    [field: BoxGroup("Base Properties")] [field: ResizableTextArea]
    [field: SerializeField] public string BattleText { get; private set; }
    
    [field: BoxGroup("Base Properties")]
    [field: SerializeField] public bool IsStackable { get; private set; }
    
    [field: BoxGroup("Base Properties")]
    [field: SerializeField] public bool HasDamage { get; private set; }
    
    [field: BoxGroup("Base Properties")] [field: ShowIf("HasDamage")]
    [field: SerializeField] public int Damage { get; private set; }
    
    [field: BoxGroup("Base Properties")]
    [field: SerializeField] public int executionOrder { get; private set; }
    
    [field: BoxGroup("Base Properties")]
    [field: SerializeField] public bool HasDuration { get; private set; }

    [field: BoxGroup("Base Properties")] [field: ShowIf("HasDuration")]
    [field: SerializeField] public int turnDuration { get; private set; }
    
    [field: BoxGroup("Base Properties")] [field: ShowAssetPreview()]
    [field: SerializeField] public Sprite Icon { get; private set; }
    
    [field: BoxGroup("Base Properties")]
    [field: SerializeField] public List<string> effectTags { get; private set; }

    public StatusEffectReceiver Target { get; private set; }
    public GameObject Source { get; protected set; }
    public int turnsLeft { get; protected set; }

    protected bool isActivated;
    
    protected string characterName => Target.character.characterData.characterName;

    public readonly Evt<StatusEffectBase, StatusEffectReceiver> OnEffectEnd = new Evt<StatusEffectBase, StatusEffectReceiver>();
    public readonly Evt<int> OnTurnsLeftChange = new Evt<int>();

    protected abstract IEnumerator OnActivate();
    protected abstract void OnDeactivate();
    
    protected virtual void OnStackEffect(StatusEffectBase newEffect_) { }
    
    protected virtual IEnumerator OnBeforeTurnTick(TurnBaseState ownerTurnState_) { yield break; }

    protected virtual IEnumerator OnAfterTurnTick(TurnBaseState ownerTurnState_) { yield break; }

    public IEnumerator Activate(StatusEffectReceiver target, GameObject source = null)
    {
        if (isActivated) yield break;
        Target = target;
        Source = source;
        isActivated = true;
        if (HasDuration) turnsLeft = turnDuration;
        yield return OnActivate();
    }

    public void Deactivate()
    {
        if(this.IsEmptyOrDestroyed()) return;
        
        if (isActivated)
        {
            OnDeactivate();
            OnEffectEnd.Invoke(this, Target);
        }
        isActivated = false;
        if(gameObject.IsValid()) Destroy(gameObject);
    }

    public void RefreshStatusEffect() { SetDurationLeft(turnDuration); }

    public IEnumerator BeforeTurnTick(TurnBaseState ownerTurnState_)
    {
        yield return OnBeforeTurnTick(ownerTurnState_);
        HasStillTurns();
        yield return null;
    }
    
    public IEnumerator AfterTurnTick(TurnBaseState ownerTurnState_)
    {
        yield return OnAfterTurnTick(ownerTurnState_);
        HasStillTurns();
        yield return null;
    }

    public void SelfRemove()
    {
        Target.RemoveStatusEffect(this);
    }
    
    public bool HasStillTurns()
    {
        if (turnsLeft > 0) return true;
        
        SelfRemove();
        return false;
    }
    
    public void StackEffect(StatusEffectBase newEffect_)
    {
        if(this == newEffect_) return;
        OnStackEffect(newEffect_);
    }
    
    public void SetDurationLeft(int durationLeft_)
    {
        turnsLeft = durationLeft_;
        if (turnsLeft <= 0)
        {
            turnsLeft = 0;
            SelfRemove();
        }
        OnTurnsLeftChange?.Invoke(turnsLeft);
    }

    public void RemoveTurn()
    {
        SetDurationLeft(turnsLeft - 1);
    }
    
    public void AddTurn()
    {
        SetDurationLeft(turnsLeft + 1);
    }
    
    public void SetDuration(int duration_)
    {
        turnsLeft = duration_;
    }
    
    public void SetDamage(int damage_)
    {
        Damage = Mathf.Clamp(damage_,1, int.MaxValue);
    }
}
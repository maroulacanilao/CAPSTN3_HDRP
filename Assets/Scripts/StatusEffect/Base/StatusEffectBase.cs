using System.Collections;
using BattleSystem.BattleState;
using CustomEvent;
using UnityEngine;
using Character;
using Character.CharacterComponents;
using NaughtyAttributes;

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
    [field: SerializeField] public bool HasDuration { get; private set; }
    
    [field: BoxGroup("Base Properties")] [field: ShowIf("HasDuration")]
    [field: SerializeField] public int turnDuration { get; private set; }
    
    [field: BoxGroup("Base Properties")] [field: ShowAssetPreview()]
    [field: SerializeField] public Sprite Icon { get; private set; }
    
    public StatusEffectReceiver Target { get; private set; }
    public GameObject Source { get; protected set; }
    public int turnsLeft { get; protected set; }

    protected bool isActivated;
    
    protected string characterName => Target.character.characterData.characterName;

    public Evt<StatusEffectBase, StatusEffectReceiver> OnEffectEnd = new Evt<StatusEffectBase, StatusEffectReceiver>();

    protected abstract void OnActivate();
    protected abstract void OnDeactivate();
    
    protected virtual void OnStackEffect(StatusEffectBase newEffect_) { }
    
    protected virtual IEnumerator OnBeforeTurnTick(TurnBaseState ownerTurnState_) { yield break; }

    protected virtual IEnumerator OnAfterTurnTick(TurnBaseState ownerTurnState_) { yield break; }

    public void Activate(StatusEffectReceiver target, GameObject source = null)
    {
        if (isActivated) return;
        Target = target;
        Source = source;
        isActivated = true;
        if (HasDuration) turnsLeft = turnDuration;
        OnActivate();
    }

    public void Deactivate()
    {
        if (isActivated)
        {
            OnDeactivate();
            OnEffectEnd.Invoke(this, Target);
        }
        isActivated = false;
        Destroy(gameObject);
    }

    public void RefreshStatusEffect() { turnsLeft = turnDuration; }

    public IEnumerator BeforeTurnTick(TurnBaseState ownerTurnState_)
    {
        yield return OnBeforeTurnTick(ownerTurnState_);
        HasStillTurns();
        yield break;
    }
    
    public IEnumerator AfterTurnTick(TurnBaseState ownerTurnState_)
    {
        yield return OnAfterTurnTick(ownerTurnState_);
        HasStillTurns();
        yield break;
    }

    public void SelfRemove()
    {
        Target.RemoveStatusEffect(this);
    }
    
    public void HasStillTurns()
    {
        if (turnsLeft > 0) return;
        
        SelfRemove();
    }
    
    public void StackEffect(StatusEffectBase newEffect_)
    {
        if(this == newEffect_) return;
        OnStackEffect(newEffect_);
    }
}
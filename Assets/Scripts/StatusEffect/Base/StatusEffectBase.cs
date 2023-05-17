using System.Collections;
using BattleSystem.BattleState;
using CustomEvent;
using UnityEngine;
using Character;
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
    
    [field: SerializeField] 
    public StatusEffectReceiver Target { get; private set; }
    public GameObject Source { get; private set; }
    public int turns { get; private set; }

    protected bool isActivated;
    
    public Evt<StatusEffectBase, StatusEffectReceiver> OnEffectEnd = new Evt<StatusEffectBase, StatusEffectReceiver>();

    protected abstract void OnActivate();
    protected abstract void OnDeactivate();
    
    protected virtual IEnumerator OnTick(TurnBaseState ownerTurnState_) { yield break; }

    public StatusEffectBase Initialize(int damage_)
    {
        Damage = damage_;
        return this;
    }

    public void Activate(StatusEffectReceiver target, GameObject source = null)
    {
        if (isActivated) return;
        Target = target;
        Source = source;
        isActivated = true;
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

    public void RefreshStatusEffect() { turns = 0; }

    public IEnumerator Tick(TurnBaseState ownerTurnState_)
    {
        if (turns >= turnDuration)
        {
            SelfRemove();
        }
        yield return OnTick(ownerTurnState_);
        turns++;
        yield break;
    }

    public void SelfRemove()
    {
        Target.RemoveStatusEffect(this);
    }
}
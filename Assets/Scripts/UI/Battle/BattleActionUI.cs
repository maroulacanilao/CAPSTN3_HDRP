using System.Collections;
using BattleSystem;
using NaughtyAttributes;
using UI.Battle;
using UnityEngine;

public class BattleActionUI : MonoBehaviour
{
    
    [BoxGroup("Panels")] [SerializeField] private GameObject mainPanel;
    [BoxGroup("Panels")] [SerializeField] private GameObject actionPanel;
    [BoxGroup("Panels")] [SerializeField] private SpellActionPanelUI spellPanel;

    public BattleCharacter player { get; private set; }

    #region Unity Functions

    private void Awake()
    {
        BattleManager.OnPlayerTurnStart.AddListener(ShowActionMenu);
    }

    private void OnDestroy()
    {
        BattleManager.OnPlayerTurnStart.RemoveListener(ShowActionMenu);
    }
    
    public void ShowSpellMenu()
    {
        actionPanel.SetActive(false);
        spellPanel.ShowPanel(this);
    }

    #endregion
    
    public void CloseOtherPanels()
    {
        actionPanel.SetActive(false);
        spellPanel.gameObject.SetActive(false);
    }

    public void BackToActionPanel()
    {
        CloseOtherPanels();
        actionPanel.SetActive(true);
    }
    
    public void Attack()
    {
        if (player == null)
        {
            Debug.LogWarning("PLAYER IS NULL");
            BattleManager.OnPlayerEndDecide.Invoke();
        }
        mainPanel.SetActive(false);
        StartCoroutine(Co_Attack());
    }

    public void UseSpell(int index_)
    {
        if (player == null)
        {
            Debug.LogWarning("PLAYER IS NULL");
            BattleManager.OnPlayerEndDecide.Invoke();
        }
        mainPanel.SetActive(false);
        StartCoroutine(Co_Spell(index_));
    }
    
    private void ShowActionMenu(BattleCharacter playerCharacter_)
    {
        player = playerCharacter_;
        mainPanel.SetActive(true);
        BackToActionPanel();
    }

    private IEnumerator Co_Attack()
    {
        yield return player.AttackTarget(BattleManager.Instance.enemy);
        BattleManager.OnPlayerEndDecide.Invoke();
    }
    
    private IEnumerator Co_Spell(int index_)
    {
        yield return player.spellUser.UseSkill(index_, BattleManager.Instance.enemy);
        BattleManager.OnPlayerEndDecide.Invoke();
    }
}

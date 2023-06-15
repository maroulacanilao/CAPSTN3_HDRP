using BaseCore;
using CustomEvent;
using UnityEngine;

namespace Character.CharacterComponents
{
    [System.Serializable]
    public class CharacterMana : CharacterCore
    {
        public readonly Evt<CharacterMana> OnAddMana = new Evt<CharacterMana>();
        public readonly Evt<CharacterMana> OnNotEnoughMana = new Evt<CharacterMana>();
        public readonly Evt<CharacterMana> OnManuallyUpdateMana = new Evt<CharacterMana>();

        public virtual int MaxMana => character.stats.maxMana;
        public float ManaPercentage => (float) CurrentMana / MaxMana;
        
        public readonly Evt<CharacterMana> OnUseMana = new Evt<CharacterMana>();

        public int CurrentMana { get; protected set; }
        
        public CharacterMana(CharacterBase character_) : base(character_)
        {
            if(character_ == null) return;
            CurrentMana = MaxMana;
        }

        public bool HasEnoughMana(int manaToCompare_)
        {
            return CurrentMana >= manaToCompare_;
        }

        protected virtual void SetCurrentMana(int newCurrMana_)
        {
            CurrentMana = Mathf.Clamp(
                newCurrMana_,
                0,
                MaxMana);
        }

        /// <summary>
        ///     return if there is enough mana and subtracts the mana used
        /// </summary>
        /// <param name="manaUsed_"></param>
        /// <returns></returns>
        public bool UseMana(int manaUsed_)
        {
            if (!HasEnoughMana(manaUsed_))
            {
                OnNotEnoughMana.Invoke(this);
                return false;
            }
            SetCurrentMana(CurrentMana - manaUsed_);

            OnUseMana.Invoke(this);
            return true;
        }

        public void AddMana(int manaAdd_)
        {
            SetCurrentMana(CurrentMana + manaAdd_);
            OnAddMana.Invoke(this);
        }
    }
}
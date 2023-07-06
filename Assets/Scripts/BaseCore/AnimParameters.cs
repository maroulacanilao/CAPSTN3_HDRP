using System;
using NaughtyAttributes;
using UnityEngine;

namespace BaseCore
{
    [Serializable]
    public class AnimParameters
    {
        [field: SerializeField] public Animator animator { get; protected set; }
        
        #region Animation Parameters
        
        [field:Header("Base Animation Parameters")]

        [field: SerializeField] [field: AnimatorParam("animator")]
        public int groundedHash { get; private set; }
        [field: SerializeField] [field: AnimatorParam("animator")]
        public int isIdleHash { get; private set; }
        [field: SerializeField] [field: AnimatorParam("animator")]
        public int xSpeedHash { get; private set; }
        [field: SerializeField] [field: AnimatorParam("animator")]
        public int ySpeedHash { get; private set; }
        [field: SerializeField] [field: AnimatorParam("animator")]
        public int attackHash { get; private set; }
        [field: SerializeField] [field: AnimatorParam("animator")]
        public int spellAnimationHash { get; private set; }
        [field: SerializeField] [field: AnimatorParam("animator")]
        public int hurtAnimationHash { get; private set; }
        [field: SerializeField] [field: AnimatorParam("animator")]
        public int deathAnimationHash { get; private set; }
        [field: SerializeField] [field: AnimatorParam("animator")]
        public int atkSpeedHash { get; private set; }
        
        #endregion

        #region Animation Events
        
        [field:Header("Base Event IDs")]

        [field: SerializeField] 
        public string animEndEvtId { get; private set; }
        [field: SerializeField] 
        public string atkHitEvtId { get; private set; }
        [field: SerializeField] 
        public string atkEndEvtId { get; private set; }
        [field: SerializeField] 
        public string castHitEvtId { get; private set; }
        [field: SerializeField] 
        public string castEndEvtId { get; private set; }
        
        #endregion
    }

    [Serializable]
    public class PlayerAnimParam : AnimParameters
    {
        
        [field: Header("Player Anim Parameters")]
        [field: SerializeField] [field: AnimatorParam("animator")]
        public int hoeHash { get; private set; }
        [field: SerializeField] [field: AnimatorParam("animator")]
        public int wateringHash { get; private set; }
        [field: SerializeField] [field: AnimatorParam("animator")]
        public int jumpHash { get; private set; }
        
        [field: Header("Player Event IDs")]
        [field: SerializeField] 
        public string jumpStartEvtId { get; private set; }
        [field: SerializeField] 
        public string hoeEndEvtId { get; private set; }
        [field: SerializeField] 
        public string waterEndEvtId { get; private set; }
    }
}
using BaseCore;
using Character;
using Managers;
using Player;
using UnityEngine;

namespace Interactable
{
    public class HouseInteractable : InteractableObject
    {

        protected override void Interact()
        {
            var _character = GameObject.FindWithTag("Player").GetComponent<PlayerCharacter>();
            _character.Refill();
            
            TimeManager.EndDay();
        }
        protected override void Enter()
        {
            
        }
        protected override void Exit()
        {
            
        }
    }
}

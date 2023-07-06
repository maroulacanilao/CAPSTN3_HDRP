using BaseCore;

namespace Dungeon
{
    public class DungeonMapExit : InteractableObject
    {

        protected override void Interact()
        {
            DungeonManager.Instance.GoToNextLevel();
        }
        protected override void Enter()
        {

        }
        protected override void Exit()
        {
        
        }
    }
}

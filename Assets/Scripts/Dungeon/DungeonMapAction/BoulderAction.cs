using UnityEngine;

namespace Dungeon.DungeonMapAction
{
    public class BoulderAction : DungeonAction
    {
        [SerializeField] private GameObject boulder;
        
        protected override void RemoveBlockerHandler()
        {
            boulder.SetActive(false);
        }
    }
}
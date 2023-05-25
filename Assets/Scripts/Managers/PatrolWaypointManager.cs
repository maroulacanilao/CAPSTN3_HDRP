using System.Collections.Generic;
using BaseCore;
using CustomHelpers;
using UnityEngine;

namespace Managers
{
    public class PatrolWaypointManager : Singleton<PatrolWaypointManager>
    {
        [field: SerializeField] public List<Transform> waypoints { get; private set; }

        public Vector3 GetRandomPosition()
        {
            return waypoints.GetRandomItem().position;
        }
        
        public Transform GetRandomTransform()
        {
            return waypoints.GetRandomItem();
        }
        
        public Transform GetRandomTransform(Transform excludeTransform_)
        {
            return waypoints.GetRandomItem(excludeTransform_);
        }
        
        public Vector3 GetRandomPosition(Transform excludeTransform_)
        {
            return waypoints.GetRandomItem(excludeTransform_).position;
        }
        
        public Transform GetClosestWaypoint(Vector3 position_)
        {
            return waypoints.GetNearestTransform(position_);
        }
        
#if UNITY_EDITOR

        [NaughtyAttributes.Button("SET WAYPOINTS")]
        private void GetWaypoints()
        {
            waypoints = new List<Transform>();
            var _count = transform.childCount;

            for (int i = 0; i < _count; i++)
            {
                waypoints.Add(transform.GetChild(i));
            }
        }

#endif
    }
}

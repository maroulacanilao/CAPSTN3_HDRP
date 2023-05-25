using System;
using BaseCore;
using CustomHelpers;
using Items;
using Items.Inventory;
using Player;
using UnityEngine;

namespace Farming
{
    public class ToolArea : Singleton<ToolArea>
    {
        [SerializeField] private PlayerInventory inventory;
        [SerializeField] private float distanceToPlayer = 2f;
        [SerializeField] private float distanceToGround = 0.01f;
        [SerializeField] private float lineWidth = 0.01f;
        [SerializeField] private Color color = Color.white;
        [SerializeField] private LayerMask farmGroundLayer;
        [SerializeField] private LayerMask farmTileLayer;

        private bool ShowLine = true;

        private LineRenderer lineRenderer;

        private Vector3[] vertices;
        private Vector2 size = Vector2.one;
        private Vector3 playerPosition;

        public void Instantiate(Vector2 size_)
        {
            size = size_;
        }
        
        protected override void Awake()
        {
            base.Awake();
            lineRenderer = GetComponent<LineRenderer>();
        
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            
            lineRenderer.positionCount = 4;
            
            vertices = new Vector3[lineRenderer.positionCount];
            playerPosition = transform.position;
            PlayerEquipment.OnChangeItemOnHand.AddListener(ChangeItem);
            InventoryEvents.OnItemOnHandUpdate.AddListener(ItemOnHandUpdate);
        }

        private void ItemOnHandUpdate(int index_, Item item_)
        {
            if (item_ == null)
            {
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(item_.ItemType is Items.ItemType.Tool or Items.ItemType.Seed);
        }
        
        private void ChangeItem(int index_)
        {
            ItemOnHandUpdate(index_, inventory.ItemTools[index_]);
        }

        public void UpdatePosition(Vector3 direction_,Vector3 playerPosition_)
        {
            if(!gameObject.activeInHierarchy) return;
            
            playerPosition = playerPosition_ + direction_ * distanceToPlayer;
            
            var _groundHit = GetGround();
            
            bool _isOnGround = _groundHit.collider;
            gameObject.SetActive(_isOnGround);
            if(!_isOnGround) return;
            
            SnapToPosition(_groundHit.point.y);
            DrawLine();
        }

        public RaycastHit GetGround()
        {
            var _ray = new Ray(playerPosition.AddY(0.5f), Vector3.down);

            Physics.Raycast(_ray, out var _hit, 0.55f, farmGroundLayer);
            return _hit;
        }

        private void SnapToPosition(float groundElevation_)
        {
            var _xPos = Mathf.Round(playerPosition.x / size.x) * size.x;
            var _zPos = Mathf.Round(playerPosition.z / size.y) * size.y;
            var _yPos = groundElevation_ + distanceToGround;
            transform.position = new Vector3(_xPos, _yPos, _zPos);
        }

        private void DrawLine()
        {
            if(!ShowLine) return;
        
            var _scaledSize = size / 2;
            var _pos = transform.position;
        
            vertices = new []
            {
                _pos + new Vector3(-_scaledSize.x, 0f, -_scaledSize.y),
                _pos + new Vector3(_scaledSize.x, 0f, -_scaledSize.y),
                _pos + new Vector3(_scaledSize.x, 0f, _scaledSize.y),
                _pos + new Vector3(-_scaledSize.x, 0f, _scaledSize.y),
            };
        
            lineRenderer.SetPositions(vertices);
        }

        #region Public Methods
        
        public FarmTile GetFarmTile()
        {
            if (!gameObject.activeInHierarchy) return null;
            
            var _ray = new Ray(transform.position.AddY(1f), Vector3.down);
            
            return Physics.Raycast(_ray, out var _hit, 1.1f, farmTileLayer) 
                ? _hit.transform.GetComponent<FarmTile>() : null;
        }
        
        public bool IsTillable()
        {
            if (!gameObject.activeInHierarchy) return false;
            
            foreach (var _pos in vertices)
            {
                var _rayDown = new Ray(_pos.AddY(0.1f), Vector3.down);
                if (!Physics.Raycast(_rayDown, out var _hitInfo, 0.2f, farmGroundLayer))
                {
                    return false;
                }
                
                //var _rayUp = new Ray(_pos, Vector3.up);
                //
                // if (Physics.Raycast(new Ray(_pos, Vector3.up), out var _hitUp, 1.1f, farmGroundLayer))
                // {
                //     return false;
                // }
            }
            return true;
        }

        public void ShowToolAreaBox(bool value_)
        {
            ShowLine = value_;
            lineRenderer.enabled = ShowLine; 
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if(vertices == null) return;
            if(vertices.Length == 0) return;
            
            foreach (var _pos in vertices)
            {
                // var _vertPos = _pos;
                //var _rayUp = new Ray(transform.position.AddY(0.1f), Vector3.up);
                Gizmos.DrawLine(_pos, _pos + Vector3.up * 1.1f);
            }
        }

        #endregion
    }
}


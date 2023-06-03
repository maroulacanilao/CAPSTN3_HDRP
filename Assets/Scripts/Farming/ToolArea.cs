using System;
using System.Collections;
using BaseCore;
using CustomHelpers;
using Items;
using Items.Inventory;
using Managers;
using Player;
using UnityEngine;

namespace Farming
{
    public class ToolArea : Singleton<ToolArea>
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private float distanceToPlayer = 2f;
        [SerializeField] private float distanceToGround = 0.01f;
        [SerializeField] private float lineWidth = 0.01f;

        private bool ShowLine = true;
        private PlayerInventory inventory;
        private LayerMask farmTileLayer;
        private LayerMask farmGroundLayer;
        
        private Vector3[] vertices;
        private Vector2 size = Vector2.one;
        private Vector3 playerPosition;

        public void Instantiate(Vector2 size_, LayerMask farmTileLayer_, LayerMask farmGroundLayer_)
        {
            size = size_;
            farmTileLayer = farmTileLayer_;
            farmGroundLayer = farmGroundLayer_;
            inventory = GameManager.Instance.PlayerData.playerInventory;
            
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;

            lineRenderer.positionCount = 4;
            lineRenderer.useWorldSpace = false;

            DrawLine();

            vertices = new Vector3[lineRenderer.positionCount];
            playerPosition = transform.position;
            PlayerEquipment.OnChangeItemOnHand.AddListener(ChangeItem);
            InventoryEvents.OnItemOnHandUpdate.AddListener(ItemOnHandUpdate);
        }
        
        #region Event Listeners

        private void ItemOnHandUpdate(int index_, Item item_)
        {
            if (item_ == null)
            {
                lineRenderer.gameObject.SetActive(false);
                return;
            }
            lineRenderer.gameObject.SetActive(item_.ItemType is Items.ItemType.Tool or Items.ItemType.Seed);
        }
        
        private void ChangeItem(int index_)
        {
            ItemOnHandUpdate(index_, inventory.ItemTools[index_]);
        }

        #endregion


        public void UpdatePosition(Vector3 direction_,Vector3 playerPosition_, bool IsGrounded_)
        {
            playerPosition = playerPosition_ + direction_ * distanceToPlayer;
            
            var _groundHit = GetGround();
            
            bool _isOnGround = _groundHit.collider;
            
            var _groundPosY = _isOnGround ? _groundHit.point.y : playerPosition.y;
            SnapToPosition(_groundPosY);
            lineRenderer.enabled = _isOnGround;
            //if(_isOnGround) DrawLine();
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

            vertices = new []
            {
                new Vector3(-_scaledSize.x, 0f, -_scaledSize.y),
                new Vector3(_scaledSize.x, 0f, -_scaledSize.y),
                new Vector3(_scaledSize.x, 0f, _scaledSize.y),
                new Vector3(-_scaledSize.x, 0f, _scaledSize.y),
            };
        
            lineRenderer.SetPositions(vertices);
        }

        #region Public Methods
        
        public FarmTile GetFarmTile()
        {
            if (!lineRenderer.enabled) return null;
            
            var _ray = new Ray(transform.position.AddY(1f), Vector3.down);
            
            return Physics.Raycast(_ray, out var _hit, 1.1f, farmTileLayer) 
                ? _hit.transform.GetComponent<FarmTile>() : null;
        }
        
        public bool IsTillable()
        {
            if (!lineRenderer.enabled) return false;

            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                var _pos = lineRenderer.GetPosition(i) + transform.position;
                var _rayDown = new Ray(_pos.AddY(0.1f), Vector3.down);
                if (!Physics.Raycast(_rayDown, out var _hitInfo, 0.2f, farmGroundLayer))
                {
                    return false;
                }
            }
            return true;
        }
        
        public RaycastHit GetGround()
        {
            var _ray = new Ray(playerPosition.AddY(0.5f), Vector3.down);

            Physics.Raycast(_ray, out var _hit, 0.55f, farmGroundLayer);
            return _hit;
        }

        public void ShowToolAreaBox(bool value_)
        {
            ShowLine = value_;
            lineRenderer.enabled = ShowLine; 
        }

        #endregion
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if(vertices == null) return;
            if(vertices.Length == 0) return;

            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                var _pos = lineRenderer.GetPosition(i) + transform.position;
                Gizmos.DrawLine(_pos, _pos + Vector3.up * 1.1f);
            }
        }
    }
}


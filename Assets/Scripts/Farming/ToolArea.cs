using System.Collections.Generic;
using BaseCore;
using CustomHelpers;
using Items;
using Items.Inventory;
using Managers;
using ObjectPool;
using Others;
using Player;
using ScriptableObjectData;
using UnityEngine;

namespace Farming
{
    public class ToolArea : Singleton<ToolArea>
    {
        [SerializeField] private GameDataBase gameDataBase;
        [SerializeField] private GameObject farmTilePrefab;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private float distanceToPlayer = 2f;
        [SerializeField] private float distanceToGround = 0.01f;
        [SerializeField] private float lineWidth = 0.01f;

        private PlayerInputController playerController;
        private PlayerInventory inventory;
        private Vector3[] vertices;
        private Vector3 playerPosition;
        private bool ShowLine = true;
        
        public Vector3 size { get; private set; } = Vector3.one;

        private LayerMask farmTileLayer => gameDataBase.farmTileLayer;
        private LayerMask plowableLayer => gameDataBase.plowableAreaLayer;
        private LayerMask foliageLayer => gameDataBase.foliageLayer;

        public void Start()
        {
            var _tile = farmTilePrefab.GetInstance();
            size = _tile.GetComponent<Collider>().bounds.size;

            _tile.ReturnInstance();
            
            inventory = GameManager.Instance.PlayerData.inventory;
            
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;

            lineRenderer.positionCount = 4;
            lineRenderer.useWorldSpace = false;

            DrawLine();

            vertices = new Vector3[lineRenderer.positionCount];
            playerPosition = transform.position;
            PlayerEquipment.OnChangeItemOnHand.AddListener(ChangeItem);
            InventoryEvents.OnItemOnHandUpdate.AddListener(ItemOnHandUpdate);
            playerController = PlayerInputController.Instance;
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

        private void LateUpdate()
        {
            if (!playerController.CanUseFarmTools)
            {
                if(lineRenderer.enabled) lineRenderer.enabled = false;
            }
            
            UpdatePosition(playerController.moveDirection, playerController.transform.position);
        }


        public void UpdatePosition(Vector3 direction_,Vector3 playerPosition_)
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
            var _zPos = Mathf.Round(playerPosition.z / size.z) * size.z;
            var _yPos = groundElevation_ + distanceToGround;
            transform.position = new Vector3(_xPos, _yPos, _zPos);
        }

        private void DrawLine()
        {
            if(!ShowLine) return;
        
            var _scaledSize = size / 2;

            vertices = new []
            {
                new Vector3(-_scaledSize.x, 0f, -_scaledSize.z),
                new Vector3(_scaledSize.x, 0f, -_scaledSize.z),
                new Vector3(_scaledSize.x, 0f, _scaledSize.z),
                new Vector3(-_scaledSize.x, 0f, _scaledSize.z),
            };
        
            lineRenderer.SetPositions(vertices);
        }

        #region Public Methods
        
        public FarmTile GetFarmTile()
        {
            var _ray = new Ray(transform.position.AddY(0.5f), Vector3.down);
            
            return Physics.Raycast(_ray, out var _hit, .7f, farmTileLayer) 
                ? _hit.transform.GetComponent<FarmTile>() : null;
        }
        
        public bool IsTillable()
        {
            if (!lineRenderer.enabled) return false;

            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                var _pos = lineRenderer.GetPosition(i) + transform.position;
                var _rayDown = new Ray(_pos.AddY(0.1f), Vector3.down);
                if (!Physics.Raycast(_rayDown, out var _hitInfo, 0.2f, plowableLayer))
                {
                    return false;
                }
            }
            return true;
        }
        
        public bool HasFoliage(out GameObject foliage_)
        {
            foliage_ = null;
            
            if (!lineRenderer.enabled) return false;
            var _pos = transform.position;
            return TrGetFoliage(_pos, out foliage_);
        }

        public RaycastHit GetGround()
        {
            var _ray = new Ray(playerPosition.AddY(0.5f), Vector3.down);

            Physics.Raycast(_ray, out var _hit, 0.55f, plowableLayer);
            return _hit;
        }

        public void ShowToolAreaBox(bool value_)
        {
            ShowLine = value_;
            lineRenderer.enabled = ShowLine; 
        }

        public static bool TrGetFoliage(Vector3 position_, out GameObject foliage_)
        {
            foliage_ = null;
            var _rayDown = new Ray(position_.AddY(-0.1f), Vector3.up);
            
            if (!Physics.Raycast(_rayDown, out var _hitInfo, 1f, Instance.foliageLayer)) return false;
            
            if(_hitInfo.collider == null) return false;
            
            foliage_ = _hitInfo.collider.gameObject;
            return true;
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


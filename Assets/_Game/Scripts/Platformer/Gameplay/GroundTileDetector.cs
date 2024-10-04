using System;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace TIGD.Platfomer.Gameplay
{
    public class GroundTileDetector : MonoBehaviour
    {
        public event Action<GroundType> OnGroundTypeChanged;

        [SerializeField] private GroundType _groundType;

        public GroundType GroundType => _groundType;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.TryGetComponent(out Tilemap tilemap))
            {
                TileBase tile = tilemap.GetTile(tilemap.WorldToCell(transform.position));
                if(tile != null)
                {
                    Debug.Log($"Tile: {tile.name}");
                }
            }
            else
            {
                SetBlockType(GroundType.Block);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            SetBlockType(GroundType.None);
        }

        private void SetBlockType(GroundType type)
        {
            if(_groundType != type)
            {
                _groundType = type;
                OnGroundTypeChanged?.Invoke(_groundType);
            }
        }
    }
}

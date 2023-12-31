using UnityEngine;

namespace Project.Spells.Casters
{
    public class DefaultZoneCaster : SpellCaster
    {
        [SerializeField] private Transform aimVisual;
        [SerializeField] private Transform zoneVisual;
        
        [SerializeField] private LayerMask groundLayerMask;
        private Camera _camera;
        
        private DefaultZoneResults _currentResults = new();

        DefaultZoneSpellData _zoneSpellData;
        
        private void Start()
        {
            _camera = Camera.main;
            zoneVisual.gameObject.SetActive(false);
            aimVisual.gameObject.SetActive(false);
        }

        public override void Init(Transform casterTransform, SpellData spell)
        {
            base.Init(casterTransform, spell);
            
            var zoneSpell = spell as DefaultZoneSpellData;

            if (zoneSpell == null)
            {
                Debug.LogError("DefaultZoneCaster can only be used with DefaultZoneData. You gave spell: " +
                               spell.name + " of Type: " + spell.GetType());
                return;
            }

            _zoneSpellData = zoneSpell;
            
            zoneVisual.localScale = Vector3.one * zoneSpell.limitRadius * 2;
            aimVisual.localScale = Vector3.one * zoneSpell.zoneRadius * 2;
        }
        
        public override void StartChanneling()
        {
            if (IsChanneling) return;
            
            base.StartChanneling();
            zoneVisual.gameObject.SetActive(true);
            aimVisual.gameObject.SetActive(true);
        }
        
        public override void StopChanneling()
        {
            if (!IsChanneling) return;
            
            base.StopChanneling();
            zoneVisual.gameObject.SetActive(false);
            aimVisual.gameObject.SetActive(false);
        }
        
        protected override void UpdateChanneling()
        {
            var pos = _currentResults.Position;
            pos.y = aimVisual.position.y;
            
            aimVisual.position = pos;
        }

        public override void EvaluateResults()
        {
            Utilities.GetMouseWorldPosition(_camera, groundLayerMask, out Vector3 position);
            
            var zoneCenter = zoneVisual.position;
            position = zoneCenter + Vector3.ClampMagnitude(position - zoneCenter, _zoneSpellData.limitRadius);
            
            _currentResults.Position = position;
        }

        public override void TryCast(int casterIndex)
        {
            SpellManager.instance.TryCastSpellServerRpc(casterIndex, _currentResults);
        }
    }
}
using UnityEngine;

namespace Project.Spells
{
    [CreateAssetMenu(fileName = "New ZoneData", menuName = "Spells/Data/Zone", order = 1)]
    public class DefaultZoneSpellData : SpellData
    {
        public float limitRadius;
        public float zoneRadius;
    }
}
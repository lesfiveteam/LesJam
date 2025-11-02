using FishingHim.VortexFish.CollectedItem;
using UnityEngine;

namespace FishingHim.VortexFish.Generator
{
    [CreateAssetMenu(fileName = "RowPattern", menuName = "Scriptable Objects/RowPattern")]
    /** 
     * Шаблоны для генерации элементов (бустов и рыбаков) в игре Vortex Fish
     */
    public class RowPattern : ScriptableObject
    {
        public AbstractCollectedItem Line1 = null;
        public AbstractCollectedItem Line2 = null;
        public AbstractCollectedItem Line3 = null;
        public AbstractCollectedItem Line4 = null;
        public AbstractCollectedItem Line5 = null;
    }
}

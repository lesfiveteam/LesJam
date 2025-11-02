using FishingHim.Common;
using FishingHim.VortexFish.Manager;
using UnityEngine;

namespace FishingHim.VortexFish.CollectedItem 
{
    /** 
     * јбстрактный класс дл€ всех элементов по€вл€ющихс€ на линии
     */
    public abstract class AbstractCollectedItem : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Player")
            {
                OnPlayerEnter();
            }
        }
        /** 
         * ѕереопредел€емый метод с событием, которое вызываетс€ при столкновении с игроком
         */
        protected abstract void OnPlayerEnter();
    }
}

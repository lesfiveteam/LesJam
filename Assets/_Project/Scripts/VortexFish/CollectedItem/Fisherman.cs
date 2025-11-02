using FishingHim.Common;
using FishingHim.VortexFish.Manager;
using UnityEngine;

namespace FishingHim.VortexFish.CollectedItem
{
    /** 
     * Класс для появляемого в линии предмета - рыбака
     */
    public class Fisherman : AbstractCollectedItem
    {
        /** 
         * Переопределяемый метод с событием, которое вызывается при столкновении с игроком
         */
        protected override void OnPlayerEnter()
        {
            if (VortexFishManager.InTurboMode())
            {
                // Сбивает рыбаков
                VortexFishManager.AddDeadFisherman();
                Destroy(gameObject);
            }
            else
            {
                ProgressManager.instance.Lose();
            }
        }
    }
}
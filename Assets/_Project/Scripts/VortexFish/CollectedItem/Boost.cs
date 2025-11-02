using FishingHim.VortexFish.Manager;

namespace FishingHim.VortexFish.CollectedItem
{
    public class Boost : AbstractCollectedItem
    {
        /** 
         * Переопределяемый метод с событием, которое вызывается при столкновении с игроком
         */
        protected override void OnPlayerEnter()
        {
            VortexFishManager.AddBoost();
            Destroy(gameObject);
        }
    }
}

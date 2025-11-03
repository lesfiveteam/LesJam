using FishingHim.VortexFish.Manager;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace FishingHim.VortexFish
{
    /** 
     * Класс для камеры. Отдаляет или приближает в зависимости от турбо режима
     */
    public class MainCamera : MonoBehaviour
    {
        public Transform turboModeTarget;
        public Transform simpleModeTarget;
        public float speed = 5.0f;

        void Update()
        {
            MoveCamera();
        }

        /** 
         * Плавно перемещает камеру в определенную точку
         */
        private void MoveCamera()
        {
            Transform target = VortexFishManager.InTurboMode()
                ? turboModeTarget 
                : simpleModeTarget;
            // Двигается с постоянной скоростью к цели
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );
        }
    }
}

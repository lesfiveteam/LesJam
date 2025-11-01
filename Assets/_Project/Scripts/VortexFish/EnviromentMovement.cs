using System.Collections;
using UnityEngine;

namespace FishingHim.VortexFish
{
    // ����� ��� �������� ���������
    public class EnviromentMovement : MonoBehaviour
    {
        public float speed = 5f;
        public float lifeTime = 15f;


        private void Start()
        {
            // ��� ����� ����� ������� - ����� �� ������ � �������������
            StartCoroutine(WaitAndDestroy());
        }

        void FixedUpdate()
        {
            // ��������� ������� ��������
            transform.position += Vector3.back * speed * Time.deltaTime;
        }

        // ��� � ���������� ������
        private IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);
        }
    }

}

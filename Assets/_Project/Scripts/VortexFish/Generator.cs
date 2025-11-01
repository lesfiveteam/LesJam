using System.Collections;
using UnityEngine;

namespace FishingHim.VortexFish 
{
    /** 
     * ����� ��� �������� ���������� �������� �� ������ VortexFish
     */
    public class Generator : MonoBehaviour
    {
        [SerializeField]
        private float time = 2f; // ������ ������� ������ ����� ��������� ������
        [SerializeField]
        private GameObject prefab;

        private SpriteRenderer lastSpriteRenderer;
        private float lastObjectRightEdge;

        void Start()
        {
            if (prefab == null)
            {
                Debug.LogError("�� ��������� Prefab");
            }
            StartCoroutine(WaitAndCreateObject());
        }

        /**
         * �����������. ������ ����������� ������ �� �������, ����� �������� ��� ����
         */
        IEnumerator WaitAndCreateObject()
        {
            yield return new WaitForSeconds(time);
            GameObject createdObject = Instantiate(prefab, transform);
            // �������� �������� ������������, ����� �� �����
            createdObject.AddComponent<EnviromentMovement>();
            //CreateObject();
            StartCoroutine(WaitAndCreateObject());
        }

        /**
         * ����� ����������� � ������, ���� ����� ����� ������� ���
         * ������ ������ � ��������� � ���� ��������
         * 
         */
        //void CreateObject()
        //{
        //    GameObject createdObject = Instantiate(prefab, transform);

        //    // �������� SpriteRenderer ��� ���������� ��������
        //    SpriteRenderer spriteRenderer = createdObject.GetComponent<SpriteRenderer>();
        //    if (spriteRenderer == null)
        //    {
        //        Debug.LogError("Prefab �� �������� SpriteRenderer");
        //        return;
        //    }

        //    // ��������� ������� �������
        //    Bounds bounds = spriteRenderer.bounds;
        //    float objectWidth = bounds.size.x;

        //    // ������������� ������ ���, ����� �� ������� �����������
        //    Vector3 spawnPosition = Vector3.zero;

        //    if (lastSpriteRenderer != null)
        //    {
        //        // ��������� ������� ��� ���������� �������
        //        float newObjectLeftEdge = lastObjectRightEdge;
        //        spawnPosition = new Vector3(newObjectLeftEdge + objectWidth * 0.5f,
        //                                  transform.position.y, 0f);
        //    }
        //    else
        //    {
        //        // ������ ������
        //        spawnPosition = transform.position;
        //    }

        //    createdObject.transform.position = spawnPosition;

        //    // ��������� ���������� � ��������� �������
        //    lastSpriteRenderer = spriteRenderer;
        //    lastObjectRightEdge = spawnPosition.x + objectWidth * 0.5f;

        //    // ��������� ��������
        //    createdObject.AddComponent<EnviromentMovement>();
        //}
    }
}

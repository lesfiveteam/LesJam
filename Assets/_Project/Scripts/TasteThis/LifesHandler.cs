using System;
using UnityEngine;
using UnityEngine.UI;

namespace FishingHim.TasteThis
{
    public class LifesHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject _lifePrefab;
        private int _lifesNumber;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Hook.Instance.FishCaught += DecreaseLifes;
            _lifesNumber = Hook.Instance.TargetFishesNumber;
            UpdateView();
        }

        private void DecreaseLifes(object _, EventArgs __)
        {
            _lifesNumber--;
            UpdateView();
        }

        private void UpdateView()
        {
            int currentLifes = transform.childCount;

            if (currentLifes < _lifesNumber)
            {
                for (int i = currentLifes; i < _lifesNumber; i++)
                    Instantiate(_lifePrefab, transform);
            }
            else if (currentLifes > _lifesNumber)
            {
                //for (int i = currentLifes - 1; i > _lifesNumber - 1; i--)
                    //Destroy(transform.GetChild(i).gameObject);
                    for (int i = 0; i < currentLifes - _lifesNumber; i++)
                        transform.GetChild(i).GetComponent<Image>().color = Color.black;
            }          
        }
    }
}

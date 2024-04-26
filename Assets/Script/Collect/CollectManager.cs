using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Collect
{
    public class CollectManager : MonoBehaviour
    {
        public static CollectManager Instance { get; private set; }
        private int itemsCollected = 0;

        [SerializeField] [Tooltip("Randomly select items to activate in the scene")]
        private bool isRandomSelectMode = true;

        [SerializeField] [Tooltip("Number of items to activate in the scene")]
        private int numberOfItemsToActivate = 5;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject); // Ensures that there isn't more than one instance
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Makes the manager persist across scenes
            }
        }

        private void Start()
        {
            Debug.Log("CollectManager Started");
            ManageCollectables();
        }

        public void CollectItem()
        {
            ++itemsCollected;
            Debug.Log("Item Collected. Total items: " + itemsCollected);
        }

        public int GetTotalCollectedItems()
        {
            return itemsCollected;
        }

        private void ManageCollectables()
        {
            if (!isRandomSelectMode)
            {
                return;
            }
            // Find all collectable items in the scene.
            CollectableItem[] allItems = FindObjectsOfType<CollectableItem>();

            // Deactivate all items.
            foreach (var item in allItems)
            {
                item.gameObject.SetActive(false);
            }

            // Randomly select 'numberOfItemsToActivate' items to reactivate.
            // min between numberOfItemsToActivate and allItems.Lengths
            ReactivateRandomItems(allItems, Mathf.Min(numberOfItemsToActivate, allItems.Length));
        }

        private void ReactivateRandomItems(CollectableItem[] items, int numberToActivate)
        {
            List<CollectableItem> itemsList = new List<CollectableItem>(items);
            while (numberToActivate > 0 && itemsList.Count > 0)
            {
                int index = Random.Range(0, itemsList.Count);
                itemsList[index].gameObject.SetActive(true);
                itemsList.RemoveAt(index);
                numberToActivate--;
            }
        }
    }
}

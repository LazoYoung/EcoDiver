using Script.Environment.Sound;
using UnityEngine;
namespace Script.Environment.Collect
{
    [RequireComponent(typeof(BoxCollider))]
    public class CollectableItem : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Sound to play when the item is collected.")]
        private SoundPlayer collectSound;

        private void Start()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("collider: " + other);
            Debug.Log("collider: " + other.tag);

            // Check if the interacting collider is a hand
            if (other.CompareTag("Hand"))
            {
                CollectManager.Instance.CollectItem();
                gameObject.SetActive(false);
                collectSound.PlayOnce();
            }
        }
    }
}

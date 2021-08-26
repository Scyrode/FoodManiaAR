using UnityEngine;

namespace Scyout.FoodMania
{
    public class LoseRectangleBehaviour : MonoBehaviour
    {
        public delegate void MissedFoodAction();
        public static event MissedFoodAction onMissedFood;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("FoodItem"))
            {
                Destroy(other.gameObject);
                onMissedFood?.Invoke();
            }
        }
    }
}

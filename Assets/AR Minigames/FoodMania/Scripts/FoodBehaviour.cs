using UnityEngine;

namespace Scyout.FoodMania
{
    public class FoodBehaviour : MonoBehaviour
    {
        public float fallSpeed = 6.0f;
        public float rotationSpeed = 1.0f;
        public float lifeTime = 2.0f;

        private Quaternion randomRotation;
        private bool animateFood = true;

        private void OnEnable()
        {
            EventManager.onGameStart += DestroyFood;
            EventManager.onGameEnd += DontAnimateFood;
        }

        private void OnDisable()
        {
            EventManager.onGameStart -= DestroyFood;
            EventManager.onGameEnd -= DontAnimateFood;
        }

        void DestroyFood()
        {
            Destroy(gameObject);
        }

        void DontAnimateFood()
        {
            animateFood = false;
        }

        private void Start()
        {
            randomRotation = Random.rotation;
        }

        private void Update()
        {
            if (animateFood)
            {
                transform.Translate(-transform.parent.up * fallSpeed * Time.deltaTime, Space.World);
                transform.Rotate(randomRotation.eulerAngles * Time.deltaTime * rotationSpeed);

                lifeTime -= Time.deltaTime;

                if (lifeTime < 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}

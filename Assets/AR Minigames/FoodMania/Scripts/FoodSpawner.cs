using UnityEngine;

namespace Scyout.FoodMania
{
    public class FoodSpawner : MonoBehaviour
    {
        public GameObject[] foodItems;
        public float delay = 2.0f;
        public float spawnOffset = 2.0f;
        public float increaseDifficultyRate = 1.1f;
        public float minSpawnPeriod = 0.75f;

        private float localDelay;
        private float timer = -1;
        private bool startFoodSpawning = false;


        private void OnEnable()
        {
            EventManager.onGameStart += foodSpawnerEnabler;
            EventManager.onGameEnd += foodSpawnerDisabler;
        }

        private void OnDisable()
        {
            EventManager.onGameStart -= foodSpawnerEnabler;
            EventManager.onGameEnd -= foodSpawnerDisabler;
        }

        void foodSpawnerDisabler()
        {
            startFoodSpawning = false;
        }

        private void foodSpawnerEnabler()
        {
            localDelay = delay;
            startFoodSpawning = true;
        }

        private void Update()
        {
            if (startFoodSpawning)
            {
                timer -= Time.deltaTime;

                if (timer < 0)
                {
                    if (localDelay > minSpawnPeriod)
                    {
                        localDelay /= increaseDifficultyRate;
                    }
                    timer = localDelay;

                    spawnFood();
                }
            }
        }

        private void spawnFood()
        {
            var spawnedFood = Instantiate(foodItems[Random.Range(0, foodItems.Length)], transform);
            spawnedFood.transform.localPosition = new Vector3(spawnedFood.transform.localPosition.x, spawnedFood.transform.localPosition.y, Random.Range(-spawnOffset, spawnOffset));
        }
    }
}
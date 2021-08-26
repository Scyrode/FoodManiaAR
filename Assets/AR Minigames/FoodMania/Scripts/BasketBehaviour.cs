using System;
using UnityEngine;

namespace Scyout.FoodMania
{
    public class BasketBehaviour : MonoBehaviour
    {
        public ParticleSystem[] trailParticleSystems;
        public ParticleSystem awardParticleSystem;

        public float basketSpeed = 10.0f;
        public float maxSpeed = 30.0f;
        public float dragFactor = 20.0f;
        //public float brakesSpeed = 60.0f;
        public float basketMoveRange = 2.25f;

        public delegate void CollectFoodAction();
        public static event CollectFoodAction onCollectFood;

        private Vector3 initialPos;

        private AudioSource source;

        private float currentVelocity = 0.0f;
        private bool slidingRight = true;
        private bool isSlidable = false;

        private void OnEnable()
        {
            EventManager.onGameStart += StartSliding;
            EventManager.onGameEnd += StopSliding;
        }

        private void OnDisable()
        {
            EventManager.onGameStart -= StartSliding;
            EventManager.onGameEnd -= StopSliding;
        }

        private void StopSliding()
        {
            isSlidable = false;
            currentVelocity = 0;
        }

        void StartSliding()
        {
            foreach (ParticleSystem particleSystem in trailParticleSystems)
            {
                particleSystem.Stop();
            }

            isSlidable = true;
            slidingRight = true;
            transform.localPosition = initialPos;
            currentVelocity = 0;

            foreach (ParticleSystem particleSystem in trailParticleSystems)
            {
                particleSystem.Play();
            }
        }

        private void Start()
        {
            source = GetComponent<AudioSource>();
            initialPos = transform.localPosition;
        }

        private void Update()
        {
            if (isSlidable)
            {
                if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    if (slidingRight)
                    {
                        currentVelocity += basketSpeed;
                    }
                    else
                    {
                        currentVelocity -= basketSpeed;
                    }

                    currentVelocity = Mathf.Clamp(currentVelocity, -maxSpeed, maxSpeed);
                }
                //} else if (Input.GetKey(KeyCode.S) || (Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Stationary))
                //{
                //    if (slidingRight && currentVelocity > 0)
                //    {
                //        currentVelocity -= Time.deltaTime * brakesSpeed;
                //    } else if (slidingRight && currentVelocity < 0)
                //    {
                //        currentVelocity = 0;
                //    }
                //    else if (!slidingRight && currentVelocity < 0)
                //    {
                //        currentVelocity += Time.deltaTime * brakesSpeed;
                //    } else if (!slidingRight && currentVelocity > 0)
                //    {
                //        currentVelocity = 0;
                //    }
                //}

                transform.Translate(Vector3.right * currentVelocity * Time.deltaTime, Space.Self);

                if (transform.localPosition.x > basketMoveRange && slidingRight)
                {
                    currentVelocity *= -1;
                    slidingRight = false;
                } else if (transform.localPosition.x < -basketMoveRange && !slidingRight)
                {
                    currentVelocity *= -1;
                    slidingRight = true;
                }

                if (slidingRight && currentVelocity > 0)
                {
                    currentVelocity -= Time.deltaTime * dragFactor;
                } else if (slidingRight && currentVelocity < 0)
                {
                    currentVelocity = 0;
                }
                else if (!slidingRight && currentVelocity < 0)
                {
                    currentVelocity += Time.deltaTime * dragFactor;
                } else if (!slidingRight && currentVelocity > 0)
                {
                    currentVelocity = 0;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("FoodItem"))
            {
                awardParticleSystem.Play();
                Destroy(other.gameObject);
                onCollectFood?.Invoke();
                source.Play();
            }
        }
    }
}

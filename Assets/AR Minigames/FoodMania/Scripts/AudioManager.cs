using System;
using UnityEngine;

namespace Scyout.FoodMania
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;

        private void Awake()
        {
            foreach (Sound sound in sounds)
            {
                var soundGameObject = new GameObject(sound.name);
                soundGameObject.transform.parent = transform;
                sound.source = soundGameObject.AddComponent<AudioSource>();
                sound.source.name = sound.name;
                sound.source.clip = sound.clip;
                sound.source.loop = sound.loop;
                sound.source.volume = sound.volume;
            }
        }

        private void OnEnable()
        {
            EventManager.onGameStart += FoodManiaGameStart;
            EventManager.onGameEnd += FoodManiaGameEnd;
        }

        private void OnDisable()
        {
            EventManager.onGameStart -= FoodManiaGameStart;
            EventManager.onGameEnd -= FoodManiaGameEnd;
        }

        private void FoodManiaGameStart()
        {
            Sound gameStartSound = Array.Find(sounds, sound => sound.name == "GameStart");
            Sound gameOverSound = Array.Find(sounds, sound => sound.name == "GameOver");
            Sound themeSound = Array.Find(sounds, sound => sound.name == "Theme");

            if (gameStartSound != null)
                gameStartSound.source.Play();
            if (gameOverSound != null)
                gameOverSound.source.Stop();
            if (themeSound != null)
                themeSound.source.Play();
        }

        private void FoodManiaGameEnd()
        {
            Sound gameStartSound = Array.Find(sounds, sound => sound.name == "GameStart");
            Sound gameOverSound = Array.Find(sounds, sound => sound.name == "GameOver");
            Sound themeSound = Array.Find(sounds, sound => sound.name == "Theme");

            if (gameStartSound != null)
                gameStartSound.source.Stop();
            if (gameOverSound != null)
                gameOverSound.source.Play();
            if (themeSound != null)
                themeSound.source.Stop();
        }
    }
}

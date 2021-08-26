using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

namespace Scyout
{
    [RequireComponent(typeof(ARTrackedImageManager))]
    public class ARImageTracking : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] placeablePrefabs;

        private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
        private ARTrackedImageManager trackedImageManager;

        private void Awake()
        {
            trackedImageManager = GetComponent<ARTrackedImageManager>();

            foreach (GameObject prefab in placeablePrefabs)
            {
                var spawnedPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                spawnedPrefab.name = prefab.name;
                spawnedPrefab.SetActive(false);
                spawnedPrefabs.Add(spawnedPrefab.name, spawnedPrefab);
            }
        }

        private void OnEnable() => trackedImageManager.trackedImagesChanged += ImageChanged;

        private void OnDisable() => trackedImageManager.trackedImagesChanged -= ImageChanged;

        void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var newImage in eventArgs.added)
            {
                UpdateImage(newImage);
            }

            foreach (var updatedImage in eventArgs.updated)
            {
                UpdateImage(updatedImage);
            }

            foreach (var removedImage in eventArgs.removed)
            {
                spawnedPrefabs[removedImage.name].SetActive(false);
            }
        }

        private void UpdateImage(ARTrackedImage trackedImage)
        {
            GameObject prefab = spawnedPrefabs[trackedImage.referenceImage.name];

            prefab.transform.position = trackedImage.transform.position;
            prefab.transform.rotation = Quaternion.LookRotation(trackedImage.transform.right, trackedImage.transform.up);
            prefab.SetActive(true);

            foreach (GameObject other in spawnedPrefabs.Values)
            {
                if (other.name != prefab.name)
                {
                    other.SetActive(false);
                }
            }
        }
    }
}

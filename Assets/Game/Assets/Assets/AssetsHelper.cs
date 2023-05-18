using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if USE_ADDRESSABLES
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif
using UnityObject = UnityEngine.Object;

namespace TeamTheDream
{
    public static class AssetsHelper
    {
        public static void LoadAssetAsync<T>(string address, Action<T> callback) where T : UnityObject
        {
#if USE_ADDRESSABLES
            LoadAssetAsyncWithAddressables<T>(address, callback);
#else
            LoadAssetAsyncWithResources<T>(address, callback);
#endif
        }

        public static void InstantiateObjectAsync<T>(string address, Action<T> callback) where T : MonoBehaviour
        {
#if USE_ADDRESSABLES
            InstantiateObjectAsyncWithResources<T>(address, callback);
#else
            InstantiateObjectAsyncWithResources<T>(address, callback);
#endif
        }

        public static void InstantiateObjectAsync<T>(string address, Transform parent, Action<T> callback) where T : MonoBehaviour
        {
#if USE_ADDRESSABLES
            InstantiateObjectAsyncWithResources<T>(address, parent, callback);
#else
            InstantiateObjectAsyncWithResources<T>(address, parent, callback);
#endif
        }

        public static void SetImageAsync(string address, UnityEngine.UI.Image image)
        {
            image.gameObject.SetActive(false);
            LoadAssetAsync<Sprite>(address, (Sprite sprite) => {
                image.sprite = sprite;
                image.gameObject.SetActive(true);
            });
        }


        public static void ReleaseAsset(UnityObject asset)
        {
#if USE_ADDRESSABLES
            ReleaseAssetWithAddressables(asset);
#else
            ReleaseAssetWithResources(asset);
#endif
        }

        public static void DestroyObject(GameObject gameObject)
        {
#if USE_ADDRESSABLES
            DestroyObjectWithAddressables(gameObject);
#else
            DestroyObjectWithResources(gameObject);
#endif
        }

        #region Resources
        private static void LoadAssetAsyncWithResources<T>(string address, Action<T> callback) where T : UnityObject
        {
            ResourceRequest request = Resources.LoadAsync<T>(address);
            request.completed += (AsyncOperation result) =>
            {
#if UNITY_EDITOR
                Debug.Assert(request.asset != null, address + " not found!");
#endif
                callback((T)request.asset);
            };
        }

        private static void InstantiateObjectAsyncWithResources<T>(string address, Transform parent, Action<T> callback) where T : MonoBehaviour
        {
            ResourceRequest request = Resources.LoadAsync<T>(address);
            request.completed += (AsyncOperation result) =>
            {
#if UNITY_EDITOR
                Debug.Assert(request.asset != null, address + " not found!");
#endif
                callback(GameObject.Instantiate<T>((T)request.asset, parent));
            };
        }

        private static void InstantiateObjectAsyncWithResources<T>(string address, Action<T> callback) where T : MonoBehaviour
        {
            ResourceRequest request = Resources.LoadAsync<T>(address);
            request.completed += (AsyncOperation result) =>
            {
#if UNITY_EDITOR
                Debug.Assert(request.asset != null, address + " not found!");
#endif
                callback(GameObject.Instantiate<T>((T)request.asset));
            };
        }

        private static void ReleaseAssetWithResources(UnityObject asset)
        {
            Resources.UnloadAsset(asset);
        }

        private static void DestroyObjectWithResources(GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
        }
        #endregion

        #region Addressables
#if USE_ADDRESSABLES
        private static void LoadAssetAsyncWithAddressables<T>(string address, Action<T> callback) where T : UnityObject
        {

        }

        private static void InstantiateObjectAsyncWithResources<T>(string address, Action<T> callback)
        {

        }

        private static void InstantiateObjectAsyncWithResources<T>(string address, Transform parent, Action<T> callback) where T : MonoBehaviour
        {

        }

        private static void ReleaseAssetWithAddressables(UnityObject asset)
        {

        }

        private static void DestroyObjectWithAddressables(GameObject gameObject)
        {

        }
#endif
        #endregion
    }
}
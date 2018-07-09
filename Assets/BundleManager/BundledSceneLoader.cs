using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BundleManagement
{
    public class BundledSceneLoader : MonoBehaviour
    {

#if UNITY_EDITOR
        [TextArea(10, 10)] public string bundlesAvailable;
        public string sceneName;
        [InspectorButton("OnLoadBundleClicked")] public bool load;

        private void GetEditorBundles()
        {
            string bandls = "";
            foreach (BundleData bd in bundleDb.Database.bundleData)
            {
                sceneName = bd.sceneName;
                bandls += sceneName + "\n";
            }
            bundlesAvailable = bandls;
        }

        private void OnLoadBundleClicked()
        {
            if (!Application.isPlaying)
                return;
            LoadSceneFromBundle(sceneName);
        }

        private void Start()
        {
            GetEditorBundles();
        }
#endif

        private Dictionary<string, AssetBundle> loadedDependencies;
        private AssetBundle loadedSceneBundle;
        private string loadedSceneBundleName = "";
        private string currentSceneName = "";
        [SerializeField] private BundleDatabaseBehaviour bundleDb;

        private void Awake()
        {
            loadedDependencies = new Dictionary<string, AssetBundle>();
            loadedSceneBundle = null;
        }

        public void LoadSceneFromBundle(string sceneName)
        {
            BundleData bundleData = bundleDb.Database.bundleData.Find((bd) => { return bd.sceneName == sceneName; });
            if (bundleData == null)
                Debug.LogError("Scene not found!" + sceneName);
            StartCoroutine(LoadSceneCoroutine(bundleData));
        }

        bool loadingScene = false;

        private IEnumerator LoadSceneCoroutine(BundleData data)
        {
            if (loadingScene)
                yield break;
            loadingScene = true;

            // unload current scene
            if (currentSceneName.Length > 0)
            {
                AsyncOperation ao = null;
                try { ao = SceneManager.UnloadSceneAsync(currentSceneName); }
                catch { Debug.LogWarning(currentSceneName + " could not be unloaded."); }
                if (ao != null)
                    yield return new WaitUntil(() => ao.isDone);
            }

            // if new scene in other bundle, unload current scene bundle
            List<string> keys = new List<string>(loadedDependencies.Keys);
            keys.ForEach((k) =>
            {
                if (!data.dependencies.Contains(k))
                {
                    AssetBundle toUnload;
                    loadedDependencies.TryGetValue(k, out toUnload);
                    toUnload.Unload(true);
                }
            });

            // unload unnecessary dependencies and/or bundle with the current scene
            if (loadedSceneBundleName != data.sceneBundleName)
            {
                if (loadedSceneBundle != null)
                {
                    loadedSceneBundle.Unload(true);
                    loadedSceneBundleName = "";
                }
            }

            // load necessary dependencies
            data.dependencies.ForEach((d) =>
            {
                if (!loadedDependencies.ContainsKey(d))
                {
                    AssetBundle toLoad = AssetBundle.LoadFromFile(Configuration.Data.bundlesPath + d);
                    loadedDependencies.Add(d, toLoad);
                }
            });

            // load scene bundle
            if (loadedSceneBundle == null)
            {
                loadedSceneBundle = AssetBundle.LoadFromFile(Configuration.Data.bundlesPath + data.sceneBundleName);
                loadedSceneBundleName = data.sceneBundleName;
            }

            // load scene
            SceneManager.LoadScene(data.sceneName, LoadSceneMode.Additive);
            currentSceneName = data.sceneName;
            loadingScene = false;
        }

    }
}
using System;
using System.Collections;
using CodeBase.Logic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly LoadingCurtain _loadingCurtain;

        private bool _isInitialized;

        public SceneLoader(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
        {
            _coroutineRunner = coroutineRunner;
            _loadingCurtain = loadingCurtain;
        }

        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            if (nextScene.Equals(LevelNames.Initial))
            {
                if (SceneManager.GetActiveScene().name.Equals(LevelNames.Initial))
                {
                    onLoaded?.Invoke();
                    yield break;
                }
            }

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
            {
                _loadingCurtain.UpdateLoadProgress(waitNextScene.progress);
                yield return null;
            }

            onLoaded?.Invoke();
        }
    }
}
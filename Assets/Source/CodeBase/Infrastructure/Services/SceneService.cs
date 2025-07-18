using System;
using Cysharp.Threading.Tasks;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using UnityEngine.SceneManagement;

namespace Source.CodeBase.Infrastructure.Services
{
  public class SceneService : ISceneService
  {
    public async UniTask LoadSceneAsync(string sceneName, IProgress<float> progress = null)
    {
      var operation = SceneManager.LoadSceneAsync(sceneName);

      if (progress != null)
      {
        while (!operation.isDone)
        {
          progress.Report(operation.progress);
          await UniTask.Yield();
        }
      }

      await operation.ToUniTask();
    }

    public string GetCurrentSceneName() =>
      SceneManager.GetActiveScene().name;
  }
}
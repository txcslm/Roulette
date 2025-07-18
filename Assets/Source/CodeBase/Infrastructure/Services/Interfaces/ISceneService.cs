using System;
using Cysharp.Threading.Tasks;

namespace Source.CodeBase.Infrastructure.Services.Interfaces
{
  public interface ISceneService
  {
    UniTask LoadSceneAsync(string sceneName, IProgress<float> progress = null);

    string GetCurrentSceneName();
  }
}
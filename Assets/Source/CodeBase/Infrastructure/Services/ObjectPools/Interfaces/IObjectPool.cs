using UnityEngine;

namespace Source.CodeBase.Infrastructure.Services.Interfaces
{
  public interface IObjectPool<T> where T : MonoBehaviour
  {
    T Get();

    void Return(T item);

    void Clear();
  }
}
using System;
using System.Collections.Generic;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.CodeBase.Infrastructure.Services
{
  public class ObjectPool<T> : IObjectPool<T> where T : MonoBehaviour
  {
    private readonly Func<T> _createFunc;
    private readonly int _maxSize;
    private readonly Action<T> _onGet;
    private readonly Action<T> _onReturn;
    private readonly Queue<T> _pool = new Queue<T>();

    public ObjectPool(Func<T> createFunc, Action<T> onGet = null, Action<T> onReturn = null, int maxSize = 20)
    {
      _createFunc = createFunc;
      _onGet = onGet;
      _onReturn = onReturn;
      _maxSize = maxSize;
    }

    public T Get()
    {
      T item;

      if (_pool.Count > 0)
      {
        item = _pool.Dequeue();
      }
      else
      {
        item = _createFunc();
      }

      item.gameObject.SetActive(true);
      _onGet?.Invoke(item);

      return item;
    }

    public void Return(T item)
    {
      if (item == null) return;

      _onReturn?.Invoke(item);
      item.gameObject.SetActive(false);

      if (_pool.Count < _maxSize)
      {
        _pool.Enqueue(item);
      }
      else
      {
        Object.Destroy(item.gameObject);
      }
    }

    public void Clear()
    {
      while (_pool.Count > 0)
      {
        var item = _pool.Dequeue();
        if (item != null)
        {
          Object.Destroy(item.gameObject);
        }
      }
    }
  }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPool<T> where T : Component
{
	private readonly Stack<T> _pool = new();
	private readonly T _prefab;
	private readonly Transform _parent;
	private readonly Action<T> _onGet;
	private readonly Action<T> _onReturn;

	public int CountActive { get; private set; }
	public int CountInactive => _pool.Count;

	public ObjectPool(
		T prefab,
		Transform parent,
		int initialSize = 0,
		Action<T> onGet = null,
		Action<T> onReturn = null)
	{
		_prefab = prefab;
		_parent = parent;
		_onGet = onGet;
		_onReturn = onReturn;

		for (var i = 0; i < initialSize; i++)
			Return(Create());
	}

	public T Get()
	{
		var item = _pool.Count > 0 ? _pool.Pop() : Create();
		item.gameObject.SetActive(true);
		_onGet?.Invoke(item);
		CountActive++;
		return item;
	}

	public void Return(T item)
	{
		item.gameObject.SetActive(false);
		item.transform.SetParent(_parent);
		_onReturn?.Invoke(item);
		_pool.Push(item);
		CountActive--;
	}

	private T Create()
	{
		var instance = Object.Instantiate(_prefab, _parent);
		instance.gameObject.SetActive(false);
		return instance;
	}
}
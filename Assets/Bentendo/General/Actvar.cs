using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bentendo
{
	public class Actvar<T>
	{
		public UnityAction<T> action;
		private T _crnt;
		public T crnt
        {
			get => _crnt;
			set
            {
				_crnt = value;
				action?.Invoke(_crnt);
            }
        }
		public Actvar(T val)
        {
			_crnt = val;
        }

		public static implicit operator T(Actvar<T> a) => a.crnt;
	}
}

using System;
using UnityEngine.Events;

namespace Bentendo
{
	public class Actvar<T> where T : IEquatable<T>
	{
		public Action<T> onChanged;
		private T _crnt;
		public T crnt
        {
			get => _crnt;
			set
            {
				if (!value.Equals(_crnt))
				{
					_crnt = value;
					onChanged?.Invoke(_crnt);
				}
			}
        }
		public Actvar(T val)
        {
			_crnt = val;
        }

		public static implicit operator T(Actvar<T> a) => a.crnt;
	}

	public class Subvar<T>
    {
		public SubvarEvent<T> onChanged = new SubvarEvent<T>();
		private T _crnt;
		public T crnt
		{
			get => _crnt;
			set
			{
				if (!value.Equals(_crnt))
				{
					_crnt = value;
					onChanged?.Invoke(_crnt);
				}
			}
		}
		public Subvar(T val)
        {
			_crnt = val;
		}
		public class SubvarEvent<G> : UnityEvent<G> { }
		public static implicit operator T(Subvar<T> s) => s.crnt;
	}
}

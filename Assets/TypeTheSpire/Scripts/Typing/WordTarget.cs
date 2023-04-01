using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bentendo.TTS
{
	public class WordTarget
	{
		public string word { get; private set; }
		public Actvar<int> completion = new Actvar<int>(0);
		public UnityAction onCharTyped;
		public UnityAction OnTypo;
		public UnityAction onCompleted;
		public bool Completed => completion == word.Length;

		public WordTarget(string word)
        {
			this.word = word;
        }

		public void ProcessPress(char c)
		{
			if (Completed)
			{
				Debug.LogError("character processed  by a completed word target");
				return;
			}
			if (word[completion] == c)
			{
				completion.crnt++;
				onCharTyped?.Invoke();
			}
			else
				OnTypo?.Invoke();
			if (Completed) onCompleted?.Invoke();
		}
	}
}

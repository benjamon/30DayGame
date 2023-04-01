using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bentendo.TTS
{
	public class CardManager : MonoBehaviour
	{
		public Dictionary<char, EmbodiedCard> activeTargets = new Dictionary<char, EmbodiedCard>();
        public LinkedList<EmbodiedCard> embodiedCards = new LinkedList<EmbodiedCard>();
        public GameObject CardPrefab;
		public KInput BattleInput;
		public CardPileUI DrawPile, DiscardPile;
		public Transform[] CardSlots;

		BattleContext context;
        Deck<Card> playerDeck;
		Entity player;
		WordProvider words;

        public void Setup(BattleContext context, Entity player)
        {
			this.context = context;
			this.player = player;
			words = WordProvider.Instance;
			playerDeck = player.Deck;
            BattleInput.SetProcessAction(FindActiveCard);
            FillSlots();
        }

		public void FillSlots()
		{
			for (int i = 0; i < CardSlots.Length; i++)
                DrawCard(i);
        }

		public void DrawCard(int n)
        {
            var go = GameObject.Instantiate(CardPrefab, CardSlots[n]);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            var ec = go.GetComponent<EmbodiedCard>();
			string word = words.GetRandom(Random.Range(4, 9));
			while (activeTargets.ContainsKey(word[0]))
                word = words.GetRandom(Random.Range(4, 9));
            ec.ShowWord(word);
			activeTargets.Add(word[0], ec);
            if (playerDeck.DrawPileEmpty())
                playerDeck.Shuffle();
            if (playerDeck.TryDrawNext(out Card card))
            {
                ec.Setup(card, () =>
                {
                    context.battleRunner.PlayCardImminent(player, card);
                    activeTargets.Remove(word[0]);
                    embodiedCards.Remove(ec);
                    GameObject.Destroy(go);
                    BattleInput.SetProcessAction(FindActiveCard);
                    playerDeck.AddToDiscard(card);
                    DrawCard(n);
                });
            }
            embodiedCards.AddLast(ec);
        }

		public void FindActiveCard(char c)
		{
			if (activeTargets.ContainsKey(c))
            {
                activeTargets[c].FocusCard(BattleInput, c);
            }
        }
	}
}

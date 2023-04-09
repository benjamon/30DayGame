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
        public LayoutUtil.Grid CardGrid;
        public TouchScreenAnchor AnchorParent;

		BattleContext context;
        Deck<Card> playerDeck;
		Entity player;
		WordProvider words;

        public void Setup(BattleContext context, Entity player)
        {
            transform.localPosition = Vector3.right * AnchorParent.ReferenceWidth * .5f + Vector3.up * CardGrid.height * .5f;
			this.context = context;
			this.player = player;
			words = WordProvider.Instance;
			playerDeck = player.Deck;
            BattleInput.SetProcessAction(FindActiveCard);
            FillSlots();
        }

		public void FillSlots()
        {
            CardGrid.ForceUpdate();
            var ct = CardGrid.CellCount;
			for (int i = 0; i < ct; i++)
                DrawCard(i);
        }

		public void DrawCard(int n)
        {
            if (playerDeck.DrawPileEmpty())
                playerDeck.Shuffle();
            if (!playerDeck.TryDrawNext(out Card card))
                throw new System.Exception("fuckin thing SUCKS aint got no CARDS in it (attempted to draw from empty deck)");
            var go = GameObject.Instantiate(CardPrefab, transform);
            go.transform.localPosition = CardGrid.GetPosition(n);
            go.transform.localScale = Vector3.one;
            var ec = go.GetComponent<EmbodiedCard>();
            ec.Setup(card, (info) =>
            {
                player.PlayCard(info);
                activeTargets.Remove(info.word[0]);
                embodiedCards.Remove(ec);
                GameObject.Destroy(go);
                BattleInput.SetProcessAction(FindActiveCard);
                playerDeck.AddToDiscard(card);
                DrawCard(n);
            });
            string word = words.GetRandom(Random.Range(4, 9));
            while (activeTargets.ContainsKey(word[0]))
                word = words.GetRandom(Random.Range(4, 9));
            ec.SetWord(word);
            activeTargets.Add(word[0], ec);
            embodiedCards.AddLast(ec);
        }

		public void FindActiveCard(char c)
		{
			if (activeTargets.ContainsKey(c))
            {
                activeTargets[c].FocusCard(BattleInput, c);
            }
        }

        private void OnDrawGizmos()
        {
            CardGrid.ForceUpdate();
            var ct = CardGrid.CellCount;
            Vector3 size = CardGrid.CellSize;
            var pp = transform.parent.TransformPoint(Vector3.right * AnchorParent.ReferenceWidth * .5f + Vector3.up * CardGrid.height * .5f);
            var ls = transform.lossyScale;
            for (int i = 0; i < ct; i++)
                Gizmos.DrawWireCube((Vector2)pp + CardGrid.GetPosition(i) * ls, new Vector3(size.x * ls.x, size.y * ls.y));
            Gizmos.DrawWireCube((Vector2)pp + CardGrid.Position * ls, new Vector3(CardGrid.width * ls.x, CardGrid.height * ls.y));
        }
    }
}

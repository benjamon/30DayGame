using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bentendo.TTS
{
	public class BattleRunner : MonoBehaviour
	{
        public static int TICKS_PER_SECOND = 1;

        public BattleAnimator battleAnims;
        public TimelineUI timelineUI;
        public CardManager cardManager;
        BattleTimeline timeline;
        BattleContext battleContext;
        bool isSetup;
        public bool isRunning { get; private set; }
        RunRunner run;

        private void Start()
        {
            if (WordProvider.Instance == null)
                SceneManager.LoadScene(0);
            if (RunRunner.Instance == null)
                SceneManager.LoadScene(1);
            run = RunRunner.Instance;
            SetupBattle(run.playerState, run.current.Encounter);
        }

        public void SetupBattle(PlayerState pstate, BattleEventDef runner)
        {
            var p1 = pstate.GetEntity(this);
            p1.OnDeceased.AddListener(LoseGame);
            battleContext = new BattleContext(battleAnims, this, new Entity[] { p1 }, runner.GetEntities(this));
            battleContext.rightEnts[0].OnDeceased.AddListener(WinRound);
            timeline = new BattleTimeline(this, TICKS_PER_SECOND);
            timelineUI.Setup(timeline);
            cardManager.Setup(battleContext, p1);
            isSetup = true;
            isRunning = true;
            foreach (var e in battleContext.rightEnts)
                StartCoroutine(PlayEnemyHand(e));
        }

        public void WinRound()
        {
            cardManager.DisableInput();
            run.playerState.UpdateHealthToEntity(battleContext.leftEnts[0]); run.SelectNextBattle();
            StartCoroutine(LoadSceneOnWin());
        }

        IEnumerator LoadSceneOnWin()
        {
            yield return new WaitForSeconds(3.5f);
            SceneManager.LoadScene((run.current == null) ? "TTS_WIN" : "TTS_Reward");
        }

        public void LoseGame()
        {
            run.StartNewGame();
        }

        public IEnumerator PlayEnemyHand(Entity e)
        {
            while (e.isAlive)
            {
                if (e.Deck.DrawPileEmpty())
                    e.Deck.Shuffle();
                if (!e.Deck.TryDrawNext(out Card card))
                    throw new System.Exception("enemy out of cards");
                PlayCard(new CastInfo(e, card), 4);
                e.Deck.AddToDiscard(card);
                yield return new WaitForSeconds(4f);
            }
        }

        private void Update()
        {
            if (!isSetup || !isRunning)
                return;
            timeline.AddTime(Time.deltaTime);
        }

        public BattleAction PlayCard(CastInfo info, int timeUntil)
        {
			return timeline.EnqueueAction(info.source, () => info.card.def.Cast(battleContext, info), info.card.def.Icon, timeUntil);
		}

        public void PlayCardImminent(CastInfo info)
        {
            PlayCard(info, 0);
        }
	}
}

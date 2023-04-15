using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Bentendo.TTS
{
	public class RewardScreen : MonoBehaviour
	{
		public List<Reward> rewards;
        public List<Reward> specialRewards;
        public List<GameObject> RewardButtons;

        List<Reward> active;

        private void Awake()
        {
            for (int i = 0; i < RewardButtons.Count; i++)
            {
                var button = RewardButtons[i].GetComponentInChildren<Button>();
                var n = i;
                button.onClick.AddListener(() => SelectReward(n));
            }
        }

        private void Start()
        {
            ShowRandom();
        }

        public void ShowRandom()
        {
            active = new List<Reward>();
            var possible = new List<Reward>(rewards);
            for (int i = 0; i < RewardButtons.Count; i++)
            {
                var text = RewardButtons[i].GetComponentInChildren<TMP_Text>();
                var reward = Random.Range(0, possible.Count);
                active.Add(possible[reward]);
                text.text = possible[reward].ToString();
                possible.RemoveAt(reward);
            }
        }

        public void SelectReward(int n)
        {
            active[n].ApplyReward();
            SceneManager.LoadScene("TTS");
        }
	}

	[System.Serializable]
	public class Reward
    {
		public RewardType rewardType;
		public CardDef cardReward;
        public StatID targetStat;
		public int amount;

        public override string ToString()
        {
            switch (rewardType)
            {
                case RewardType.ModifyStat:
                    return $"+{amount} {Benums.Stringify(targetStat)}";
                case RewardType.HP:
                    return $"Heal {amount} HP";
                case RewardType.Card:
                    return $"Card: {cardReward.title}";
            }
            return "empty";
        }

        public void ApplyReward()
        {
            var player = RunRunner.Instance.playerState;
            switch (rewardType)
            {
                case RewardType.ModifyStat:
                    player.PlayerStats.GetStat(targetStat).Value += amount;
                    if (targetStat == StatID.MAX_HP)
                        player.HP.Value = Mathf.Min(player.PlayerStats.MaxHP, player.HP + amount);
                    break;
                case RewardType.HP:
                    player.HP.Value = Mathf.Min(player.PlayerStats.MaxHP, player.HP + amount);
                    break;
                case RewardType.Card:
                    player.GetCards().Add(new Card(cardReward));
                    break;
            }
        }
    }

    public enum RewardType
    {
		ModifyStat,
		Card,
        HP,
    }
}

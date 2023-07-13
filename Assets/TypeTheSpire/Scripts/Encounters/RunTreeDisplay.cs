using TMPro;
using UnityEngine;

namespace Bentendo.TTS
{
    public class RunTreeDisplay : MonoBehaviour
	{
		public GameObject TreeNode;
		public Transform TreeParent;

        private void Start()
        {
			SetupTree(RunRunner.Instance.Current);
        }

        public void SetupTree(EncounterNode node, GameObject parent = null, int depth = 0)
        {
			var crnt = node;
			var go = SetupNode(crnt);
			go.transform.localPosition = ((parent == null) ? Vector3.zero : parent.transform.localPosition) + Vector3.up * 1.5f;
			foreach (EncounterNode next in crnt.NextNodes)
            {
				SetupTree(next,  go, depth + 1);
            }
        }

		GameObject SetupNode(EncounterNode node)
        {
			var go = GameObject.Instantiate(TreeNode, TreeParent);
			go.transform.Find("icon").GetComponent<SpriteRenderer>().sprite = node.Encounter.GetIcon();
			go.transform.Find("title").GetComponent<TMP_Text>().text = node.Encounter.name;
			return go;
        }
	}
}

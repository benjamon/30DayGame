using TMPro;
using UnityEngine;

namespace Bentendo.TTS
{
    public class RunTreeDisplay : MonoBehaviour
	{
		public GameObject TreeNode;
		public Transform TreeParent;

		public void SetupTree(EncounterNode node, GameObject parent = null, int depth = 0)
        {
			var crnt = node;
			int n = 0;
			var go = SetupNode(crnt);
			go.transform.position = parent.transform.position + Vector3.up;
			foreach (EncounterNode next in crnt.NextNodes)
            {
				SetupTree(next, go, depth + 1);
            }
        }

		GameObject SetupNode(EncounterNode node)
        {
			var go = GameObject.Instantiate(TreeNode, TreeParent);
			var icon = go.transform.Find("icon").GetComponent<SpriteRenderer>().sprite = node.Encounter.GetIcon();
			var title = go.transform.Find("title").GetComponent<TMP_Text>().text = node.Encounter.name;
			return go;
        }
	}
}

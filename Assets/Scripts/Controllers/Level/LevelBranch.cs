using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelBranch {
	public int[] TilesTypes;
	public LerpCreatable[] Enemies, Objects;
	public int MinLevel, MaxLevel;

	public void PrepareLevel (float level) {
		if (level<MinLevel) level = MinLevel;
		float t = level/MaxLevel;
		Lerps(Enemies, t);
		Lerps(Objects, t);
	}

	private void Lerps (LerpCreatable[] creatable, float t) {
		for (int i=0; i<creatable.Length; i++)
			creatable[i].LerpBy(t);
	}
}

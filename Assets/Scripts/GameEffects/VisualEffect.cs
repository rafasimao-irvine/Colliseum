using UnityEngine;
using System.Collections;

public class VisualEffect : MonoBehaviour {

	public ParticleSystem Effect;

	public bool DestroySelf;

	public enum EffectType {
		FromOrigin, OnTarget, TowardsTarget
	}
	[SerializeField]
	protected EffectType _Type;

	public void MakeEffect (Interactive origin, Interactive target) {
		switch (_Type) {
		case EffectType.FromOrigin:
			DoEffect(origin.transform.position);
			break;
		case EffectType.OnTarget:
			DoEffect(target.transform.position);
			break;
		case EffectType.TowardsTarget:
			//TODO Do not know if it is necessary
			break;
		}

	}

	private void DoEffect (Vector3 pos) {
		if (Effect != null) {
			pos.y = Effect.transform.position.y;
			Effect.transform.position = pos;
			Effect.Play();
			if (DestroySelf) {
				Effect.transform.parent = transform.parent;
				Destroy(Effect.gameObject,Effect.duration);
			}
		}
	}
}

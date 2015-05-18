using UnityEngine;
using System.Collections;

public interface IReceiver {

	void BeTriggered(Interactive origin,Interactive actor);
}

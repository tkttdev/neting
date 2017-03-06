using UnityEngine.EventSystems;

interface IRecieveMessage : IEventSystemHandler {
	void DeadEnemy ();
	void CopyEnemy ();
}

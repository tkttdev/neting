using UnityEngine.EventSystems;

interface IRecieveMessage : IEventSystemHandler {
	void DeadEnemy(int _id,bool _isCopy);
}

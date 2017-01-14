using UnityEngine.EventSystems;

interface IRecieveMessage : IEventSystemHandler {
	void OnRecieveInfo();
	void OnRecieveInfo (int _allEnemyNum);
}

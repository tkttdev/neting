using System.Collections.ObjectModel;
using System;

public static class ENEMY_DEFINE {

    public const int enemyVarietyNum = 10;
    public static readonly ReadOnlyCollection<int> HP = Array.AsReadOnly(new int[]	   { 1, 1, 1, 1, 1, 1, 1, 2, 1, 3 });
    public static readonly ReadOnlyCollection<int> DAMAGE = Array.AsReadOnly(new int[] { 1, 1, 1, 3, 1, 1, 1, 1, 1, 1 });
	public static readonly ReadOnlyCollection<int> MONEY = Array.AsReadOnly(new int[]  { 10, 20, 30, 25, 55, 50, 55, 100, 75, 125 });
	public static readonly ReadOnlyCollection<string> PATH = Array.AsReadOnly(new string[] {  "Prefabs/Enemy/Enemy0",
                                                                                              "Prefabs/Enemy/Enemy1",
                                                                                              "Prefabs/Enemy/Enemy2",
                                                                                              "Prefabs/Enemy/Enemy3",
                                                                                              "Prefabs/Enemy/Enemy4",
                                                                                              "Prefabs/Enemy/Enemy5",
                                                                                              "Prefabs/Enemy/Enemy6",
                                                                                              "Prefabs/Enemy/Enemy7",
                                                                                              "Prefabs/Enemy/Enemy8",
                                                                                              "Prefabs/Enemy/Enemy9"});
}

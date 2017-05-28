using System.Collections.ObjectModel;
using System;

public static class ENEMY_DEFINE {

    public const int enemyVarietyNum = 18;
    public static readonly ReadOnlyCollection<float> HP = Array.AsReadOnly(new float[]     { 1, 1, 1, 1, 1, 1, 1, 2, 1, 3, 1, 9, 2, (float)1.5, 1 , 2, 2});
    public static readonly ReadOnlyCollection<int> DAMAGE = Array.AsReadOnly(new int[]     { 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1 , 1, 1});
	public static readonly ReadOnlyCollection<int> MONEY = Array.AsReadOnly(new int[]      { 10,20,30,40,50,60,50,80,70,100,0,0,40, 60,70 ,70, 70});
	public static readonly ReadOnlyCollection<float> SPEED = Array.AsReadOnly(new float[]  { 2, 2, 2, 2, 2, 2, 5, 2, 3, 2, 2, 2, 2, 2, 2 , 2 , 5});
	public static readonly ReadOnlyCollection<string> PATH = Array.AsReadOnly(new string[] {  "Prefabs/Enemy/Enemy0",
                                                                                              "Prefabs/Enemy/Enemy1",
                                                                                              "Prefabs/Enemy/Enemy2",
                                                                                              "Prefabs/Enemy/Enemy3",
                                                                                              "Prefabs/Enemy/Enemy4",
                                                                                              "Prefabs/Enemy/Enemy5",
                                                                                              "Prefabs/Enemy/Enemy6",
                                                                                              "Prefabs/Enemy/Enemy7",
                                                                                              "Prefabs/Enemy/Enemy8",
                                                                                              "Prefabs/Enemy/Enemy9",
                                                                                              "Prefabs/Enemy/Enemy10",
																							  "Prefabs/Enemy/Enemy11",
																							  "Prefabs/Enemy/Enemy12",
																							  "Prefabs/Enemy/Enemy13",
																							  "Prefabs/Enemy/Enemy14",
																							  "Prefabs/Enemy/Enemy15",
																							  "Prefabs/Enemy/Enemy16",});
	
}

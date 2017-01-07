using System.Collections.ObjectModel;
using System;

public static class ENEMY_DEFINE {

    public const int enemyVarietyNum = 10;
    public static readonly ReadOnlyCollection<int> HP = Array.AsReadOnly(new int[] { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 });
    public static readonly ReadOnlyCollection<int> DAMAGE = Array.AsReadOnly(new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
    public static readonly ReadOnlyCollection<string> PATHS = Array.AsReadOnly(new string[] { "Prefabs/Enemy/Enemy",
                                                                                              "Prefabs/Enemy/Enemy",
                                                                                              "Prefabs/Enemy/Enemy",
                                                                                              "Prefabs/Enemy/Enemy",
                                                                                              "Prefabs/Enemy/Enemy",
                                                                                              "Prefabs/Enemy/Enemy",
                                                                                              "Prefabs/Enemy/Enemy",
                                                                                              "Prefabs/Enemy/Enemy",
                                                                                              "Prefabs/Enemy/Enemy",
                                                                                              "Prefabs/Enemy/Enemy"});
}

using System.Collections.ObjectModel;
using System;

public static class ENEMY_DEFINE {

    public const int enemyVarietyNum = 10;
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

using System.Collections.ObjectModel;
using System;
using UnityEngine;

public enum CharacterID : int {
    CHARA0 = 0,
    CHARA1 = 1,
    CHARA2 = 2,
    CHARA3 = 3,
    CHARA4 = 4,
}

public static class CHARACTER_STATUS_DEFINE {

    public static readonly ReadOnlyCollection<int>    HP                  = Array.AsReadOnly(new int[] { 3, 4, 2, 4, 5 });
    public static readonly ReadOnlyCollection<int>    MAX_BULLET_STOCK    = Array.AsReadOnly(new int[] { 3, 2, 5, 5, 2 });
    public static readonly ReadOnlyCollection<int>    BULLET_INTERVAL     = Array.AsReadOnly(new int[] { 3, 3, 3, 1, 4 });
    public static readonly ReadOnlyCollection<string> BULLET_PREFAB_PATHS = Array.AsReadOnly(new string[] { "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet"});
}

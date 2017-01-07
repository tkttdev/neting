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

    public static readonly ReadOnlyCollection<int>    HP                  = Array.AsReadOnly(new int[]    {    3,    4,    2,    4,    5,    2,    4,    7,   3,     8 });
    public static readonly ReadOnlyCollection<int>    MAX_BULLET_STOCK    = Array.AsReadOnly(new int[]    {    3,    2,    5,    5,    2,    3,    4,    5,   7,    10 });
    public static readonly ReadOnlyCollection<float>  BULLET_INTERVAL     = Array.AsReadOnly(new float[]  { 2.5f, 1.2f, 3.0f, 1.0f, 2.0f, 1.0f, 2.0f, 2.0f, 3.5f, 1.3f });
    public static readonly ReadOnlyCollection<string> BULLET_PREFAB_PATHS = Array.AsReadOnly(new string[] { "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",});
}

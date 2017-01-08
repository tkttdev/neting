using System.Collections.ObjectModel;
using System;

public enum CharacterID : int {
    CHARA0 = 0,
    CHARA1 = 1,
    CHARA2 = 2,
    CHARA3 = 3,
    CHARA4 = 4,
}

public static class CHARACTER_DEFINE {

    public const int characterVarietyNum = 10;
    public static readonly ReadOnlyCollection<int>    LIFE                = Array.AsReadOnly(new int[]    {    3,    4,    2,    4,    5,    2,    4,    7,   3,     8 });
    public static readonly ReadOnlyCollection<int>    MAX_BULLET_STOCK    = Array.AsReadOnly(new int[]    {    3,    2,    5,    5,    2,    3,    4,    5,   7,    10 });
    public static readonly ReadOnlyCollection<float>  BULLET_INTERVAL     = Array.AsReadOnly(new float[]  { 10.0f, 1.2f, 3.0f, 1.0f, 2.0f, 1.0f, 2.0f, 2.0f, 3.5f, 1.3f });
    public static readonly ReadOnlyCollection<int>    BULLET_DAMAGE       = Array.AsReadOnly(new int[]    {   30,   30,   30,   30,   30,   30,   30,   30,   30,   10 });
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

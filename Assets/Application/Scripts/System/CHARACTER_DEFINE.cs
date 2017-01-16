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
	public static readonly ReadOnlyCollection<string> NAME                = Array.AsReadOnly(new string[] { "TYPE-ALPHA","TYPE-BETA","TYPE-GAMMA","TYPE-DELTA","TYPE-EPSILON","TYPE-ZETA","TYPE-ETA","TYPE-THETA","TYPE-IOTA","TYPE-KAPPA"});
	public static readonly ReadOnlyCollection<int>    MONEY               = Array.AsReadOnly(new int[]    {   30,   30,   30,   30,   30,   30,   30,   30,   30,   30 });
    public static readonly ReadOnlyCollection<int>    LIFE                = Array.AsReadOnly(new int[]    {    3,    3,    2,    3,    4,    2,    3,    2,    4,    1 });
    public static readonly ReadOnlyCollection<int>    MAX_BULLET_STOCK    = Array.AsReadOnly(new int[]    {    3,    2,    5,    3,    1,    2,    4,    5,    3,    1 });
    public static readonly ReadOnlyCollection<float>  BULLET_INTERVAL     = Array.AsReadOnly(new float[]  { 2.0f, 1.5f, 2.0f, 2.5f, 1.0f, 2.0f, 1.5f, 2.0f, 2.5f, 0.5f });
    public static readonly ReadOnlyCollection<int>    BULLET_DAMAGE       = Array.AsReadOnly(new int[]    {    1,    1,    1,    2,    1,    2,    1,    2,    3,    1 });
    public static readonly ReadOnlyCollection<string> BULLET_PREFAB_PATH  = Array.AsReadOnly(new string[] { "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet",
                                                                                                            "Prefabs/Bullet/CharacterBullet"});
	public static readonly ReadOnlyCollection<string> IMAGE_RESOURCES_PATH  = Array.AsReadOnly(new string[]{ "Images/Chara/Alpha",
		"Images/Chara/Beta",
		"Images/Chara/Gamma",
		"Images/Chara/Delta",
		"Images/Chara/Epsilon",
		"Images/Chara/Zeta",
		"Images/Chara/Eta",
		"Images/Chara/Theta",
		"Images/Chara/IOTA",
		"Images/Chara/KAPPA"});
}


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
	public static readonly ReadOnlyCollection<string> NAME                = Array.AsReadOnly(new string[] { "ATLANTA","BISMARCK","CHESTER","TYPE-DELTA","TYPE-EPSILON","TYPE-ZETA","TYPE-ETA","TYPE-THETA","TYPE-IOTA","TYPE-KAPPA"});
	public static readonly ReadOnlyCollection<int>    MONEY               = Array.AsReadOnly(new int[]    {    0,   2000,   4000,   6500,   9000,   11500,   14000,   18000,   25000,   50000 });
    public static readonly ReadOnlyCollection<int>    MAX_BULLET_STOCK    = Array.AsReadOnly(new int[]    {    3,    2,    5,    2,    1,    4,    3,    5,    4,    1 });
    public static readonly ReadOnlyCollection<float>  BULLET_INTERVAL     = Array.AsReadOnly(new float[]  { 2.0f, 1.6f, 2.0f, 2.2f, 1.2f, 2.0f, 1.6f, 2.0f, 2.0f, 1.0f });
    public static readonly ReadOnlyCollection<int>    BULLET_DAMAGE       = Array.AsReadOnly(new int[]    {    1,    1,    1,    2,    1,    2,    1,    2,    3,    1 });
    public static readonly ReadOnlyCollection<string> BULLET_PREFAB_PATH  = Array.AsReadOnly(new string[] { "Prefabs/Bullet/CharacterBullet0",
                                                                                                            "Prefabs/Bullet/CharacterBullet1",
                                                                                                            "Prefabs/Bullet/CharacterBullet2",
                                                                                                            "Prefabs/Bullet/CharacterBullet3",
                                                                                                            "Prefabs/Bullet/CharacterBullet4",
                                                                                                            "Prefabs/Bullet/CharacterBullet5",
                                                                                                            "Prefabs/Bullet/CharacterBullet6",
                                                                                                            "Prefabs/Bullet/CharacterBullet7",
                                                                                                            "Prefabs/Bullet/CharacterBullet8",
                                                                                                            "Prefabs/Bullet/CharacterBullet9"});
	
	public static readonly ReadOnlyCollection<string> IMAGE_RESOURCES_PATH  = Array.AsReadOnly(new string[]{ "Images/Chara/Alpha",
		"Images/Chara/Beta",
		"Images/Chara/Gamma",
		"Images/Chara/Delta",
		"Images/Chara/Epsilon",
		"Images/Chara/Zeta",
		"Images/Chara/Eta",
		"Images/Chara/Theta",
		"Images/Chara/Iota",
		"Images/Chara/Kappa"});
	
	public static readonly ReadOnlyCollection<string> FACE_IMAGE_RESOURCES_PATH  = Array.AsReadOnly(new string[]{ "Images/Chara/FaceAlpha",
		"Images/Chara/FaceBeta",
		"Images/Chara/FaceGamma",
		"Images/Chara/FaceDelta",
		"Images/Chara/FaceEpsilon",
		"Images/Chara/FaceZeta",
		"Images/Chara/FaceEta",
		"Images/Chara/FaceTheta",
		"Images/Chara/FaceIota",
		"Images/Chara/FaceKappa"});
}


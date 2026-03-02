using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    public static List<GameObject> enemyPrefabs = new List<GameObject>();

    public static string enemyCombatID;
    public static List<string> defeatedEnemies = new List<string>();
    public static Vector2 playerReturnPos = Vector2.zero;

    public static int playerHealth = -1;
    public static int playerLevel = 1;
    public static int playerXP;
    public static int nextLevelXP = 100;
    public static int playerEcon;
}

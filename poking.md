# poking around

- save editor: https://bloodorca.github.io/hollow/

```cs
// also GameManager.CreateRestorePoint
private void GameManager.SaveGame(/**/) {
    if (!CheatManager.AllowSaving) {        // public static bool CheatManager.AllowSaving
        // ...
    }
    else if (/**/) {
        // ...
        if (!gameConfig.disableSaveGame) {  // public bool GameConfig.disableSaveGame
            // ...
        }
    }
}
```

```cs
public static bool CheatManager.IsWorldRumbleDisabled
// https://thunderstore.io/c/hollow-knight-silksong/p/tisawesomeness/ShutUpGMS/
// https://github.com/Tisawesomeness/ShutUpGMS/blob/2433cf4295eb5756b79ae4c4a2dc11c097337b8d/ShutUpGMS.cs
// ^ uses harmony prefix patch on WorldRumbleManager.DoNewRumble instead (arbitrary(?) __result and skip orig)
// Maybe more desirable as less time in yield while loop and not modifying a variable that might (however unlikely) be saved?
```

```cs
public int grubFarmLevel;
public bool PlayerData.farmer_grubGrown_3; // is the third upgrade accessible??
```

# needle swapper

A small mod that allows you to change which Needle upgrade Hornet uses.

## why?
i want to be able to downgrade my needle so bosses don't die so quickly!

*(e.g. cleaning up optional bosses, [Godhome](https://thunderstore.io/c/hollow-knight-silksong/p/Cuckson/Godhome/))*

## how?

- with the Needle selected in the Inventory menu:
    - use the Sprint input *(default: `c`)* to "downgrade" the needle
        - use the Attack input on controller instead(? — should be transforming/reloading certain Tools)
    - use the Jump input *(default: `z`; equivalent on controller)* to "upgrade" the needle
<!--  -->
- ranges from the following (from downgraded to upgraded):
    - unmodded needle *(i.e. what you have without the mod)* ← **default setting**
    - needle
    - sharpened needle
    - shining needle
    - hivesteel needle
    - pale steel needle
<!--  -->
- "downgrading" all the way uses your default needle upgrade
- by default, you cannot "upgrade" past your current max upgrade level
    - *e.g. if you've upgraded to Shining Needle via the Pinmaster (i.e. vanilla), you'll be capped at Shining Needle until you obtain Hivesteel Needle via the Pinmaster*
        - ***it (should!) be safe to upgrade your needle as if this was not installed***
            - *e.g. if you have Hivesteel Needle but are using Sharpened Needle via the mod, Pinmaster will still unlock the Pale Steel Needle when you give him Pale Oil*
    - the mod's internal tracker can still increase to the last possible upgrade, so you may need to hit the "downgrade" input multiple times to see a change
        - *e.g. if you're capped at Shining Needle but have spammed "upgrade", you may need to "downgrade" multiple times before Shining Needle becomes Sharpened Needle*
        - this is because the `targetNeedleUpgradeLevel` is shared across save files
    - you can remove this limit by setting the config option `allowTargetAboveUpgradedLevel` to `true`
<!--  -->
- only modifies the inventory appearance and damage calculations
    - *i.e. doesn't (shouldn't) affect completion percentage / game progression; non-permanent*
    - note: [damage calculation](https://hollowknight.wiki/w/Damage_Values_and_Enemy_Health_(Silksong)#Damage_Calculation) is complicated in Silksong because needle damage is multiplied by enemy-specific modifiers dependent on which needle upgrade you have
<!--  -->
- **this mod should be safe to install and uninstall without affecting your save file at all**
    - **however, you should always make a backup of your save files, just in case**
    - testing was conducted, but i am only one person

## issues? questions? fixes?
- open an issue on the [GitHub repository](https://github.com/itsschwer/silksong-needle-swapper/issues)!
    - bug reports
    - suggestions *(e.g. README readability)*

## todo
- only allow at bench
- fix non-working audio feedback — ***need help!***
    - *open an issue on the GitHub repository if you have any info!*

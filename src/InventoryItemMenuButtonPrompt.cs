using GlobalEnums;

namespace PaleOilSoap
{
    /// <summary>
    /// The Voltvessels transform prompt uses MenuButtonIcon, which converts a Platform.MenuActions to a HeroActionButton when updating the display,
    /// whereas InventoryItemButtonPrompt is fed into ActionButtonIcon, which only has a set HeroActionButton with no conversion when updating the display.
    ///
    /// The least invasive workaround seems to be updating the data source right before it is fed to ActionButtonIcon.
    /// This means that the button prompt will be correct for the input type at the time of selection,
    /// but may desync if the input type is changed while the item is selected (deselecting and reselecting will resync).
    /// </summary>
    internal sealed class InventoryItemMenuButtonPrompt : InventoryItemButtonPrompt
    {
        private Platform.MenuActions _menuAction;
        public Platform.MenuActions menuAction {
            get {
                return _menuAction;
            }
            set {
                _menuAction = value;
                data.Action = Action;
            }
        }

        public HeroActionButton Action {
            // see global::MenuButtonIcon.Action
            get {
                if (Platform.Current.WasLastInputKeyboard) {
                    switch (menuAction) {
                        case Platform.MenuActions.Submit:
                            return HeroActionButton.JUMP;
                        case Platform.MenuActions.Cancel:
                            return HeroActionButton.CAST;
                        case Platform.MenuActions.Extra:
                            return HeroActionButton.DASH;
                        case Platform.MenuActions.Super:
                            return HeroActionButton.DREAM_NAIL;
                    }
                }
                else {
                    switch (menuAction) {
                        case Platform.MenuActions.Submit:
                            return HeroActionButton.MENU_SUBMIT;
                        case Platform.MenuActions.Cancel:
                            return HeroActionButton.MENU_CANCEL;
                        case Platform.MenuActions.Extra:
                            return HeroActionButton.MENU_EXTRA;
                        case Platform.MenuActions.Super:
                            return HeroActionButton.MENU_SUPER;
                    }
                }
                return HeroActionButton.MENU_CANCEL;
            }
        }

        public override void OnShow(InventoryItemButtonPromptDisplayList displayList, InventoryItemButtonPromptData data)
        {
            // Will only update upon reselecting the inventory item (compromise)
            data.Action = Action;
            base.OnShow(displayList, data);
        }

        private void Awake()
        {
            // Avoid NRE from unassigned appearCondition
            enabled = false;
        }
    }
}

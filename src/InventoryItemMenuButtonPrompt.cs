using GlobalEnums;

namespace PaleOilSoap
{
    internal class InventoryItemMenuButtonPrompt : InventoryItemButtonPrompt
    {
        private InputHandler ih; // see global::ActionButtonIconBase

        internal Platform.MenuActions menuAction;
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

        // Base class has a private OnEnable, so cannot override
        private void Awake()
        {
            if (ih == null) {
                ih = GameManager.instance.inputHandler;
            }
            if (ih != null) {
                ih.RefreshActiveControllerEvent += Ih_RefreshActiveControllerEvent;
            }
        }

        // Base class has a private OnDisable, so cannot override
        private void OnDestroy()
        {
            if (ih != null) {
                ih.RefreshActiveControllerEvent -= Ih_RefreshActiveControllerEvent;
            }
        }

        private void Ih_RefreshActiveControllerEvent()
        {
            Plugin.Logger.LogWarning(data.Action);
            data.Action = Action;
            Plugin.Logger.LogInfo(data.Action);
        }
    }
}

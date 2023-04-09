using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ScreenSystem;

namespace DonateToGranary
{
    public partial class DonateFoodBaseBehavior : CampaignBehaviorBase
    {

        public static void CreatePopupVMLayer(string title, string smallText, string bigText, string textOverImage, string spriteName, string closeButtonText)
        {
            try
            {
                bool flag = _gauntletLayer != null;
                if (!flag)
                {
                    _gauntletLayer = new GauntletLayer(1000, "GauntletLayer", false);

                    bool flag2 = _popupVM == null;
                    if (flag2)
                    {
                        // Localization
                        TextObject titleTO = new(title);
                        TextObject smallTextTO = new(smallText);
                        TextObject bigTextTO = new(bigText);
                        TextObject textOverImageTO = new(textOverImage);
                        TextObject closeButtonTextTO = new(closeButtonText);

                        _popupVM = new DTGPopupVM(titleTO.ToString(), smallTextTO.ToString(), bigTextTO.ToString(), textOverImageTO.ToString(), spriteName, closeButtonTextTO.ToString());
                    }
                    _gauntletMovie = (GauntletMovie)_gauntletLayer.LoadMovie("LTPopup", _popupVM);
                    _gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
                    ScreenManager.TopScreen.AddLayer(_gauntletLayer);
                    _gauntletLayer.IsFocusLayer = true;
                    ScreenManager.TrySetFocus(_gauntletLayer);
                    if (_popupVM != null) _popupVM.Refresh();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public static void DeletePopupVMLayer()
        {
            ScreenBase topScreen = ScreenManager.TopScreen;
            
            if (_gauntletLayer != null)
            {
                _gauntletLayer.InputRestrictions.ResetInputRestrictions();
                _gauntletLayer.IsFocusLayer = false;
                bool flag2 = _gauntletMovie != null;
                if (flag2)
                {
                    _gauntletLayer.ReleaseMovie(_gauntletMovie);
                }
                topScreen.RemoveLayer(_gauntletLayer);
            }

            _gauntletLayer = null;
            _gauntletMovie = null;
            _popupVM = null;
        }

    }
}

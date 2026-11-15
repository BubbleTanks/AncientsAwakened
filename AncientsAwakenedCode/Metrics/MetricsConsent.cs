using AncientsAwakened.AncientsAwakenedCode.UI;
using BaseLib.Config;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

namespace AncientsAwakened.AncientsAwakenedCode.Metrics;

[HarmonyPatch(
    typeof(NCharacterSelectScreen),
    nameof(NCharacterSelectScreen.SelectCharacter))]
internal static class MetricsConsentPatch
{
    [HarmonyPostfix]
    internal static void Postfix()
    {
        if (!AncientConfigs.UploadMetricsFtueSeen)
        {
            AncientsAwakenedMetricsConsent.Show();
        }
    }
}

public static class AncientsAwakenedMetricsConsent
{
    public static void Show()
    {
        NGenericPopup? promptPopup =
            NGenericPopup.Create();

        if (promptPopup == null ||
            NModalContainer.Instance == null)
        {
            return;
        }

        promptPopup.Connect(
            Node.SignalName.Ready,
            Callable.From(() =>
            {
                NVerticalPopup popup =
                    promptPopup.GetNode<NVerticalPopup>(
                        (NodePath)"VerticalPopup");

                popup.SetText(
                    new LocString(
                        "main_menu_ui",
                        "ANCIENTSAWAKENED-METRICS_FTUE_PROMPT.header"),

                    new LocString(
                        "main_menu_ui",
                        "ANCIENTSAWAKENED-METRICS_FTUE_PROMPT.body"));

                popup.InitYesButton(
                    new LocString(
                        "main_menu_ui",
                        "GENERIC_POPUP.confirm"),
                    _ =>
                    {
                        ClosePopup(promptPopup);
                        AfterSelection(true);
                    });

                popup.InitNoButton(
                    new LocString(
                        "main_menu_ui",
                        "GENERIC_POPUP.cancel"),
                    _ =>
                    {
                        ClosePopup(promptPopup);
                        AfterSelection(false);
                    });
            }),
            (uint)GodotObject.ConnectFlags.OneShot);

        NModalContainer.Instance.CallDeferred(
            NModalContainer.MethodName.Add,
            promptPopup,
            true);
    }

    private static void AfterSelection(bool enabled)
    {
        AncientConfigs.UploadMetrics = enabled;
        AncientConfigs.UploadMetricsFtueSeen = true;

        ModConfig.SaveDebounced<AncientConfigs>();

        ShowConfirmation(enabled);
    }

    private static void ShowConfirmation(bool enabled)
    {
        NGenericPopup? messagePopup =
            NGenericPopup.Create();

        if (messagePopup == null ||
            NModalContainer.Instance == null)
        {
            return;
        }

        messagePopup.Connect(
            Node.SignalName.Ready,
            Callable.From(() =>
            {
                LocString header = new(
                    "main_menu_ui",
                    enabled
                        ? "ANCIENTSAWAKENED-METRICS_FTUE_ENABLED.header"
                        : "ANCIENTSAWAKENED-METRICS_FTUE_DISABLED.header");

                LocString body = new(
                    "main_menu_ui",
                    enabled
                        ? "ANCIENTSAWAKENED-METRICS_FTUE_ENABLED.body"
                        : "ANCIENTSAWAKENED-METRICS_FTUE_DISABLED.body");

                NVerticalPopup popup =
                    messagePopup.GetNode<NVerticalPopup>(
                        (NodePath)"VerticalPopup");

                popup.SetText(
                    header,
                    body);

                popup.InitYesButton(
                    new LocString(
                        "main_menu_ui",
                        "GENERIC_POPUP.ok"),
                    _ => ClosePopup(messagePopup));

                popup.HideNoButton();
            }),
            (uint)GodotObject.ConnectFlags.OneShot);

        NModalContainer.Instance.CallDeferred(
            NModalContainer.MethodName.Add,
            messagePopup,
            true);
    }

    private static void ClosePopup(
        NGenericPopup popup)
    {
        popup.QueueFreeSafely();
        NModalContainer.Instance?.Clear();
    }
}
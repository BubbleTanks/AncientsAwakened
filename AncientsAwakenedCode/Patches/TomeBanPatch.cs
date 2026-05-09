using BaseLib.Patches.Content;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

public class TomeBanPatch
{
    [HarmonyPatch(typeof(DustyTomePatch), nameof(DustyTomePatch.CustomTome), MethodType.Getter)]
    public static class TomeBanPatchCustomTome
    {
        public static void Prefix()
        {
            if (!DustyTomePatch._initialized)
            {
                DustyTomePatch._customTome[ModelDb.Character<Ironclad>().Id] = [ModelDb.Card<Corruption>().Id];
                DustyTomePatch._customTome[ModelDb.Character<Silent>().Id] = [ModelDb.Card<WraithForm>().Id];
                DustyTomePatch._customTome[ModelDb.Character<Regent>().Id] = [ModelDb.Card<TheSealedThrone>().Id];
                DustyTomePatch._customTome[ModelDb.Character<Necrobinder>().Id] = [ModelDb.Card<ForbiddenGrimoire>().Id];
                DustyTomePatch._customTome[ModelDb.Character<Defect>().Id] = [ModelDb.Card<BiasedCognition>().Id];
            }
        }
    }
}
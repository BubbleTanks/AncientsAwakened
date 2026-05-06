using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace AncientsAwakened.AncientsAwakenedCode.Patches;

public class SaltyPatch
{
    
    public static readonly SavedSpireField<PotionModel, bool> SaltyField = new(() => false, "ancientsawakened-salty");
    
    [HarmonyPatch(typeof(PotionModel), nameof(PotionModel.Title),  MethodType.Getter)]
    public class SaltyPotions
    {
        public static void Postfix(PotionModel __instance, ref LocString __result)
        {
            if (SaltyField.Get(__instance))
            {
                LocString saltyLoc = new LocString("potions", "ANCIENTSAWAKENED-SALTY.title");

                saltyLoc.Add("Potion", __result);
                
                __result = saltyLoc;
            }
        }
    }

     /* fuck ass shaders that dont work because potions dont have anything related to their fucking texture >:(
      public class SaltyShaders
    {
        public static void GoFuckYourself()
        {
            var shaderCode = @"
            shader_type canvas_item;
            uniform vec4 tint_color : source_color = vec4(0.6, 0.6, 0, 1.0);
            void fragment() {
                vec4 tex = texture(TEXTURE, UV);
                COLOR = vec4(tint_color.rgb, tex.a);
            }";
            var shader = new Shader();
            shader.Code = shaderCode;
            var material = new ShaderMaterial();
            material.Shader = shader;
            material.SetShaderParameter("tint_color", Colors.DarkCyan);
            icon.Material = material;
        }
    } */
}
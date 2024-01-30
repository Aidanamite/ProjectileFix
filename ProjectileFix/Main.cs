using BepInEx;
using HarmonyLib;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace ProjectileFix
{
    [BepInPlugin("Aidanamite.ProjectileFix", "ProjectileFix", "1.0.1")]
    public class Main : BaseUnityPlugin
    {
        internal static Assembly modAssembly = Assembly.GetExecutingAssembly();
        internal static string modName = $"{modAssembly.GetName().Name}";
        internal static string modDir = $"{Environment.CurrentDirectory}\\BepInEx\\{modName}";

        void Awake()
        {
            new Harmony($"com.Aidanamite.{modName}").PatchAll(modAssembly);
            Logger.LogInfo($"{modName} has loaded");
        }
    }

    [HarmonyPatch(typeof(AreaEffect))]
    class Patch_AreaEffect
    {
        static List<AreaEffect> landed = new List<AreaEffect>();
        [HarmonyPatch("Spawn")]
        static void Postfix(AreaEffect __instance)
        {
            landed.RemoveAll((x) => !x);
            landed.Add(__instance);
        }

        [HarmonyPatch("ApplyDamageToPlayer")]
        static bool Prefix(AreaEffect __instance) => landed.Contains(__instance);
    }
}
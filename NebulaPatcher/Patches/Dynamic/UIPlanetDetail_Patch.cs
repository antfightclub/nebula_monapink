﻿using HarmonyLib;
using NebulaAPI;
using NebulaModel.Packets.Universe;
using NebulaWorld;
using System;
using UnityEngine;

namespace NebulaPatcher.Patches.Dynamic
{
    [HarmonyPatch(typeof(UIPlanetDetail))]
    internal class UIPlanetDetail_Patch
    {
        private static int backupUniverseObserveLevel = -1;

        [HarmonyPostfix]
        [HarmonyPatch(nameof(UIPlanetDetail.OnNameInputEndEdit))]
        public static void OnNameInputEndEdit_Postfix(UIPlanetDetail __instance)
        {
            if (Multiplayer.IsActive && !Multiplayer.Session.Factories.IsIncomingRequest.Value)
            {
                if (__instance.planet != null && !string.IsNullOrEmpty(__instance.planet.overrideName))
                {
                    // Send packet with new planet name
                    Multiplayer.Session.Network.SendPacket(new NameInputPacket(__instance.planet.overrideName, NebulaModAPI.STAR_NONE, __instance.planet.id, Multiplayer.Session.LocalPlayer.Id));
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(UIPlanetDetail._OnUpdate))]
        public static bool _OnUpdate_Prefix(UIPlanetDetail __instance)
        {
            if(UIRoot.instance.galaxySelect == null || UIRoot.instance.galaxySelect.starmap == null || UIRoot.instance.galaxySelect.starmap.clickText == "")
            {
                return true;
            }

            if (Time.frameCount % 30 == 0)
            {
                __instance.RefreshDynamicProperties();
            }
            __instance.trslBg.gameObject.SetActive(true);
            __instance.imgBg.gameObject.SetActive(true);

            return false;
        }

        // temp set universe exploration to max for planet detail display in galaxy select screen
        [HarmonyPrefix]
        [HarmonyPatch(nameof(UIPlanetDetail.OnPlanetDataSet))]
        [HarmonyPatch(nameof(UIPlanetDetail.RefreshDynamicProperties))]
        public static bool OnPlanetDataSet_Prefix(UIPlanetDetail __instance)
        {
            if(UIRoot.instance.galaxySelect.starmap.clickText != "")
            {
                backupUniverseObserveLevel = GameMain.history.universeObserveLevel;
                GameMain.history.universeObserveLevel = 3;
            }
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(UIPlanetDetail.OnPlanetDataSet))]
        [HarmonyPatch(nameof(UIPlanetDetail.RefreshDynamicProperties))]
        public static void OnPlanetDataSet_Postfix(UIPlanetDetail __instance)
        {
            if(backupUniverseObserveLevel != -1)
            {
                GameMain.history.universeObserveLevel = backupUniverseObserveLevel;
                backupUniverseObserveLevel = -1;
            }
        }
    }
}

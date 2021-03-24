﻿using HarmonyLib;

/*
 * This Patch is part of the remote factory loading process.
 * As the client requests the factory the localPlanet is already set by the ArrivePlanet() method.
 * As UIPowerGizmo::_OnUpdate() lacks a null check on the factory of the localPlanet we need to do that here
 * because the factory data may not be received/loaded by this time.
 */
namespace NebulaPatcher.Patches.Dynamic
{
    class UIPowerGizmo_OnUpdate_Patch
    {
        [HarmonyPatch(typeof(UIPowerGizmo), "_OnUpdate")]
        class OnUpdatePatch
        {
            public static bool Prefix()
            {
                return GameMain.localPlanet?.factory != null;
            }
        }
    }
}
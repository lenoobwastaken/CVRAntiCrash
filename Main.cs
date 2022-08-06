 using System;
using System.Collections;
using System.Linq;
using MelonLoader;
using UnityEngine;
using Harmony;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using ABI_RC.Core.Player;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using ABI.CCK.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CVRANTICRASH
{
   
    public class Main : MelonMod
    {
        
       
        public static void CheckProp()
        {
            if (!ConfigMan.GetConfig().PropAnti)
            {
                return;
            }
            foreach (var s in GameObject.FindObjectsOfType<ABI.CCK.Components.CVRPickupObject>())
            {
                if (s.gameObject.name == "QuickMenu")
                {
                    return;
                }
                if (s.GetComponentInChildren<SkinnedMeshRenderer>() != null)
                {
                    foreach (var w in s.GetComponentsInChildren<SkinnedMeshRenderer>())
                    {
                        foreach (var ww in w.materials)
                        {
                            ww.shader = Shader.Find("Standard");
                        }
                    }
                }
                if (s.GetComponentInChildren<MeshRenderer>() != null)
                {
                    foreach (var w in s.GetComponentsInChildren<MeshRenderer>())
                    {
                        foreach (var ww in w.materials)
                        {
                            ww.shader = Shader.Find("Standard");
                        }
                    }
                }
            }
        }
      
        
        private static Harmony.HarmonyInstance Instance = new Harmony.HarmonyInstance(Guid.NewGuid().ToString());
       
        public override void OnApplicationStart()
        {

            ConfigMan.SaveConfig();
          //  ConfigMan.LoadConfig();
            MelonLogger.Msg("To Turn on/off shader anti go to CVRMG/Config.txt and change the on/off to off/on");
            Patch();
          //  MelonCoroutines.Start(WaitForUi());
          //  new Config();
        }
        private  static List<CVRPickupObject> pickupList = new List<CVRPickupObject>();
        private static void OnPickupStart()
        {
            CheckProp();
        }
      
        public static void ViewDropTextPatch(string __0, string __1)
        {
            __0 += "Joe \n";
        }
        private static  void Patch()
        {
            Instance.Patch(typeof(ABI.CCK.Components.CVRPickupObject).GetMethod(nameof(ABI.CCK.Components.CVRPickupObject.Start)), null, typeof(Main).GetMethod(nameof(OnPickupStart), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).ToNewHarmonyMethod());
            Instance.Patch(typeof(ABI_RC.Core.Player.PuppetMaster).GetMethod(nameof(ABI_RC.Core.Player.PuppetMaster.AvatarInstantiated)), null, typeof(Main).GetMethod(nameof(OnAvatarChanged), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).ToNewHarmonyMethod());

        }
        private static void OnAvatarChanged(ABI_RC.Core.Player.PuppetMaster __instance)
        {
            bool avatarb = false;

            if (ABI_RC.Core.Networking.IO.Social.Friends.FriendsWith(__instance.gameObject.name) && ConfigMan.GetConfig().WhitelistFriend)
            {
                MelonLogger.Msg("Friends with player returning...");
                avatarb = true;
            }
            if (ConfigMan.GetConfig().UserWhiteList.Contains(__instance.GetComponentInChildren<PlayerDescriptor>().userName))
            {
                MelonLogger.Msg("Player WhiteListed Returning...");
                avatarb = true;
            }
            if (ConfigMan.GetConfig().UserBlackList.Contains(__instance.GetComponentInChildren<PlayerDescriptor>().userName))
            {
                __instance.GetComponentInChildren<IndexIK>().gameObject.SetActive(false);
                MelonLogger.Msg("Player BlackListed Returning...");
                avatarb = true;
            }
            foreach (var q in ABI_RC.Core.Player.CVRPlayerManager.Instance.NetworkPlayers)
            {
                if (q.Username == __instance.GetComponentInChildren<PlayerDescriptor>().userName)
                {
                    MelonLogger.Msg($"Loading {q.AvatarId} ");
                    if (ConfigMan.GetConfig().AvatarWhitelist.Contains(q.AvatarId))
                    {
                       //__instance.GetComponentInChildren<IndexIK>().gameObject.SetActive(false);
                        MelonLogger.Msg("Avatar BlackListed Returning...");
                        avatarb = true;
                      //  return;

                    }
                    if (ConfigMan.GetConfig().AvatarBlackList.Contains(q.AvatarId)) 
                    {
                        __instance.GetComponentInChildren<IndexIK>().gameObject.SetActive(false);
                        MelonLogger.Msg("Avatar BlackListed Returning...");
                        avatarb = true;

                    }
                }
            }
            int dynamicbonecount = 0;
            int particlesystemcount = 0;
            int meshrenderercount = 0;
            int skinnedmeshrcount = 0;
            int audio = 0;
            int lightcount = 0;
            int dynamicbonecollidercount = 0;
            int transform = 0;
            int meshcount = 0;
            MelonLogger.Msg($"Initiating Checks on {__instance.gameObject.GetComponentInChildren<PlayerDescriptor>().userName}");
            ABI_RC.Core.UI.CohtmlHud.Instance.ViewDropText($"Initiating Checks on {__instance.gameObject.GetComponentInChildren<PlayerDescriptor>().userName}'s avatar", "CVRAntiCrash by lenoob#2972");
            if (ConfigMan.GetConfig().AntiSus && !avatarb)
            {
                foreach (var s in __instance.gameObject.GetComponentsInChildren<Transform>() )
                {
                    if (transform >= 100000)
                    {
                        MelonLogger.Msg("Avatar hidden due to being to suspicous" + $" on {__instance.GetComponentInChildren<PlayerDescriptor>().userName}'s avatar");
                        ABI_RC.Core.UI.CohtmlHud.Instance.ViewDropText("CVRAntiCrash", "Avatar hidden due to being to suspicous" + $" on {__instance.GetComponentInChildren<PlayerDescriptor>().userName}'s avatar", "CVRAntiCrash");
                        __instance.GetComponentInChildren<IndexIK>().gameObject.SetActive(false);
                    }
                    transform += 1;
                }
            }
            if (__instance.gameObject.GetComponent<AudioSource>() != null && ConfigMan.GetConfig().Audio && !avatarb)
            {
                foreach (var w in __instance.gameObject.GetComponentsInChildren<AudioSource>())
                {
                    if (audio >= 1000)
                    {

                    }
                    audio += 1;
                }
            }
            if (__instance.gameObject.GetComponentInChildren<Light>() != null && ConfigMan.GetConfig().Light && !avatarb)
            {
                foreach (var w in __instance.gameObject.GetComponentsInChildren<Light>())
                {
                    if (lightcount >= 100)
                    {
                        MelonLogger.Msg("Light Limit has been reached " + lightcount + $" on {__instance.GetComponentInChildren<PlayerDescriptor>().userName}'s avatar");

                        GameObject.Destroy(w);
                    }
                    lightcount += 1;
                }
            }
            if (__instance.gameObject.GetComponentInChildren<Mesh>() != null && ConfigMan.GetConfig().Mesh && !avatarb)
            {
                foreach (var w in __instance.gameObject.GetComponentsInChildren<Mesh>())
                {
                    
                    if (meshcount >= 2000)
                    {
                        MelonLogger.Msg("Mesh Lenght Limit has been reached " + meshcount + $" on {__instance.GetComponentInChildren<PlayerDescriptor>().userName}'s avatar");

                        GameObject.Destroy(w);
                    }
                    if (w.vertexCount >= 100000)
                    {
                        MelonLogger.Msg("Mesh Poly Limit has been reached " + w.vertexCount + $" on {__instance.GetComponentInChildren<PlayerDescriptor>().userName}'s avatar");

                        GameObject.Destroy(w);
                    }

                    meshcount += 1;
                }
            }

            if (__instance.gameObject.GetComponentInChildren<ParticleSystem>() != null && ConfigMan.GetConfig().Particle && !avatarb)
            {
                foreach (var w in __instance.gameObject.GetComponentsInChildren<ParticleSystem>())
                {
                    if (particlesystemcount >= 2000)
                    {
                        MelonLogger.Msg("ParticleSystem Lenght Limit has been reached " + particlesystemcount + $" on {__instance.GetComponentInChildren<PlayerDescriptor>().userName}'s avatar");

                        GameObject.Destroy(w);
                    }
                    if (w.particleCount >= 100000)
                    {
                        MelonLogger.Msg("ParticleCount Limit has been reached " + w.particleCount + $" on {__instance.GetComponentInChildren<PlayerDescriptor>().userName}'s avatar");

                        GameObject.Destroy(w);
                    }
                    if (w.collision.maxCollisionShapes >= 10000)
                    {
                        MelonLogger.Msg("ParticleCount Limit has been reached " + w.collision.maxCollisionShapes + $" on {__instance.GetComponentInChildren<PlayerDescriptor>().userName}'s avatar");

                        GameObject.Destroy(w);
                    }
                    
                    particlesystemcount += 1;
                }
            }
            
            
                if (__instance.gameObject.GetComponentInChildren<SkinnedMeshRenderer>() != null && ConfigMan.GetConfig().SkinnedMeshRender && !avatarb)
            {
                foreach (var w in __instance.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    foreach (var s in w.materials)
                    {
                        if (ConfigMan.GetConfig().Shader)
                        {
                            s.shader = Shader.Find("Standard");

                        }

                    }

                    if (skinnedmeshrcount >= 2000)
                    {
                        MelonLogger.Msg("SkinnedMeshRender Lenght Limit has been reached " + skinnedmeshrcount + $" on {__instance.GetComponentInChildren<PlayerDescriptor>().userName}'s avatar");

                        GameObject.Destroy(w);
                    }
                    if (w.materials.Length >= 1000) 
                    {
                        MelonLogger.Msg("Material Lenght Limit has been reached " + w.materials.Length + $" on {__instance.GetComponentInChildren<PlayerDescriptor>().userName}'s avatar");
                        GameObject.Destroy(w);
                    }

                    skinnedmeshrcount += 1;
                }
            }
            if (__instance.gameObject.GetComponentInChildren<MeshRenderer>() != null && ConfigMan.GetConfig().MeshRenderer && !avatarb)
            {
                foreach (var w in __instance.gameObject.GetComponentsInChildren<MeshRenderer>())
                {
                    foreach(var s in w.materials)
                    {
                        s.shader = Shader.Find("Standard");

                    }
                    if (skinnedmeshrcount >= 2000)
                    { 
                        MelonLogger.Msg("SkinnedMeshRender Lenght Limit has been reached " + meshrenderercount + $" on {__instance.GetComponentInChildren<PlayerDescriptor>().userName}'s avatar");
                        
                        GameObject.Destroy(w);
                    }
                    if (w.materials.Length >= 1000)
                    {
                        MelonLogger.Msg("Material Lenght Limit has been reached " + w.materials.Length + $" on {__instance.GetComponentInChildren<PlayerDescriptor>().userName}'s avatar");
                        GameObject.Destroy(w);
                    }

                    meshrenderercount += 1;
                }
            }
            if (__instance.gameObject.GetComponentInChildren<DynamicBone>() != null && ConfigMan.GetConfig().DynamicBone && !avatarb)
            {
                foreach (var w in __instance.gameObject.GetComponentsInChildren<DynamicBone>())
                {
                    if (dynamicbonecount >= 500)
                    {
                        MelonLogger.Msg("DynamicBone Lenght Limit has been reached " + dynamicbonecount + $" on {__instance.GetComponentInChildren<PlayerDescriptor>().userName}'s avatar");

                        GameObject.Destroy(w);
                    }


                    dynamicbonecount += 1;
                }
            }
            if (__instance.gameObject.GetComponentInChildren<DynamicBoneCollider>() != null  && ConfigMan.GetConfig().DynamicBone && !avatarb)
            {
                foreach (var w in __instance.gameObject.GetComponentsInChildren<DynamicBoneCollider>())
                {
                    if (dynamicbonecollidercount >= 500)
                    {
                        MelonLogger.Msg("DynamicBoneCollider Lenght Limit has been reached " + dynamicbonecollidercount + $" on {__instance.GetComponentInChildren<PlayerDescriptor>().userName}'s avatar");

                        GameObject.Destroy(w);
                    }


                    dynamicbonecollidercount += 1;
                }
            }

        }

    }



}

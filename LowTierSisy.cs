using UnityEngine;
using HarmonyLib;
using BepInEx;
using System.IO;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace LowTierSisy
{
    [BepInPlugin("ImNotSimon.LowTierSisyphus", "LowTierSisyphus", "1.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        private static Harmony harmony;

        internal static AssetBundle LowTierSisyBundle;

        private void Awake()
        {
            Debug.Log("BEGONE! (low tier sisyphus starting)");

            //load the asset bundle
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "lowtiersisy";
            {
                LowTierSisyBundle = AssetBundle.LoadFromFile(Path.Combine(ModPath(), resourceName));
            }

            //start harmonylib to swap assets
            harmony = new Harmony("ltg.Sisyphus");
            harmony.PatchAll();
        }

        public static string ModPath()
        {
            return Assembly.GetExecutingAssembly().Location.Substring(0, Assembly.GetExecutingAssembly().Location.LastIndexOf(Path.DirectorySeparatorChar));
        }


        private static SubtitledAudioSource.SubtitleDataLine MakeLine(string subtitle, float time)
        {
            var sub = new SubtitledAudioSource.SubtitleDataLine();
            sub.subtitle = subtitle;
            sub.time = time;
            return sub;
        }

        //use map info to inject data
        [HarmonyPatch(typeof(StockMapInfo), "Awake")]
        internal class Patch00
        {
            static void Postfix(StockMapInfo __instance)
            {
                //try to find dialog in scene and replace it
                foreach (var source in Resources.FindObjectsOfTypeAll<AudioSource>())
                {
                    if (source.clip)
                    {
                        bool replaced = false;
                        var subtitles = new List<SubtitledAudioSource.SubtitleDataLine>();
                        if (source.clip.name == "sp_intro")
                        {
                            Debug.Log("Replacing sissypuss intro");
                            source.clip = LowTierSisyBundle.LoadAsset<AudioClip>("lowtiersisy.wav");
                            replaced = true;

                            subtitles.Add(MakeLine("You are a worthless", 0f));
                            subtitles.Add(MakeLine("BITCH ASS machine.", 1.1f));
                            subtitles.Add(MakeLine("Your life LITERALLY is as valuable as a summer's ant.", 2.26f));
                            subtitles.Add(MakeLine("I'm just gonna stomp you,", 5.86f));
                            subtitles.Add(MakeLine("and you're gonna keep coming back.", 7.36f));
                            subtitles.Add(MakeLine("I'mma seal up all my cracks,", 9.77f));
                            subtitles.Add(MakeLine("you're gonna keep coming back.", 11.48f));
                            subtitles.Add(MakeLine("Why?", 13.28f));
                            subtitles.Add(MakeLine("'Cause YOU smellin' the BLOOD,", 13.87f));
                            subtitles.Add(MakeLine("you worthless", 15.5f));
                            subtitles.Add(MakeLine("BITCH ASS machine.", 16.5f));
                            subtitles.Add(MakeLine("You're gonna stay on my dick until you DIE.", 17.78f));
                            subtitles.Add(MakeLine("You serve no purpose in life.", 20.54f));
                            subtitles.Add(MakeLine("Your purpose in life is to be on my stream", 22.021f));
                            subtitles.Add(MakeLine("SUCKING on my dick daily.", 24.34f));
                            subtitles.Add(MakeLine("Your purpose in life is to be in that chat", 26.605f));
                            subtitles.Add(MakeLine("BLOWING the dick daily.", 29f));
                            subtitles.Add(MakeLine("Your life is nothing, you serve ZERO purpose!", 32f));
                            subtitles.Add(MakeLine("Come forth, Child of Man", 34.69f));
                            subtitles.Add(MakeLine("and kill yourself", 36.73f));
                            subtitles.Add(MakeLine("NOW!", 38.036f));
                        }
                        //update subtitles if needed
                        if (replaced)
                        {
                            var subsource = source.GetComponent<SubtitledAudioSource>();
                            Traverse field = Traverse.Create(subsource).Field("subtitles");
                            (field.GetValue() as SubtitledAudioSource.SubtitleData).lines = subtitles.ToArray();
                        }
                    }
                }


            }
        }
    }
}

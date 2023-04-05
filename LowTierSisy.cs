using UMM;
using UnityEngine;
using HarmonyLib;
using System.IO;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace LowTierSisy
{
    [UKPlugin("ltg.Sisyphus", "Low Tier Sisyphus Speech", "1.0.0", "Replaces Sisyphus Prime's intro with a low tier one.\nOriginal audio: https://www.youtube.com/watch?v=DOskY5a3ZCM", true, false)]
    public class LowTierSis : UKMod
    {
        private static Harmony harmony;

        internal static AssetBundle LowTierSisyBundle;

        public override void OnModLoaded()
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
            return Assembly.GetExecutingAssembly().Location.Substring(0, Assembly.GetExecutingAssembly().Location.LastIndexOf(@"\"));
        }

        public override void OnModUnload()
        {
            harmony.UnpatchSelf();
            base.OnModUnload();
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
                        if (source.clip.GetName() == "sp_intro")
                        {
                            Debug.Log("Replacing sissypuss intro");
                            source.clip = LowTierSisyBundle.LoadAsset<AudioClip>("lowtiersisy.mp3");
                            replaced = true;

                            subtitles.Add(MakeLine("You are a worthless", 0f));
                            subtitles.Add(MakeLine("BITCH ASS machine.", 1.5f));
                            subtitles.Add(MakeLine("Your life LITERALLY is as valuable as a", 3.5f));
                            subtitles.Add(MakeLine("summer ant.", 5.5f));
                            subtitles.Add(MakeLine("I'm just gonna stomp you,", 7.6f));
                            subtitles.Add(MakeLine("and you're gonna keep coming back.", 8.8f));
                            subtitles.Add(MakeLine("I'mma seal up all my cracks,", 10.4f));
                            subtitles.Add(MakeLine("you're gonna keep coming back.", 12.1f));
                            subtitles.Add(MakeLine("Why?", 13.7f));
                            subtitles.Add(MakeLine("'Cause YOU smellin' the BLOOD,", 14.6f));
                            subtitles.Add(MakeLine("you worthless", 17.1f));
                            subtitles.Add(MakeLine("BITCH ASS machine.", 18.0f));
                            subtitles.Add(MakeLine("You're gonna stay on my dick until you DIE.", 20.2f));
                            subtitles.Add(MakeLine("You serve no purpose in life.", 23.3f));
                            subtitles.Add(MakeLine("Your purpose in life is to be on my stream sucking on my dick daily.", 25.6f));
                            subtitles.Add(MakeLine("Your purpose in life is to be in that chat BLOWING a dick daily,", 30.5f));
                            subtitles.Add(MakeLine("your life is nothing, you serve ZERO purpose!", 34.1f));
                            subtitles.Add(MakeLine("YOU SHOULD KILL YOURSELF", 36.8f));
                            subtitles.Add(MakeLine("NOW!", 38.1f));
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

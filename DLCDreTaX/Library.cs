using System;
using System.Collections.Generic;

namespace DLCDreTaX
{
    public class Library
    {
        private static readonly List<string> DLCS = new List<string>()
        {
            "Darkrock Ventures",
            "Euphoria",
            "Mistfall",
            "Mr. Game!",
            "RARRR!!",
            "Superfight",
            "Simurgh",
            "The Captain Is Dead",
            "Spirits of the Rice Paddy",
            "Three Cheers For Master",
            "Tiny Epic Defenders",
            "Tiny Epic Galaxies",
            "Tiny Epic Kingdoms",
            "Wizard's Academy",
            "Zombicide",
            "Scuttle!",
            "Scythe",
            "Warfighter",
            "Xia - Legends of a Drift System",
            "Abraca What",
            "Cavern Tavern",
            "In the Name of Odin",
            "Three Kingdoms Redux",
            "Tiny Epic Western",
            "Battle For Greyport",
            "Khronos Hunter",
            "The Great Dinosaur Rush",
            "Darkest Night",
            "Viticulture",
            "Indonesia",
            "Tiny Epic Quest",
            "Battle For Souls",
            "Pillars of Eternity - Lords of the Eastern Reach",
            "Cosmic Encounter",
            "Draco Magi",
            "Tortuga 1667",
            "Deck Quest",
        };
        
        public static List<string> GetOwnedDLCsFake()
        {
            return DLCS;
        }
        
        
        public static bool CanWeLoadThisDLCFake(string DLCName)
        {
            Chat.LogError("FakeDLC Load " + DLCName, true);
            return true;
            //(SteamManager.IsSubscribedApp(DLCManager.NameToDLCInfo(DLCName).AppId) || DLCManager.HostDLCs.Contains(DLCName));
        }


        public static void LoadFake(DLCManager instance, string DLCName)
        {
            DLCManager.SetHostOwnedDLCs(DLCS);

            DLCWebsiteInfo dlcInfo = DLCManager.NameToDLCInfo(DLCName);
            /*if (!SteamManager.IsSubscribedApp(dlcInfo.AppId))
            {
                Chat.Log("You do not own DLC " + dlcInfo.Name + ". Cannot load.", Colour.red, ChatMessageType.Game,
                    false);
                TTSUtilities.OpenURL("http://store.steampowered.com/app/" + dlcInfo.AppId.ToString());
            }
            else
            {
                
            }*/
            
            Chat.Log(dlcInfo.Name + " has been haxed. Thanks & have fun.", Colour.red, ChatMessageType.Game,
                false);
            instance.StartCoroutine(instance.LoadSaveFile(dlcInfo));
        }
    }
}
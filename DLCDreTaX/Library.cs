using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DLCDreTaX
{
    public class Library
    {
        /*private static readonly List<string> DLCS = new List<string>()
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
        };*/
        
        public static List<string> GetOwnedDLCsFake()
        {
            List<DLCWebsiteInfo> infos = Singleton<DLCManager>.Instance.DLCWebsites;
            return infos.Select(x => x.Name).ToList();
        }
        
        
        public static bool CanWeLoadThisDLCFake(string DLCName)
        {
            DLCManager.SetHostOwnedDLCs(GetOwnedDLCsFake());
            Chat.LogError("FakeDLC Load " + DLCName, true);
            return true;
            //(SteamManager.IsSubscribedApp(DLCManager.NameToDLCInfo(DLCName).AppId) || DLCManager.HostDLCs.Contains(DLCName));
        }

        public static void DLCStartFake()
        {
            DLCManager.SetHostOwnedDLCs(GetOwnedDLCsFake());
        }

        public static void LoadFake(DLCManager instance, string DLCName)
        {
            DLCManager.SetHostOwnedDLCs(GetOwnedDLCsFake()); // I don't remember if this is needed.

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
            
            Chat.Log(dlcInfo.Name + " has been haxed. Thanks & have fun.", Colour.Red, ChatMessageType.Game,
                false);
            instance.StartCoroutine(instance.LoadSaveFile(dlcInfo));
        }
        
        public static List<UIGridMenu.GridButtonDLC> GetDLCGridButtonsFake(UIGridMenuGames gridmenu, GameObject closeMenu)
        {
            Action<List<UIGridMenu.GridButtonDLC>> action;
            List<UIGridMenu.GridButtonDLC> list = new List<UIGridMenu.GridButtonDLC>();
            List<DLCWebsiteInfo> dLCInfos = DLCManager.DLCInfos;
            for (int i = 0; i < dLCInfos.Count; i++)
            {
                DLCWebsiteInfo info = dLCInfos[i];
                UIGridMenu.GridButtonDLC item = new UIGridMenu.GridButtonDLC {
                    Name = string.IsNullOrEmpty(info.DisplayName) ? info.Name : info.DisplayName,
                    LoadName = info.Name,
                    ThumbnailURL = info.ThumbnailURL,
                    AppId = info.AppId,
                    DiscountPercent = info.DiscountPercent,
                    New = info.New,
                    Lock = false,//!SteamManager.IsSubscribedApp(info.AppId),
                    Purchased = SteamManager.SubscribeDate(info.AppId),
                    ButtonColor = gridmenu.DLCColor,
                    BackgroundColor = gridmenu.DLCDarkColor,
                    CloseMenu = closeMenu
                };
                item.Tags.TryAdd<string>("dlc");
                list.Add(item);
            }

            // Reflection
            Dictionary<string, System.Action<List<UIGridMenu.GridButtonDLC>>> DLCSorts = 
                (Dictionary<string, Action<List<UIGridMenu.GridButtonDLC>>>) GetInstanceField(typeof(UIGridMenuGames), gridmenu, "DLCSorts");
            
            // Didn't check if this changes
            string currentDLCSort = (string) GetInstanceField(typeof(UIGridMenuGames), gridmenu, "currentDLCSort");
            
            if (DLCSorts.TryGetValue(currentDLCSort, out action) && (action != null))
            {
                action(list);
            }
            list.Reverse();
            return list;
        }

        public static void SetKickstarterFlags()
        {
            SteamManager.bKickstarterGold = true;
            SteamManager.bKickstarterPointer = true;
        }
        
        private static object GetInstanceField(Type type, object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                     | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            if (field != null)
            {
                object v = field.GetValue(instance);
                return v;
            }

            return null;
        }

    }
}
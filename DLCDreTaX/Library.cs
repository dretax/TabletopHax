using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using UnityEngine;
using Network = NewNet.Network;

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
            ChatScript.LogError("FakeDLC Load " + DLCName, true);
            return true;
            //(SteamManager.IsSubscribedApp(DLCManager.NameToDLCInfo(DLCName).AppId) || DLCManager.HostDLCs.Contains(DLCName));
        }


        public static void LoadFake(DLCManager instance, string DLCName)
        {
            DLCManager.SetHostOwnedDLCs(DLCS);
            ChatScript.LogError("Can't load. Already loading another DLC. Please wait for that to finish.", true);
            if (instance.bLoadingDLC)
            {
                ChatScript.LogError("Can't load. Already loading another DLC. Please wait for that to finish.", true);
            }
            else
            {
                DLCBaseInfo info = DLCManager.NameToDLCInfo(DLCName);
                ChatScript.LogError("LoadFake Test Call.", true);
                /*if (!SteamManager.IsSubscribedApp(info.AppId))
                {
                    ChatScript.Log("You do not own DLC " + info.Name + ". Cannot load.", Colour.red, ChatMessageType.Game, false);
                    NetworkSingleton<NetworkUI>.Instance.OpenURL("http://store.steampowered.com/app/" + info.AppId.ToString());
                }
                else
                {*/
                    if (Network.isServer)
                    {
                        NetworkSingleton<ManagerPhysicsObject>.Instance.LoadSaveState(Json.Load<SaveState>(info.Json.text), false, true, false);
                    }
                    else if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
                    {
                        NetworkSingleton<ManagerPhysicsObject>.Instance.LoadPromotedSaveState(Json.GetBson(Json.Load<SaveState>(info.Json.text)));
                    }
                    for (int i = 0; i < info.Expansions.Count; i++)
                    {
                        DLCInfo info2 = info.Expansions[i];
                        if (!SteamManager.IsSubscribedApp(info2.AppId))
                        {
                            ChatScript.Log("Expansion " + info2.Name + " not owned.", ChatMessageType.Game);
                        }
                        else
                        {
                            ChatScript.Log("Expansion " + info2.Name + " loading...", ChatMessageType.Game);
                            if (Network.isServer)
                            {
                                NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnOffsetObjectStates(Json.Load<SaveState>(info2.Json.text).ObjectStates, Vector3.zero, true);
                            }
                            else if (NetworkSingleton<PlayerManager>.Instance.IsAdmin(-1))
                            {
                                NetworkSingleton<ManagerPhysicsObject>.Instance.SpawnOffsetJsonSaveState(Json.GetBson(Json.Load<SaveState>(info2.Json.text)), Vector3.zero);
                            }
                        }
                    }
                //}
            }
        }
    }
}
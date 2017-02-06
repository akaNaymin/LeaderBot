using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace LeaderBot
{
    public static class ApiSender //sends requests
    {

        public static string GetLeaderboard(string id, int offset) // https://partner.steamgames.com/documentation/community_data
            //returns data as xml files
        {
            string response = "";
            try
            {
                using (WebClient client = new WebClient())
                {
                    if (id == "")
                        response = client.DownloadString("http://steamcommunity.com/stats/247080/leaderboards/?xml=1");
                        //leaderboard indexes
                    else
                        response = client.DownloadString("http://steamcommunity.com/stats/247080/leaderboards/" + id + "/?xml=1&start=" + offset + "&end=" + (offset + 14));
                        //specific leaderboards
                }
            }
            catch
            {
                Console.WriteLine("Serious error occured. Server not responding.");
            }
            return response;
        }


        public static void GetSteamNames(List<Entry> ids) // https://developer.valvesoftware.com/wiki/Steam_Web_API#GetPlayerSummaries_.28v0002.29
        {
            //searches for the profile names of given ids
            ids = ids.OrderBy(Entry => Entry.Steamid).ToList();
            SteamUser[] steamUsers;
            string response;
            string request = "";
            foreach(Entry e in ids)
            {
                request += e.Steamid + ",";
            }
            using (WebClient client = new WebClient())
            {
                response = client.DownloadString("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" + Program.config.SteamKey + "&steamids=" + request);
            }
            steamUsers = JsonConvert.DeserializeObject<SteamResponse>(response).Response.Players;
            steamUsers = steamUsers.OrderBy(SteamUser => SteamUser.Steamid).ToArray();
            for (int i = 0; i < ids.Count; i++)
            {
                ids[i].Personaname = steamUsers[i].Personaname;
            }
        }
    }
}


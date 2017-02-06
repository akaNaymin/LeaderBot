using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Xml;
using System.Net;
using Newtonsoft.Json;

namespace LeaderBot
{

    public static class Parser
    {

        public static Dictionary<Leaderboard, LeaderboardInfo> lbInfo;

        public static void RegisterLeaderboards(Dictionary<Leaderboard, LeaderboardInfo> lbInfo)
        {
            File.WriteAllText(@"Leaderboards.json", JsonConvert.SerializeObject(lbInfo, Newtonsoft.Json.Formatting.Indented));
        }    

        public static Dictionary<Leaderboard, LeaderboardInfo> ParseIndex()
        {
            string xml = ApiSender.GetLeaderboard("", 0);
            Dictionary<Leaderboard, LeaderboardInfo> list = new Dictionary<Leaderboard, LeaderboardInfo>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode n in doc.DocumentElement.ChildNodes)
            {
                if (n.Name == "leaderboard")
                {
                    LeaderboardInfo lb = new LeaderboardInfo();

                    foreach (XmlNode e in n)
                    {
                        switch (e.Name)
                        {
                            case ("lbid"):
                                lb.Id = e.InnerText;
                                break;
                            case ("entries"):
                                lb.EntryCount = int.Parse(e.InnerText);
                                break;
                            case ("display_name"):
                                string s = e.InnerText;
                                lb.DisplayName = s;
                                if (!s.Contains("Amplified"))
                                    lb.Leaderboard.Amplified = false;
                                if (s.Contains("Seeded"))
                                    lb.Leaderboard.Seeded = true;
                                for (int i = 0; i < 3; i++)
                                {
                                    if (s.Contains(Enum.GetNames(typeof(RunType))[i]))
                                    {
                                        lb.Leaderboard.Type = (RunType)i;
                                        break;
                                    }
                                }
                                for (int i = 0; i < 13; i++)
                                {
                                    if (s.Contains(Enum.GetNames(typeof(Character))[i]))
                                    {
                                        lb.Leaderboard.Char = (Character)i;
                                        break;
                                    }
                                }
                                break;
                        }
                    }

                    list.Add(lb.Leaderboard, lb);
                }
            }
            RegisterLeaderboards(list);
            return list;
        }

        public static List<Entry> ParseLeaderboard(string id, int offset)
        {
            string xml = ApiSender.GetLeaderboard(id, offset);
            List<Entry> list = new List<Entry>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode n in doc.DocumentElement.ChildNodes[8])
            {
                    Entry en = new Entry();

                    foreach (XmlNode e in n)
                    {
                        switch (e.Name)
                        {
                        case "steamid":
                            en.Steamid = e.InnerText;
                            break;
                        case "score":
                            en.Score = int.Parse(e.InnerText);
                            break;
                        case "rank":
                            en.Rank = int.Parse(e.InnerText);
                            break;
                        case "ugcid":
                            en.UgcId = e.InnerText;
                            break;
                        }
                    }

                    list.Add(en);
            }
            return list;
        }
    }
}

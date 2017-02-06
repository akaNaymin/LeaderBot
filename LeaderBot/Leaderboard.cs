using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaderBot
{

    public enum Character{ All, Aria, Bard, Bolt, Cadence, Coda, Dorian, Dove, Eli, Melody, Monk, Nocturna, Story }
    public enum RunType { Deathless, Score, Speed }

    public class Leaderboard
    {
        public bool Amplified { get; set; } = true;
        public bool Seeded { get; set; }
        public Character Char { get; set; }
        public RunType Type { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            Leaderboard compare = obj as Leaderboard;
            if (Amplified == compare.Amplified && Seeded == compare.Seeded && Char == compare.Char && Type == compare.Type)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            string hash = "";
            hash += (int)Char;
            hash += (int)Type;
            if (Seeded)
                hash += "1";
            else
                hash += "0";
            if (Amplified)
                hash += "1";
            else
                hash += "0";
            return Convert.ToInt16(hash);
        }
    }

    
}

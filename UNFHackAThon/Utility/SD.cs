using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UNFHackAThon.Utility
{
    public static class SD
    {
        public const string DefaultCompetitionImage = "data.jpg";

        public const string ManageUser = "Manager";

        public const string ParticipantEndUser = "Participant";

        public const string ssCompetitionCartCount = "SD.ssCartCount";


        public static string ConvertToRawHtml(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}

using System.Collections.Generic;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class FontTextureMappingHelper
    {
        internal static Dictionary<char, int> Default => FontTextureMappingHelper.GenerateDefault();

        private static Dictionary<char, int> GenerateDefault()
        {
            char[] charset = "abcdefghijklmnopqrstuvwxyz!?.:$0123456789>+".ToCharArray();
            var dict = new Dictionary<char, int>();
            for (int i = 0; i < charset.Length; i++)
            {
                dict[charset[i]] = i;
            }

            return dict;
        }
    }
}
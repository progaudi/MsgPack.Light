using System.Collections.Generic;
using System.Linq;

namespace msgpack.light.benchmark
{
    public static class Data
    {
        #region Integers

        public static readonly int[] Integers =
        {
            34492,
            6603,
            44033,
            8874,
            47607,
        };

#endregion

        public static double[] Doubles => Integers.Select(i => (double) i).ToArray();

        #region Belgium beers

        public static readonly Beer[] Belgium = new[]
        {
            new Beer { Brand = "3 SchtГ©ng", Alcohol = 6F, Brewery = "Brasserie Grain d'Orge", Sort = new List<string> { "high fermentation" } },
            new Beer { Brand = "IV Saison", Alcohol = 6.5F, Brewery = "Brasserie de Jandrain-Jandrenouille", Sort = new List<string> { "saison" } },
            new Beer { Brand = "V Cense", Alcohol = 7.5F, Brewery = "Brasserie de Jandrain-Jandrenouille", Sort = new List<string> { "high fermentation" } },
            new Beer { Brand = "VI Wheat", Alcohol = 6F, Brewery = "Brasserie de Jandrain-Jandrenouille", Sort = new List<string> { "high fermentation", "wheat beer" } },
            new Beer { Brand = "Aardmonnik", Alcohol = 8F, Brewery = "De Struise Brouwers", Sort = new List<string> { "oud bruin" } },
            new Beer { Brand = "Aarschotse Bruine", Alcohol = 6F, Brewery = "Stadsbrouwerij Aarschot", Sort = new List<string> { "brown ale" } },
            new Beer { Brand = "Abbay d'Aulne Blonde des PГЁres 6", Alcohol = 6F, Brewery = "Brasserie Val de Sambre", Sort = new List<string> { "Abbey Beer", "blond" } },
        };

        #endregion
    }
}
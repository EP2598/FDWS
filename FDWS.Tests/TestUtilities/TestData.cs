using System.Collections.Generic;

namespace FDWS.Tests.TestUtilities
{
    public static class TestData
    {
        public static List<int[]> GetValidVillagerData()
        {
            return new List<int[]>
            {
                new int[] { 65, 2020 },
                new int[] { 72, 2019 },
                new int[] { 58, 2021 }
            };
        }

        public static List<int[]> GetInvalidVillagerData()
        {
            return new List<int[]>
            {
                new int[] { 2020, 65 }, // Invalid: year < age
                new int[] { 2019, 72 }  // Invalid: year < age
            };
        }

        public static List<int[]> GetMixedVillagerData()
        {
            return new List<int[]>
            {
                new int[] { 65, 2020 }, // Valid
                new int[] { 2019, 72 }, // Invalid
                new int[] { 58, 2021 }  // Valid
            };
        }

        public static List<int[]> GetEmptyVillagerData()
        {
            return new List<int[]>();
        }
    }
}
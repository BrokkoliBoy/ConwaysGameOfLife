using System.Collections.Generic;

namespace Gavi
{
    public enum GameRule
    {
        Conway,
        HighLife,
        DayAndNight,
        Seeds,
        Maze
    }

    public static class Rules
    {
        private static readonly Dictionary<GameRule, HashSet<int>> BirthCounts = new()
        {
            { GameRule.Conway, new HashSet<int> { 3 } },
            { GameRule.HighLife, new HashSet<int> { 3, 6 } },
            { GameRule.DayAndNight, new HashSet<int> { 3, 6, 7, 8 } },
            { GameRule.Seeds, new HashSet<int> { 2 } },
            { GameRule.Maze, new HashSet<int> { 3 } },
        };

        private static readonly Dictionary<GameRule, HashSet<int>> SurviveCounts = new()
        {
            { GameRule.Conway, new HashSet<int> { 2, 3 } },
            { GameRule.HighLife, new HashSet<int> { 2, 3 } },
            { GameRule.DayAndNight, new HashSet<int> { 3, 4, 6, 7, 8 } },
            { GameRule.Seeds, new HashSet<int>() },
            { GameRule.Maze, new HashSet<int> { 1, 2, 3, 4, 5 } },
        };

        public static bool ShouldBeBorn(GameRule rule, int aliveNeighbours) =>
            BirthCounts[rule].Contains(aliveNeighbours);

        public static bool ShouldSurvive(GameRule rule, int aliveNeighbours) =>
            SurviveCounts[rule].Contains(aliveNeighbours);
    }
}

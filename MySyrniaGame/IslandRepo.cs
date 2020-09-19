using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySyrniaGame
{
    public class IslandRepo
    {
        private List<Island> _listOfIslands = new List<Island>();

        // add island to list
        public void AddIslandToList(Island _island)
        {
            _listOfIslands.Add(_island);
        }

        // return list of islands
        public List<Island> GetIslandList()
        {
            return _listOfIslands;
        }

        // return island by name
        public Island GetIslandByName(string islandName)
        {
            foreach (Island island in _listOfIslands)
            {
                if (island.IslandName == islandName)
                {
                    return island;
                }
            }
            return null;
        }
    }
}

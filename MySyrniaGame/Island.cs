using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySyrniaGame
{
    public class Island
    {
        public string IslandName { get; }
        public string IslandSplash { get; }
        public List<string> IslandConnections { get; }
        public List<KeyValuePair<string, string>> ConnectedIslands { get; }
        public List<Area> IslandAreas { get; }

        public Island(string islandName, string islandSplash, List<string> islandConnections, List<KeyValuePair<string, string>> connectedIslands, List<Area> islandAreas)
        {
            IslandName = islandName;
            IslandSplash = islandSplash;
            IslandConnections = islandConnections;
            ConnectedIslands = connectedIslands;
            IslandAreas = islandAreas;
        }

        // Add new area to list
        public void AddArea(Area newArea)
        {
            IslandAreas.Add(newArea);
        }

        //return island area by name
        public Area GetAreaByName(string name)
        {
            foreach (Area area in IslandAreas)
            {
                if (area.AreaName == name)
                {
                    return area;
                }
            }
            return null;
        }
    }
}

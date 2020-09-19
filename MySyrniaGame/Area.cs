using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySyrniaGame
{
    public class Area
    {
        public string AreaName { get; }
        public string AreaSplash { get; }
        public List<string> AreaConnections { get; }
        public List<Activity> AreaActivities { get; }


        public Area(string areaName, String areaSplash, List<string> areaConnections, List<Activity> areaActivities)
        {
            AreaName = areaName;
            AreaSplash = areaSplash;
            AreaConnections = areaConnections;
            AreaActivities = areaActivities;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MySyrniaGame
{
    class ProgramUI
    {
        private IslandRepo _islandRepo = new IslandRepo();

        public void Run()
        {
            CreateTheLand();

            // set starting island and area
            Island currentIsland = _islandRepo.GetIslandByName("Remer Island");
            Area currentArea = currentIsland.GetAreaByName("Sanfew");
            //Area currentArea = currentIsland.GetAreaByName("Port Senyn");

            // build area conections dictionary
            Dictionary<string, Area> areaConnections = UpdateAreaDictionary(currentIsland);

            //build island onections dictionary
            Dictionary<string, Island> islandConnections = UpdateIslandDictionary(currentIsland);

            // starting storyline
            Console.WriteLine("Shipwrecked on an unfamilure island, you find yourself in a small village. \n" +
                "Here are a few commands: \n" +
                "\"inv\" will show you inventory. \n" +
                "\"Exit\" will quit the game. \n" +
                "Press any key to begin.");
            Console.ReadKey();
            Console.Clear();

            // show starting area splash
            Console.WriteLine(currentArea.AreaSplash);

            // start inventory
            List<Result.Item> inventory = new List<Result.Item>();

            // game ui/logic
            bool gameInPlay = true;

            while (gameInPlay)
            {
                string command = Console.ReadLine().ToLower();
                Console.Clear();

                //check if command contains activity triggerPhrase
                bool commandContainsActivityTrigger = false;
                foreach (Activity areaActivity in currentArea.AreaActivities)
                {
                    if (command.Contains(areaActivity.TriggerPhrase))
                    {
                        commandContainsActivityTrigger = true;
                    }
                }

                // move to different area
                if (command.StartsWith("go ") && !commandContainsActivityTrigger)
                {
                    bool foundConnection = false;
                    foreach (string connection in currentArea.AreaConnections)
                    {
                        if (command.Contains(connection.ToLower()) && areaConnections.ContainsKey(connection.ToLower()))
                        {
                            // change to new area
                            currentArea = currentIsland.GetAreaByName(connection);
                            foundConnection = true;
                            Console.Write($"You are on your way to {connection}....");

                            // show console spinner
                            var spinner = new SpinnerRepo.Spinner(Console.CursorLeft, Console.CursorTop, 50);
                            spinner.Start();
                            Thread.Sleep(7000);  // to do -- add travel time for each area
                            spinner.Stop();
                            Console.Clear();
                            break;
                        }
                    }
                    if (!foundConnection)
                    {
                        Console.WriteLine("I don't understand. Go where?");
                    }
                }

                // sail to different island
                else if (command.StartsWith("sail ") && !commandContainsActivityTrigger)
                {
                    bool foundConnection = false;
                    foreach (string connection in currentArea.AreaConnections)
                    {
                        if (command.Contains(connection.ToLower()) && islandConnections.ContainsKey(connection))
                        {
                            // change to new island
                            currentIsland = islandConnections[connection];
                            // change to new area
                            currentArea = currentIsland.GetAreaByName(connection);

                            foundConnection = true;
                            Console.Write($"You are on sailing to {connection} on {currentIsland.IslandName}....");

                            // show console spinner
                            var spinner = new SpinnerRepo.Spinner(Console.CursorLeft, Console.CursorTop, 50);
                            spinner.Start();
                            Thread.Sleep(7000);  // to do -- add travel time for each area
                            spinner.Stop();
                            Console.Clear();
                            break;
                        }
                    }
                    if (!foundConnection)
                    {
                        Console.WriteLine("I don't understand. Sail where?");
                    }
                }

                // do activity
                        else if (commandContainsActivityTrigger)
                {
                    foreach (Activity areaActivity in currentArea.AreaActivities)
                    {
                        if (command.Contains(areaActivity.TriggerPhrase) && command.Contains(areaActivity.Result.ResultItem.ToString()))
                        {
                            // show activity message
                            Console.Write(areaActivity.Message);

                            // show console spinner
                            var spinner = new SpinnerRepo.Spinner(Console.CursorLeft, Console.CursorTop, 50);
                            spinner.Start();
                            Thread.Sleep(5000);  // to do -- add time for each activity
                            spinner.Stop();

                            // add random fails
                            Random rand = new Random();
                            int activityResult = rand.Next(0, 3);
                            // Console.WriteLine(activityResult);   // show activityResult for debuging
                            string resultMessage;
                            switch (activityResult)
                            {
                                case 0:
                                    switch (areaActivity.TriggerPhrase)
                                    {
                                        case "fish":
                                            resultMessage = "That one got away...";
                                            break;
                                        case "mine":
                                            resultMessage = "You swing the pickaxe and miss...";
                                            break;
                                        case "chop":
                                            resultMessage = "You swing the axe and miss...";
                                            break;
                                        case "clean":
                                            resultMessage = "You missed a spot, no gold for you!...";
                                            break;
                                        default:
                                            resultMessage = "Good try...";
                                            break;
                                    }
                                    break;
                                case 1:
                                case 2:
                                default:
                                    resultMessage = areaActivity.Result.ResultMessage;
                                    // add item to inventory
                                    inventory.Add(areaActivity.Result.ResultItem);
                                    break;
                            }

                            // show result message
                            Console.WriteLine(resultMessage);
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();

                            Console.Clear();
                        }
                        else
                        {
                            // show result message
                            Console.WriteLine("Did you miss something?");
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();

                            Console.Clear();
                        }
                    }
                }

                // show inventory
                else if (command == "inv")
                {
                    if (inventory.Count == 0)
                    {
                        Console.WriteLine("Your bag is empty!");
                    }
                    else
                    {
                        foreach (Result.Item item in Enum.GetValues(typeof(Result.Item)))
                        {
                            int total = inventory.Count(s => s == item);
                            if (total != 0)
                            {
                                Console.WriteLine($"You have {total} {item}.");
                            }
                        }
                    }
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }

                // exit game
                else if (command == "exit")
                {
                    Console.WriteLine("Goodbye!");
                    gameInPlay = false;
                }

                // default response
                else
                {
                    Console.WriteLine("You want to try smoething else?");
                }


                if (gameInPlay)
                {
                    Console.WriteLine(currentArea.AreaSplash);
                }

            }




            // keep the console up
            Console.ReadKey();
        }

        // build area conections dictionary
        public Dictionary<string, Area> UpdateAreaDictionary(Island _island)
        {
            Dictionary<string, Area> areaOutput = new Dictionary<string, Area>();
            foreach (Area _area in _island.IslandAreas)
            {
                areaOutput.Add(_area.AreaName.ToLower(), _island.GetAreaByName(_area.AreaName));
            }
            return areaOutput;
        }

        // build island conections dictionary
        public Dictionary<string, Island> UpdateIslandDictionary(Island currentIsland)
        {
            Dictionary<string, Island> islandOutput = new Dictionary<string, Island>();
            foreach (Island _island in _islandRepo.GetIslandList())
            {
                foreach (KeyValuePair<string, string> kv in _island.ConnectedIslands)
                {
                    islandOutput.Add(kv.Key, _islandRepo.GetIslandByName(kv.Value));
                }
            }
            return islandOutput;
        }

        public void CreateTheLand()
        {
            // create island "Remer Island"
            Island remer = new Island(
                "Remer Island",
                "This is the island you can begin training your skills and explore the lands.",
                new List<string> { "Port Dazar" },
                new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("Port Dazar", "Dearn Island"),
                    new KeyValuePair<string, string>("Xanso", "Mezno Island")
                },
                new List<Area> { }
                );

            // add areas
            Area portSenyn = new Area(
                "Port Senyn",
                "You are at Port Senyn. You can fish off the dock. \n" +
                "To the south you see a sign that says \"To Harnith\". \n" +
                "You can also sail on the next boat to Port Dazar on Dearn Island \n" +
                "What do you want to do?",
                new List<string> { "Harnith", "Port Dazar" },
                new List<Activity> { new Activity("fish", Activity.ActivityType.fish, "You are now fishing....", new Result(Result.Item.fish, "You got a fish!")) }
                );
            remer.AddArea(portSenyn);

            Area harnith = new Area(
                "Harnith",
                "You have entered the town of Harnith. To your right you see a sign that says \"To Eully\". Up ahead, you see a sign that says \"To Rynir Mines\". Behind you is a sign that says \"To Port Senyn\".\nWhat do you want to do?",
                new List<string> { "Port Senyn", "Eully", "Rynir Mines" },
                new List<Activity> { }
                );
            remer.AddArea(harnith);

            Area rynirMines = new Area(
                "Rynir Mines",
                "You have entered Rynir Mines. You can mine copper at the copper mine, or mine for iron at the iron mine. \n" +
                "To you right you see a sign that says \"To Lisim\". Up ahead, you see a sign that says \"To Sanfew\". " +
                "Behind you is a sign that says \"To Harnith\".\n" +
                "What do you want to do?",
                new List<string> { "Sanfew", "Lisim", "Harnith" },
                new List<Activity> {
                    new Activity("mine", Activity.ActivityType.mine, "You are mining for copper....",
                        new Result(Result.Item.copper, "You got some copper!")),
                    new Activity("mine", Activity.ActivityType.mine, "You are mining for iron....",
                        new Result(Result.Item.iron, "You got some iron!"))
                }
                );
            remer.AddArea(rynirMines);

            Area sanfew = new Area(
                "Sanfew",
                "You have entered the town of Sanfew. This is the location of island jail, which you can clean for some gold peices.\n" +
                "Behind you is a sign that says \"To Rynir Mines\". \n" +
                "What do you want to do?",
                new List<string> { "Rynir Mines" },
                new List<Activity> { new Activity("clean", Activity.ActivityType.clean, "You are cleaning the jail....", new Result(Result.Item.gold, "You got a gold coin!")) }
                );
            remer.AddArea(sanfew);

            Area lisim = new Area(
                "Lisim",
                "You have entered Lisim. You can fish at the lake. \n" +
                "To you right you see a sign that says \"To Valera\" up ahead " +
                "Behind you is a sign that says \"To Rynir Mines\".\n" +
                "What do you want to do?",
                new List<string> { "Rynir Mines", "Valera" },
                new List<Activity> { new Activity("fish", Activity.ActivityType.fish, "You are now fishing....", new Result(Result.Item.fish, "You got a fish!")) }
                );
            remer.AddArea(lisim);

            Area valera = new Area(
                "Valera",
                "You have entered the town of Valera. To your right you see a sign that says \"To Isri\". Up ahead, you see a sign that says \"To Endarx\". Behind you is a sign that says \"To Lisim\".\nWhat do you want to do?",
                new List<string> { "Isri", "Endarx", "Lisim" },
                new List<Activity> { }
                );
            remer.AddArea(valera);

            Area isri = new Area(
                "Isri",
                "You have entered the woods of Isri. You can chop for wood in the forest. \n" +
                "Up ahead, you see a sign that says \"To Eully\". " +
                "Behind you is a sign that says \"To Valera\".\n" +
                "What do you want to do?",
                new List<string> { "Eully", "Valera" },
                new List<Activity> { new Activity("chop", Activity.ActivityType.chop, "You are now chopping for wood....", new Result(Result.Item.wood, "You got a log!")) }
                );
            remer.AddArea(isri);

            Area eully = new Area(
                "Eully",
                "You have entered the town of Eully. Up ahead, you see a sign that says \"To Harnith\". Behind you is a sign that says \"To Isri\". \nWhat do you want to do?",
                new List<string> { "Harnith", "Isri" },
                new List<Activity> { }
                );
            remer.AddArea(eully);

            // add island to list
            _islandRepo.AddIslandToList(remer);


            // create island "Dearn Island"
            Island dearn = new Island(
                "Dearn Island",
                "This is the island advanced. Watch your back!",
                new List<string> { "Port Senyn" },
                new List<KeyValuePair<string, string>> {
                   new KeyValuePair<string, string>("Port Senyn", "Remer Island")},
                new List<Area> { }
                );

            // add areas
            Area portDazar = new Area(
                "Port Dazar",
                "You are at Port Dazar. \n" +
                "Up ahead, you see a sign that says \"To Unera\". \n" +
                "You can also sail on the next boat to Port Senyn on Remer Island \n" +
                "What do you want to do?",
                new List<string> { "Unera", "Port Senyn" },
                new List<Activity> { }
                );
            dearn.AddArea(portDazar);

            // add island to list
            _islandRepo.AddIslandToList(dearn);
        }
    }
}

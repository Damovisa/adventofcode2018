using System;
using System.Collections.Generic;
using System.Linq;

namespace day15
{

    // todo: fix A* so it's first in reading order.

    class Program
    {
        //( (character/wall/empty, hit points), original wall/empty)
        public static char[] characters = { 'G', 'E' };
        public static ((char, int), char)[,] map;
        public static string[] lines;

        public static int width;
        public static int height;

        static void Main(string[] args)
        {
            BuildMap("inputtest0.txt");

            DrawMap();

            var i = -1;
            var go = true;
            while (go)
            {
                go = DoMoves();
                i++;
                Console.WriteLine($"*** {i} ***");
            }

            // calculate stats
            var hitpoints = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    hitpoints += map[x, y].Item1.Item2;
                }
            }
            Console.WriteLine($"Total Moves: {i}, total hit points: {hitpoints}. Outcome: {i * hitpoints}");
        }

        public static void BuildMap(string file)
        {
            lines = System.IO.File.ReadAllLines(file);
            width = lines[0].Length;
            height = lines.Length;
            // build map
            map = new ((char, int), char)[width, height];
            var x = 0;
            var y = 0;
            foreach (var line in lines)
            {
                x = 0;
                foreach (var c in line)
                {
                    var character = characters.Contains(c);
                    map[x, y] = ((c, (character ? 200 : 0)), character ? '.' : c);
                    x++;
                }
                y++;
            }
        }
        public static void DrawMap()
        {
            var hitpoints = new List<int>();
            var y = 0;
            foreach (var line in lines)
            {
                var x = 0;
                foreach (var c in line)
                {
                    Console.Write($"{map[x, y].Item1.Item1}");
                    if (map[x, y].Item1.Item2 > 0)
                        hitpoints.Add(map[x, y].Item1.Item2);
                    x++;
                }
                Console.WriteLine();
                y++;
            }
            foreach (var h in hitpoints)
                Console.Write($"{h}  ");
            Console.WriteLine();
        }

        public static bool DoMoves()
        {
            // (character, x, y)
            var chPositions = new List<(char, int, int)>();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var c = map[x, y];
                    if (characters.Contains(c.Item1.Item1))
                    {
                        chPositions.Add((c.Item1.Item1, x, y));
                    }
                }
            }
            var attackers = new List<(char, int, int)>();   // character, coordinates, attack coordinates

            for (int i = 0; i < chPositions.Count; i++)
            {
                var astarNodes = Node.MakeMap(map, width, height, new[] { 'E', 'G', '#' });

                var ch = chPositions[i];
                // find closest of the opposite character
                var minBestEnemyPath = 1000000;
                List<Node> bestEnemyPath = null;
                var enemies = chPositions.Where(t => t.Item1 != ch.Item1);
                if (enemies.Count() == 0)
                {
                    return false;
                }

                var inrange = false;
                foreach (var enemy in enemies)
                {
                    if (enemy.Item2 == ch.Item2 && enemy.Item3 == ch.Item3)
                    {
                        // skip - it me!
                    }
                    else if (Math.Abs(enemy.Item2 - ch.Item2) + Math.Abs(enemy.Item3 - ch.Item3) == 1)
                    {
                        // skip - we're in range!
                        Console.WriteLine($"{ch.Item1} at ({ch.Item2},{ch.Item3}) is in range!");
                        inrange = true;
                    }
                    else
                    {
                        // get target positions
                        var targets = new List<(int, int)>();
                        if (map[enemy.Item2, enemy.Item3 - 1].Item1.Item1 == '.')
                            targets.Add((enemy.Item2, enemy.Item3 - 1));
                        if (map[enemy.Item2 - 1, enemy.Item3].Item1.Item1 == '.')
                            targets.Add((enemy.Item2 - 1, enemy.Item3));
                        if (map[enemy.Item2 + 1, enemy.Item3].Item1.Item1 == '.')
                            targets.Add((enemy.Item2 + 1, enemy.Item3));
                        if (map[enemy.Item2, enemy.Item3 + 1].Item1.Item1 == '.')
                            targets.Add((enemy.Item2, enemy.Item3 + 1));

                        var minpath = 100000;
                        List<Node> bestpath = null;
                        foreach (var t in targets)
                        {
                            var path = AStar.FindPath(astarNodes, ch.Item2, ch.Item3, t.Item1, t.Item2);
                            if (path != null && path.Count < minpath)
                            {
                                bestpath = path;
                                minpath = path.Count;
                            }
                        }

                        if (bestpath != null && minpath < minBestEnemyPath)
                        {
                            bestEnemyPath = bestpath;
                            minBestEnemyPath = minpath;
                        }
                    }
                }
                if (!inrange && bestEnemyPath != null)
                {
                    var newX = bestEnemyPath[1].X;
                    var newY = bestEnemyPath[1].Y;

                    //Console.WriteLine($"The best step for {ch.Item1} at ({ch.Item2},{ch.Item3}) is ({newX},{newY})");
                    // move to the new location
                    map[newX, newY].Item1 = (ch.Item1, map[ch.Item2, ch.Item3].Item1.Item2); // set the new char and hit points
                    // reset old spot
                    map[ch.Item2, ch.Item3].Item1 = (map[ch.Item2, ch.Item3].Item2, 0);  // "original" char and 0 hitpoints
                    // set chPositions
                    chPositions[i] = (chPositions[i].Item1, newX, newY);
                }
            }
            //DrawMap();

            // Attack!

            foreach (var ch in chPositions)
            {
                char enemy = 'E';
                if (ch.Item1 == 'E')
                    enemy = 'G';
                var minHitPoints = 500;
                (int, int) attack = (-1, -1);
                var character = map[ch.Item2, ch.Item3];

                // who to attack?
                var potential = map[ch.Item2, ch.Item3 - 1].Item1;
                if (potential.Item1 == enemy && potential.Item2 < minHitPoints)
                {
                    attack = (ch.Item2, ch.Item3 - 1);
                    minHitPoints = potential.Item2;
                }
                potential = map[ch.Item2 - 1, ch.Item3].Item1;
                if (potential.Item1 == enemy && potential.Item2 < minHitPoints)
                {
                    attack = (ch.Item2 - 1, ch.Item3);
                    minHitPoints = potential.Item2;
                }
                potential = map[ch.Item2 + 1, ch.Item3].Item1;
                if (potential.Item1 == enemy && potential.Item2 < minHitPoints)
                {
                    attack = (ch.Item2 + 1, ch.Item3);
                    minHitPoints = potential.Item2;
                }
                potential = map[ch.Item2, ch.Item3 + 1].Item1;
                if (potential.Item1 == enemy && potential.Item2 < minHitPoints)
                {
                    attack = (ch.Item2, ch.Item3 + 1);
                    minHitPoints = potential.Item2;
                }
                if (attack.Item1 == -1)
                {
                    // they must have moved!
                }
                else
                {
                    // attack!
                    map[attack.Item1, attack.Item2].Item1.Item2 -= 3;
                    if (map[attack.Item1, attack.Item2].Item1.Item2 <= 0)
                        map[attack.Item1, attack.Item2].Item1 = (map[attack.Item1, attack.Item2].Item2, 0);  // remove from the map
                }
            }
            DrawMap();
            return true;
        }

        public static (int, int) FindBestNextStep(int startX, int startY, int targetX, int targetY) {
            var spaces = new List<(int,int,bool)>();
            for (int y=0;y<height;y++) {
                for (int x=0;x<width;x++) {
                    if (!characters.Contains(map[x,y].Item1.Item1))
                        spaces.Add((x,y,false));
                }
            }
            
            var s = new Stack<(int,int)>();
            s.Append((startX, startY));
            while (s.Any()) {

            }
        }
    }
}

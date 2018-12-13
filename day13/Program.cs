using System;
using System.Collections.Generic;
using System.Linq;

namespace day13
{
    class Program
    {
        public static List<(int, int)> cartcoords;
        public static char[] cartchars = { '<', '>', '^', 'v' };
        public static (char, (char, NextTurn?))[,] map;

        static void Main(string[] args)
        {
            // create map
            var fileLines = System.IO.File.ReadAllLines("input.txt");
            var w = fileLines[0].Length;
            var h = fileLines.Length;
            map = new (char, (char, NextTurn?))[w, h];
            cartcoords = new List<(int, int)>();

            var y = 0;
            var x = 0;
            foreach (var line in fileLines)
            {
                x = 0;
                foreach (var c in line)
                {
                    if (c == '<' || c == '>')
                    {
                        map[x, y] = (c, ('-', NextTurn.Left));
                        cartcoords.Add((x, y));
                    }
                    else if (c == 'v' || c == '^')
                    {
                        map[x, y] = (c, ('|', NextTurn.Left));
                        cartcoords.Add((x, y));
                    }
                    else
                    {
                        map[x, y] = (c, (' ', null));
                    }
                    x++;
                }
                y++;
            }

            Console.WriteLine($"{w}x{h} map created");

            while (true)
            {
                for (int i = 0; i < cartcoords.Count; i++)
                {
                    x = cartcoords[i].Item1;
                    y = cartcoords[i].Item2;
                    if (x != -1)    // cart has been removed
                    {
                        (char, (char, NextTurn?)) thisSpace;
                        char nextChar;
                        var thisChar = map[x, y].Item1;
                        switch (thisChar)
                        {
                            case '>':
                                // moving right
                                thisSpace = map[x, y];
                                nextChar = map[x + 1, y].Item1;
                                if (cartchars.Contains(nextChar))
                                {
                                    // collision!
                                    Console.WriteLine($"Collision! {x + 1},{y}");

                                    // remove carts
                                    cartcoords[i] = (-1, -1);
                                    cartcoords[cartcoords.IndexOf((x + 1, y))] = (-1, -1);
                                    // reset map for those carts
                                    map[x, y] = (thisSpace.Item2.Item1, (' ', null));
                                    map[x+1, y] = (map[x+1, y].Item2.Item1, (' ', null));
                                }
                                else
                                {
                                    // reset current track
                                    map[x, y] = (thisSpace.Item2.Item1, (' ', null));
                                    // set new position
                                    if (nextChar == '/')
                                    {
                                        // new direction is up
                                        map[x + 1, y] = ('^', ('/', thisSpace.Item2.Item2));
                                    }
                                    else if (nextChar == '\\')
                                    {
                                        // new direction is down
                                        map[x + 1, y] = ('v', ('\\', thisSpace.Item2.Item2));
                                    }
                                    else if (nextChar == '+')
                                    {
                                        // intersection
                                        var choice = thisSpace.Item2.Item2 ?? 0;
                                        char[] directions = { '^', '>', 'v' };
                                        var newDirection = directions[(int)choice];
                                        map[x + 1, y] = (newDirection, ('+', (NextTurn)(((int)choice + 1) % 3)));
                                    }
                                    else
                                    {
                                        // continue
                                        map[x + 1, y] = (thisSpace.Item1, (nextChar, thisSpace.Item2.Item2));
                                    }
                                }
                                if (cartcoords[i].Item1 > -1) cartcoords[i] = (x + 1, y);
                                Console.Write($"({x},{y})->({x + 1},{y}) ");
                                break;

                            case '<':
                                // moving left
                                thisSpace = map[x, y];
                                nextChar = map[x - 1, y].Item1;
                                if (cartchars.Contains(nextChar))
                                {
                                    // collision!
                                    Console.WriteLine($"Collision! {x - 1},{y}");
                                    
                                    // remove carts
                                    cartcoords[i] = (-1, -1);
                                    cartcoords[cartcoords.IndexOf((x - 1, y))] = (-1, -1);
                                    map[x, y] = (thisSpace.Item2.Item1, (' ', null));
                                    map[x-1, y] = (map[x-1, y].Item2.Item1, (' ', null));
                                }
                                else
                                {
                                    // reset current track
                                    map[x, y] = (thisSpace.Item2.Item1, (' ', null));
                                    // set new position
                                    if (nextChar == '/')
                                    {
                                        // new direction is down
                                        map[x - 1, y] = ('v', ('/', thisSpace.Item2.Item2));
                                    }
                                    else if (nextChar == '\\')
                                    {
                                        // new direction is up
                                        map[x - 1, y] = ('^', ('\\', thisSpace.Item2.Item2));
                                    }
                                    else if (nextChar == '+')
                                    {
                                        // intersection
                                        var choice = thisSpace.Item2.Item2 ?? 0;
                                        char[] directions = { 'v', '<', '^' };
                                        var newDirection = directions[(int)choice];
                                        map[x - 1, y] = (newDirection, ('+', (NextTurn)(((int)choice + 1) % 3)));
                                    }
                                    else
                                    {
                                        // continue
                                        map[x - 1, y] = (thisSpace.Item1, (nextChar, thisSpace.Item2.Item2)); ;
                                    }
                                }
                                if (cartcoords[i].Item1 > -1) cartcoords[i] = (x - 1, y);
                                Console.Write($"({x},{y})->({x - 1},{y}) ");
                                break;

                            case '^':
                                // moving up
                                thisSpace = map[x, y];
                                nextChar = map[x, y - 1].Item1;
                                if (cartchars.Contains(nextChar))
                                {
                                    // collision!
                                    Console.WriteLine($"Collision! {x},{y - 1}");
                                    
                                    // remove carts
                                    cartcoords[i] = (-1, -1);
                                    cartcoords[cartcoords.IndexOf((x, y - 1))] = (-1, -1);
                                    map[x, y] = (thisSpace.Item2.Item1, (' ', null));
                                    map[x, y-1] = (map[x, y-1].Item2.Item1, (' ', null));
                                }
                                else
                                {
                                    // reset current track
                                    map[x, y] = (thisSpace.Item2.Item1, (' ', null));
                                    // set new position
                                    if (nextChar == '/')
                                    {
                                        // new direction is right
                                        map[x, y - 1] = ('>', ('/', thisSpace.Item2.Item2));
                                    }
                                    else if (nextChar == '\\')
                                    {
                                        // new direction is left
                                        map[x, y - 1] = ('<', ('\\', thisSpace.Item2.Item2));
                                    }
                                    else if (nextChar == '+')
                                    {
                                        // intersection
                                        var choice = thisSpace.Item2.Item2 ?? 0;
                                        char[] directions = { '<', '^', '>' };
                                        var newDirection = directions[(int)choice];
                                        map[x, y - 1] = (newDirection, ('+', (NextTurn)(((int)choice + 1) % 3)));
                                    }
                                    else
                                    {
                                        // continue
                                        map[x, y - 1] = (thisSpace.Item1, (nextChar, thisSpace.Item2.Item2)); ;
                                    }
                                }
                                if (cartcoords[i].Item1 > -1) cartcoords[i] = (x, y - 1);
                                Console.Write($"({x},{y})->({x},{y - 1}) ");
                                break;

                            case 'v':
                                // moving down
                                thisSpace = map[x, y];
                                nextChar = map[x, y + 1].Item1;
                                if (cartchars.Contains(nextChar))
                                {
                                    // collision!
                                    Console.WriteLine($"Collision! {x},{y + 1}");
                                    
                                    // remove carts
                                    cartcoords[i] = (-1, -1);
                                    cartcoords[cartcoords.IndexOf((x, y + 1))] = (-1, -1);
                                    map[x, y] = (thisSpace.Item2.Item1, (' ', null));
                                    map[x, y+1] = (map[x, y+1].Item2.Item1, (' ', null));
                                }
                                else
                                {
                                    // reset current track
                                    map[x, y] = (thisSpace.Item2.Item1, (' ', null));
                                    // set new position
                                    if (nextChar == '/')
                                    {
                                        // new direction is left
                                        map[x, y + 1] = ('<', ('/', thisSpace.Item2.Item2));
                                    }
                                    else if (nextChar == '\\')
                                    {
                                        // new direction is right
                                        map[x, y + 1] = ('>', ('\\', thisSpace.Item2.Item2));
                                    }
                                    else if (nextChar == '+')
                                    {
                                        // intersection
                                        var choice = thisSpace.Item2.Item2 ?? 0;
                                        char[] directions = { '>', 'v', '<' };
                                        var newDirection = directions[(int)choice];
                                        map[x, y + 1] = (newDirection, ('+', (NextTurn)(((int)choice + 1) % 3)));
                                    }
                                    else
                                    {
                                        // continue
                                        map[x, y + 1] = (thisSpace.Item1, (nextChar, thisSpace.Item2.Item2)); ;
                                    }
                                }
                                if (cartcoords[i].Item1 > -1) cartcoords[i] = (x, y + 1);
                                Console.Write($"({x},{y})->({x},{y + 1}) ");
                                break;

                            default:
                                Console.WriteLine("something wrong!");
                                break;
                        }
                    }
                }
                Console.WriteLine("No collisions, going again.");
                cartcoords.RemoveAll(c => c.Item1 == -1);   // remove dead carts
                if (cartcoords.Count <= 2) {
                    Console.WriteLine($"The last cart is located at {cartcoords[0].Item1},{cartcoords[0].Item2}");
                    Environment.Exit(0);
                }
                cartcoords.Sort((a, b) =>
                {
                    int result = a.Item2.CompareTo(b.Item2);
                    return result == 0 ? a.Item1.CompareTo(b.Item1) : result;
                });
            }
        }

        public void DoMove()
        {
            //todo: extract to method?
        }
    }

    public enum NextTurn
    {
        Left = 0,
        Straight = 1,
        Right = 2
    }
}

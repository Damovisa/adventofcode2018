using System.Collections.Generic;

namespace Classes {
    public class Problem {
        public List<List<MapCell>> MapCells { get; set; }
        private int width;
        private int height;
        public List<Coordinates> CartCoords {get;set;}

        public Problem(string[] mapStringLines)
        {
            this.MapCells = new List<List<MapCell>>();
            this.CartCoords = new List<Coordinates>();

            int rowNum = 0;
            foreach (var line in mapStringLines) {
                var mapLine = new List<MapCell>();
                int colNum = 0;
                foreach (var segment in line) {
                    switch (segment) {
                        case '|':
                            mapLine.Add(new MapCell(TrackShape.Vertical, null));
                            break;
                        case '-':
                            mapLine.Add(new MapCell(TrackShape.Horizontal, null));
                            break;
                        case '/':
                            mapLine.Add(new MapCell(TrackShape.UpRightLeftDown, null));
                            break;
                        case '\\':
                            mapLine.Add(new MapCell(TrackShape.UpLeftRightDown, null));
                            break;
                        case '+':
                            mapLine.Add(new MapCell(TrackShape.Intersection, null));
                            break;
                        case '<':
                            mapLine.Add(new MapCell(TrackShape.Horizontal, new Cart(Direction.Left, 0)));
                            CartCoords.Add(new Coordinates(colNum, rowNum));
                            break;
                        case '>':
                            mapLine.Add(new MapCell(TrackShape.Horizontal, new Cart(Direction.Right, 0)));
                            CartCoords.Add(new Coordinates(colNum, rowNum));
                            break;
                        case '^':
                            mapLine.Add(new MapCell(TrackShape.Vertical, new Cart(Direction.Up, 0)));
                            CartCoords.Add(new Coordinates(colNum, rowNum));
                            break;
                        case 'v':
                            mapLine.Add(new MapCell(TrackShape.Vertical, new Cart(Direction.Down, 0)));
                            CartCoords.Add(new Coordinates(colNum, rowNum));
                            break;
                        case ' ':
                            mapLine.Add(new MapCell(TrackShape.Empty, null));
                            break;
                    }
                    colNum++;
                }
                rowNum++;
                MapCells.Add(mapLine);
            }
        }

        public string GetLocation(int x, int y) {
            var cell = MapCells[x][y];
            return $"{cell.Track} {cell.Cart?.Direction}";
        }
    }

    public struct MapCell {
        public TrackShape Track {get;set;}
        public Cart Cart {get;set;}
        public MapCell(TrackShape shape, Cart cart)
        {
            Track = shape;
            Cart = cart;
        }
    }

    public enum TrackShape {
        Vertical,
        Horizontal,
        UpLeftRightDown,
        UpRightLeftDown,
        Intersection,
        Empty
    }
}
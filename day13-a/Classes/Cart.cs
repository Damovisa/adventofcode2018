namespace Classes {
    public class Cart {
        public Direction Direction { get; set; }
        public Turn NextTurn { get; set;}

        public Cart(Direction direction, Turn nextTurn)
        {
            Direction = direction;
            NextTurn = nextTurn;
        }
    }

    public enum Turn {
        Left = 0,
        Straight = 1,
        Right = 2
    }
}
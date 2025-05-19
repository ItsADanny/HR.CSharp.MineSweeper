public class Spot { 
    public int X;
    public int Y;
    public bool Show = false;

    public Spot(int x, int y) {
        X = x;
        Y = y;
    }

    public void Cleared() => Show = true;
}
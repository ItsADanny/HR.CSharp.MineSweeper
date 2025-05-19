public class Bomb {
    public int X;
    public int Y;
    public bool Show = false;

    public Bomb(int x, int y) {
        X = x;
        Y = y;
    }

    public void Found() => Show = true;    
}
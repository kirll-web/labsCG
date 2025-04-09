namespace z1.presentation;

public class LabirintViewModel
{
    public static int MapHeight = 10;
    public static int MapWidth = 10;
    public static int START_POSITION = 2;
    public static int END_LABIRINT = 3;
    private int[][] _map;


    public LabirintViewModel()
    {
        _map = [
            [1, START_POSITION, 0, 0, 0, 0, 0, 0, 1, 0],
            [1, 1, 1, 0, 1, 1, 1, 0, 1, 0],
            [0, 0, 1, 0, 1, 0, 1, 0, 1, 0],
            [1, 0, 0, 0, 0, 0, 1, 0, 1, 0],
            [1, 0, 1, 0, 1, 1, 1, 0, 0, 0],
            [1, 0, 1, 0, 1, 0, 0, 0, 1, 1],
            [1, 0, 1, 0, 1, 0, 1, 0, 1, 0],
            [1, 0, 1, 0, 1, 0, 1, 0, 0, 0],
            [1, 0, 1, 1, 1, 1, 1, 0, 1, 0],
            [1, 0, 0, 0, 0, 0, 0, 0, 1, END_LABIRINT]
        ];
    }
    
    
}

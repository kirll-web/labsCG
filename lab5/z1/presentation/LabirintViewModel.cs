namespace z1.presentation;


public class LabirintViewModel
{
    public static int START_POSITION_X = 1;
    public static int START_POSITION_Y = 1;
    public int[][] Map;
    public float PlayerX { get; set; }
    public float PlayerY { get; set; }
    public float PlayerZ { get; set; }
    public float PlayerRotation { get; set; }
    
    public LabirintViewModel()
    {
        Map = new int[][]
        {
            new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            new int[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            new int[] {1, 0, 1, 1, 1, 1, 1, 1, 0, 1},
            new int[] {1, 0, 1, 0, 0, 0, 0, 1, 0, 1},
            new int[] {1, 0, 1, 0, 1, 1, 0, 1, 0, 1},
            new int[] {1, 0, 1, 0, 1, 0, 0, 1, 0, 1},
            new int[] {1, 0, 1, 0, 1, 1, 1, 1, 0, 1},
            new int[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            new int[] {1, 0, 1, 1, 1, 1, 1, 1, 0, 1},
            new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        };
        
        PlayerX = START_POSITION_X + 0.5f;
        PlayerY = 0.5f;
        PlayerZ = START_POSITION_Y + 0.5f;
        PlayerRotation = 0f;
    }
    
    public bool CanMoveTo(float x, float z)
    {
        int mapX = (int)x;
        int mapZ = (int)z;
        
        if (mapX < 0 || mapX >= Map[0].Length || mapZ < 0 || mapZ >= Map.Length)
            return false;
            
        return Map[mapZ][mapX] == 0;
    }
}

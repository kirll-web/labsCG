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
    private float _moveSpeed = 0.1f;
    private float _rotationSpeed = 2f;

    public LabirintViewModel()
    {
        Map = new int[][]
        {
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            new int[] { 6, 0, 5, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 6, 0, 5, 5, 0, 1, 3, 1, 0, 1, 4, 1, 0, 1, 1, 1, 0, 1 },
            new int[] { 6, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1 },
            new int[] { 6, 6, 0, 5, 5, 5, 0, 1, 1, 1, 0, 5, 1, 1, 0, 1, 0, 1 },
            new int[] { 6, 0, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1 },
            new int[] { 6, 0, 1, 1, 0, 5, 1, 1, 0, 6, 1, 1, 0, 1, 0, 1, 0, 1 },
            new int[] { 6, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1 },
            new int[] { 6, 1, 0, 3, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1 },
            new int[] { 6, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1 },
            new int[] { 6, 0, 1, 1, 1, 4, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1 },
            new int[] { 6, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1 },
            new int[] { 6, 1, 1, 1, 0, 1, 1, 5, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1 },
            new int[] { 6, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1 },
            new int[] { 6, 0, 1, 6, 0, 1, 0, 1, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1 },
            new int[] { 6, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1 },
            new int[] { 6, 1, 0, 1, 0, 2, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1 },
            new int[] { 6, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        PlayerX = START_POSITION_X + 0.5f;
        PlayerY = 0.5f;
        PlayerZ = START_POSITION_Y + 0.5f;
        PlayerRotation = 0f;
    }
    
    public bool TryMove(bool[] _keysPressed)
    {
        bool moved = false;
        float newX = PlayerX;
        float newZ = PlayerZ;
    
        if (_keysPressed[(int)Keys.W])
        {
            newX = PlayerX + (float)Math.Sin(PlayerRotation) * _moveSpeed;
            newZ = PlayerZ + (float)Math.Cos(PlayerRotation) * _moveSpeed;
        }
        else if (_keysPressed[(int)Keys.S])
        {
            newX = PlayerX - (float)Math.Sin(PlayerRotation) * _moveSpeed;
            newZ = PlayerZ - (float)Math.Cos(PlayerRotation) * _moveSpeed;
        }
    
        if (CanMoveTo(newX, PlayerZ))
        {
            PlayerX = newX;
            moved = true;
        }
    
        if (CanMoveTo(PlayerX, newZ))
        {
            PlayerZ = newZ;
            moved = true;
        }
    
        if (_keysPressed[(int)Keys.A])
        {
            PlayerRotation += _rotationSpeed * 0.01f;
            moved = true;
        }
        else if (_keysPressed[(int)Keys.D])
        {
            PlayerRotation -= _rotationSpeed * 0.01f;
            moved = true;
        }
    
        return moved;
    }

    private bool CanMoveTo(float x, float z, float playerRadius = 0.3f)
    {
        return IsCellWalkable(x - playerRadius, z - playerRadius) &&
               IsCellWalkable(x + playerRadius, z - playerRadius) &&
               IsCellWalkable(x - playerRadius, z + playerRadius) &&
               IsCellWalkable(x + playerRadius, z + playerRadius);
    }
    
    private bool IsCellWalkable(float x, float z)
    {
        int mapX = (int)Math.Floor(x);
        int mapZ = (int)Math.Floor(z);

        if (mapX < 0 || mapX >= Map[0].Length || mapZ < 0 || mapZ >= Map.Length)
            return false;

        return Map[mapZ][mapX] == 0;
    }
}

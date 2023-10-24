using OpenTK.Mathematics;

namespace Hexoidra.Globals
{
    public class Settings
    {

        public const int CHUNK_SIZE = 16;
        public const int CHUNK_HEIGHT = 64;

        public const int RENDER_DISTANCE = 4;
    }

    public class Data
    {
        public static Vector3 playerPosition = Vector3.Zero;
        public static Vector3i playerCoordinates = Vector3i.Zero;
    }

}

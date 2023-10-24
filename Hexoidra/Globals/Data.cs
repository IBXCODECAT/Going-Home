using OpenTK.Mathematics;

namespace Hexoidra.Globals
{
    public class Settings
    {

        public const int CHUNK_SIZE = 16;
        public const int CHUNK_HEIGHT = 128;

        public const int RENDER_DISTANCE = 2;

        public const float AMBIENT_LIGHTING_STRENGTH = 0.3f;
        public static readonly Vector3 AMBIENT_LIGHTING_COLOR = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public class Data
    {
        public static Vector3 playerPosition = Vector3.Zero;
        public static Vector3i playerCoordinates = Vector3i.Zero;
    }
}

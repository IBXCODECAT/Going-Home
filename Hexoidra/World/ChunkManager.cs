using Hexoidra.Data;
using Hexoidra.Graphics;
using OpenTK.Mathematics;

namespace Hexoidra.World
{
    /// <summary>
    /// A class containing extension methods for chunks
    /// </summary>
    internal static class ChunkManager
    {
        internal static List<Chunk> activeChunks { get; private set; } = new List<Chunk>() { };
        internal static List<Chunk> chunkCache { get; private set; } = new List<Chunk> { };

        internal static void BuildAll()
        {
            List<Vector2i> chunksToDraw = new List<Vector2i>();

            for(int x = 0; x < PlayerSettings.RENDER_DISTANCE * Chunk.CHUNK_SIZE; x += Chunk.CHUNK_SIZE)
            {
                for(int z = 0; z < PlayerSettings.RENDER_DISTANCE * Chunk.CHUNK_SIZE; z += Chunk.CHUNK_SIZE)
                {
                    float pointDistance = MathF.Sqrt(MathF.Pow(x - PlayerInfo.playerCoordinates.X, 2) + MathF.Pow(z - PlayerInfo.playerCoordinates.Z, 2));

                    Console.WriteLine(pointDistance);

                    if (pointDistance < PlayerSettings.RENDER_DISTANCE)
                    {
                        chunksToDraw.Add(new Vector2i(x, z));
                    }
                }
            }

            if(chunksToDraw.Count == activeChunks.Count) { return; }

            foreach(Vector2i c in chunksToDraw)
            {
                activeChunks.Add(new Chunk(new Chunk.ChunkPositionInfo(c)));
            }
        }

        internal static void RenderAll(Shader shader)
        {
            foreach(Chunk chunk in activeChunks)
            {
                chunk.Render(shader);
            }
        }

        internal static void DisposeAll()
        {
            foreach(Chunk chunk in activeChunks)
            {
                chunk.Dispose();
            }

            activeChunks.Clear();
        }
    }
}

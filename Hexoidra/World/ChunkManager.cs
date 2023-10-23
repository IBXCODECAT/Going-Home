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
        internal static List<Chunk> activeChunkCache { get; private set; } = new List<Chunk>() { };
        internal static List<Vector2i> activeChunkPositions { get; private set; } = new List<Vector2i> { };

        private static List<Vector2i> newChunkCoordsToDraw = new List<Vector2i>();

        internal static void BuildAll()
        {   
            newChunkCoordsToDraw.Clear();

            Vector3i playerCoords = PlayerInfo.playerCoordinates;

            for(int x = playerCoords.X - PlayerSettings.RENDER_DISTANCE; x < playerCoords.X + PlayerSettings.RENDER_DISTANCE; x++)
            {
                for(int z = playerCoords.Z - PlayerSettings.RENDER_DISTANCE; z < playerCoords.Z + PlayerSettings.RENDER_DISTANCE; z++)
                {
                    float pointDistance = MathF.Sqrt(MathF.Pow(x - PlayerInfo.playerCoordinates.X, 2) + MathF.Pow(z - PlayerInfo.playerCoordinates.Z, 2));

                    //Console.WriteLine(pointDistance);

                    if (pointDistance < PlayerSettings.RENDER_DISTANCE)
                    {
                    Vector2i chunkPos = new Vector2i(x, z);

                        newChunkCoordsToDraw.Add(chunkPos);
                    }
                }
            }

            if (newChunkCoordsToDraw == activeChunkPositions) return;

            foreach (Vector2i c in newChunkCoordsToDraw)
            {
                Console.WriteLine($"New chunk: {c.X}, {c.Y}");

                activeChunkPositions.Clear();
                activeChunkCache.Clear();
                
                activeChunkPositions.Add(c);
                activeChunkCache.Add(new Chunk(new Chunk.ChunkPositionInfo(c)));
            }

            newChunkCoordsToDraw.Clear();

            Console.WriteLine(activeChunkCache.Count);
        }

        internal static void RenderAll(Shader shader)
        {
            
            foreach(Chunk chunk in activeChunkCache)
            {
                chunk.Render(shader);
            }
        }

        internal static void DisposeAll()
        {
            foreach(Chunk chunk in activeChunkCache)
            {
                chunk.Dispose();
            }

            activeChunkCache.Clear();
        }
    }
}

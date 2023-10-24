using Hexoidra.Globals;
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
        internal static List<Vector2i> activeChunkPositions { get; private set; } = new List<Vector2i> { };

        
        internal static void BuildChunks()
        {
            //Store the player's coordinates (position truncated to chunk coords) for later use
            Vector3i playerCoords = Data.playerCoordinates / Settings.CHUNK_SIZE;

            //Create a list to store positions we want to create our potential new chunks at
            List<Vector2i> potentialChunkPositions = new List<Vector2i>();

            //Loop through chunk positions on both the X/Z axis ranging from -distance to +distance
            for(int x = playerCoords.X - Settings.RENDER_DISTANCE; x < playerCoords.X + Settings.RENDER_DISTANCE; x++)
            {
                for(int z = playerCoords.Z - Settings.RENDER_DISTANCE; z < playerCoords.Z + Settings.RENDER_DISTANCE; z++)
                {
                    //Define a position for a potential new chunk and add it to the list of chunkPositions
                    Vector2i chunkPos = new Vector2i(x, z);
                    potentialChunkPositions.Add(chunkPos);
                }
            }

            //Loop through each chunk position in our list of active chunk positions (currently loaded chunk positions)
            foreach(Vector2i chunkPos in activeChunkPositions.ToArray())
            {
                //If our list of potential chunk positions does not contain an active chunk position...
                if(!potentialChunkPositions.Contains(chunkPos))
                {
                    //Remove the chunk position from the list of active chunks
                    activeChunkPositions.Remove(chunkPos);

                    //Get the chunk at this position and remove it form the list of active chunks to render
                    Chunk chunk = Chunk.GetChunkAtPosition(chunkPos);
                    activeChunks.Remove(chunk);

                    //Safely dispose of this chunk to prevent a memory leak
                    chunk.Dispose();

                    Console.WriteLine($"Chunk disposed at ({chunkPos.X}, {chunkPos.Y})");
                }
            }

            //Loop through each chunk position in our list of potential chunk positions (chunks expected to be rendered next frame)
            foreach(Vector2i chunkPos in potentialChunkPositions.ToArray())
            {
                //If our list of active chunk positions does not contain a potential chunk position...
                if (!activeChunkPositions.Contains(chunkPos))
                {
                    //Add teh chunk position from the list of active chunks
                    activeChunkPositions.Add(chunkPos);

                    //Construct a new chunk and add it to our list of active chunks to render
                    Chunk chunk = new Chunk(new Chunk.ChunkPositionInfo(chunkPos));
                    activeChunks.Add(chunk);

                    Console.WriteLine($"Chunk constructed at ({chunkPos.X}, {chunkPos.Y})");
                }
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

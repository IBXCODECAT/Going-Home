namespace Hexoidra.World
{
    /// <summary>
    /// A class containing extension methods for chunks
    /// </summary>
    internal static class ChunkManager
    {
        internal static List<Chunk> chunksToDraw { get; private set; } = new List<Chunk>() { };
        internal static List<Chunk> chunkCache { get; private set; } = new List<Chunk> { };

        /// <summary>
        /// Prepares this chunk to be included in the next render
        /// by adding it to a list of chunks to draw
        /// </summary>
        /// <param name="chunk">The chunk to load</param>
        internal static void PrepareForLoad(this Chunk chunk)
        {
            chunksToDraw.Add(chunk);
        }

        /// <summary>
        /// Prepares this chunk for deletion in the next frame
        /// by removing it from a list of chunks to draw
        /// </summary>
        /// <param name="chunk">The chunk to unload</param>
        internal static void PrepareForDeletion(this Chunk chunk)
        {
            chunksToDraw.Remove(chunk);
        }

        private static void ValidateChunkCache()
        {
            //Make sure chunk cache stays under a memory limit (EX: 512 mb)
        }
    }
}

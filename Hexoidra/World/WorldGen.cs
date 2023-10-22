using SimplexNoise;
using OpenTK.Mathematics;

namespace Hexoidra.World
{
    internal class WorldGen
    {
        private static Random rng = new Random();

        internal static Block[,,] GenChunkBlocks(Chunk.ChunkPositionInfo chunkInfo)
        {
            float[,] heightmap = new float[Chunk.CHUNK_SIZE, Chunk.CHUNK_SIZE];

            Noise.Seed = 123456;

            for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
            {
                for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
                {
                    heightmap[x, z] = Noise.CalcPixel2D(x + chunkInfo.position.X, z + chunkInfo.position.Y, 0.01f);
                }
            }

            Block[,,] chunkBlocks = new Block[Chunk.CHUNK_SIZE, Chunk.CHUNK_HEIGHT, Chunk.CHUNK_SIZE];

            for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
            {
                for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
                {
                    heightmap[x, z] = (int)(heightmap[x, z] / 10);

                    for (int y = 0; y < Chunk.CHUNK_HEIGHT; y++)
                    {
                        BlockType type = BlockType.AIR;

                        if (y < heightmap[x, z] - rng.Next(3, 5))
                        {
                            type = BlockType.STONE;
                        }
                        else if (y < heightmap[x, z] - 1)
                        {
                            type = BlockType.DIRT;
                        }
                        else if (y == heightmap[x, z] - 1)
                        {
                            type = BlockType.GRASS;
                        }

                        chunkBlocks[x, y, z] = new Block(new Vector3(x, y, z), type);
                    }
                }
            }

            return chunkBlocks;
        }
    }
}

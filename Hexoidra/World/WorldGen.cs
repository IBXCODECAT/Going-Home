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
                    heightmap[x, z] = Noise.CalcPixel2D(x + chunkInfo.blockStartPosition.X, z + chunkInfo.blockStartPosition.Y, 0.01f);
                }
            }

            //Create an array to store the block data for this chunk
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

                        //Generate this block and offset the block's position by the chunk's offset
                        chunkBlocks[x, y, z] = new Block(new Vector3(chunkInfo.blockStartPosition.X + x, y, chunkInfo.blockStartPosition.Y + z), type);
                    }
                }
            }

            return chunkBlocks;
        }
    }
}

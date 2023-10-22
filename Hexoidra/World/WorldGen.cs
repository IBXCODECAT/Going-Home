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
                    heightmap[x, z] = Noise.CalcPixel2D(x + chunkInfo.chunkXYOriginInWorldSpace.X, z + chunkInfo.chunkXYOriginInWorldSpace.Y, 0.01f);
                }
            }

            //Create an array to store the block data for this chunk
            Block[,,] chunkBlocks = new Block[Chunk.CHUNK_SIZE, Chunk.CHUNK_HEIGHT, Chunk.CHUNK_SIZE];

            for (int blockPosX = 0; blockPosX < Chunk.CHUNK_SIZE; blockPosX++)
            {
                for (int blockPosZ = 0; blockPosZ < Chunk.CHUNK_SIZE; blockPosZ++)
                {
                    heightmap[blockPosX, blockPosZ] = (int)heightmap[blockPosX, blockPosZ] / 10 + 25;

                    for (int blockPosY = 0; blockPosY < Chunk.CHUNK_HEIGHT; blockPosY++)
                    {
                        BlockType blockType = BlockType.AIR;

                        if (blockPosY < heightmap[blockPosX, blockPosZ] - rng.Next(3, 5))
                        {   
                            blockType = BlockType.STONE;
                        }
                        else if (blockPosY < heightmap[blockPosX, blockPosZ] - 1)
                        {
                            blockType = BlockType.DIRT;
                        }
                        else if (blockPosY == heightmap[blockPosX, blockPosZ] - 1)
                        {
                            blockType = BlockType.GRASS;
                        }

                        //Generate this block and offset the block's X/-/Z positions by the chunk's X/Y offset
                        chunkBlocks[blockPosX, blockPosY, blockPosZ] = new Block
                        (
                            new Vector3
                            (
                                chunkInfo.chunkXYOriginInWorldSpace.X + blockPosX,
                                blockPosY,
                                chunkInfo.chunkXYOriginInWorldSpace.Y + blockPosZ
                            ),
                            blockType
                        );
                    }
                }
            }

            return chunkBlocks;
        }
    }
}

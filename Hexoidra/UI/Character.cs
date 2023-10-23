using Hexoidra.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;
using Hexoidra.Data;

namespace Hexoidra.UI
{
    internal class UICharacter
    {
        private Vector3 position;
        private char character;

        internal Dictionary<char, List<Vector2>> charUVs = new Dictionary<char, List<Vector2>>();

        public UICharacter(Vector3 position, char character = '#')
        {
            this.position = position;
            this.character = character;

            charUVs = new Dictionary<char, List<Vector2>>();

            if (character != ' ')
            {
                charUVs = GetUvsFromAtlasCoordinates(TextureData.characterUVCoordinates);
            }
        }

        private Dictionary<char, List<Vector2>> GetUvsFromAtlasCoordinates(Dictionary<char, List<Vector2>> coords)
        {
            Dictionary<char, List<Vector2>> charUVData = new Dictionary<char, List<Vector2>>();

            foreach (KeyValuePair<char, List<Vector2>> coord in coords)
            {
                charUVData[coord.Key] = new List<Vector2>
                {
                    new Vector2(coord.Value[0].X + 1f / TextureData.TEXT_ITEM_SIZE, (coord.Value[0].Y + 1f) / TextureData.TEXT_ITEM_SIZE),
                    new Vector2(coord.Value[1].X + 1f / TextureData.TEXT_ITEM_SIZE, (coord.Value[1].Y + 1f) / TextureData.TEXT_ITEM_SIZE),
                    new Vector2(coord.Value[2].X + 1f / TextureData.TEXT_ITEM_SIZE, (coord.Value[2].Y + 1f) / TextureData.TEXT_ITEM_SIZE),
                    new Vector2(coord.Value[3].X + 1f / TextureData.TEXT_ITEM_SIZE, (coord.Value[3].Y + 1f) / TextureData.TEXT_ITEM_SIZE),

                    //new Vector2((Value.X + 1f) / TextureData.ATLAS_ITEM_SIZE, (faceCoord.Value.Y + 1f) /TextureData.ATLAS_ITEM_SIZE),
                    //new Vector2(faceCoord.Value.X / TextureData.ATLAS_ITEM_SIZE, (faceCoord.Value.Y + 1f) / TextureData.ATLAS_ITEM_SIZE),
                    //new Vector2(faceCoord.Value.X / TextureData.ATLAS_ITEM_SIZE, faceCoord.Value.Y / TextureData.ATLAS_ITEM_SIZE),
                    //new Vector2((faceCoord.Value.X + 1f) / TextureData.ATLAS_ITEM_SIZE, faceCoord.Value.Y / TextureData.ATLAS_ITEM_SIZE),
                };
            }

            return charUVData;
        }
    }
}

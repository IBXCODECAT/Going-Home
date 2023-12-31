﻿using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Hexoidra.World
{
    internal class TextureData
    {
        internal const float ATLAS_SIZE = 16f;

        internal static Dictionary<BlockType, Dictionary<Faces, Vector2>> blockTypeUVCoordinates = new Dictionary<BlockType, Dictionary<Faces, Vector2>>()
        {
            {BlockType.DOLOMITE , new Dictionary<Faces, Vector2>() {
                {Faces.FRONT, new Vector2(0f, 15f) },
                {Faces.BACK, new Vector2(0f, 15f) },
                {Faces.LEFT, new Vector2(0f, 15f) },
                {Faces.RIGHT, new Vector2(0f, 15f) },
                {Faces.TOP, new Vector2(0f, 15f) },
                {Faces.BOTTOM, new Vector2(0f, 15f) },
            } },
            {BlockType.SLATE , new Dictionary<Faces, Vector2>() {
                {Faces.FRONT, new Vector2(1f, 15f) },
                {Faces.BACK, new Vector2(1f, 15f) },
                {Faces.LEFT, new Vector2(1f, 15f) },
                {Faces.RIGHT, new Vector2(1f, 15f) },
                {Faces.TOP, new Vector2(1f, 15f) },
                {Faces.BOTTOM, new Vector2(1f, 15f) },
            } },
            {BlockType.DIRT , new Dictionary<Faces, Vector2>() {
                {Faces.FRONT, new Vector2(2f, 15f) },
                {Faces.BACK, new Vector2(2f, 15f) },
                {Faces.LEFT, new Vector2(2f, 15f) },
                {Faces.RIGHT, new Vector2(2f, 15f) },
                {Faces.TOP, new Vector2(2f, 15f) },
                {Faces.BOTTOM, new Vector2(2f, 15f) },
            } },
            {BlockType.GRASS , new Dictionary<Faces, Vector2>() {
                {Faces.FRONT, new Vector2(3f, 15f) },
                {Faces.BACK, new Vector2(3f, 15f) },
                {Faces.LEFT, new Vector2(3f, 15f) },
                {Faces.RIGHT, new Vector2(3f, 15f) },
                {Faces.TOP, new Vector2(4f, 15f) },
                {Faces.BOTTOM, new Vector2(2f, 15f) },
            } },
            {BlockType.COAL , new Dictionary<Faces, Vector2>() {
                {Faces.FRONT, new Vector2(0f, 14f) },
                {Faces.BACK, new Vector2(0f, 14f) },
                {Faces.LEFT, new Vector2(0f, 14f) },
                {Faces.RIGHT, new Vector2(0f, 14f) },
                {Faces.TOP, new Vector2(0f, 14f) },
                {Faces.BOTTOM, new Vector2(0f, 14f) },
            } },


        };
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexLayout : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private Vector2 gridSize;

    [Header("Tile Settings")]
    [SerializeField] private float outerSize = 1f;
    [SerializeField] private float innerSize = 0f;
    [SerializeField] private float height = 1f;
    [SerializeField] private bool isFlatTopped;

    [SerializeField] private Material material;

    private void OnEnable()
    {
        LayoutGrid();
    }

    private void LayoutGrid()
    {
        /*
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = new GameObject($"Hex {x}, {y}", typeof(HexRenderer));
                tile.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(x, y));

                HexRenderer hr = tile.GetComponent<HexRenderer>();
                hr.SetMaterial(material);

                hr.useFlatTop = isFlatTopped;
                hr.outerSize = outerSize;
                hr.innerSize = innerSize;
                hr.height = height;

                HexFaceBooleans faceFilter = new HexFaceBooleans();

                faceFilter.top = true;
                faceFilter.bottom = true;

                faceFilter.sides.side0 = false;
                faceFilter.sides.side1 = false;
                faceFilter.sides.side2 = false;
                faceFilter.sides.side3 = false;
                faceFilter.sides.side4 = false;
                faceFilter.sides.side5 = false;

                hr.DrawMesh(faceFilter);

                tile.transform.SetParent(transform, true);
            }
        }*/

        /*
        for(int y = 0; y < gridSize.y; y++)
        {
            for(int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = new GameObject($"Tile {x} {y}");
                tile.transform.position = GetPositionForHexFromCoordinate2D(new Vector2Int(x, y));
            }
        }*/
    }

    private void OnDrawGizmos()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(GetPositionForHexFromCoordinate2D(new Vector2Int(x, y), 0f), outerSize / 5f);
            }
        }
    }

    private Vector3 GetPositionForHexFromCoordinate2D(Vector2Int coordinate, float worldYLevel)
    {
        int column = coordinate.x;
        int row = coordinate.y;

        float width;
        float height;
        float xPosition;
        float yPosition;
        bool shouldOffset;
        float horizontalDistance;
        float verticalDistance;
        float offset;
        float size = outerSize;

        if (!isFlatTopped)
        {
            shouldOffset = (row % 2) == 0;
            width = Mathf.Sqrt(3) * size;
            height = 2f * size;

            horizontalDistance = width;
            verticalDistance = height * (3f / 4f);

            offset = (shouldOffset) ? width / 2 : 0;

            xPosition = (column * (horizontalDistance)) + offset;
            yPosition = (row * (verticalDistance));

        }
        else
        {
            shouldOffset = (column % 2) == 0;
            width = 2f * size;
            height = Mathf.Sqrt(3f) * size;

            horizontalDistance = width * (3f / 4f);
            verticalDistance = height;

            offset = (shouldOffset) ? height / 2 : 0;
            xPosition = (column * (horizontalDistance));
            yPosition = (row * (verticalDistance)) - offset;

        }

        return new Vector3(xPosition, worldYLevel, -yPosition);
    }
}

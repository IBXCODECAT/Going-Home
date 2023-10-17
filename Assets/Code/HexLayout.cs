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

    private void OnValidate()
    {
        if(Application.isPlaying)
        {
            LayoutGrid();
        }
    }

    private void LayoutGrid()
    {
        for(int y = 0; y < gridSize.y; y++)
        {
            for(int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = new GameObject($"Hex {x}, {y}", typeof(HexRenderer));
                tile.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(x, y));

                HexRenderer hr = tile.GetComponent<HexRenderer>();
                hr.useFlatTop = isFlatTopped;
                hr.outerSize = outerSize;
                hr.innerSize = innerSize;
                hr.height= height;

                hr.SetMaterial(material);
                hr.DrawMesh();

                tile.transform.SetParent(transform, true);
            }
        }
    }

    private Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
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

        if(!isFlatTopped)
        {
            shouldOffset = (row % 2) == 0;
            width = Mathf.Sqrt(3) * size;
            height = 2f * size;

            horizontalDistance = width;
            verticalDistance = height * (3f/4f);

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

        return new Vector3(xPosition, 0, -yPosition);
    }
}

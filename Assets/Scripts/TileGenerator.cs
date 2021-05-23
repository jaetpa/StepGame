using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    
    [SerializeField] private int tileCount;
    [SerializeField] private float tileSize;
    [SerializeField] private float tileSpacing;

    [Space] [SerializeField] private Transform TileContainer;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Generate Tiles")]
    private void GenerateTiles()
    {
        TileContainer.DestroyAllChildren();
        for (int i = 0; i < tileCount; i++)
        {
            var tile = Instantiate(tilePrefab, TileContainer);
            tile.transform.localPosition = Vector3.zero.WithZ(i * (tileSize + tileSpacing));
        }
    }
}

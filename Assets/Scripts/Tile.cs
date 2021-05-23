using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileType Type;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            DoTileAction();
        }
    }

    private void DoTileAction()
    {
        switch (Type)
        {
            case TileType.Standard:
                break;
            case TileType.Fake:
                gameObject.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum TileType
{
    Standard,
    Fake
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public int componentCount;

    public Component addedComponent;

    public BlockType blockType;

    public enum BlockType{
        Empty = 0,
        SingleCoin = 1,
        MultipleCoin = 2,
        LevelUp = 3,
        Star = 4,
        OneUp = 5,
    }
}

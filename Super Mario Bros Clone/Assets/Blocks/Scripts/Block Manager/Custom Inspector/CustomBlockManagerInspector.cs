
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(BlockManager))]
[CanEditMultipleObjects]
public class CustomBlockManagerInspector : Editor
{

    BlockManager.BlockType blockType;

    BlockManager blockManager;

    private bool componentError = false;
    private bool activeResetButton = false;

    private void OnEnable()
    {
        blockManager = (BlockManager)target;
    }
    public override void OnInspectorGUI()
    {
        blockType = blockManager.blockType;

        blockType = (BlockManager.BlockType)EditorGUILayout.EnumPopup("Block Type", blockType);

        blockManager.blockType = blockType;

        if (IsComponentAddedOrDeletedWithoutEditor())
        {
            EditorGUILayout.HelpBox("Block component changed without custom inspector. Block can cause bugs, use reset button and set block again.", MessageType.Warning);
            activeResetButton = true;
        }

        if (GUILayout.Button("Save Block"))
        {
            if (blockManager.componentCount == 0)
            {
                blockManager.componentCount++;
                switch (blockType)
                {
                    case BlockManager.BlockType.Empty:
                        blockManager.gameObject.AddComponent<EmptyBlock>();
                        blockManager.addedComponent = blockManager.gameObject.GetComponent<EmptyBlock>();
                        break;
                    case BlockManager.BlockType.SingleCoin:
                        blockManager.gameObject.AddComponent<SingleCoinBlock>();
                        blockManager.addedComponent = blockManager.gameObject.GetComponent<SingleCoinBlock>();
                        break;
                    case BlockManager.BlockType.MultipleCoin:
                        blockManager.gameObject.AddComponent<MultipleCoinBlock>();
                        blockManager.addedComponent = blockManager.gameObject.GetComponent<MultipleCoinBlock>();
                        break;
                    case BlockManager.BlockType.LevelUp:
                        blockManager.gameObject.AddComponent<LevelUpBlock>();
                        blockManager.addedComponent = blockManager.gameObject.GetComponent<LevelUpBlock>();
                        break;
                    case BlockManager.BlockType.Star:
                        blockManager.gameObject.AddComponent<StarBlock>();
                        blockManager.addedComponent = blockManager.gameObject.GetComponent<StarBlock>();
                        break;
                    case BlockManager.BlockType.OneUp:
                        blockManager.gameObject.AddComponent<OneUpBlock>();
                        blockManager.addedComponent = blockManager.gameObject.GetComponent<OneUpBlock>();
                        break;
                }
                EditorUtility.SetDirty(blockManager);
                EditorSceneManager.MarkAllScenesDirty();
            }
            else
            {
                componentError = true;
            }
        }

        GUI.enabled = (blockManager.componentCount != 0) || activeResetButton;

        if (GUILayout.Button("Reset Block"))
        {
            ResetAllComponents();
            blockManager.addedComponent = null;
            blockManager.componentCount = 0;
        }

        if (componentError)
            EditorGUILayout.HelpBox("This block already have a component !", MessageType.Error);
    }
    private void OnDisable()
    {
        componentError = false;
    }
    private void ResetAllComponents()
    {
        int enumLength = (int)BlockManager.BlockType.OneUp;

        for (int i = 0; i <= enumLength; i++)
        {
            switch (i)
            {
                case ((int)BlockManager.BlockType.Empty):
                    if (blockManager.gameObject.GetComponent<EmptyBlock>() != null) DestroyImmediate(blockManager.GetComponent<EmptyBlock>());
                    break;
                case ((int)BlockManager.BlockType.SingleCoin):
                    if (blockManager.gameObject.GetComponent<SingleCoinBlock>() != null) DestroyImmediate(blockManager.GetComponent<SingleCoinBlock>());
                    break;
                case ((int)BlockManager.BlockType.MultipleCoin):
                    if (blockManager.gameObject.GetComponent<MultipleCoinBlock>() != null) DestroyImmediate(blockManager.GetComponent<MultipleCoinBlock>());
                    break;
                case ((int)BlockManager.BlockType.LevelUp):
                    if (blockManager.gameObject.GetComponent<LevelUpBlock>() != null) DestroyImmediate(blockManager.GetComponent<LevelUpBlock>());
                    break;
                case ((int)BlockManager.BlockType.Star):
                    if (blockManager.gameObject.GetComponent<StarBlock>() != null) DestroyImmediate(blockManager.GetComponent<StarBlock>());
                    break;
                case ((int)BlockManager.BlockType.OneUp):
                    if (blockManager.gameObject.GetComponent<OneUpBlock>() != null) DestroyImmediate(blockManager.GetComponent<OneUpBlock>());
                    break;
            }
        }
    }
    private bool IsComponentAddedOrDeletedWithoutEditor()
    {
        int enumLength = (int)BlockManager.BlockType.OneUp;
        int totalComponentCount = 0;
        bool returnValue = false;

        for (int i = 0; i <= enumLength; i++)
        {
            switch (i)
            {
                case ((int)BlockManager.BlockType.Empty):
                    if (blockManager.gameObject.GetComponent<EmptyBlock>() != null)
                    {
                        if (totalComponentCount != 0)
                        {
                            returnValue = true;
                            break;
                        }

                        totalComponentCount++;

                        if (blockManager.gameObject.GetComponent<EmptyBlock>() != blockManager.addedComponent)
                        {
                            returnValue = true;
                            break;
                        }
                    }
                    break;
                case ((int)BlockManager.BlockType.SingleCoin):
                    if (blockManager.gameObject.GetComponent<SingleCoinBlock>() != null && totalComponentCount == 0)
                    {
                        if (totalComponentCount != 0)
                        {
                            returnValue = true;
                            break;
                        }

                        totalComponentCount++;
                        if (blockManager.gameObject.GetComponent<SingleCoinBlock>() != blockManager.addedComponent)
                        {
                            returnValue = true;
                            break;
                        }
                    }
                    break;
                case ((int)BlockManager.BlockType.MultipleCoin):
                    if (blockManager.gameObject.GetComponent<MultipleCoinBlock>() != null)
                    {
                        if (totalComponentCount != 0)
                        {
                            returnValue = true;
                            break;
                        }

                        totalComponentCount++;
                        if (blockManager.gameObject.GetComponent<MultipleCoinBlock>() != blockManager.addedComponent)
                        {
                            returnValue = true;
                            break;
                        }
                    }
                    break;
                case ((int)BlockManager.BlockType.LevelUp):
                    if (blockManager.gameObject.GetComponent<LevelUpBlock>() != null)
                    {
                        if (totalComponentCount != 0)
                        {
                            returnValue = true;
                            break;
                        }

                        totalComponentCount++;
                        if (blockManager.gameObject.GetComponent<LevelUpBlock>() != blockManager.addedComponent)
                        {
                            returnValue = true;
                            break;
                        }
                    }
                    break;
                case ((int)BlockManager.BlockType.Star):
                    if (blockManager.gameObject.GetComponent<StarBlock>() != null)
                    {
                        if (totalComponentCount != 0)
                        {
                            returnValue = true;
                            break;
                        }

                        totalComponentCount++;
                        if (blockManager.gameObject.GetComponent<StarBlock>() != blockManager.addedComponent)
                        {
                            returnValue = true;
                            break;
                        }
                    }
                    break;
                case ((int)BlockManager.BlockType.OneUp):
                    if (blockManager.gameObject.GetComponent<OneUpBlock>() != null)
                    {
                        if (totalComponentCount != 0)
                        {
                            returnValue = true;
                            break;
                        }

                        totalComponentCount++;
                        if (blockManager.gameObject.GetComponent<OneUpBlock>() != blockManager.addedComponent)
                        {
                            returnValue = true;
                            break;
                        }
                    }
                    break;
            }
        }
        return returnValue;
    }

}
#endif

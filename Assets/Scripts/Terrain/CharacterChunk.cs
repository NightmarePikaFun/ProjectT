using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChunk : MonoBehaviour
{
    public GameObject mainChunkGO;
    private ChunkData mainChunk;

    private List<ChunkData> playerChunk;
    private List<float> playerChunkRange;

    private void Awake()
    {
        playerChunk = new List<ChunkData>();
        playerChunkRange = new List<float>();
    }

    public void AddChunk(ChunkData addedChunk)
    {
        if(!playerChunk.Contains(addedChunk))
        {
            playerChunk.Add(addedChunk);
            playerChunkRange.Add(CalcRange(addedChunk));
        }
        if (playerChunk.Count > 1)
            CalcMainChunk(playerChunkRange, 0, playerChunkRange.Count, playerChunk);
        mainChunk = playerChunk[0];
        mainChunkGO = playerChunk[0].gameObject;
        mainChunkGO.name = "PCC";
    }

    public void RemoveChunk(ChunkData removedChunk)
    {
        int removedIndex = playerChunk.IndexOf(removedChunk);
        if (removedIndex >= 0)
        {
            playerChunk[removedIndex].name = "Chunk";
            playerChunkRange.RemoveAt(removedIndex);
            playerChunk.RemoveAt(removedIndex);
        }
        if (playerChunk.Count > 1)
            CalcMainChunk(playerChunkRange, 0, playerChunkRange.Count, playerChunk);
        mainChunk = playerChunk[0];
        mainChunkGO = playerChunk[0].gameObject;
        mainChunkGO.name = "PCC";
    }
    
    private float CalcRange(ChunkData addedChunk)
    {
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 chunkPos = new Vector2(addedChunk.transform.position.x, addedChunk.transform.position.z);
        return Vector2.Distance(playerPos, chunkPos);
    }

    private void CalcMainChunk(List<float> arr, int low, int high, List<ChunkData> chunksData)
    {
        BubbleSort(arr, chunksData);
        //QuickSortChunk(arr, low, high, chunksData);
        
    }

    private void QuickSortChunk(List<float> arr, int low, int high, List<ChunkData> chunksData)
    {
        if (low < high)
        {
            int pivotIndex = Partition(arr, low, high, chunksData);
            QuickSortChunk(arr, low, pivotIndex-1, chunksData);
            QuickSortChunk(arr, pivotIndex+1, high, chunksData);
        }
    }

    private int Partition(List<float> arr, int low, int high, List<ChunkData> chunksData)
    {
        float pivot = arr[low];
        int left = low+1;
        int right = high;
        while(true)
        {
            while(left <= right && arr[left] <= pivot)
            {
                left++;
            }
            while(right >= left && arr[right] >= pivot)
            {
                right--;
            }
            if(right < left)
            {
                break;
            }
            else
            {
                float tempW = arr[left];
                ChunkData tempChunkW = chunksData[left];
                arr[left] = arr[right];
                chunksData[left] = chunksData[right];
                arr[right] = tempW;
                chunksData[right] = tempChunkW;
            }
        }
        float temp = arr[low];
        arr[low] = arr[right];
        arr[right] = temp;
        ChunkData tempChunk = chunksData[low];
        chunksData[low] = chunksData[right];
        chunksData[right] = tempChunk;
        return right;
    }

    private void BubbleSort(List<float> arr, List<ChunkData> chunks)
    {
        int n = arr.Count;
        for(int j = 1; j < n; j++)
        {
            bool isSorted = true;
            for(int i = 0; i < n-j; i++)
            {
                if (arr[i] > arr[i+1]) 
                {
                    float tmp = arr[i];
                    arr[i] = arr[i+1];
                    arr[i+1] = tmp;
                    ChunkData tmpChunk = chunks[i];
                    chunks[i] = chunks[i + 1];
                    chunks[i + 1] = tmpChunk;
                    isSorted = false;
                }
            }
            if(isSorted)
            {
                break;
            }
        }
    }
}

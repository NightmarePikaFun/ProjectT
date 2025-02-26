using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkData : MonoBehaviour
{
    private DateTime awakeTime;

    Vector2Int chunkNumber;
    // Start is called before the first frame update

    public void Load()
    {
        awakeTime = DateTime.Now;
        gameObject.SetActive(true);
    }

    public void Unload()
    {
        gameObject.SetActive(false);
    }
   

    public DateTime GetStartTime()
    {
        return awakeTime;
    }

    public void UpdateTime()
    {
        awakeTime = DateTime.Now;
    }

    public void SetChunkNumber(Vector2Int value)
    {
        chunkNumber = value;
    }

    public Vector2Int GetChunkNumder()
    {
        return chunkNumber;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Player")
        {
            for(int i = chunkNumber.x - GenerateChank.Instance.playerLoadRadius; i < chunkNumber.x+GenerateChank.Instance.playerLoadRadius;i++)
            {
                for (int j = chunkNumber.y - GenerateChank.Instance.playerLoadRadius; j < chunkNumber.y + GenerateChank.Instance.playerLoadRadius; j++)
                {
                    GenerateChank.Instance.LoadChunk((ushort)i, (ushort)j);
                }
            }
            other.GetComponent<CharacterChunk>().AddChunk(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<CharacterChunk>().RemoveChunk(this);
        }
    }
}

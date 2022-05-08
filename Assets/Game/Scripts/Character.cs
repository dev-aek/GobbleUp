using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum CharacterID
    {
        Player,
        Stack,
        Boss,
        None
    }


 
    

    public CharacterID currentCharacterID = CharacterID.None;
    public int characterSize = 1;
    public Vector3 startSize;
    public Material currentMaterial;

    private void Start()
    {
        //currentMaterial = GetComponent<MeshRenderer>().material;
        if (currentCharacterID == CharacterID.Player)transform.localScale = startSize;
        //if (currentCharacterID == CharacterID.Stack)transform.localScale = new Vector3(characterSize, characterSize, characterSize);
        GameManager.Instance.onRightCharacterTake += OnRightTake;
        GameManager.Instance.onWrongCharacterTake += OnWrongTake;
    }
    private void OnDisable()
    {
        GameManager.Instance.onRightCharacterTake -= OnRightTake;
        GameManager.Instance.onWrongCharacterTake -= OnWrongTake;
    }
    private void OnRightTake()
    {
        if (currentCharacterID == CharacterID.Player && this.transform.localScale.x <= 60f)
        {
            transform.localScale = new Vector3(
                transform.localScale.x + GameManager.Instance.playerGrowSize,
                transform.localScale.y + (GameManager.Instance.playerGrowSize*3/2),
                transform.localScale.z + GameManager.Instance.playerGrowSize);
            if(this.transform.localScale.x >= 60f) transform.localScale = new Vector3(60f, 90f, 60f);
            //if (this.transform.localScale.x == 0f) transform.localScale = new Vector3(1.35f, 0.45f, 1.35f);

        }

    }
    private void OnWrongTake()
    {
        if (currentCharacterID == CharacterID.Player)
        {
            transform.localScale = new Vector3(
                transform.localScale.x - GameManager.Instance.playerGrowSize,
                transform.localScale.y - GameManager.Instance.playerGrowSize*3/2,
                transform.localScale.z - GameManager.Instance.playerGrowSize);
            if (currentCharacterID == CharacterID.Player && this.transform.localScale.x <= 0f)
            {

                GameManager.onLoseEvent?.Invoke();
                //GameManager.Instance.currentState = GameManager.GameState.Failed;
            }
        }


    }

    public float GetMamaValue()
    {
        return characterSize;
    }

    private void ExecuteOnWin()
    {
        GameManager.Instance.currentState = GameManager.GameState.Victory;
    }
    private void ExecuteOnLose()
    {
        GameManager.Instance.currentState = GameManager.GameState.Failed;
    }
}

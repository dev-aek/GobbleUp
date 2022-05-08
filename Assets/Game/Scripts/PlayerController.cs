using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.onTouchStart += ProcessPlayerSwere;
        InputManager.Instance.onTouchMove += ProcessPlayerSwere;
    }

    private void OnDisable()
    {
        InputManager.Instance.onTouchStart -= ProcessPlayerSwere;
        InputManager.Instance.onTouchMove -= ProcessPlayerSwere;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessPlayerForwardMovement();
    }

    private void ProcessPlayerForwardMovement()
    {
        if (GameManager.Instance.currentState == GameManager.GameState.Normal)
        {
            GetComponent<Mover>().MoveTo(new Vector3(
                0f,
                0f,
                GameManager.Instance.forwardSpeed));
        }
    }

    private void ProcessPlayerSwere()
    {
        if (GameManager.Instance.currentState == GameManager.GameState.Normal)
        {
            GetComponent<Mover>().MoveTo(new Vector3(
                InputManager.Instance.GetDirection().x * GameManager.Instance.horizontalSpeed, 0f, 0f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Character>() != null)
        {
            Character targetCharacter = other.GetComponent<Character>();
            Character currentCharacter = this.GetComponentInChildren<Character>();

            if (targetCharacter.currentCharacterID == Character.CharacterID.Stack)
            {
                print("Same material");
                int currentAmount = currentCharacter.characterSize;
                currentAmount++;
                currentCharacter.characterSize = currentAmount;
                GameManager.Instance.onCharacterTake?.Invoke(currentAmount);
                GameManager.Instance.onRightCharacterTake?.Invoke();
                Destroy(other.gameObject);
            }
        }

        if (other.GetComponent<Obstacle>() != null)
        {
            Obstacle targetCharacter = other.GetComponent<Obstacle>();
            Character currentCharacter = this.GetComponentInChildren<Character>();

            if (targetCharacter.currentObstacleID == Obstacle.ObstacleID.Barrier)
            {
                int currentAmount = currentCharacter.characterSize;
                currentCharacter.characterSize = currentAmount;
                GameManager.Instance.onCharacterTake(currentAmount);
                GameManager.Instance.onWrongCharacterTake?.Invoke();
                Destroy(other.gameObject);
                print("Not same material");
            }

            if (targetCharacter.currentObstacleID == Obstacle.ObstacleID.Space)
            {
                int currentAmount = currentCharacter.characterSize;
                currentCharacter.characterSize = currentAmount;
                GameManager.Instance.onCharacterTake(currentAmount);
                GameManager.Instance.onWrongCharacterTake?.Invoke();
                //Destroy(other.gameObject);
                other.gameObject.GetComponent<MeshRenderer>().enabled = true;
                print("space");
            }

            if (targetCharacter.currentObstacleID == Obstacle.ObstacleID.Finish)
            {
                GameManager.Instance.currentState = GameManager.GameState.Boss;
                StartCoroutine(EWaitCoroutine());
            }

        }

    }
    IEnumerator EWaitCoroutine()
    {


        yield return new WaitForSeconds(1.65f);
        GameManager.onBossScene?.Invoke();

    }

}

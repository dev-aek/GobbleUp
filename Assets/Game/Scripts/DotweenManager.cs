using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotweenManager : MonoBehaviour
{
    [SerializeField]
    private GameObject spoon;
    [SerializeField]
    private GameObject objectToThrow;
    [SerializeField]
    private GameObject objectToThrows;
    [SerializeField]
    private Vector3 spoonBossRotate;
    [SerializeField]
    private Vector3 spoonUpRotate;
    [SerializeField]
    private GameObject endPoint;
    public bool isRotated=false;

    private bool isStarted = false;

    private void Awake()
    {
    }

    private void OnDisable()
    {

    }
    private void Start()
    {
        GameManager.onBossScene += RotateSpoon;
    }

    private void Update()
    {

       /* if (!isStarted)
        {
            StartCoroutine(ESWaitCoroutine());
            isStarted = true;
        }*/


        if (GameManager.Instance.currentState == GameManager.GameState.Boss)
        {

                if (Input.GetMouseButton(0))
                {
                /*spoon.transform.DOMoveY(7.5f, 0.05f).OnComplete(() => {
                    spoon.transform.DORotate(new Vector3(-75f, 180f, 0), 0.7f, RotateMode.Fast);
                });*/
                spoon.transform.DOMoveY(7.5f, 0.5f);
                spoon.transform.DORotate(new Vector3(-75f, 180f, 0), 0.5f, RotateMode.Fast).OnComplete(()=> {
                    objectToThrow.GetComponent<MeshRenderer>().enabled = true;
                    /* objectToThrow.transform.DOJump(endPoint.transform.localPosition, 2f, 1, 1, true).OnComplete(() => { objectToThrows.SetActive(true); });*/
                    objectToThrow.transform.DOMove(endPoint.transform.localPosition, 0.5f, false).OnComplete(() =>
                    {
                        objectToThrows.GetComponentInChildren<MeshRenderer>().enabled = true;
                        StartCoroutine(EWaitCoroutine());
                    });
                });
            }



        }
    }

    private void RotateSpoon()
    {
        spoon.transform.DORotate(spoonBossRotate, 1f, RotateMode.FastBeyond360).OnComplete(() =>
        {
            if (Input.GetMouseButton(0))
            {
                spoon.transform.DORotate(new Vector3(-75f, 180f, 0), 0.7f, RotateMode.Fast);

            }
        });
    }

    private void RotateUp()
    {
        spoon.transform.DORotate(spoonUpRotate, 0.2f, RotateMode.Fast);
        //spoon.transform.DORotate(spoonDownRotate, 0.2f, RotateMode.Fast);

    }

    IEnumerator EWaitCoroutine()
    {


        yield return new WaitForSeconds(1.65f);
        GameManager.onWinEvent?.Invoke();

    }



}

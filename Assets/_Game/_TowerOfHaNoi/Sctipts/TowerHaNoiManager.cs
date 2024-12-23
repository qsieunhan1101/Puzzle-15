using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class TowerHaNoiManager : MonoBehaviour
{

    [SerializeField] private int ringNummbers;
    [SerializeField] private PillarHaNoi[] pillarHaNois = new PillarHaNoi[3];
    [SerializeField] private PillarHaNoi pillarHaNoiPrefab;
    [SerializeField] private RingItem ringItemPrefab;
    [SerializeField] private Color[] ringColors;
    [SerializeField] private RingItem ringItemSelected;
    [SerializeField] private bool isPickUp = false;
    [SerializeField] private bool isDoMove = false;

    [SerializeField] private Transform stage;


    private float deltaScale = 0.5f;
    private Stack<RingItem>[] ringStacks = new Stack<RingItem>[3];

    public bool IsPickUp => isPickUp;
    private void Start()
    {
        isPickUp = false;
        for (int i = 0; i < ringStacks.Length; i++)
        {
            ringStacks[i] = new Stack<RingItem>();
        }
        SetUpPillar();
        SpawnRing();
    }

    private void SetUpPillar()
    {
        pillarHaNois = new PillarHaNoi[3];
        for (int i = 0; i < pillarHaNois.Length; i++)
        {
            PillarHaNoi pillar = Instantiate(pillarHaNoiPrefab, transform);
            pillarHaNois[i] = pillar;
            pillarHaNois[i].Init(i, OnClickPillar, ringNummbers);
        }

        float ring_0 = 2 + (ringNummbers - 1) * deltaScale;
        float ring_1 = 2 + (ringNummbers - 2) * deltaScale;
        float posX = ring_0 / 2 + ring_1 / 2 + 0.5f;
        pillarHaNois[1].transform.localPosition = new Vector3(0, stage.localPosition.y, 0);
        pillarHaNois[2].transform.localPosition = new Vector3(posX, stage.localPosition.y, 0);
        pillarHaNois[0].transform.localPosition = new Vector3(-posX, stage.localPosition.y, 0);
    }
    private void SpawnRing()
    {
        int scaleCount = ringNummbers - 1;
        for (int i = 0; i < ringNummbers; i++)
        {
            RingItem ringItem = Instantiate(ringItemPrefab, transform);
            ringItem.Init(i, ringColors[i], scaleCount * deltaScale);
            Transform pillar_1 = pillarHaNois[0].transform;
            ringItem.transform.DOLocalMove(new Vector3(pillar_1.localPosition.x, pillar_1.localPosition.y + i, pillar_1.localPosition.z), 0f);
            scaleCount--;

            ringStacks[0].Push(ringItem);
        }
    }


    private void OnClickPillar(int index)
    {
        if (isDoMove == false)
        {
            if (isPickUp == false)
            {
                PickUpRing(index);
            }
            else
            {
                DropDownRing(index);
            }
        }
    }

    private void PickUpRing(int index)
    {
        if (ringStacks[index].Count == 0)
        {
            return;
        }
        isPickUp = true;
        isDoMove = true;
        ringItemSelected = ringStacks[index].Pop();
        ringItemSelected.lastIndexStack = index;
        ringItemSelected.transform.DOLocalMoveY(4, 0.5f).OnComplete(() =>
        {
            isDoMove = false;
        });
    }

    private void DropDownRing(int index)
    {
        if (ringStacks[index].Count != 0)
        {
            if (ringItemSelected.index < ringStacks[index].Peek().index)
            {
                return;
            }
        }
        ringStacks[index].Push(ringItemSelected);
        Sequence dropSequence = DOTween.Sequence();

        isDoMove = true;

        if (ringItemSelected.lastIndexStack != index)
        {
            dropSequence.Append(ringItemSelected.transform.DOLocalMoveX(pillarHaNois[index].transform.localPosition.x, 0.5f));

        }
        int count = ringStacks[index].Count - 1;
        dropSequence.Append(ringItemSelected.transform.DOLocalMoveY(pillarHaNois[index].transform.localPosition.y + 1f * count, 0.5f));

        dropSequence.OnComplete(() =>
        {
            ringItemSelected.lastIndexStack = -1;
            ringItemSelected = null;
            isPickUp = false;
            isDoMove = false;
            CheckWin();
        });
    }

    private bool CheckWin()
    {
        if (ringStacks[2].Count == ringNummbers)
        {
            Debug.Log("Win Game -----------------");
        }
        return false;
    }
}

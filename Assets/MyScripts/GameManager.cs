using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum CURRENT_KEY
    {
        None = 0,
        F1 = 1,
        F2 = 2,
        F3 = 3
    }
    private CURRENT_KEY currentkey = CURRENT_KEY.None;
    public GameObject[] Obj;
    public TextMeshPro[] TitleText;
    // Start is called before the first frame update
    void Start()
    {
        Obj[0].SetActive(false);
        Obj[1].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) && currentkey != CURRENT_KEY.F1 && currentkey != CURRENT_KEY.F2)
        {
            Obj[0].SetActive(true);
            Obj[1].SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.F2) && currentkey != CURRENT_KEY.F2 && currentkey != CURRENT_KEY.F1)
        {
            Obj[0].SetActive(false);
            Obj[1].SetActive(true);
        }
        //F3:: 게임 끝내기
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            Obj[0].SetActive(false);
            Obj[1].SetActive(false);
        }
    }
}

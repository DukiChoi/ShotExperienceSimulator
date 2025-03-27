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
    public GameObject Controller;

    void Start()
    {
        Obj[0].SetActive(false);
        Obj[1].SetActive(false);
        TitleText[0].text = "";
        TitleText[1].text = "";

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) && currentkey != CURRENT_KEY.F1 && currentkey != CURRENT_KEY.F2)
        {
            Obj[0].SetActive(true);
            Obj[1].SetActive(false);
            TitleText[0].text = "1. GunShot";
            TitleText[0].color = new Color32(0xD4, 0xFF, 0x00, 255);
            TitleText[1].text = "";
            StartCoroutine(DisableStimuliAfterSeconds(0));
        }
        else if (Input.GetKeyDown(KeyCode.F2) && currentkey != CURRENT_KEY.F2 && currentkey != CURRENT_KEY.F1)
        {
            Obj[0].SetActive(false);
            Obj[1].SetActive(true);
            TitleText[0].text = "";
            TitleText[1].text = "2. Gerenade";
            TitleText[1].color = new Color32(0xD4, 0xFF, 0x00, 255);
            StartCoroutine(DisableStimuliAfterSeconds(0));
        }
        //F3:: 게임 끝내기
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            Obj[0].SetActive(false);
            Obj[1].SetActive(false);
            TitleText[0].text = "";
            TitleText[1].text = "";
            StartCoroutine(DisableStimuliAfterSeconds(0));

        }
    }
    IEnumerator DisableStimuliAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Controller.SendMessage("SendSerialMessage", "FF00000000000000000000000000000000");
        Debug.Log("FF00000000000000000000000000000000");
        yield return new WaitForSeconds(0.1f);
        Controller.SendMessage("SendSerialMessage", "F000000000000000000000000000000000");
        Debug.Log("F000000000000000000000000000000000");
    }
}

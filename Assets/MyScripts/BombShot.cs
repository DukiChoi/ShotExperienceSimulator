using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BombShot : MonoBehaviour
{

    public GameObject Controller;
    public TextMeshPro targetText;
    public ParticleSystem[] explosion_effect;
    public GameObject Obj_to_shake; 
    [Header("Bomb setting")]
    public float impactForce = 100;

    [Header("Audio setting")]
    public float ClipLength1 = 2.4f;
    public float ClipLength2 = 3.4f;
    public GameObject[] AudioClip;
    string lastTarget = "";

    [Header("Stimulation Setting")]
    public float StimulationLength = 8f;
    private enum CURRENT_KEY
    {
        None = 0,
        Jump = 1
    }
    private CURRENT_KEY currentkey = CURRENT_KEY.None;
    private Coroutine currentShakeCoroutine;

    void Start()
    {
        AudioClip[0].SetActive(false);
        AudioClip[1].SetActive(false);
        foreach(var item in explosion_effect)
        {
            item.Pause();
        }
    }
    private void OnEnable()
    {
        // 소리랑 불빛 꺼놓기
        AudioClip[0].SetActive(false);
        AudioClip[1].SetActive(false);
        foreach (var item in explosion_effect)
        {
            item.Pause();
        }
    }
    private void OnDisable()
    {
        // 소리랑 불빛 꺼놓기
        AudioClip[0].SetActive(false);
        AudioClip[1].SetActive(false);
        currentShakeCoroutine = null;
        foreach (var item in explosion_effect)
        {
            item.Pause();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && currentkey != CURRENT_KEY.Jump)
        {
            targetText.text = "Fire in the hole!";
            Explode();
            currentkey = CURRENT_KEY.Jump;
        }
        else
        {
            currentkey = CURRENT_KEY.None;
        }
    }
    void Explode()
    {
        Vector3 startPos = transform.position;
        StartCoroutine(Obj_to_shake.GetComponent<CameraShaker>().Shake(0.1f, 0.2f));
        StartCoroutine(ExplosionRoutine(1f));
        //소리켜기.
        AudioClip[0].SetActive(true);
        StartCoroutine(DisableAudioAfterSeconds(AudioClip[0], ClipLength1));
        StartCoroutine(SecondAudio(ClipLength1));

        string cmd_explosion_F = "";
        string cmd_explosion_V = "";
        for (int i = 0; i < 16; i++)
        {
            cmd_explosion_F += "46";  // 모든 자리에 "44"
            cmd_explosion_V += "F4";  // 모든 자리에 "EF"
        }



        StartCoroutine(EnableStimuli(cmd_explosion_F, cmd_explosion_V));
        Debug.Log("FF" + (string)cmd_explosion_F);
        Debug.Log("F0" + (string)cmd_explosion_V);
        // 이전 코루틴 중지
        if (currentShakeCoroutine != null)
            StopCoroutine(currentShakeCoroutine);
        // 새 코루틴 실행
        currentShakeCoroutine = StartCoroutine(DisableStimuliAfterSeconds(StimulationLength));
    }

    IEnumerator ExplosionRoutine(float seconds)
    {
        
        explosion_effect[0].Play();
        yield return new WaitForSeconds(seconds / 8);
        explosion_effect[0].Play();
        explosion_effect[1].Play();
        yield return new WaitForSeconds(seconds / 8);
        explosion_effect[1].Play();
        yield return new WaitForSeconds(seconds / 8);
        explosion_effect[1].Play();
        explosion_effect[2].Play();
    }
    IEnumerator SecondAudio(float seconds)
    {
        yield return new WaitForSeconds(seconds/2);
        AudioClip[1].SetActive(true);
        StartCoroutine(DisableAudioAfterSeconds(AudioClip[1], ClipLength2));
    }
    IEnumerator DisableAudioAfterSeconds(GameObject audioclip, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        audioclip.SetActive(false);
    }
    IEnumerator EnableStimuli(string cmd_F, string cmd_V)
    {
        Controller.SendMessage("SendSerialMessage", "FF" + (string)cmd_F);
        Debug.Log("FF" + (string)cmd_F);
        yield return new WaitForSeconds(0.1f);
        Controller.SendMessage("SendSerialMessage", "F0" + (string)cmd_V);
        Debug.Log("F0" + (string)cmd_V);
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


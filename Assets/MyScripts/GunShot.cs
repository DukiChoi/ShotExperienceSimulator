using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GunShot : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    [Tooltip("Reference to an scene object that will receive the events of connection, " +
             "disconnection and the messages from the serial device.")]
    public GameObject Controller;
    public GameObject[] target;
    public TextMeshPro targetText;
    public ParticleSystem muzzleFlash;
    private LineRenderer lineRenderer;
    public GameObject impactEffect;
    [Header("<<Gun setting")]
    public float impactForce = 30;
    public float fireRate = 15f;
    public float nextTimeToFire = 0.1f;
    [Header("Audio setting")]
    public float ClipLength = 0.4f;
    public GameObject AudioClip;
    string lastTarget = "";
    
    int layerMask;
    float ray_length = 10f;
    Color ray_color = Color.yellow;
    private enum CURRENT_KEY
    {
        nothing = -1,
        T1 = 1, T2 = 2,
        T3 = 3, T4 = 4,
        T5 = 5, T6 = 6,
        T7 = 7,T8 = 8, 
        T9 = 9, T10 = 10, 
        T11 = 11, T12 = 12, 
        T13 = 13, T14 = 14, 
        T15 = 15, T16 = 16
    }
    CURRENT_KEY currentkey = CURRENT_KEY.nothing;
    private Dictionary<KeyCode, int> keyTargetMap = new Dictionary<KeyCode, int>()
{
    { KeyCode.Alpha1, 1 },
    { KeyCode.Alpha2, 2 },
    { KeyCode.Alpha3, 3 },
    { KeyCode.Alpha4, 4 },
    { KeyCode.Alpha5, 5 },
    { KeyCode.Alpha6, 6 },
    { KeyCode.Alpha7, 7 },
    { KeyCode.Alpha8, 8 },
    { KeyCode.Alpha9, 9 },
    { KeyCode.Alpha0, 10 },
    { KeyCode.Minus, 11 },
    { KeyCode.Equals, 12 },
    { KeyCode.F5, 13 },
    { KeyCode.F6, 14 },
    { KeyCode.F7, 15 },
    { KeyCode.F8, 16 },
};

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.005f;
        lineRenderer.positionCount = 2;

        // 빨간 선!
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        // 소리 꺼놓기
        AudioClip.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * ray_length, ray_color);
        // 스페이스바 누르면 총쏘기
        if (Input.GetButton("Jump") && Time.time >= nextTimeToFire)
        {
            ray_color = Color.red;
            targetText.text = "Gun fired at Target " + currentkey;
            targetText.color = ray_color;
            nextTimeToFire = Time.time + 1f / fireRate;
            Fire();
        }

        // 타겟 선택 (Alpha1 ~ Alpha7)
        foreach (var pair in keyTargetMap)
        {
            if (Input.GetKeyDown(pair.Key))
            {
                int index = pair.Value - 1;

                // 범위 체크
                if (index >= 0 && index < target.Length)
                {
                    currentkey = (CURRENT_KEY)pair.Value;
                    ray_color = Color.yellow;
                    targetText.text = "Aiming at Target " + currentkey;
                    targetText.color = ray_color;
                    LookAtPosition(target[index].transform.position);
                    RedDotSight();
                }
                break;
            }
        }

        // 항상 바라보기
        if (currentkey != CURRENT_KEY.nothing)
        {
            LookAtPosition(target[(int)currentkey - 1].transform.position);
        }
    }
    void Fire()
    {
        RaycastHit hit;
        layerMask = LayerMask.GetMask("RaycastOnly");
        Vector3 startPos = transform.position;
        if (Physics.Raycast(startPos, transform.forward, out hit, range, layerMask))
        {
            //총 이펙트!!
            muzzleFlash.Play();
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO,0.2f);

            //소리켜기.
            AudioClip.SetActive(true);
            StartCoroutine(DisableAudioAfterSeconds(ClipLength));


            if (hit.rigidbody != null) 
            {
                hit.rigidbody.AddForce(-hit.normal*impactForce);
            }
            Debug.DrawRay(startPos, transform.forward * ray_length, ray_color);
            Debug.Log(hit.transform.name + " is shot");
            string cmd_number = "";
            for (int i = 1; i <= 16; i++)
            {
                if (i == (int)currentkey)
                    cmd_number += "10";  // 해당 자리만 "10"
                else
                    cmd_number += "00";  // 나머지는 "00"
            }
            if (hit.transform.name != lastTarget)
            {
                Controller.SendMessage("SendSerialMessage", "FF" + (string)cmd_number);
                Debug.Log("FF" + cmd_number);
                lastTarget = hit.transform.name;
            }

        }       
    }
    IEnumerator DisableLineAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        lineRenderer.enabled = false;

    }

    IEnumerator DisableAudioAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        AudioClip.SetActive(false);

    }


    void LookAtPosition(Vector3 targetPosition)
    {
        // 현재 오브젝트의 위치에서 목표 위치로 가는 방향 계산
        Vector3 direction = targetPosition - transform.position;
        
        // 방향 벡터가 0이 아니면 회전 수행
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
    void RedDotSight()
    {
        RaycastHit hit;
        layerMask = LayerMask.GetMask("RaycastOnly");
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + transform.forward * range;
        if (Physics.Raycast(startPos, transform.forward, out hit, range, layerMask))
        {
            endPos = hit.point;
            // 실제 눈에 보이는 빨간 선을 그림
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
            StartCoroutine(DisableLineAfterSeconds(2f));  // 2초 뒤에 선 사라짐
        }
    }
}

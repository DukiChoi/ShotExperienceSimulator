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
    public float impactForce = 30;
    public float fireRate = 15f;
    public float nextTimeToFire = 0.1f;

    public float ClipLength = 0.4f;
    public GameObject AudioClip;

    
    int layerMask;
    float ray_length = 10f;
    Color ray_color = Color.yellow;
    private enum CURRENT_KEY
    {
        nothing = -1,
        T1 = 1,
        T2 = 2,
        T3 = 3,
        T4 = 4,
        T5 = 5,
        T6 = 6,
        T7 = 7
    }
    CURRENT_KEY currentkey = CURRENT_KEY.nothing;

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
        //스페이스바 누르면 총쏘기
        if (Input.GetButton("Jump") && Time.time >= nextTimeToFire)
        {
            ray_color = Color.red;
            targetText.text = "Gun fired at Target " + currentkey;
            targetText.color = ray_color;
            nextTimeToFire = Time.time + 1f / fireRate;
            Fire();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentkey = CURRENT_KEY.T1;
            ray_color = Color.yellow;
            targetText.text = "Aiming at Target " + currentkey;
            targetText.color = ray_color;
            LookAtPosition(target[0].transform.position);
            RedDotSight();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentkey = CURRENT_KEY.T2;
            ray_color = Color.yellow;
            targetText.text = "Aiming at Target " + currentkey;
            targetText.color = ray_color;
            LookAtPosition(target[1].transform.position);
            RedDotSight();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentkey = CURRENT_KEY.T3;
            ray_color = Color.yellow;
            targetText.text = "Aiming at Target " + currentkey;
            targetText.color = ray_color;
            LookAtPosition(target[2].transform.position);
            RedDotSight();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentkey = CURRENT_KEY.T4;
            ray_color = Color.yellow;
            targetText.text = "Aiming at Target " + currentkey;
            targetText.color = ray_color;
            LookAtPosition(target[3].transform.position);
            RedDotSight();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentkey = CURRENT_KEY.T5;
            ray_color = Color.yellow;
            targetText.text = "Aiming at Target " + currentkey;
            targetText.color = ray_color;
            LookAtPosition(target[4].transform.position);
            RedDotSight();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentkey = CURRENT_KEY.T6;
            ray_color = Color.yellow;
            targetText.text = "Aiming at Target " + currentkey;
            targetText.color = ray_color;
            LookAtPosition(target[5].transform.position);
            RedDotSight();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            currentkey = CURRENT_KEY.T7;
            ray_color = Color.yellow;
            targetText.text = "Aiming at Target " + currentkey;
            targetText.color = ray_color;
            LookAtPosition(target[6].transform.position);
            RedDotSight();
        }
        else
        {
            if (currentkey != CURRENT_KEY.nothing)
            {
                LookAtPosition(target[(int)currentkey - 1].transform.position);
            }
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
            Controller.SendMessage("SendSerialMessage", "FF"+cmd_number);
            Debug.Log("FF" + cmd_number);

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

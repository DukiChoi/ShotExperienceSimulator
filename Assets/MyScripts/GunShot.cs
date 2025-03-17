using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GunShot : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public GameObject[] targets;
    public TextMeshPro targetText;
    private enum CURRENT_KEY
    {
        n1 = 1,
        n2 = 2,
        n3 = 3,
        n4 = 4,
        n5 = 5
    }
    CURRENT_KEY currentkey = CURRENT_KEY.n1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 3f, Color.yellow);
        //스페이스바 누르면 총쏘기
        if (Input.GetButtonDown("Jump"))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentkey != CURRENT_KEY.n1)
        {
            currentkey = CURRENT_KEY.n1;
            targetText.text = "1";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && currentkey != CURRENT_KEY.n2)
        {
            currentkey = CURRENT_KEY.n2;
            targetText.text = "2";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && currentkey != CURRENT_KEY.n3)
        {
            currentkey = CURRENT_KEY.n3;
            targetText.text = "3";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && currentkey != CURRENT_KEY.n4)
        {
            currentkey = CURRENT_KEY.n4;
            targetText.text = "4";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && currentkey != CURRENT_KEY.n5)
        {
            currentkey = CURRENT_KEY.n5;
            targetText.text = "5";
        }
    }
    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Debug.DrawRay(transform.position, transform.forward * 3f, Color.yellow);
            Debug.Log(hit.transform.name);
        }
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
}

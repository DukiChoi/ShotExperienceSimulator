using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GunShot : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public GameObject[] target;
    public TextMeshPro targetText;
    int layerMask;
    private enum CURRENT_KEY
    {
        n1 = 1,
        n2 = 2,
        n3 = 3,
        n4 = 4,
        n5 = 5,
        n6 = 6,
        n7 = 7
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentkey = CURRENT_KEY.n1;
            targetText.text = "Target 1";
            LookAtPosition(target[0].transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentkey = CURRENT_KEY.n2;
            targetText.text = "Target 2";
            LookAtPosition(target[1].transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentkey = CURRENT_KEY.n3;
            targetText.text = "Target 3";
            LookAtPosition(target[2].transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentkey = CURRENT_KEY.n4;
            targetText.text = "Target 4";
            LookAtPosition(target[3].transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentkey = CURRENT_KEY.n5;
            targetText.text = "Target 5";
            LookAtPosition(target[4].transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentkey = CURRENT_KEY.n6;
            targetText.text = "Target 6";
            LookAtPosition(target[5].transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            currentkey = CURRENT_KEY.n7;
            targetText.text = "Target 7";
            LookAtPosition(target[6].transform.position);
        }
    }
    void Shoot()
    {
        RaycastHit hit;
        layerMask = LayerMask.GetMask("RaycastOnly");
        if (Physics.Raycast(transform.position, transform.forward, out hit, range, layerMask))
        {
            Debug.DrawRay(transform.position, transform.forward * 4f, Color.yellow);
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

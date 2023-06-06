using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region 변수들
    public int id;
    public int prefab_Id;
    public float dmg;
    public int count;
    public float speed;

    float timer;

    Player player;
    #endregion

    private void Awake()
    {
        // weapon오브젝트는 player오브젝트의 하위 오브젝트다
        // GetComponent는 동일한 계층에서만 가져올 수 있음
        // 그러므로 상위 계층(부모)인 player의 컴포넌트를 가져오기 위해서는 GetComponentInParent를 사용해야 함
        player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        switch (id)
        {
            case 0:
                {
                    transform.Rotate(Vector3.back * speed * Time.deltaTime);

                    break;
                }

            default:
                {
                    timer += Time.deltaTime;

                    if (timer > speed)
                    {
                        timer = 0f;
                        Weap2_Knife();
                    }

                    break;
                }
        }
    }

    #region 레벨업 시, 무기 성능 업그레이드
    public void LevelUp(float dmg, int count)
    {
        this.dmg += dmg;
        this.count += count;

        if (id == 0)        // 레벨업 하는 경우, 무기 id에 따라 함수 실행
        {
            Weap1_Sword();
        }
    }
    #endregion

    #region 무기들 초기 설정값
    public void Init()
    {
        switch (id)
        {
            case 0:     // speed : 무기의 회전방향
                {
                    //speed = 150;       // 무기0이 시계방향으로 회전
                    Weap1_Sword();

                    break;
                }

            default:    // speed : 연사 속도
                {
                    //speed = 0.3f;
                    break;
                }
        }
    }
    #endregion

    #region 무기1 : 회전하는 칼
    void Weap1_Sword()
    {
        for (int i = 0; i < count; i++)     // 원하는 개수(count)만큼 playerAttack
        {
            Transform playerAttack;         // 오브젝트의 transform인 playerAttack선언

            if (i < transform.childCount)       // index가 아직 weapon이 갖고 있는 자식오브젝트 개수보다 적다면, GetChild로 현재 갖고 있는 자식오브젝트를 가져옴(정확히는 오브젝트의 transform을 가져옴)
            {
                playerAttack = transform.GetChild(i);
            }
            else       // index가 weapon이 갖고 있는 자식오브젝트 개수보다 많다면, 모자란 만큼 오브젝트 풀에서 가져옴
            {
                playerAttack = GameManager.instance.poolMng.Get(prefab_Id).transform;
                playerAttack.parent = transform;        // 생성된 무기의 부모가 플레이어 내의 weapon이 되도록 설정
            }

            #region 생성된 무기 위치/회전/관통 설정
            playerAttack.localPosition = Vector3.zero;      // 공격오브젝트를 생성 할 때에, Weapon오브젝트의 위치가 플레이어 위치가 되도록 위치 초기화 (그래야 플레이어 위치에서 생성되므로)
            playerAttack.localRotation = Quaternion.identity;       // 위와 마찬가지로 회전도 초기화

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            playerAttack.Rotate(rotVec);
            playerAttack.Translate(playerAttack.up * 1.5f, Space.World);

            playerAttack.GetComponent<PlayerAttack>().Init(dmg, -1, Vector3.zero);        // -1 is Infinity penetrate (-1은 무한관통을 의미한다는 주석), 이 무기에 방향요소는 필요없으므로 Vector3.zero를 넣는다
            #endregion
        }
    }
    #endregion

    #region 무기2 : 던지는 단검
    void Weap2_Knife()
    {
        if (!player.scanner.nearTarget)     // 근처에 적이 없다면 return
        {
            return;
        }
        else         // 가장 가까운 거리의 적을 찾으면 단검을 생성
        {
            #region 단검이 나갈 방향 결정
            Vector3 targetPos = player.scanner.nearTarget.position;     // 타겟 위치는 스캐너에 탐색된 가장 가까운 적의 위치
            Vector3 dir = targetPos - transform.position;               // 타겟과의 거리는 가까운 적 위치 - 단검의 위치
            dir = dir.normalized;
            #endregion

            Transform playerAttack = GameManager.instance.poolMng.Get(prefab_Id).transform;

            #region 단검의 위치와 회전 결정 후, 무기 스크립트에 전달
            playerAttack.position = transform.position;
            playerAttack.rotation = Quaternion.FromToRotation(Vector3.up, dir);         // FromToRotation : 지정된 축을 중심으로 목표를 향해 회전하는 함수 ~ z축 회전을 위해 축을 Vector3.up으로 잡음(0, 1, 0)
            playerAttack.GetComponent<PlayerAttack>().Init(dmg, count, dir);
            #endregion
        }
    }
    #endregion
}

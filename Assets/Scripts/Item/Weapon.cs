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

    public float temp;
    public float spd_init;

    float timer;

    Player player;
    #endregion

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    private void Update()       // 무기 수치 지속적인 업데이트 (레벨업 시 고려)
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

    #region 무기들 초기 설정값
    public void Init(ItemData data)
    {
        #region Basic Setting
        //====================================================================================
        name = "Weapon " + data.itemName;
        transform.parent = player.transform;        // 생성된 무기의 부모는 player가 되도록 설정
        transform.localPosition = Vector3.zero;     // 생성된 무기 위치를 player안에서 (0, 0, 0)으로 맞추기 ~ 그래야 플레이어 위치에 스킬이 소환되므로
        //====================================================================================
        #endregion

        #region Property Setting
        //====================================================================================
        id = data.itemID;
        dmg = data.baseDmg;
        count = data.baseCount;
        speed = data.baseSpeed;
        spd_init = speed;

        for (int i = 0; i < GameManager.instance.poolMng.prefs.Length; i++)
        {
            if (data.projectile == GameManager.instance.poolMng.prefs[i])
            {
                prefab_Id = i;
                break;
            }
        }
        //====================================================================================
        #endregion

        switch (id)
        {
            case 0:
                {
                    Weap1_Sword();

                    break;
                }
            case 1:
                {
                    break;
                }
            case 2:
                {
                    break;
                }

            default: { break; }
        }

        player.BroadcastMessage("Apply_PlayerPassive", SendMessageOptions.DontRequireReceiver);         // 갖고 있는 모든 자식 오브젝트에게 "Apply_PlayerPassive" 함수를 실행하라고 알림
    }
    #endregion

    #region 레벨업 시, 무기 성능 업그레이드
    public void LevelUp(float dmg, int count)
    {
        temp = speed;

        this.dmg += dmg;
        this.count += count;

        if (id == 0)        // 레벨업 하는 경우, 무기 id에 따라 함수 실행
        {
            Weap1_Sword();      // 회전체 같은 경우에는 레벨업 마다 추가된 회전체에 따른 각도와 위치를 설정해줘야 해서 초기화가 필요
        }

        player.BroadcastMessage("Apply_PlayerPassive", SendMessageOptions.DontRequireReceiver);
        
        speed = temp;
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
            playerAttack.Translate(playerAttack.up * 1.8f, Space.World);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
        {
            return;
        }
        // 이 스크립트를 가진 오브젝트가 만약에 Area태그를 가진 오브젝트와 충돌하지 않는다면 return(반환), 즉 함수가 위에 return에서 끊기도록 해놓음.
        // else {} 안에 아래 내용이 들어가는 것인데, 굳이 else를 쓰지 않고 생략하고 아래처럼 바로 써도 됨. 어차피 위에서 말했듯이 아닌 경우에는 위에서 return으로 인하여 함수 작동이 중지되므로.

        Vector3 playerPos = GameManager.instance.player.transform.position;     // player의 위치
        Vector3 myPos = transform.position;     // 현재 내 위치

        float diffX = Mathf.Abs(playerPos.x - myPos.x);     // Mathf.Abs 를 사용하여 player와 만든 Tile의 x축 차이값의 절대값을 구함
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = GameManager.instance.player.inputVec;       // player가 향하는 방향
        float dirX = playerDir.x < 0 ? -1 : 1;      // playerDir.x가 0보다 작은 경우 -1을, 0보다 큰 경우 1을 넣음 (삼항연산자 사용)
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)      // 플레이어가 이 스크립트를 가진 오브젝트의 콜라이더에서 "벗어나는 경우", 트리거 발동
        {
            // 문제점 : 플레이어가 아무런 움직임을 취하지 않은 상태로 몬스터에게 밀려서 이동한 경우, dirX & dirY 값이 0이어서 아래 코드가 꼬여 맵이 이상하게 이동함
            case "Ground":      // 플레이어가 Ground 태그를 가진 콜라이더에서 벗어난 경우
                {
                    if (diffX > diffY)      // 만약 diffX가 diffY보다 크다면 타일을 이동.
                    {
                        transform.Translate(Vector3.right * dirX * 40);     // Tile 한개가 20칸, 총 4개가 깔려있으므로 이동 시에는 한번에 40칸이 움직여야 옆의 타일과 겹치지 않음.
                    }
                    else if (diffX < diffY)
                    {
                        transform.Translate(Vector3.up * dirY * 40);
                    }

                    break;
                }

            case "Enemy":      // 플레이어가 Enemy 태그를 가진 콜라이더에서 벗어난 경우
                {
                    if (col.enabled)
                    {
                        transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));        // 몬스터가 화면 밖으로 벗어나면 플레이어 앞 화면 랜덤한 위치로 소환되도록 설정
                    }

                    break;
                }
        }
    }
}

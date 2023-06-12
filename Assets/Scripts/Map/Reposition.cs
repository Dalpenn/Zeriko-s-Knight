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

        else
        {
            // 굳이 else를 쓰지 않고 생략하고 써도 됨. 어차피 위에서 말했듯이 아닌 경우에는 위에서 return으로 인하여 함수 작동이 중지되므로.
            Vector3 playerPos = GameManager.instance.player.transform.position;     // player의 위치
            Vector3 myPos = transform.position;     // 현재 내 위치

            #region 플레이어가 인식하는 공간 밖으로 나가는 충돌체들에 대한 코드
            switch (transform.tag)      // 플레이어가 이 스크립트를 가진 오브젝트의 콜라이더에서 "벗어나는 경우", 트리거 발동
            {
                // 문제점 : 플레이어가 아무런 움직임을 취하지 않은 상태로 몬스터에게 밀려서 이동한 경우, dirX & dirY 값이 0이어서 아래 코드가 꼬여 맵이 이상하게 이동함
                case "Ground":      // 플레이어가 Ground 태그를 가진 콜라이더에서 벗어난 경우
                    {
                        #region 타일의 이동 방향을 정하는 코드
                        // (플레이어 위치 - 타일 위치)를 이용하여 -값이 나오면 -1을, +값이 나오면 +1을 변수에 넣어서 타일이 플레이어의 왼/오른/위/아래인지 방향을 구분하는 코드
                        // -1이 나오는 경우 : 타일이 플레이어보다 오른쪽에 있으므로(타일 위치값이 플레이어보다 더 크다) 타일이 향할 방향은 왼쪽이다(-1)
                        float diffX = playerPos.x - myPos.x;
                        float diffY = playerPos.y - myPos.y;

                        // diffX,Y값을 이용하여 타일 배치 방향을 먼저 구함
                        float dirX = diffX < 0 ? -1 : 1;      // diffX가 0보다 작은 경우 -1을, 0보다 큰 경우 1을 넣음 (삼항연산자 사용)
                        float dirY = diffY < 0 ? -1 : 1;

                        // 타일 배치 방향을 구했으면, 플레이어와 일정 거리 이상 떨어지면 타일을 이동할 수 있도록 거리를 구하기 위해 diff값을 다시 절대값으로 바꿈
                        diffX = Mathf.Abs(diffX);
                        diffY = Mathf.Abs(diffY);
                        #endregion

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
                            Vector3 distance = playerPos - myPos;
                            Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                            
                            // 몬스터가 화면 밖으로 벗어나면 플레이어 앞 화면 랜덤한 위치로 소환되도록 설정
                            // distance만큼만 움직이면 몬스터가 플레이어 위치로 그 차이만큼 움직임 ~ x2를 해주면 그 차이만큼 플레이어의 앞으로 움직임
                            transform.Translate(ran + distance * 2);
                        }

                        break;
                    }
            }
            #endregion
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    #region 변수들
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearTarget;
    #endregion

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);     // 원형의 범위와 충돌하는 모든 LayerMask에 대한 결과를 반환하는 함수
        // Physics2D.CircleCastAll(중심위치, 원 범위 ~ 반지름, 광선 쏘는 방향 ~ 원이므로 없음, 쏘는 광선 길이 ~ 원이므로 없음, 충돌을 감지할 대상 레이어)

        nearTarget = GetNearest();      // nearTarget으로 선언한 변수 값에 GetNearest함수에 의해 가장 가까운 타겟의 위치정보가 담기게 된다
    }

    Transform GetNearest()
    {
        Transform result = null;

        float diff = 100;

        foreach (RaycastHit2D target in targets)        // 원형범위와 충돌한 오브젝트들(RaycastHit2D target)을 순차적으로 모두 가져온다
        {
            Vector3 myPos = transform.position;         // 플레이어 위치
            Vector3 targetPos = target.transform.position;      // 충돌한 오브젝트의 위치

            float curDiff = Vector3.Distance(myPos, targetPos);         // 플레이어 위치와 충돌한 오브젝트 위치의 거리 차이를 구함

            if (curDiff < diff)         // 만약 거리차이가 diff보다 작으면, 그 거리차이값을 diff문에 넣고 result에 그 타겟의 위치정보를 넣음 ~ 이런식으로 모든 타겟을 비교하면, 마지막에 거리가 가장 가까운 타겟이 남게된다.
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}

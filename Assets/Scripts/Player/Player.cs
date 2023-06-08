using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region ������
    public Vector2 inputVec;
    public float player_spd = 3;
    public float player_initSpd;

    public Scanner scanner;

    Rigidbody2D rigid;
    SpriteRenderer sp;
    Animator anim;
    #endregion
    

    void Awake()
    {
        #region ������ ������ �ʱ�ȭ(�ݵ�� �ʿ�)
        scanner = GetComponent<Scanner>();
        rigid = GetComponent<Rigidbody2D>();        // Unity�� Player�� ���� �ִ� Rigidbody2D�Ӽ��� ��ũ��Ʈ�� ������.
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        #endregion

        player_initSpd = player_spd;
    }


    // InputSystem�� ������� �ʴ� ���, �÷��̾� �̵� �ڵ�
    //void Update()
    //{
    //    inputVec.x = Input.GetAxisRaw("Horizontal");
    //    inputVec.y = Input.GetAxisRaw("Vertical");
    //}


    void FixedUpdate()
    {
        Vector2 nextVec = inputVec * player_spd * Time.fixedDeltaTime;      
        // normalize�� ��� �������ε� 1��ŭ �̵���Ŵ(Ư�� �밢�� �̵��� ���) ~ ������ InputSystem�� normalize����� ����ϰ� �־ �ڵ忡 ���� �߰��� �ʿ� ����.
        // DeltaTime�� Update, FixedDeltaTime�� FixedUpdate�� ������ �ϳ��� �Һ��� �ð�

        rigid.MovePosition(rigid.position + nextVec);
    }


    void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);     // magnitude�� �����ϰ� "ũ��"�� ���� �ִ� �����̴�. inputVec�� ũ�Ⱑ 0���� �ƴ����� Ȯ���ϱ� ���� �뵵.

        if (inputVec.x != 0)
        {
            sp.flipX = inputVec.x > 0;      // inputVec.x�� 0���� ������ true, ũ�� false�� ����
        }
    }

    #region InputSystem �Լ�
    // Unity�� Input System ����� ���� �Լ�
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    #endregion
}

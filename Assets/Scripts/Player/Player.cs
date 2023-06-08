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

    private void Update()
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }
    }


    void FixedUpdate()
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }

        Vector2 nextVec = inputVec * player_spd * Time.fixedDeltaTime;      
        // normalize�� ��� �������ε� 1��ŭ �̵���Ŵ(Ư�� �밢�� �̵��� ���) ~ ������ InputSystem�� normalize����� ����ϰ� �־ �ڵ忡 ���� �߰��� �ʿ� ����.
        // DeltaTime�� Update, FixedDeltaTime�� FixedUpdate�� ������ �ϳ��� �Һ��� �ð�

        rigid.MovePosition(rigid.position + nextVec);
    }


    void LateUpdate()
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }

        anim.SetFloat("Speed", inputVec.magnitude);     // magnitude�� �����ϰ� "ũ��"�� ���� �ִ� �����̴�. inputVec�� ũ�Ⱑ 0���� �ƴ����� Ȯ���ϱ� ���� �뵵.

        if (inputVec.x != 0)
        {
            sp.flipX = inputVec.x > 0;      // inputVec.x�� 0���� ������ true, ũ�� false�� ����
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }
        else if (collision.transform.CompareTag("Enemy"))
        {
            float dmg = collision.transform.GetComponent<Enemy>().dmg;
            
            GameManager.instance.curHp -= Time.deltaTime * dmg;
            //GameManager.instance.curHp -= Time.deltaTime * 10;      // �÷��̾ �޴� ������ 10

            if (GameManager.instance.curHp < 0)     // �÷��̾� ��� �� �ڵ�
            {
                for (int i = 2; i < transform.childCount; i++)      // player�ڽ� ��, Shadow�� Area ���� �͵��� �� ��Ȱ��ȭ �ϱ� ���Ͽ� 0, 1���� 2���� index ����
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }

                anim.SetTrigger("Death");
            }
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

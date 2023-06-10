using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region ������
    public Vector2 inputVec;
    public float player_spd = 3;
    public float player_initSpd;
    bool isDamaged = false;

    public Scanner scanner;
    public RuntimeAnimatorController[] animCtrl;

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
    }

    private void OnEnable()
    {
        #region �÷��̾� ������ ���� �ʱ� �̵��ӵ� ����
        player_spd *= Character.movSpd;         // ����Ư���� ���� �̵��ӵ� ���� (������ 1, ������ �� ��ġ�� ��)
        #endregion

        player_initSpd = player_spd;

        anim.runtimeAnimatorController = animCtrl[GameManager.instance.playerID];       // �������� runtimeAnimCtrl�� �÷��̾� ���̵� �´� �ִ���Ʈ�ѷ��� �־���
    }

    #region InputSystem ������ ��� �÷��̾� �̵� �ڵ�
    //void Update()
    //{
    //    inputVec.x = Input.GetAxisRaw("Horizontal");
    //    inputVec.y = Input.GetAxisRaw("Vertical");
    //}
    #endregion

    private void Update()
    {
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }
    }


    void FixedUpdate()
    {
        #region �÷��̾� �̵� Ű Ȱ��ȭ ����
        if (!GameManager.instance.isGameStarted)
        {
            return;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt") || GameManager.instance.curHp < 1)
        {
            return;
        }
        #endregion

        #region �÷��̾� �̵� �ڵ�
        Vector2 nextVec = inputVec * player_spd * Time.fixedDeltaTime;      
        // normalize�� ��� �������ε� 1��ŭ �̵���Ŵ(Ư�� �밢�� �̵��� ���) ~ ������ InputSystem�� normalize����� ����ϰ� �־ �ڵ忡 ���� �߰��� �ʿ� ����.
        // DeltaTime�� Update, FixedDeltaTime�� FixedUpdate�� ������ �ϳ��� �Һ��� �ð�

        rigid.MovePosition(rigid.position + nextVec);
        #endregion
    }


    void LateUpdate()
    {
        #region �÷��̾� �̵� �ִϸ��̼� �� �¿���� Ȱ��ȭ ����
        if (!GameManager.instance.isGameStarted || anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        {
            return;
        }

        anim.SetFloat("Speed", inputVec.magnitude);     // magnitude�� �����ϰ� "ũ��"�� ���� �ִ� �����̴�. inputVec�� ũ�Ⱑ 0���� �ƴ����� Ȯ���ϱ� ���� �뵵.

        if (inputVec.x != 0)
        {
            sp.flipX = inputVec.x > 0;      // inputVec.x�� 0���� ������ true, ũ�� false�� ����
        }
        #endregion
    }

    #region �÷��̾� �ǰ� ���� �ڵ�
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isGameStarted || collision.transform.CompareTag("PlayerAttack") || isDamaged)
        {
            return;
        }
        else
        {
            StartCoroutine(OnDamaged());      // �÷��̾ ���Ϳ� �浹 �� �Լ�
        }
    }

    #region �÷��̾� �ǰ� �� ������ �� ����Ʈ �Լ�
    IEnumerator OnDamaged()
    {
        // ü�� ����
        GameManager.instance.curHp--;
        //GameManager.instance.hpCtrl.HPBarSync();

        if (GameManager.instance.curHp < 1)     // �÷��̾� ��� �� �ڵ�
        {
            for (int i = 2; i < transform.childCount; i++)      // player�ڽ� ��, Shadow�� Area ���� �͵��� �� ��Ȱ��ȭ �ϱ� ���Ͽ� 0, 1���� 2���� index ����
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            anim.SetTrigger("Death");

            GameManager.instance.GameOver();
        }
        else
        {

            // ��� ����
            isDamaged = true;

            anim.SetTrigger("Hurt");

            // �ǰ� ����Ʈ
            for (float i = 0; i < 0.5f; i += 0.1f)
            {
                sp.color = new Color(0, 0, 0, 0.3f);
                yield return new WaitForSeconds(0.1f);
                sp.color = new Color(1, 1, 1, 1);
                yield return new WaitForSeconds(0.1f);
            }

            isDamaged = false;
        }
    }
    #endregion
    #endregion

    #region InputSystem �Լ�
    // Unity�� Input System ����� ���� �Լ�
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    #endregion
}

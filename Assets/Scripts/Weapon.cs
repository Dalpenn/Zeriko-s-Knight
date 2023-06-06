using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region ������
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
        // weapon������Ʈ�� player������Ʈ�� ���� ������Ʈ��
        // GetComponent�� ������ ���������� ������ �� ����
        // �׷��Ƿ� ���� ����(�θ�)�� player�� ������Ʈ�� �������� ���ؼ��� GetComponentInParent�� ����ؾ� ��
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

    #region ������ ��, ���� ���� ���׷��̵�
    public void LevelUp(float dmg, int count)
    {
        this.dmg += dmg;
        this.count += count;

        if (id == 0)        // ������ �ϴ� ���, ���� id�� ���� �Լ� ����
        {
            Weap1_Sword();
        }
    }
    #endregion

    #region ����� �ʱ� ������
    public void Init()
    {
        switch (id)
        {
            case 0:     // speed : ������ ȸ������
                {
                    //speed = 150;       // ����0�� �ð�������� ȸ��
                    Weap1_Sword();

                    break;
                }

            default:    // speed : ���� �ӵ�
                {
                    //speed = 0.3f;
                    break;
                }
        }
    }
    #endregion

    #region ����1 : ȸ���ϴ� Į
    void Weap1_Sword()
    {
        for (int i = 0; i < count; i++)     // ���ϴ� ����(count)��ŭ playerAttack
        {
            Transform playerAttack;         // ������Ʈ�� transform�� playerAttack����

            if (i < transform.childCount)       // index�� ���� weapon�� ���� �ִ� �ڽĿ�����Ʈ �������� ���ٸ�, GetChild�� ���� ���� �ִ� �ڽĿ�����Ʈ�� ������(��Ȯ���� ������Ʈ�� transform�� ������)
            {
                playerAttack = transform.GetChild(i);
            }
            else       // index�� weapon�� ���� �ִ� �ڽĿ�����Ʈ �������� ���ٸ�, ���ڶ� ��ŭ ������Ʈ Ǯ���� ������
            {
                playerAttack = GameManager.instance.poolMng.Get(prefab_Id).transform;
                playerAttack.parent = transform;        // ������ ������ �θ� �÷��̾� ���� weapon�� �ǵ��� ����
            }

            #region ������ ���� ��ġ/ȸ��/���� ����
            playerAttack.localPosition = Vector3.zero;      // ���ݿ�����Ʈ�� ���� �� ����, Weapon������Ʈ�� ��ġ�� �÷��̾� ��ġ�� �ǵ��� ��ġ �ʱ�ȭ (�׷��� �÷��̾� ��ġ���� �����ǹǷ�)
            playerAttack.localRotation = Quaternion.identity;       // ���� ���������� ȸ���� �ʱ�ȭ

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            playerAttack.Rotate(rotVec);
            playerAttack.Translate(playerAttack.up * 1.5f, Space.World);

            playerAttack.GetComponent<PlayerAttack>().Init(dmg, -1, Vector3.zero);        // -1 is Infinity penetrate (-1�� ���Ѱ����� �ǹ��Ѵٴ� �ּ�), �� ���⿡ �����Ҵ� �ʿ�����Ƿ� Vector3.zero�� �ִ´�
            #endregion
        }
    }
    #endregion

    #region ����2 : ������ �ܰ�
    void Weap2_Knife()
    {
        if (!player.scanner.nearTarget)     // ��ó�� ���� ���ٸ� return
        {
            return;
        }
        else         // ���� ����� �Ÿ��� ���� ã���� �ܰ��� ����
        {
            #region �ܰ��� ���� ���� ����
            Vector3 targetPos = player.scanner.nearTarget.position;     // Ÿ�� ��ġ�� ��ĳ�ʿ� Ž���� ���� ����� ���� ��ġ
            Vector3 dir = targetPos - transform.position;               // Ÿ�ٰ��� �Ÿ��� ����� �� ��ġ - �ܰ��� ��ġ
            dir = dir.normalized;
            #endregion

            Transform playerAttack = GameManager.instance.poolMng.Get(prefab_Id).transform;

            #region �ܰ��� ��ġ�� ȸ�� ���� ��, ���� ��ũ��Ʈ�� ����
            playerAttack.position = transform.position;
            playerAttack.rotation = Quaternion.FromToRotation(Vector3.up, dir);         // FromToRotation : ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ� ~ z�� ȸ���� ���� ���� Vector3.up���� ����(0, 1, 0)
            playerAttack.GetComponent<PlayerAttack>().Init(dmg, count, dir);
            #endregion
        }
    }
    #endregion
}

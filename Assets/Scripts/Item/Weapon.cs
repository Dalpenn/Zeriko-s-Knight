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

    public float temp;
    public float spd_init;

    float timer;

    Player player;
    #endregion

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    private void Update()       // ���� ��ġ �������� ������Ʈ (������ �� ���)
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

    #region ����� �ʱ� ������
    public void Init(ItemData data)
    {
        #region Basic Setting
        //====================================================================================
        name = "Weapon " + data.itemName;
        transform.parent = player.transform;        // ������ ������ �θ�� player�� �ǵ��� ����
        transform.localPosition = Vector3.zero;     // ������ ���� ��ġ�� player�ȿ��� (0, 0, 0)���� ���߱� ~ �׷��� �÷��̾� ��ġ�� ��ų�� ��ȯ�ǹǷ�
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

        player.BroadcastMessage("Apply_PlayerPassive", SendMessageOptions.DontRequireReceiver);         // ���� �ִ� ��� �ڽ� ������Ʈ���� "Apply_PlayerPassive" �Լ��� �����϶�� �˸�
    }
    #endregion

    #region ������ ��, ���� ���� ���׷��̵�
    public void LevelUp(float dmg, int count)
    {
        temp = speed;

        this.dmg += dmg;
        this.count += count;

        if (id == 0)        // ������ �ϴ� ���, ���� id�� ���� �Լ� ����
        {
            Weap1_Sword();      // ȸ��ü ���� ��쿡�� ������ ���� �߰��� ȸ��ü�� ���� ������ ��ġ�� ��������� �ؼ� �ʱ�ȭ�� �ʿ�
        }

        player.BroadcastMessage("Apply_PlayerPassive", SendMessageOptions.DontRequireReceiver);
        
        speed = temp;
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
            playerAttack.Translate(playerAttack.up * 1.8f, Space.World);

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

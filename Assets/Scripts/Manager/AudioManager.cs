using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region ������
    [Header("# Singleton")]
    public static AudioManager instance;

    [Header("# BGM")]
    public AudioClip clip_BGM;
    AudioSource player_BGM;
    public float volume_BGM;
    AudioHighPassFilter effect_BGM;

    [Header("# SFX(ȿ����)")]
    public AudioClip[] clips_SFX;      // ȿ������ ������ �����Ƿ� �迭�� ����
    AudioSource[] players_SFX;         // ȿ������ ������ �����Ƿ� �迭�� ����
    public float volume_SFX;

    public int channels;               // �ѹ��� ��� ȿ������ �鸮���� �Ұ����� ����
    int channelindex;

    public enum SFX { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win };      // ���� ���ڸ� �Է��Ͽ� ��ȣ�� �������ټ��� �ִ�
    #endregion

    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        #region �����
        //--------------------------------------------------------------------------------------------
        #region �÷��̾� ����
        GameObject object_BGM = new GameObject("BGMPlayer");
        object_BGM.transform.parent = transform;                // AudioManager�� �ڽ� ������Ʈ�� ���尡 �����ǵ��� ����

        player_BGM = object_BGM.AddComponent<AudioSource>();    // ���� ���� Object�� AudioSource�� �����ϰ�, player_BGM������ �̸� ����
        #endregion

        #region ���� ����
        player_BGM.playOnAwake = false;         // ĳ���� �� ��, ���� ���۵ɶ� ���;� �ϹǷ� false
        player_BGM.loop = true;                 // ��������� �ݺ��ؼ� �����Ƿ�
        player_BGM.volume = volume_BGM;
        player_BGM.clip = clip_BGM;
        #endregion

        effect_BGM = Camera.main.GetComponent<AudioHighPassFilter>();
        //--------------------------------------------------------------------------------------------
        #endregion

        // ȿ���� �÷��̾� �ʱ�ȭ
        #region ȿ����
        //--------------------------------------------------------------------------------------------
        #region �÷��̾� ����
        GameObject object_SFX = new GameObject("SFXPlayer");
        object_SFX.transform.parent = transform;

        players_SFX = new AudioSource[channels];
        for (int i = 0; i < channels; i++)
        {
            players_SFX[i] = object_SFX.AddComponent<AudioSource>();

            #region ���� ����
            players_SFX[i].playOnAwake = false;
            players_SFX[i].volume = volume_SFX;
            // ȿ������ ������ loop���ϰ� Ŭ������ ���� �迭�� �־��ֹǷ� ���⼭ ������ �ʿ� ����

            players_SFX[i].bypassListenerEffects = true;        // ȿ���� ������Ʈ�鿡 AudioHighPassFilter�� bypass�� �� �ֵ��� ����
            #endregion
        }
        #endregion
        //--------------------------------------------------------------------------------------------
        #endregion
    }

    public void PlayBGM(bool isPlay)
    {
        if (isPlay)
        {
            player_BGM.Play();
        }
        else
        {
            player_BGM.Stop();
        }
    }

    public void EffectBGM(bool isPlay)
    {
        effect_BGM.enabled = isPlay;
    }

    public void PlaySFX(SFX sfx)
    {
        for (int i = 0; i < channels; i++)
        {
            // ä�� ������ŭ ���尡 ��ȸ�ϵ��� ����
            #region loopIndex ����
            // 5��° ä�κ��� 16��°���� �� ���� �ٽ� ù��° ä�κ��� ��밡���ϵ���
            // channel������ ���� �������̹Ƿ� ����� ���� ä�� �������� ������ �� ����
            //
            // ���� 4��° �����÷��̾ ������� (channelindex = 4)
            // for���� ���� ó���� loopindex�� 4�� �ǳʶٰ� 5�� �ȴ� (1 + 4)
            // 5��° �����÷��̾ ��� �� ä���ε����� 5�� ��
            #endregion
            int loopIndex = (i + channelindex) % channels;

            if (players_SFX[loopIndex].isPlaying)       // ���� ������� �����÷��̾�� �״�� ����ϵ��� ����
            {
                continue;       // �ݺ��� ���߿� ���� �ܰ�� �ǳʶٴ� Ű����
            }
            else
            {
                #region �� ��ǿ� �ִ� �������� ���� �� �ϳ��� �������� ����ϰ� ����� ��������
                int ranIndex = 0;
                if (sfx == SFX.Hit || sfx == SFX.Melee)
                {
                    ranIndex = Random.Range(0, 2);      // Range(0, ���� ����)
                }
                #endregion

                channelindex = loopIndex;
                players_SFX[loopIndex].clip = clips_SFX[(int)sfx + ranIndex];      // enum���� ���ڷ� ����ϱ� ���� �տ� (int) ����
                players_SFX[loopIndex].Play();
                break;          // ���⼭ �ݵ�� break�� �ɾ���� �ش� �����÷��̾ ���� ����ϵ��� �����ϰ� for���� ���⼭ ���� ���� (������ ���� �÷��̾�� ��� ���� ����϶�� ��Ŵ)
            }
        }
    }
}

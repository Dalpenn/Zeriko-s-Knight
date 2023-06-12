using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region 변수들
    [Header("# Singleton")]
    public static AudioManager instance;

    [Header("# BGM")]
    public AudioClip clip_BGM;
    AudioSource player_BGM;
    public float volume_BGM;
    AudioHighPassFilter effect_BGM;

    [Header("# SFX(효과음)")]
    public AudioClip[] clips_SFX;      // 효과음은 종류가 많으므로 배열로 선언
    AudioSource[] players_SFX;         // 효과음은 종류가 많으므로 배열로 선언
    public float volume_SFX;

    public int channels;               // 한번에 몇개의 효과음이 들리도록 할것인지 설정
    int channelindex;

    public enum SFX { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win };      // 직접 숫자를 입력하여 번호를 지정해줄수도 있다
    #endregion

    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        #region 배경음
        //--------------------------------------------------------------------------------------------
        #region 플레이어 생성
        GameObject object_BGM = new GameObject("BGMPlayer");
        object_BGM.transform.parent = transform;                // AudioManager의 자식 오브젝트로 사운드가 생성되도록 설정

        player_BGM = object_BGM.AddComponent<AudioSource>();    // 새로 만든 Object에 AudioSource를 생성하고, player_BGM변수에 이를 넣음
        #endregion

        #region 사운드 설정
        player_BGM.playOnAwake = false;         // 캐릭터 고른 후, 게임 시작될때 나와야 하므로 false
        player_BGM.loop = true;                 // 배경음악은 반복해서 나오므로
        player_BGM.volume = volume_BGM;
        player_BGM.clip = clip_BGM;
        #endregion

        effect_BGM = Camera.main.GetComponent<AudioHighPassFilter>();
        //--------------------------------------------------------------------------------------------
        #endregion

        // 효과음 플레이어 초기화
        #region 효과음
        //--------------------------------------------------------------------------------------------
        #region 플레이어 생성
        GameObject object_SFX = new GameObject("SFXPlayer");
        object_SFX.transform.parent = transform;

        players_SFX = new AudioSource[channels];
        for (int i = 0; i < channels; i++)
        {
            players_SFX[i] = object_SFX.AddComponent<AudioSource>();

            #region 사운드 설정
            players_SFX[i].playOnAwake = false;
            players_SFX[i].volume = volume_SFX;
            // 효과음은 어차피 loop안하고 클립들은 직접 배열에 넣어주므로 여기서 설정할 필요 없음

            players_SFX[i].bypassListenerEffects = true;        // 효과음 오브젝트들에 AudioHighPassFilter를 bypass할 수 있도록 설정
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
            // 채널 개수만큼 사운드가 순회하도록 설정
            #region loopIndex 설명
            // 5번째 채널부터 16번째까지 다 쓰면 다시 첫번째 채널부터 사용가능하도록
            // channel개수로 나눈 나머지이므로 절대로 값이 채널 개수보다 많아질 수 없다
            //
            // 만약 4번째 사운드플레이어가 사용중임 (channelindex = 4)
            // for문을 돌며 처음에 loopindex는 4를 건너뛰고 5가 된다 (1 + 4)
            // 5번째 사운드플레이어를 재생 및 채널인덱스는 5가 됨
            #endregion
            int loopIndex = (i + channelindex) % channels;

            if (players_SFX[loopIndex].isPlaying)       // 현재 재생중인 사운드플레이어는 그대로 재생하도록 놔둠
            {
                continue;       // 반복문 도중에 다음 단계로 건너뛰는 키워드
            }
            else
            {
                #region 한 모션에 있는 여러개의 사운드 중 하나를 랜덤으로 재생하게 만드는 랜덤변수
                int ranIndex = 0;
                if (sfx == SFX.Hit || sfx == SFX.Melee)
                {
                    ranIndex = Random.Range(0, 2);      // Range(0, 종류 개수)
                }
                #endregion

                channelindex = loopIndex;
                players_SFX[loopIndex].clip = clips_SFX[(int)sfx + ranIndex];      // enum값을 숫자로 사용하기 위해 앞에 (int) 붙임
                players_SFX[loopIndex].Play();
                break;          // 여기서 반드시 break를 걸어줘야 해당 사운드플레이어가 사운드 재생하도록 설정하고 for문이 여기서 끝이 난다 (없으면 이후 플레이어에도 계속 사운드 재생하라고 시킴)
            }
        }
    }
}

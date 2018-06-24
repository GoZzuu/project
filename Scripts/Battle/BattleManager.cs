using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    public List<EnemyAIbase> AllEnemys = new List<EnemyAIbase>();
    public List<EnemyAIbase> EnemyesInBattle = new List<EnemyAIbase>();

    public bool BattleStarted = false;

    AudioSource[] audioSrcs;
    public AudioClip LevelMusic;
    public AudioClip BattleMusic;

    private void OnEnable()
    {
        EnemyAIbase.EnemyCreatedEventHandler += EnemyCreated;
        EnemyAIbase.EnemyDiedEventHandler += EnemyDied;
        EnemyAIbase.EnemyLookAtPlayerEventHandler += SeeByEnemy;
    }
    private void OnDisable()
    {
        EnemyAIbase.EnemyCreatedEventHandler -= EnemyCreated;
        EnemyAIbase.EnemyDiedEventHandler -= EnemyDied;
        EnemyAIbase.EnemyLookAtPlayerEventHandler -= SeeByEnemy;
    }

    private void Start()
    {
        audioSrcs = GetComponents<AudioSource>();
        audioSrcs[0].clip = LevelMusic;
        audioSrcs[1].clip = BattleMusic;

        //audioSrcs[0].Play();
    }

    void EnemyCreated(EnemyAIbase enemy)
    {
        AllEnemys.Add(enemy);
    }
    void EnemyDied(EnemyAIbase enemy)
    {
        AllEnemys.Remove(enemy);
        EnemyesInBattle.Remove(enemy);

        if (EnemyesInBattle.Count == 0) {
            EndBattle();
        }
    }
    void SeeByEnemy(EnemyAIbase enemy)
    {
        EnemyesInBattle.Add(enemy);

        if (!BattleStarted)
        {
            StartBattle();
        }
    }

    void StartBattle()
    {
        BattleStarted = true;
        Debug.Log("Battle started");
    }
    void EndBattle()
    {
        BattleStarted = false;
        Debug.Log("Battle ended");
    }

    private void FixedUpdate()
    {        
        if (!BattleStarted)
        {
            if(!audioSrcs[0].isPlaying)
            audioSrcs[0].Play();

            if (audioSrcs[0].volume < 1 || audioSrcs[1].volume > 0)
            {
                audioSrcs[0].volume += 0.05f * Time.fixedDeltaTime;
                audioSrcs[1].volume -= 0.3f * Time.fixedDeltaTime;
            }
            else if (audioSrcs[0].volume == 1 && audioSrcs[1].volume == 0 && audioSrcs[1].isPlaying)
                audioSrcs[1].Pause();
        }
        else if (BattleStarted)
        {
            if (!audioSrcs[1].isPlaying)
                audioSrcs[1].Play();

            if (audioSrcs[1].volume < 1 || audioSrcs[0].volume > 0)
            {
                audioSrcs[1].volume += 0.4f * Time.fixedDeltaTime;
                audioSrcs[0].volume -= 0.4f * Time.fixedDeltaTime;
            }
            else if (audioSrcs[1].volume == 1 && audioSrcs[0].volume == 0 && audioSrcs[0].isPlaying)
                audioSrcs[0].Pause();
        }
    }
}

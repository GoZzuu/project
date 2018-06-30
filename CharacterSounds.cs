using UnityEngine;

public class CharacterSounds : MonoBehaviour {

    AudioSource[] _audioSources;

    public AudioClip[] stepClips;
    public AudioClip[] attackClips;
    public AudioClip[] activateClips;
    public AudioClip[] skillClips;
    public AudioClip[] damagedClips;
    public AudioClip[] dieClips;

    CharacterStats characterForAudio;

    // Use this for initialization
    void OnEnable () {
        _audioSources = GetComponentsInChildren<AudioSource>();

        characterForAudio = GetComponent<CharacterStats>();
        characterForAudio.CharacterActivatedEventHandler += ActivateSound;
        characterForAudio.CharacterAttackEventHandler += AttackSound;
        characterForAudio.CharacterDieEventHandler += DieSound;
        characterForAudio.CharacterDamagedEventHandler += DamagedSound;
        characterForAudio.CharacterSkillCastEventHandler += SkillSound;
    }

    internal void PlaySound(AudioClip clip)
    {
        if (clip == null)
            return;

        if (_audioSources.Length < 2)
        {
            _audioSources[0].clip = clip;
            _audioSources[0].pitch = 1 + Random.Range(-0.08f, 0.08f);
            _audioSources[0].Play();
        }
        else
        {
            bool isPlayed = false;

            for (int i = 0; i < _audioSources.Length; i++)
            {
                if (_audioSources[i].isPlaying)
                    continue;

                _audioSources[i].clip = clip;
                _audioSources[i].pitch = 1 + Random.Range(-0.08f, 0.08f);
                _audioSources[i].Play();
                isPlayed = true;
                break;

            }

            if (!isPlayed)
            {
                _audioSources[0].clip = clip;
                _audioSources[0].pitch = 1 + Random.Range(-0.08f, 0.08f);
                _audioSources[0].Play();
            }
        }
    }

    private void OnDisable()
    {
        characterForAudio.CharacterActivatedEventHandler -= ActivateSound;
        characterForAudio.CharacterAttackEventHandler -= AttackSound;
        characterForAudio.CharacterDieEventHandler -= DieSound;
        characterForAudio.CharacterDamagedEventHandler -= DamagedSound;
        characterForAudio.CharacterSkillCastEventHandler -= SkillSound;
    }

    void ActivateSound()
    {
        PlaySound(activateClips);
        
    }

    public void PlaySound(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return;

        if (_audioSources.Length < 2)
        {
            _audioSources[0].clip = clips[Random.Range(0, clips.Length)];
            _audioSources[0].pitch = 1 + Random.Range(-0.08f, 0.08f);
            _audioSources[0].Play();
        }
        else
        {
            bool isPlayed = false;

            for (int i = 0; i < _audioSources.Length; i++)
            {
                if (_audioSources[i].isPlaying)
                    continue;

                _audioSources[i].clip = clips[Random.Range(0, clips.Length)];
                _audioSources[i].pitch = 1 + Random.Range(-0.08f, 0.08f);
                _audioSources[i].Play();
                isPlayed = true;
                break;

            }

            if(!isPlayed)
            {
                _audioSources[0].clip = clips[Random.Range(0, clips.Length)];
                _audioSources[0].pitch = 1 + Random.Range(-0.08f, 0.08f);
                _audioSources[0].Play();
            }
        }

    }

    void DieSound()
    {
        PlaySound(dieClips);
    }
    void AttackSound()
    {
        PlaySound(attackClips);      
    }
    void DamagedSound(int damage)
    {
        PlaySound(damagedClips);
    }
    void SkillSound()
    {
        PlaySound(skillClips);
       
    }

    public void StepSound()
    {
        PlaySound(stepClips);
    }


}

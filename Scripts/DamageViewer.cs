using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageViewer : MonoBehaviour {

    Text text;
    CharacterStats character;

    WaitForFixedUpdate waitFixed = new WaitForFixedUpdate();
    WaitForSeconds waitTime = new WaitForSeconds(1.5f);

    private void OnEnable()
    {
        text = GetComponentInChildren<Text>();
        character = GetComponentInParent<CharacterStats>();
        character.CharacterDamagedEventHandler += SeeDamage;

        if(character.GetType() == typeof(PlayerStats))
        {
            text.color = Color.red;
        }

    }
    private void OnDisable()
    {
        character.CharacterDamagedEventHandler -= SeeDamage;
    }

    void SeeDamage(int damage)
    {
        text.text = damage.ToString();

        StartCoroutine(SeeText());
    }
    IEnumerator SeeText()
    {
        var color = text.color;

        color.a = 1;
        text.color = color;

        yield return waitTime;
        color = text.color;

        color.a = 0;
        text.color = color;
    }
}

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Attack {

    public enum ATTACK_TYPE
    {
        STANDART,
        ARROUND,
        RANGED
    }
    public ATTACK_TYPE attackType = ATTACK_TYPE.STANDART;

    public float DamageMultiplier = 1;
    float Range = 0.9f;
    public float stunChance = 0.1f;
    public bool CanBeBlocked = true;
    public string triggerName = "Attack";

    Transform attackPoint;
    

    LayerMask attacksTo;
    Collider[] attackTo = new Collider[5];

    public Attack()
    {
        DamageMultiplier = 1;
        Range = 0.9f;
        stunChance = 0.1f;
        CanBeBlocked = true;
        triggerName = "Attack";
    }


    public void Initialize(LayerMask attacksTo, Transform character)
    {
        //Debug.Log("Attack " + attackType + " initialized");
        this.attacksTo = attacksTo;
        switch (attackType)
        {
            case ATTACK_TYPE.STANDART:
                attackPoint = character.Find("attackPoint");
                
                break;
            case ATTACK_TYPE.ARROUND:
                Range = 1.3f;
                attackPoint = character;
                break;
            case ATTACK_TYPE.RANGED:
                Range = 0.2f;           
                break;
        }
    }

    public void DetectUnits(float damage)
    {
        //Debug.Log("Attack " + attackType + " attacking");
        int colCount = Physics.OverlapSphereNonAlloc(attackPoint.position, Range, attackTo, attacksTo);

        if (colCount > 0)
            for (int i = 0; i < colCount; i++)
            {
                var h = attackTo[i].GetComponent<CharacterStats>();
                if (h)
                {
                    float angle = Vector3.Angle(h.transform.forward, attackPoint.forward);
                    bool canBeBlocked = angle > 140 || angle < -140;
                    h.GetDamage(damage * DamageMultiplier, canBeBlocked, stunChance);
                }
            }

    }
}



/*
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Attack[]))]
public class AttackPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float x = position.x;
        float y = position.y;
        float inspectorWidth = position.width;

        var props = new[] { "attackType", "DAMAGE", "RANGE", "ATTACK_TIME", "Interuption", "triggerName" };

        
            Rect rect = new Rect(x, y, inspectorWidth, 15);
            y += 20;
            EditorGUI.PropertyField(rect, property);
        
    }
}
#endif
*/
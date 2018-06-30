using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Item : ScriptableObject {

    public delegate void InventoryEvents(Item item);
    public static event InventoryEvents ItemCollectedEventHandler;
    //public static event InventoryEvents ItemUsedEventHandler;

    public Sprite Icon;

    public AudioClip CollectSound;
    public AudioClip UseSound;

    public int Price = 10;

    public bool Useable = true;
    public bool Sellable = true;

    public virtual void Use(PlayerStats byCharacter) {
        if (!Useable)
            return;

        byCharacter.CharacterAudio.PlaySound(UseSound);
    }

    public virtual void Collect(PlayerStats byCharacter)
    {
        byCharacter.CharacterAudio.PlaySound(CollectSound);
        ItemCollectedEventHandler(this);
    }
}

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItem : Item
{
    public WeaponBase weapon;

    public override void Collect(PlayerStats byCharacter)
    {      
        UIManager.instance.SetText(weapon.name + " collected!");
        byCharacter.AddWeapon(weapon);
    }

    public override void Use(PlayerStats byCharacter)
    {
        
    }


}
[CreateAssetMenu(menuName = "Items/Heal Potion")]
public class HealingPotion : Item
{
    public int HealValue = 30;
    

    public override void Collect(PlayerStats byCharacter)
    {
        base.Collect(byCharacter);
        UIManager.instance.SetText(name + " collected!");
    }

    public override void Use(PlayerStats byCharacter)
    {
        Debug.Log("Healing!");
        byCharacter.GetComponent<CharacterSounds>().PlaySound(UseSound);
        byCharacter.Heal(HealValue);
    }
}

[CreateAssetMenu(menuName = "Items/Bomb")]
public class Bomb : Item
{
    public float Damage = 10;
    public float Radius = 3f;

    public ParticleSystem BlastParticles;

    public override void Collect(PlayerStats byCharacter)
    {
        base.Collect(byCharacter);
        UIManager.instance.SetText(name + " collected!");
    }

    public override void Use(PlayerStats byCharacter)
    {
        Debug.Log("Boom!");
    }
}

[CreateAssetMenu(menuName = "Items/Resource")]
public class Resource : Item
{
    public override void Collect(PlayerStats byCharacter)
    {
        base.Collect(byCharacter);
        UIManager.instance.SetText(name + " collected!");
    }

    public override void Use(PlayerStats byCharacter)
    {
        
    }
}
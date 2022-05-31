using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт обработки эффектов улучшения при использовании (подборе)
/// </summary>
public class ItemEffects : MonoBehaviour
{
    public void SaveEffects(GameObject player, StateMachine.WeaponLink Weapon)
    {
        player.transform.GetChild(0).Find(Weapon.ToString()).GetComponent<Weapon>().Save();
    }

    public void MaxPlayerHpUp(GameObject player, float UpEffect)
    {
        player.GetComponent<Player>().MaxHealth += UpEffect;
    }
    public void PlayerSpeedUp(GameObject player, float UpEffect)
    {
        player.GetComponent<Player>().Speed += UpEffect;
    }

    public void WeaponDmgUp(GameObject player, float UpEffect, StateMachine.WeaponLink Weapon)
    {
        player.transform.GetChild(0).Find(Weapon.ToString()).GetComponent<Weapon>().DamageBonusLevel += UpEffect;
        SaveEffects(player, Weapon);
    }
    public void WeaponFireRateUp(GameObject player, float UpEffect, StateMachine.WeaponLink Weapon)
    {
        player.transform.GetChild(0).Find(Weapon.ToString()).GetComponent<Weapon>().FireRateBonusLevel += UpEffect;
        SaveEffects(player, Weapon);
    }
    public void WeaponBulletSpeedUp(GameObject player, float UpEffect, StateMachine.WeaponLink Weapon)
    {
        player.transform.GetChild(0).Find(Weapon.ToString()).GetComponent<Weapon>().BulletSpeedBonusLevel += UpEffect;
        SaveEffects(player, Weapon);
    }
    public void WeaponPenetrationUp(GameObject player, float UpEffect, StateMachine.WeaponLink Weapon)
    {
        player.transform.GetChild(0).Find(Weapon.ToString()).GetComponent<Weapon>().PenetrationBonusLevel += (int)UpEffect;
        SaveEffects(player, Weapon);
    }
    public void WeaponClipSizeUp(GameObject player, float UpEffect, StateMachine.WeaponLink Weapon)
    {
        player.transform.GetChild(0).Find(Weapon.ToString()).GetComponent<Weapon>().ClipSizeBonusLevel += (int)UpEffect;
        SaveEffects(player, Weapon);
    }
    public void WeaponMultiShotUp(GameObject player, float UpEffect, StateMachine.WeaponLink Weapon)
    {
        player.transform.GetChild(0).Find(Weapon.ToString()).GetComponent<Weapon>().MultiShotBonusLevel += (int)UpEffect;
        SaveEffects(player, Weapon);
    }
    public void WeaponRecoilUp(GameObject player, float UpEffect, StateMachine.WeaponLink Weapon)
    {
        player.transform.GetChild(0).Find(Weapon.ToString()).GetComponent<Weapon>().ClipRecoilBonusLevel += (int)UpEffect;
        SaveEffects(player, Weapon);
    }
}
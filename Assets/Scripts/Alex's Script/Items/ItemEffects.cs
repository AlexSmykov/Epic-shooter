using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffects : MonoBehaviour
{
    private delegate void MyDelegate(Collider2D collision);

    MyDelegate[] Effects = new MyDelegate[42];

    private void Start()
    {
        Effects[0] = Effect_0;
        Effects[1] = Effect_1;
        Effects[2] = Effect_2;
        Effects[3] = Effect_3;
        Effects[4] = Effect_4;
        Effects[5] = Effect_5;
        Effects[6] = Effect_6;
        Effects[7] = Effect_7;
        Effects[8] = Effect_8;
        Effects[9] = Effect_9;
        Effects[10] = Effect_10;
        Effects[11] = Effect_11;
        Effects[12] = Effect_12;
        Effects[13] = Effect_13;
        Effects[14] = Effect_14;
        Effects[15] = Effect_15;
        Effects[16] = Effect_16;
        Effects[17] = Effect_17;
        Effects[18] = Effect_18;
        Effects[19] = Effect_19;
        Effects[20] = Effect_20;
        Effects[21] = Effect_21;
        Effects[22] = Effect_22;
        Effects[23] = Effect_23;
        Effects[24] = Effect_24;
        Effects[25] = Effect_25;
        Effects[26] = Effect_26;
        Effects[27] = Effect_27;
        Effects[28] = Effect_28;
        Effects[29] = Effect_29;
        Effects[30] = Effect_30;
        Effects[31] = Effect_31;
        Effects[32] = Effect_32;
        Effects[33] = Effect_33;
        Effects[34] = Effect_34;
        Effects[35] = Effect_35;
        Effects[36] = Effect_36;
        Effects[37] = Effect_37;
        Effects[38] = Effect_38;
        Effects[39] = Effect_39;
        Effects[40] = Effect_40;
        Effects[41] = Effect_41;
    }

    public void SaveEffects(Collider2D collision, int WeaponIndex)
    {
        collision.GetComponent<Player>().PlayerGuns[WeaponIndex].Save();
    }
    public void UseEffect(Collider2D collision, int ItemIndex)
    {
        Effects[ItemIndex](collision);
    }
    private void MaxPlayerHpUp(Collider2D collision, float UpEffect)
    {
        collision.GetComponent<Player>().MaxHealth += UpEffect;
    }
    private void PlayerSpeedUp(Collider2D collision, float UpEffect)
    {
        collision.GetComponent<Player>().Speed += UpEffect;
    }
    private void WeaponDmgUp(Collider2D collision, float UpEffect, int WeaponIndex)
    {
        collision.GetComponent<Player>().PlayerGuns[WeaponIndex].DamageBonusLevel += UpEffect;
        SaveEffects(collision, WeaponIndex);
    }
    private void WeaponFireRateUp(Collider2D collision, float UpEffect, int WeaponIndex)
    {
        collision.GetComponent<Player>().PlayerGuns[WeaponIndex].FireRateBonusLevel += UpEffect;
        SaveEffects(collision, WeaponIndex);
    }
    private void WeaponBulletSpeedUp(Collider2D collision, float UpEffect, int WeaponIndex)
    {
        collision.GetComponent<Player>().PlayerGuns[WeaponIndex].BulletSpeedBonusLevel += UpEffect;
        SaveEffects(collision, WeaponIndex);
    }
    private void WeaponPenetrationUp(Collider2D collision, float UpEffect, int WeaponIndex)
    {
        collision.GetComponent<Player>().PlayerGuns[WeaponIndex].PenetrationBonusLevel += (int)UpEffect;
        SaveEffects(collision, WeaponIndex);
    }
    private void WeaponClipSizeUp(Collider2D collision, float UpEffect, int WeaponIndex)
    {
        collision.GetComponent<Player>().PlayerGuns[WeaponIndex].ClipSize += (int)UpEffect;
        SaveEffects(collision, WeaponIndex);
    }
    private void WeaponMultiShotUp(Collider2D collision, float UpEffect, int WeaponIndex)
    {
        collision.GetComponent<Player>().PlayerGuns[WeaponIndex].MultiShotBonusLevel += (int)UpEffect;
        SaveEffects(collision, WeaponIndex);
    }
    private void WeaponRecoilUp(Collider2D collision, float UpEffect, int WeaponIndex)
    {
        collision.GetComponent<Player>().PlayerGuns[WeaponIndex].ClipRecoilBonusLevel += (int)UpEffect;
        SaveEffects(collision, WeaponIndex);
    }
    
    private void Effect_0(Collider2D collision)
    {
        WeaponDmgUp(collision, 1, 0);
    }
    private void Effect_1(Collider2D collision)
    {
        WeaponDmgUp(collision, 1, 1);
    }
    private void Effect_2(Collider2D collision)
    {
        WeaponDmgUp(collision, 1, 2);
    }
    private void Effect_3(Collider2D collision)
    {
        WeaponDmgUp(collision, 1, 3);
    }
    private void Effect_4(Collider2D collision)
    {
        WeaponDmgUp(collision, 1, 4);
    }
    private void Effect_5(Collider2D collision)
    {
        WeaponDmgUp(collision, 1, 5);
    }
    private void Effect_6(Collider2D collision)
    {
        WeaponFireRateUp(collision, 1, 0);
    }
    private void Effect_7(Collider2D collision)
    {
        WeaponFireRateUp(collision, 1, 1);
    }
    private void Effect_8(Collider2D collision)
    {
        WeaponFireRateUp(collision, 1, 2);
    }
    private void Effect_9(Collider2D collision)
    {
        WeaponFireRateUp(collision, 1, 3);
    }
    private void Effect_10(Collider2D collision)
    {
        WeaponFireRateUp(collision, 1, 4);
    }
    private void Effect_11(Collider2D collision)
    {
        WeaponFireRateUp(collision, 1, 5);
    } 
    private void Effect_12(Collider2D collision)
    {
        WeaponClipSizeUp(collision, 2, 0);
    }
    private void Effect_13(Collider2D collision)
    {
        WeaponClipSizeUp(collision, 1, 1);
    }
    private void Effect_14(Collider2D collision)
    {
        WeaponClipSizeUp(collision, 5, 2);
    }
    private void Effect_15(Collider2D collision)
    {
        WeaponClipSizeUp(collision, 1, 3);
    }
    private void Effect_16(Collider2D collision)
    {
        WeaponClipSizeUp(collision, 4, 4);
    }
    private void Effect_17(Collider2D collision)
    {
        WeaponClipSizeUp(collision, 1, 5);
    }
    private void Effect_18(Collider2D collision)
    {
        WeaponPenetrationUp(collision, 1, 0);
    }
    private void Effect_19(Collider2D collision)
    {
        WeaponPenetrationUp(collision, 1, 1);
    }
    private void Effect_20(Collider2D collision)
    {
        WeaponPenetrationUp(collision, 1, 2);
    }
    private void Effect_21(Collider2D collision)
    {
        WeaponPenetrationUp(collision, 1, 3);
    }
    private void Effect_22(Collider2D collision)
    {
        WeaponPenetrationUp(collision, 1, 4);
    }
    private void Effect_23(Collider2D collision)
    {
        WeaponPenetrationUp(collision, 1, 5);
    }
    private void Effect_24(Collider2D collision)
    {
        WeaponBulletSpeedUp(collision, 1, 0);
    }
    private void Effect_25(Collider2D collision)
    {
        WeaponBulletSpeedUp(collision, 1, 1);
    }
    private void Effect_26(Collider2D collision)
    {
        WeaponBulletSpeedUp(collision, 1, 2);
    }
    private void Effect_27(Collider2D collision)
    {
        WeaponBulletSpeedUp(collision, 1, 3);
    }
    private void Effect_28(Collider2D collision)
    {
        WeaponBulletSpeedUp(collision, 1, 4);
    }
    private void Effect_29(Collider2D collision)
    {
        WeaponBulletSpeedUp(collision, 1, 5);
    }
    private void Effect_30(Collider2D collision)
    {
        WeaponMultiShotUp(collision, 1, 0);
    }
    private void Effect_31(Collider2D collision)
    {
        WeaponMultiShotUp(collision, 3, 1);
    }
    private void Effect_32(Collider2D collision)
    {
        WeaponMultiShotUp(collision, 1, 2);
    }
    private void Effect_33(Collider2D collision)
    {
        WeaponMultiShotUp(collision, 1, 3);
    }
    private void Effect_34(Collider2D collision)
    {
        WeaponMultiShotUp(collision, 1, 4);
    }
    private void Effect_35(Collider2D collision)
    {
        WeaponMultiShotUp(collision, 1, 5);
    }
    private void Effect_36(Collider2D collision)
    {
        WeaponRecoilUp(collision, 1, 0);
    }
    private void Effect_37(Collider2D collision)
    {
        WeaponRecoilUp(collision, 1, 1);
    }
    private void Effect_38(Collider2D collision)
    {
        WeaponRecoilUp(collision, 1, 2);
    }
    private void Effect_39(Collider2D collision)
    {
        WeaponRecoilUp(collision, 1, 3);
    }
    private void Effect_40(Collider2D collision)
    {
        WeaponRecoilUp(collision, 1, 4);
    }
    private void Effect_41(Collider2D collision)
    {
        WeaponRecoilUp(collision, 1, 5);
    }
}

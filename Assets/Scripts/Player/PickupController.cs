using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт для обработки подобранных игроком предметов
/// </summary>
[RequireComponent(typeof(ResourcesManager))]
public class PickupController : MonoBehaviour
{
    private StateMachine _Statemachine;
    private WeaponSwitch _WeaponSwitch;
    private ResourcesManager _ResourcesManager;
    private ItemEffects _ItemEffects = new ItemEffects();

    private void Start()
    {
        _Statemachine = GameObject.Find("StateMachine").GetComponent<StateMachine>();
        _WeaponSwitch = GameObject.Find("PlaceForGun").GetComponent<WeaponSwitch>();
        _ResourcesManager = GetComponent<ResourcesManager>();
    }

    public void OnPickup(GameObject item)
    {
        Factory.BaseObjects type = item.GetComponent<Pickupable>().PickupType;
        if (type == Factory.BaseObjects.Weapon)
        {
            _Statemachine.WeaponsUnlocked(item.GetComponent<WithId>().WhichWeapon);
            Debug.Log(type);
            _WeaponSwitch.UnlockWeapon(item.GetComponent<WithId>().WhichWeapon);
        }
        else if (type == Factory.BaseObjects.Pickup)
        {
            Pickups pickup = item.GetComponent<Pickups>();
            if (item.GetComponent<Pickups>().Type == Factory.PickupTypes.Coin)
            {
                _ResourcesManager.Coins += pickup.PickupCount;
            }
            else if (pickup.Type == Factory.PickupTypes.Key)
            {
                _ResourcesManager.Keys += pickup.PickupCount;
            }
            else if (pickup.Type == Factory.PickupTypes.Material)
            {
                _ResourcesManager.Materials += pickup.PickupCount;
            }
            else if (pickup.Type == Factory.PickupTypes.HealPotion)
            {
                GetComponent<Player>().HealthChange(pickup.PickupCount);
            }
            else if (pickup.Type == Factory.PickupTypes.Bullets)
            {
                Weapon weapon = transform.GetChild(0).Find(pickup.GetComponent<WithId>().WhichWeapon.ToString()).GetComponent<Weapon>();
                weapon.BulletsCount += pickup.PickupCount;
                weapon.BulletTextUpdate();
            }

            GetComponent<ResourcesManager>().UpdateResourcesText();
        }
        else if (type == Factory.BaseObjects.Item)
        {
            ItemInfo itemInfo = GameObject.FindGameObjectWithTag("ItemInfo").GetComponent<ItemInfo>();
            Item itemEffects = item.GetComponent<Item>();
            WithId ItemId = itemEffects.GetComponent<WithId>();

            if (itemEffects.DamageUp != 0)
            {
                _ItemEffects.WeaponDmgUp(gameObject, itemEffects.DamageUp, ItemId.WhichWeapon);
            }
            if (itemEffects.FireRateUp != 0)
            {
                _ItemEffects.WeaponFireRateUp(gameObject, itemEffects.FireRateUp, ItemId.WhichWeapon);
            }
            if (itemEffects.RecoilTimeUp != 0)
            {
                _ItemEffects.WeaponRecoilUp(gameObject, itemEffects.RecoilTimeUp, ItemId.WhichWeapon);
            }
            if (itemEffects.ClipSizeUp != 0)
            {
                _ItemEffects.WeaponClipSizeUp(gameObject, itemEffects.ClipSizeUp, ItemId.WhichWeapon);
            }
            if (itemEffects.BulletSpeedUp != 0)
            {
                _ItemEffects.WeaponBulletSpeedUp(gameObject, itemEffects.BulletSpeedUp, ItemId.WhichWeapon);
            }
            if (itemEffects.PenetrationUp != 0)
            {
                _ItemEffects.WeaponPenetrationUp(gameObject, itemEffects.PenetrationUp, ItemId.WhichWeapon);
            }
            if (itemEffects.MultyShotUp != 0)
            {
                _ItemEffects.WeaponMultiShotUp(gameObject, itemEffects.MultyShotUp, ItemId.WhichWeapon);
            }

            transform.GetChild(0).Find(ItemId.WhichWeapon.ToString()).GetComponent<Weapon>().UpdateSkin();
            itemInfo.ActivateInfo(itemEffects.Name, itemEffects.Description);
        }
    }
}

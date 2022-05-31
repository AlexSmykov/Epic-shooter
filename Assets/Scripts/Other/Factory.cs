using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Скрипт для создания разных объектов по запросу
/// </summary>
public class Factory : MonoBehaviour
{
	private static GameObject _Storages;

	//Типы базовых объектов
	public enum BaseObjects
	{
		Pickup,
		Weapon,
		Item,
		Chest,
		RoomReward,
		ChestReward,
	}

	//Типы объектов, которые будут сохранятся в txt
	public enum SavableObjects
	{
		Pickup,
		Weapon,
		Item,
		Chest,
		Other,
	}
	
	//Типы комнат, возможные для размещения
	public enum Rooms
	{
		Default,
		Start,
		Chest,
		Shop,
		Lab,
		Boss,
		SuperBoss,
	}

	//Типы подбираемых предметов
	public enum PickupTypes
	{
		None,
		Coin,
		Material,
		Key,
		HealPotion,
		Bullets
	}

	//Типы наград после зачистки комнаты (пока что одинаковые с прошлым, но могут меняться в дальнейшем)
	public enum RewardTypes
	{
		None,
		Coin,
		Material,
		Key,
		HealPotion,
		Bullets,
	}

	public void LinkStorage()
	{
		_Storages = GameObject.Find("ImportantThings").transform.GetChild(0).gameObject;
	}

	/// <summary>
	/// Создать случайный объект из переданного типа
	/// </summary>
	public GameObject CreateRandom(BaseObjects type)
	{
		if (type == BaseObjects.Pickup)
		{
			return Randomer.GetRandomFromArray(_Storages.transform.Find("AllPickups").GetComponent<ArrayHolder>().Items);
		}
		else if (type == BaseObjects.Weapon)
		{
			return Randomer.GetRandomFromArray(_Storages.transform.Find("AllWeapons").GetComponent<ArrayHolder>().Items);
		}
		else if (type == BaseObjects.Item)
		{
			return Randomer.GetRandomFromArray(_Storages.transform.Find("AllItems").GetComponent<ArrayHolder>().Items);
		}
		else if (type == BaseObjects.ChestReward)
		{
			return CreateRandomChestReward(StateMachine.ItemActivityState.AvailableToUse, RewardTypes.None);
		}
		else if (type == BaseObjects.RoomReward)
		{
			return CreateRandomRoomReward(StateMachine.ItemActivityState.AvailableToUse, RewardTypes.None);
		}

		return null;
	}

	/// <summary>
	/// Создать объекта по имени
	/// </summary>
	public GameObject CreateDeterminedSaveObjectByName(string Name)
	{
		foreach(SavableObjects type in Enum.GetValues(typeof(SavableObjects)))
		{
			foreach (GameObject item in _Storages.transform.Find("All" + type.ToString() + "s").GetComponent<ArrayHolder>().Items)
			{
				if (item.name == Name)
				{
					return item;
				}
			}
		}

		return null;
	}

	/// <summary>
	/// создать случайную комнату по типу
	/// </summary>
	public GameObject CreateRandomRoom(Rooms type)
	{
		return Randomer.GetRandomFromArray(_Storages.transform.Find("Rooms/All" + type.ToString() + "Rooms").GetComponent<ArrayHolder>().Items);
	}

	/// <summary>
	/// Создать определенную комнату, необходимо для загрузки комнат из сохранения
	/// </summary>
	public GameObject CreateDeterminedRoom(Rooms type, string Name, bool IsClear)
	{
		foreach(GameObject item in _Storages.transform.Find("Rooms/All" + type.ToString() + "Rooms").GetComponent<ArrayHolder>().Items)
        {
			if (item.name == Name)
            {
				GameObject ReturnItem = Instantiate(item);

				if (IsClear)
				{
					ReturnItem.GetComponent<Room>().InstantClearRoom();
				}

				return ReturnItem;
            }
		}

		return null;
	}

	/// <summary>
	/// Создать случайное улучшение по степени активности
	/// </summary>
	public GameObject CreateRandomItem(StateMachine.ItemActivityState State, StateMachine.WeaponLink Weapon)
	{
		GameObject[] Items = _Storages.transform.Find("AllItems").GetComponent<ArrayHolder>().Items;
		List<GameObject> Variants = new List<GameObject>();

		foreach (GameObject item in Items)
        {
			if (item.GetComponent<WithId>().GetActivityState() == State &&
				(item.GetComponent<WithId>().WhichWeapon == Weapon || Weapon == StateMachine.WeaponLink.None))
            {
				Variants.Add(item);
			}
        }

		return Randomer.GetRandomFromList(Variants);
	}

	/// <summary>
	/// Создать случайный подбираемый предмет по степени активности
	/// </summary>
	public GameObject CreateRandomPickup(StateMachine.ItemActivityState State, PickupTypes Type, StateMachine.WeaponLink Weapon)
	{
		GameObject[] pickups = _Storages.transform.Find("AllPickups").GetComponent<ArrayHolder>().Items;
		List<GameObject> Variants = new List<GameObject>();

		foreach (GameObject Pickup in pickups)
        {
			if (Pickup.GetComponent<WithId>().GetActivityState() == State && 
				(Pickup.GetComponent<Pickups>().Type == Type || Type == PickupTypes.None) && 
				(Pickup.GetComponent<WithId>().WhichWeapon == Weapon ||  Weapon == StateMachine.WeaponLink.None))
			{
				Variants.Add(Pickup);
            }
        }

		return Randomer.GetRandomFromList(Variants);
	}

	public GameObject CreateRandomChestReward(StateMachine.ItemActivityState State, RewardTypes Type)
	{
		GameObject[] Rewards = _Storages.transform.Find("AllChestRewards").GetComponent<ArrayHolder>().Items;
		List<GameObject> Variants = new List<GameObject>();

		foreach (GameObject Reward in Rewards)
		{
			if (Reward.GetComponent<WithId>().GetActivityState() == State)
			{
				Variants.Add(Reward);
			}
		}

		return Randomer.GetRandomFromList(Variants);
	}

	public GameObject CreateRandomRoomReward(StateMachine.ItemActivityState State, RewardTypes Type)
	{
		GameObject[] Rewards = _Storages.transform.Find("AllRoomRewards").GetComponent<ArrayHolder>().Items;
		List<GameObject> Variants = new List<GameObject>();

		foreach (GameObject Reward in Rewards)
		{
			if (Reward.GetComponent<WithId>().GetActivityState() == State)
			{
				Variants.Add(Reward);
			}
		}

		return Randomer.GetRandomFromList(Variants);
	}

	/// <summary>
	/// Для получения списка всех типов комнат кроме определённых типов
	/// </summary>
	public static List<Rooms> GetRoomTypesList(List<Rooms> Except)
    {
		List<Rooms> RoomList = new List<Rooms>();
		Rooms[] RoomArray = (Rooms[]) Enum.GetValues(typeof(Rooms));

		foreach(Rooms Room in RoomArray)
        {
			if (!Except.Contains(Room))
            {
				RoomList.Add(Room);
			}
        }

		return RoomList;
	}
}

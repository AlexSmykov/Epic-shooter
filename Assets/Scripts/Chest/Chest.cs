using UnityEngine;

/// <summary>
/// Класс сундука (абстрактный)
/// </summary>
[RequireComponent(typeof(WithId))]
public abstract class Chest : MonoBehaviour
{
    [SerializeField] protected Sprite UnlockedChest;
    protected Animator Animator;
    private float _SpawnLifeTime = 1;
    protected Factory Factory = new Factory();
    [SerializeField] private int _KeysNeed;
    private bool _ChestUnlocked;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_ChestUnlocked && _SpawnLifeTime <= 0 && collision.GetComponent<ResourcesManager>().Keys >= _KeysNeed)
        {
            collision.GetComponent<ResourcesManager>().Keys -= _KeysNeed;
            collision.GetComponent<ResourcesManager>().UpdateResourcesText();


            SpawnObjectFromChest(collision);
            GetComponent<SpriteRenderer>().sprite = UnlockedChest;
            _ChestUnlocked = true;
        }
    }

    private void Update()
    {
        _SpawnLifeTime -= Time.deltaTime;
    }

    private void DestroyByAnimation()
    {
        GetComponent<WithId>().OnGrab();
        Destroy(gameObject);
    }

    public abstract void SpawnObjectFromChest(Collider2D collision);
}

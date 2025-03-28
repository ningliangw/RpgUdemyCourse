using System.Diagnostics;
using UnityEngine;

public class PlayerManager : MonoBehaviour,ISaveManager
{
    public static PlayerManager Instance { get; private set; }

    public GameObject player;
    public GameObject fx;
    public GameObject item;

    public int currency;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            //Instance = null;
        }
        else
            Instance = this;
    }

    public bool HaveEnoughMoney(int _price)
    {
        if (_price > currency)
        {
            return false;
        }

        currency = currency - _price;
        return true;
    }
    public int GetCurrency()
    {
        return currency;
    }

    public void LoadData(GameData _data)
    {
        this.currency = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
    }
}

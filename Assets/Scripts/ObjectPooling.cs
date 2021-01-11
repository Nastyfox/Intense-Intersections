using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling SharedInstance;

    public List<GameObject> pooledCars;
    public GameObject carToPool;
    public int carAmountToPool;

    public List<GameObject> pooledPeds;
    public GameObject maleToPool;
    public GameObject femaleToPool;
    public int pedAmountToPool;

    // Start is called before the first frame update
    void Awake()
    {
        SharedInstance = this;
    }

    // Update is called once per frame
    void Start()
    {
        pooledCars = new List<GameObject>();
        GameObject tmp;
        
        for(int i = 0; i < carAmountToPool; i++)
        {
            tmp = Instantiate(carToPool);
            tmp.SetActive(false);
            pooledCars.Add(tmp);
        }

        for (int i = 0; i < pedAmountToPool; i++)
        {
            tmp = Instantiate(maleToPool);
            tmp.SetActive(false);
            pooledPeds.Add(tmp);
            tmp = Instantiate(femaleToPool);
            pooledPeds.Add(tmp);
            tmp.SetActive(false);
        }
    }

    public GameObject GetPedObject(int type)
    {
        for (int i = 0; i < 2 * pedAmountToPool; i += 2)
        {
            if (!pooledPeds[i + type].activeInHierarchy)
            {
                return pooledPeds[i + type];
            }
        }
        return null;
    }
    
    public GameObject GetCarObject()
    {
        for (int i = 0; i < carAmountToPool; i++)
        {
            if (!pooledCars[i].activeInHierarchy)
            {
                return pooledCars[i];
            }
        }
        return null;
    }
}

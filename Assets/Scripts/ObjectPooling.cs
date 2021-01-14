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
        //Instantiate every necessary objects for the game
        pooledCars = new List<GameObject>();
        GameObject tmp;
        
        //Instantiation of cars
        for(int i = 0; i < carAmountToPool; i++)
        {
            tmp = Instantiate(carToPool);
            tmp.SetActive(false);
            pooledCars.Add(tmp);
        }

        //Instantiation of male and female pedestrians
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
        //Return first inactive pedestrian object based on type for male or female
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
        //Return first inactive car object
        for (int i = 0; i < carAmountToPool; i++)
        {
            if (!pooledCars[i].activeInHierarchy)
            {
                return pooledCars[i];
            }
        }
        return null;
    }

    public List<GameObject> GetActiveCars()
    {
        //Return all active cars
        List<GameObject> activeCars = new List<GameObject>();

        for (int i = 0; i < carAmountToPool; i++)
        {
            if (pooledCars[i].activeInHierarchy)
            {
                activeCars.Add(pooledCars[i]);
            }
        }
        return activeCars;
    }
}

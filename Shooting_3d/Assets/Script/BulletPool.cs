using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int maxElements = 75;

    Stack<GameObject> pool = new Stack<GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < maxElements; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            pool.Push(bullet);
        }
    }

    public GameObject PopObject()
    {
        GameObject objReturn = null;

        if (pool.Count != 0)
        {
            objReturn = pool.Pop();
        }
        else
        {
            // Pool empty — create a new bullet temporarily
            objReturn = Instantiate(bulletPrefab);
            objReturn.SetActive(false);
            objReturn.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }

        return objReturn;
    }

    public void PushObject(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        obj.SetActive(false);
        pool.Push(obj);
    }
}

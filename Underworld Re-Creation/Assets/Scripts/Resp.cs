using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resp : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> EnemiesSkeletons = new List<GameObject>();
    GameObject Skeleton;
    public float TimeResp = 5;
   
    
    void Start()
    {
        Vector3 poz = transform.position;
        int l = Random.Range(0, EnemiesSkeletons.Count - 1);
        Skeleton = Instantiate(EnemiesSkeletons[l], poz, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Skeleton == null)
        {
            TimeResp -= Time.deltaTime;
            if (TimeResp <= 0)
            {
                Vector3 poz = transform.position;
                int l = Random.Range(0, EnemiesSkeletons.Count - 1);
                Skeleton = Instantiate(EnemiesSkeletons[l], poz, Quaternion.identity);
                TimeResp = 5;
            }
        }
    }
}

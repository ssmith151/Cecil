using UnityEngine;
using System.Collections;

public class MonsterMaker : MonoBehaviour {

    public float spawnWait;
    public float spawnRepeat;
    public GameObject monster;
    
	// Use this for initialization
	void Start () {
        InvokeRepeating("CreateMonster", spawnWait, spawnRepeat);
	}
	
    void CreateMonster()
    {
        Instantiate(monster, gameObject.transform.position, gameObject.transform.rotation);
    }
}

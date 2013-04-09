using UnityEngine;
using System.Collections;

public class CoroutineRunner : MonoBehaviour {
 
    static CoroutineRunner Instance;

	// Use this for initialization
	void Start () {
	    Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public static void StartFutileCoroutine(IEnumerator f){
        Instance.StartCoroutine(f);
    }
    
    
}

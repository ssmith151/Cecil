using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainUngalate : MonoBehaviour {

    private BoxCollider2D boxCol;
    private List<GameObject> cells = new List<GameObject> { };

    public bool bob;
    public float bobHeight;
    public float bobTime;
    public float bobSpace;
    public float bobSpeed;
    public float replayTime;
	void Awake()
    {
        bobHeight *= 0.1f;
        boxCol = gameObject.GetComponent<BoxCollider2D>();
        MakeBorder();
        StartCoroutine(BobStart());
    }
    IEnumerator BobStart()
    {
        // switch this to for-loop and keep track of index
        // then space the cells out over the index
        // differentiate the times accourding to the replaytime
        foreach (GameObject Go in cells)
        {
            float bobOffsetOut = (cells.IndexOf(Go) * 1.0f) / cells.Count;
            StartCoroutine(BobControl(Go, bobOffsetOut * bobSpace));
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator BobControl(GameObject GO, float bobOffset)
    {
        float currentHeight = 0;
        float startHeight = GO.transform.position.y;
        while (bob)
        {
            currentHeight = Mathf.Sin(Time.time * bobSpeed + bobOffset) * bobHeight;
            GO.transform.position = new Vector3(GO.transform.position.x, currentHeight + startHeight, GO.transform.position.z);
            yield return new WaitForSeconds(bobTime);
        }
        yield return null;
    }
    void MakeBorder()
    {
        // root of bounds should contain the foreground element, change to gameObject.transform.root.gameObject;
        GameObject rootOfBounds = boxCol.gameObject;
        Transform[] rootGOs = rootOfBounds.GetComponentsInChildren<Transform>();
        foreach (Transform GO in rootGOs)
        {
            if (boxCol.bounds.Contains(new Vector2(GO.position.x,GO.position.y)))
            {
                if (GO.gameObject != rootOfBounds)
                    cells.Add(GO.gameObject);
            }
        }
        cells.Sort(delegate (GameObject a, GameObject b) {
            return (a.GetComponent<Transform>().position.x).CompareTo(b.GetComponent<Transform>().position.x);
        });
    }
}

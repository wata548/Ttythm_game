using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleBackGround : MonoBehaviour
{
    public int keyLine;

    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);

        keyLine = ChartReader.Instance.keyLineCount;
        transform.localScale = new Vector3(transform.localScale.x * keyLine, transform.localScale.y, 1);
    }
    void Start()
    {
        StartCoroutine(wait(0.001f));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

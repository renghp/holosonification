using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathToNextSnappedPoint : MonoBehaviour
{

    private Transform target;
    private int index;
    public GameObject mainCamera;

    private LineRenderer line;

    // Use this for initialization
    void Start()
    {

        //line = this.gameObject.AddComponent<LineRenderer>();
        // Set the width of the Line Renderer
        // line.startWidth = 0.5f;
        // line.endWidth = 0.5f;
        // Set the number of vertex fo the Line Renderer
        // line.positionCount = 2;

    }

    // Update is called once per frame
    void Update()
    {

        //se for muito pesado, podemos tirar isso do update. botar apenas no start, porém fazendo o objeto atual mandar o objeto anterior (index - 1, que já foi criado) apontar para o atual

        index = transform.GetSiblingIndex();

        if (index > 1)
        {
            GameObject target2;
            GameObject target1;

            target1 = transform.parent.GetChild(index).gameObject;

            target2 = transform.parent.GetChild(index - 1).gameObject;

            if (target1.gameObject != null && target2.gameObject != null)
            {
                target1.gameObject.GetComponent<LineRenderer>().SetPosition(0, target1.transform.position);
                target1.gameObject.GetComponent<LineRenderer>().SetPosition(1, target2.transform.position);
                gameObject.gameObject.GetComponent<LineRenderer>().enabled = true;

            }
        }
        else
        {
            gameObject.gameObject.GetComponent<LineRenderer>().enabled = false;
        }

       // float yPos = transform.position.y - 1f;

        //transform.position = new Vector3(transform.position.x, transform.position.y - (transform.position.y - mainCamera.transform.position.y+1.5f) / 20f, transform.position.z);  //smoothing things out

    }
}

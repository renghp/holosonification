using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowPointsToNextSibling : MonoBehaviour {

    private Transform target;
    private int index;
    public GameObject mainCamera;

    // Use this for initialization
    void Start () {

        //line = this.gameObject.AddComponent<LineRenderer>();
        // Set the width of the Line Renderer
       // line.startWidth = 0.5f;
       // line.endWidth = 0.5f;
        // Set the number of vertex fo the Line Renderer
       // line.positionCount = 2;

    }
	
	// Update is called once per frame
	void Update () {

        //se for muito pesado, podemos tirar isso do update. botar apenas no start, porém fazendo o objeto atual mandar o objeto anterior (index - 1, que já foi criado) apontar para o atual

        if (transform.GetChild(0).GetComponent<MeshRenderer>().enabled)
        {

            index = transform.GetSiblingIndex();

            if (transform.parent.childCount > index + 1)
            {

                target = transform.parent.GetChild(index + 1).transform;

                //Vector3 targetPos = new Vector3(transform.localPosition.x, transform.localPosition.y, target.transform.localPosition.z);

                transform.LookAt(target.transform.position);        //needs to be the sibling's world position, not the local one

                if (transform.GetSiblingIndex() == 1)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 2f);
                }
                // else
                //   transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.x);

            }
            else
                transform.localEulerAngles = new Vector3(90f, 0f, 0f);
        }
        else
            transform.LookAt(mainCamera.transform);


        //transform.position = new Vector3(transform.position.x, transform.position.y - (transform.position.y - mainCamera.transform.position.y + 1f) /20f , transform.position.z);     //smoothing things out

    }
}

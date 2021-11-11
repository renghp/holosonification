

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnableMesh : MonoBehaviour
{

    public Material wireSR;       
    public Material occlusionSR;      
 


    private bool status = true;

    [SerializeField]
    private Text meshName;

    void OnSelect()
    {
        SpatialMapping.Instance.DrawVisualMeshes = false;

        status = !status;
        if (status)
        {
            
            SpatialMapping.Instance.DrawMaterial = wireSR;
            meshName.text = "Atual: \n wireSR";
            SpatialMapping.Instance.DrawVisualMeshes = true;
        }
        else
        {
            
            SpatialMapping.Instance.DrawMaterial = occlusionSR;
            meshName.text = "Atual: \n occlusionSR";
            SpatialMapping.Instance.DrawVisualMeshes = true;
        }

    }


    
}
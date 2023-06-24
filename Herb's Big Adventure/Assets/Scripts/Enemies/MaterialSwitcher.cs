using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    [SerializeField]
    private BossController bc;

    [SerializeField]
    private MeshRenderer Renderer;

    [SerializeField]
    private Material grayColor, greenColor;

    private Material[] originalMaterials;

    //private Material currentMaterialRight, currentMaterialLeft;

    // Start is called before the first frame update
    void Start()
    {
        originalMaterials = Renderer.sharedMaterials;
        //currentMaterialRight = Renderer.sharedMaterial;
        //currentMaterialLeft = Renderer.sharedMaterial;
    }

    public void SwitchMaterials()
    {
        if (Renderer != null)
        {
            if (bc.canTakeDamage)
            {
                if (originalMaterials[0] == grayColor)
                {
                    originalMaterials[0] = greenColor;
                }
                else
                {
                    originalMaterials[0] = originalMaterials[0];
                }
            }
            else
            {
                if (originalMaterials[0] == greenColor)
                {
                    originalMaterials[0] = grayColor;
                }
                else
                {
                    originalMaterials[0] = originalMaterials[0];
                }
            }

        }
    }
}

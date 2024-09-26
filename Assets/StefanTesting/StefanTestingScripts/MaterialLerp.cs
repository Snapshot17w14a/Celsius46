using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MaterialLerp : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer targetMesh;
    [SerializeField]
    private Material[] materials;
    private Material tempMaterial;
    [SerializeField]
    private float currentStage;
    public float CurrentStage { get => currentStage; set => UpdateMaterial(); } //goes from 0f to 1f

    private void Start()
    {
        tempMaterial = materials[0];
    }

    private void Update()
    {
        CurrentStage = currentStage;
    }

    private void UpdateMaterial()
    {
        print("trying");
        tempMaterial.Lerp(materials[0], materials[1], CurrentStage);
        targetMesh.material = tempMaterial;
    }
}

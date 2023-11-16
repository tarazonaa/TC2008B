/*
Use transformation matrices to modify the vertices of a mesh

Andrés Tarazona
A01023332
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTransforms : MonoBehaviour
{
    [SerializeField] Vector3 displacement;
    [SerializeField] float angle;
    [SerializeField] AXIS rotationAxis;

    Mesh carMesh;
    Vector3[] baseVertices;
    Vector3[] newVertices;

    [SerializeField] GameObject[] wheelPrefabs;
    Mesh wheelMesh;
    Vector3[][] wheelVertices;
    Vector3[][] newWheelVertices;

    void Start()
    {
        carMesh = GetComponentInChildren<MeshFilter>().mesh;
        baseVertices = carMesh.vertices;

        newVertices = new Vector3[baseVertices.Length];
        for (int i = 0; i < baseVertices.Length; i++) {
            newVertices[i] = baseVertices[i];
        }

        wheelVertices = new Vector3[wheelPrefabs.Length][];
        for (int i = 0; i < wheelPrefabs.Length; i++) {
            wheelMesh = wheelPrefabs[i].GetComponentInChildren<MeshFilter>().mesh;
            wheelVertices[i] = wheelMesh.vertices;
        }

        newWheelVertices = new Vector3[wheelPrefabs.Length][];
        for (int i = 0; i < wheelPrefabs.Length; i++) {
            newWheelVertices[i] = new Vector3[wheelVertices[i].Length];
            for (int j = 0; j < wheelVertices[i].Length; j++) {
                newWheelVertices[i][j] = wheelVertices[i][j];
            }
        }
    }

    void Update()
    {
        DoTransform();
    }

    void DoTransform()
    {

        Matrix4x4 move = HW_Transforms.TranslationMat(displacement.x * Time.time, displacement.y * Time.time, displacement.z * Time.time);


        for (int i = 0; i < newVertices.Length; i++) {
            Vector4 temp = new Vector4(baseVertices[i].x, baseVertices[i].y, baseVertices[i].z, 1);
            newVertices[i] = move * temp;
        }
        carMesh.vertices = newVertices;
        carMesh.RecalculateNormals();


        for (int i = 0; i < wheelPrefabs.Length; i++) {

            Matrix4x4 rotate = HW_Transforms.RotateMat(angle * Time.time, rotationAxis);


            for (int j = 0; j < newWheelVertices[i].Length; j++) {
                Vector4 temp = new Vector4(wheelVertices[i][j].x, wheelVertices[i][j].y, wheelVertices[i][j].z, 1);
                newWheelVertices[i][j] = rotate * (move * temp);
            }

            wheelPrefabs[i].GetComponentInChildren<MeshFilter>().mesh.vertices = newWheelVertices[i];
            wheelPrefabs[i].GetComponentInChildren<MeshFilter>().mesh.RecalculateNormals();
        }
    }
}

/* Copyright (C) 2023 NTNU - All Rights Reserved
 * Developer: Jorge Garcia
 * Ask your questions by email: jorgeega@ntnu.no
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This Script plots a simulation of Atoms moving given a .csv file
/// This script should be changed if the data scructure of the .csv file is different
/// Currently we have in the format of (x,y,z) positions of each molecule on one line per mulecule. followed by the same data for the next capture etc 
/// </summary>

public class PlotCSV : MonoBehaviour
{


    public TextAsset csvFile;
    public float timeInterval = 0.2f;
    private int moleculeCount = 0;
    [SerializeField] private bool highlightVibration ;
    [SerializeField] private bool highlightSpeed;
    [SerializeField] private Vector3 multiplier;

    public List<MoleculeType> MoleculePrefabs;

    [SerializeField] private Material neutralMaterial;


    float[] pos = new float[3];
    private string[] records;

    private bool pause = false ; 
    private bool started = false ;

  
    private List<List<float[]>> FloatMatrix = new List<List<float[]>>(); //matrix that holds all the data for each frame
    private List<List<float>> Distances = new List<List<float>>(); //distance traveled by frame <molecule>
    private List<List<float>> IntervalDistance = new List<List<float>>(); //average distance every n frames
    
    
    //holds a game object per molecule in the same order as the data
    private List<Molecule> Molecules = new List<Molecule>();     
    
    void Start()
    {
       
       //NOTEL: as we get bigger simulations having a file for  each molecule type may be needed

        //// Splitting the dataset in the end of line
        records = csvFile.text.Split('\n');

        //initialize prefabs 
        foreach (MoleculeType type in MoleculePrefabs)
        {            
            GameObject atomCopy = (GameObject)Instantiate(type.element, new Vector3(0, 0, 0), Quaternion.identity);
            if (highlightVibration) ChangeMat(atomCopy, neutralMaterial); //change the color of the instance 

            moleculeCount += type.ammount;
            for (int i = 0; i < type.ammount; i++)
            {
                GameObject mol1 = (GameObject)Instantiate(atomCopy, new Vector3(0, 0, 0), Quaternion.identity);
                Molecules.Add(new Molecule (mol1, type.maxVibration));

            }
        }
        //number of captures aka frames
        int frames = records.Length / moleculeCount;
      
        //adds a List to the matrix for each capture , this will be populated by AssignData (AKA intizialising lists )
        for (int i = 0; i < frames; i++)
        {
            FloatMatrix.Add(new List<float[]>());
            Distances.Add(new List<float>());
            IntervalDistance.Add(new List<float>());
        }



        AssignData();

        Debug.Log("prefabs" + Molecules.Count);
        Debug.Log("frames" + frames);

        StartAnimation();

    }

    public virtual void ChangeMat(GameObject ob, Material newMat)
    {
        ob.GetComponent<Renderer>().material = newMat;
    }

    public void AssignData()
    {


        //go throw each line
        for (int i = 0; i < records.Length - 1; i++)
        {
            //  adding x y z to positions
            //NOTE: when getting more colums they should be added here
            float[] atomData = new float[5];
            atomData[0] = float.Parse(records[i].Split(',')[0]);
            atomData[1] = float.Parse(records[i].Split(',')[1]);
            atomData[2] = float.Parse(records[i].Split(',')[2]);
            atomData[3] = float.Parse(records[i].Split(',')[3]); //vibration A.K.A heat

            atomData[4] = float.Parse(records[i].Split(',')[6]); // highlight the particularly speedy ones



            //index of wich frame we are on
            int index = (int)Mathf.Floor(i / moleculeCount);

            // ading the data to the matrix
            FloatMatrix[index].Add(atomData);

          /**
            NOW we have its own data colum from matlab
            //asign distances after the first frame
            if (index > 0) {

                float[] prev = new float[3];
                prev[0] = float.Parse(records[i - 1].Split(',')[0]);
                prev[1] = float.Parse(records[i - 1].Split(',')[1]);
                prev[2] = float.Parse(records[i - 1].Split(',')[2]);

                float distance = Mathf.Abs(positions[0] - prev[0]) + Mathf.Abs(positions[1] - prev[1]) + Mathf.Abs(positions[2] - prev[2]);
                Distances[index].Add(distance);
            }
            else Distances[index].Add(0); //asign distance 0 to the first frame

            **/            
        }

        // check to vefiry the data is well process
        Debug.Log("counted frames:  " + FloatMatrix.Count);
        Debug.Log("counted molecule pos:  " + FloatMatrix[0].Count);

    }

    public void CalcMaxVibration (List<List<float[]>> data , int colum)
    {
        for (int i = 0; i < FloatMatrix.Count; i++)
        {
            for (int j = 0; j < moleculeCount; j++)
            {
                
            }
        }
    }

    
    public IEnumerator ANimateMolecules()
    {
        for (int i = 0; i < FloatMatrix.Count; i++)// goes throw all the frames 
        {

           
            //postion all molecules ;
            for (int j=0; j< moleculeCount; j++)
            {
                
                Molecules[j].gameobject.transform.position = new Vector3(FloatMatrix[i][j][0] * multiplier.x, FloatMatrix[i][j][1] * multiplier.y, FloatMatrix[i][j][2] * multiplier.z);
                Molecules[j].doesGlow = ((int)FloatMatrix[i][j][4] == 1);
               
                //change the color based on vibration
                if (highlightVibration)
                {
                    Renderer renderer = Molecules[j].gameobject.GetComponent<Renderer>();
                    Material uniqueMaterial = renderer.material;
                    uniqueMaterial.EnableKeyword("_EMISSION");
                    uniqueMaterial.SetColor("_EmissionColor", new Color(0, FloatMatrix[i][j][3] / Molecules[j].maxVibration, 0, 1)); //giving green emission based on atom vibration
                }else if (highlightSpeed )
                {
                    Renderer renderer = Molecules[j].gameobject.GetComponent<Renderer>();
                    Material uniqueMaterial = renderer.material;
                    uniqueMaterial.EnableKeyword("_EMISSION");
                    if (Molecules[j].doesGlow)
                    {
                        uniqueMaterial.SetColor("_EmissionColor", new Color(1, 1, 1, 1)); //giving green emission based on atom vibration 
                    }else
                    {
                        uniqueMaterial.SetColor("_EmissionColor", new Color(0.02f, 0.02f, 0.02f, 1)); //giving green emission based on atom vibration 
                    }
                 
                }     
           
            }
            
            while (pause == true ){
                yield return null ;
            }
            yield return new WaitForSeconds(timeInterval);

            
        }

    }

    //starts animation or pauses it after started, we call this with the controll right trigger.

    public void StartAnimation (){
        
        if (started==false){
        StartCoroutine(ANimateMolecules());
        started = true;
        } else {
           pause = !pause; 
        }
    }


   


    // MoleculeTYpe (or atom type) stores how many exist on a given simulation and wich prefab is going to represent them

    [System.Serializable]
    public class MoleculeType
    {

        public GameObject element;
        public int ammount;
        public float maxVibration;

        public MoleculeType(GameObject pref, int bul)
        {
            element = pref;
            ammount = bul;
        }
    }

    private class Molecule
    {
        public GameObject gameobject;
        public float maxVibration;
        public bool doesGlow = false;

        public Molecule(GameObject go, float heat)
        {
            gameobject = go;
            maxVibration = heat;
        }
    }

}





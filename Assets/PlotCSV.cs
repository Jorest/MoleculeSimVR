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
    public int moleculeCount = 608;
    public int multiplier = 5;

    public List<MoleculeType> MoleculePrefabs;



    float[] pos = new float[3];
    private string[] records;

    private bool pause = false ; 
    private bool started = false ;

    //matrix that holds all the data for each frame
    private List<List<float[]>> FloatMatrix = new List<List<float[]>>();
    //holds a game object per molecule in the same order as the data
    private List<GameObject> Molecules = new List<GameObject>();     
 
    
    void Start()
    {
       
       //NOTE: as we get bigger simulations having a file for  each molecule type may be needed

        //// Splitting the dataset in the end of line
        records = csvFile.text.Split('\n');
        Debug.Log(records[0]);



        //initialize prefabs 
        foreach (MoleculeType type in MoleculePrefabs)
        {
            for (int i = 0; i < type.ammount; i++)
            {

                GameObject mol1 = (GameObject)Instantiate(type.element, new Vector3(0, 0, 0), Quaternion.identity);
                Molecules.Add(mol1);

            }
        }
        //number of captures aka frames
        int frames = records.Length / moleculeCount;
      
        //adds a List to the matrix for each capture , this will be populated by AssignData
        for (int i = 0; i < frames; i++)
        {
            FloatMatrix.Add(new List<float[]>());
        }


        Debug.Log("prefabs" + Molecules.Count);
        Debug.Log("frames" + frames);
       

    }

    public void AssignData()
    {


        //go throw each line
        for ( int i=0; i< records.Length-1;i++)
        {
         //  adding x y z to positions
         //NOTE: when getting more colums they should be added here
            float[] positions = new float[3];
            positions[0] = float.Parse(records[i].Split(',')[0]);
            positions[1] = float.Parse(records[i].Split(',')[1]);
            positions[2] = float.Parse(records[i].Split(',')[2]);
            //index of wich frame we are on
            int index = (int)Mathf.Floor  ( i/moleculeCount );
     
            // ading the data to the matrix
            FloatMatrix[index].Add(positions); 


        }


        // check to vefiry the data is well process
        Debug.Log("counted frames:  " + FloatMatrix.Count);
        Debug.Log("counted molecule pos:  " + FloatMatrix[0].Count);

    }

    public IEnumerator ANimateMolecules()
    {
        for (int i = 0; i < FloatMatrix.Count-1; i++)// goes throw all the frames 
        {

           
            //postion all molecules ;
            for (int j=0; j< moleculeCount; j++)
            {
                

                Molecules[j].transform.position = new Vector3(FloatMatrix[i][j][0] * multiplier, FloatMatrix[i][j][1] * multiplier, FloatMatrix[i][j][2]) * multiplier;
            }
            

            while (pause == true ){
                yield return null ;
            }
            yield return new WaitForSeconds(timeInterval);

            
        }

    }

    //starts animation or pauses it after started

    public void StartAnimation (){
        
        if (started==false){
            AssignData();
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


        public MoleculeType(GameObject pref, int bul)
        {
            element = pref;
            ammount = bul;
        }
    }
}





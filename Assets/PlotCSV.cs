using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlotCSV : MonoBehaviour
{


    public TextAsset csvFile;
    public float timeInterval = 0.2f;
    public int moleculeCount = 608;
    public int multiplier = 5;

    public List<MoleculeType> MoleculePrefabs;



    float[] pos = new float[3];
    private string[] records;

    private List<List<float[]>> FloatMatrix = new List<List<float[]>>();
    private List<GameObject> Molecules = new List<GameObject>();


    private float[][][] newMatrix = new float[10][][];
    void Start()
    {
        //different ways of loading the data
        // var dataset = Resources.Load<TextAsset>(path);
        //  var data = System.IO.File.ReadAllText(path);

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

  

        int frames = records.Length / moleculeCount;
      

        for (int i = 0; i < frames; i++)
        {
            FloatMatrix.Add(new List<float[]>());
        }


        Debug.Log("prefabs" + Molecules.Count);
        Debug.Log("frames" + frames);
        AssignData();
        StartCoroutine(ANimateMolecules());



    }

    public void AssignData()
    {


        //go throw each line
        for ( int i=0; i< records.Length-1;i++)
        {
         //   MatrixList.Add(new List<List<float>>() ); //one per each second
    
            float[] positions = new float[3];
            positions[0] = float.Parse(records[i].Split(',')[0]);
            positions[1] = float.Parse(records[i].Split(',')[1]);
            positions[2] = float.Parse(records[i].Split(',')[2]);
            int index = (int)Mathf.Floor  ( i/moleculeCount );
         //   Debug.Log("index:"+ index);
            // ading the data to the matrix
            FloatMatrix[index].Add(positions);


        }

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
            


            yield return new WaitForSeconds(timeInterval);
        }

    }




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





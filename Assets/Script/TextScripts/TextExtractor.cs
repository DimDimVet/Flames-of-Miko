using System.IO;
using UnityEngine;

public class TextExtractor : MonoBehaviour
{ 
    private JsonFileTrainingKeys _trainingJson;
    private JsonFileEndingKeys _endingJson;
    private string _trainingPath;
    private string _endingPath;

    public JsonFileTrainingKeys TrainingJson 
    { 
        get { return _trainingJson; }
        private set { _trainingJson = value; }
    }

    public JsonFileEndingKeys EndingJson
    {
        get { return _endingJson; }
        private set { _endingJson = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Preparing();

        //Пример
        Debug.Log(TrainingJson.training_dialogue_1);
        Debug.Log(EndingJson.ending_dialogue_2);
    }    

    // Update is called once per frame
    void Update()
    {
       
    }

    private void Preparing()
    {
        _trainingPath = Application.streamingAssetsPath + "/" + "TrainingDialogs.json";
        TrainingJson = JsonUtility.FromJson<JsonFileTrainingKeys>(File.ReadAllText(_trainingPath));
        _endingPath = Application.streamingAssetsPath + "/" + "EndingDialogs.json";
        EndingJson = JsonUtility.FromJson<JsonFileEndingKeys>(File.ReadAllText(_endingPath));
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReplayController : MonoBehaviour {
    [SerializeField] GameObject handPrefab;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject gameEnv;

    public static ReplayController Instance;

    private bool isReplaying;

    private int index1;
    private int index2;

    private float timeValue;

    private Dictionary<string, GameObject> trackedObjects;

    private void Awake() {
        Instance = this;

        trackedObjects = new Dictionary<string, GameObject>();
    }

    public void StartReplay() {
        gameEnv.SetActive(true);
        isReplaying = true;
        timeValue = DataRecorder.Instance.sessionStartTime - 0.5f;
        print("Starting replay from " + timeValue);
    }


    public void StopReplay() {
        gameEnv.SetActive(false);
        isReplaying = false;

        foreach (var kvp in trackedObjects) {
            Destroy(kvp.Value);
        }

        trackedObjects.Clear();
    }

    public void Update() {
        if (isReplaying) {
            timeValue += Time.unscaledDeltaTime;

            GetIndex();
            SetTransform();
        }
    }

    private void GetIndex() {
        var objData = GetObjectData();

        for (int i = 0; i < objData.Count - 2; i++) {
            if (objData[i].timeStamp == timeValue) {
                index1 = i;
                index2 = i;
                return;
            }
            else if (objData[i].timeStamp < timeValue && timeValue < objData[i + 1].timeStamp) {
                index1 = i;
                index2 = i + 1;
                return;
            }
        }

        index1 = objData.Count - 1;
        index2 = objData.Count - 1;
    }

    private List<DataPoint> GetObjectData() {
        return DataRecorder.Instance.data.Values.ToList()[0];
    }


    private void SetTransform() {
        foreach (KeyValuePair<string, List<DataPoint>> kvp in DataRecorder.Instance.data) {

            if (!trackedObjects.ContainsKey(kvp.Key)) {
                print("Creating new " + kvp.Value[0].objectName);
                // Instantiate the object only if it's not already tracked
                switch (kvp.Value[0].objectName) {
                    case "Left Hand":
                        trackedObjects[kvp.Key] = Instantiate(handPrefab);
                        break;
                    case "Right Hand":
                        trackedObjects[kvp.Key] = Instantiate(handPrefab);
                        break;
                    case "OnlineBall(Clone)":
                        trackedObjects[kvp.Key] = Instantiate(ballPrefab);
                        break;
                }
            }

            var trackedObject = trackedObjects[kvp.Key];

            if (index1 == index2) {
                trackedObject.transform.position = DataRecorder.Instance.data[kvp.Key][index1].position;
            }
            else {
                float interpolationFactor = (timeValue - DataRecorder.Instance.data[kvp.Key][index1].timeStamp) / (DataRecorder.Instance.data[kvp.Key][index2].timeStamp - DataRecorder.Instance.data[kvp.Key][index1].timeStamp);

                trackedObject.transform.position = Vector3.Lerp(DataRecorder.Instance.data[kvp.Key][index1].position, DataRecorder.Instance.data[kvp.Key][index2].position, interpolationFactor);
            }
        }
    }
}

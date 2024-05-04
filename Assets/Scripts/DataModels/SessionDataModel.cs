using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SessionDataModel {
    public string _id;
    public float sessionStartTime;
    public string sessionStartDate;
    public float sessionEndTime;
    public string patientName;
    public string doctorName;
    public Dictionary<string, List<DataPoint>> data;

    public SessionDataModel(string _id, float sessionStartTime, float sessionEndTime, string patientName, string doctorName, Dictionary<string, List<DataPoint>> data, string sessionStartDate) {
        this._id = _id;
        this.doctorName = doctorName;
        this.sessionStartTime = sessionStartTime;
        this.sessionEndTime = sessionEndTime;
        this.patientName = patientName;
        this.data = data;
        this.sessionStartDate = sessionStartDate;
    }
}

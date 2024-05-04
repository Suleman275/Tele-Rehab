using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {
    public static TimeManager Instance;

    public float timeSinceApplicationStart;

    public DateTime currentTime;

    private void Awake() {
        Instance = this;

        timeSinceApplicationStart = 0;
    }

    private void Update() {
        timeSinceApplicationStart += Time.unscaledDeltaTime;

        DateTime currentTime = DateTime.Now;
    }
}
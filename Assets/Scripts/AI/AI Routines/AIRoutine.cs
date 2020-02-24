using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRoutine : MonoBehaviour {
    public Clock clock = new Clock();

    //isRunning is ONLY change by the coroutine or Kill().
    public bool isRunning = false;

    public string routineName = "Routine";

    //Coroutines check for forceBreak at the end of their loops. This allows it to finish its current loop and perform side effects.
    public bool forceBreak = false;

    Coroutine routine;

    public IEnumerator routineBase;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        clock.timeElapsed += Time.deltaTime;
    }

    public void Run() {
        if(!isRunning) {
            try {
                routine = StartCoroutine(routineName);
            } catch {
                throw new BadRoutineName("Called routine does not exist.");
            }
        } else {
            return;
        }

        clock.startTime = Time.time;
        clock.timeElapsed = 0f;
    }

    public void Run(IEnumerator routineBase) {
        try {
            routine = StartCoroutine(routineBase);
        } catch {
            throw new BadRoutineName("Called routine does not exist.");
        }

        clock.startTime = Time.time;
        clock.timeElapsed = 0f;
    }

    //Ends the coroutine. forceImmediate kills it immediately
    public void Kill(bool forceImmediate = false) {
        if(isRunning) {
            if(forceImmediate) {
                StopCoroutine(routine);
                isRunning = false;
            } else {
                forceBreak = true;
            }
        }
    }

    public class Clock {
        public Clock() {

        }

        //The time the routine was started (from Time.time)
        public float startTime = 0f;

        //This tells how long since the coroutine was last started (using deltaTime)
        public float timeElapsed = 0f;

        //This tells how long since the coroutine was last started (without using deltaTime)
        public float sinceStart() {
            return Time.time - startTime;
        }
    }
}
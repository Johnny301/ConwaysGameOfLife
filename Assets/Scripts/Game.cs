using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using System;
using System.Collections.Concurrent;

public class Game : MonoBehaviour
{
    public GameObject cellPrefab;
    public int maxCells;
    public Cell[] cells;
    public List<Vector2> cellsToSpawn = new List<Vector2>();
    public ConcurrentDictionary<Vector2, int> timesEmptyCellHasBeenChecked= new ConcurrentDictionary<Vector2, int>();
    private void Start() {
        
        lastRun = tickSpeed;
        Application.runInBackground = true;
        //cameraGameObject.GetComponent<Camera>().enabled = false;
    }

    bool stepOneDone = false;
    bool isPaused = true;
    public Text timeStatus;
    public float tickSpeed;
    float lastRun;
    public Slider loadingBar;
    public float placementDepth;

    public Sprite pause;
    public Sprite play;
    public Image pausePlay;
    bool isPausedByInteracting;
    private void Update() {

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
            isPausedByInteracting = true;
        }
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) {
            isPausedByInteracting = false;
        }

        if (Input.GetMouseButton(0)) { 
            SpawnCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        } else if (Input.GetMouseButton(1)) {
            KillCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetKeyUp(KeyCode.E)) {
            UnityEngine.Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetKeyUp(KeyCode.F)) {
            if (!stepOneDone) {
                CountStep();
                stepOneDone = true;
            } else {
                EvolveStep();
                stepOneDone=false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            if (isPaused) {
                isPaused = false;
                //timeStatus.text = "Playing";
                pausePlay.sprite = play;
            } else {
                isPaused = true;
                pausePlay.sprite = pause;
                //timeStatus.text = "Paused";
            }
        }
        

    }


    IEnumerator CountStep() {
        cells = (from go in GameObject.FindGameObjectsWithTag("Cell") select go.GetComponent<Cell>()).ToArray();
        int counter = 0;
        int maxCounter = maxCells / 700;
        foreach (Cell cell in cells) {
            cell.CountCells();
            if (counter >= maxCounter) {
                yield return null;
                counter = 0;
            }else {
                counter++;
            }
           
        }
        
        UnityEngine.Debug.Log("COUNTED");
    }

    IEnumerator EvolveStep() {
        foreach (Vector2 newCell in (from pos in timesEmptyCellHasBeenChecked where pos.Value == 3 select pos.Key)) {
            try {
                SpawnCell(newCell).GetComponent<Cell>().aliveNeighbors = 3;
            } catch (NullReferenceException) {
                UnityEngine.Debug.LogError("Cooooooooooooooooooookc");
                continue;
            }
            
        }
        yield return null;
        timesEmptyCellHasBeenChecked.Clear();
        cells = (from go in GameObject.FindGameObjectsWithTag("Cell") select go.GetComponent<Cell>()).ToArray();
        foreach (Cell cell in cells) {
            cell.Evolve();
            
        }
        cellsToSpawn = new List<Vector2> ();
     
        UnityEngine.Debug.Log("EVOLVED");

    }
    public GameObject SpawnCell(Vector3 spawnPos) {
        spawnPos = new Vector3(Mathf.Round(spawnPos.x), Mathf.Round(spawnPos.y), placementDepth);
        Collider[] colliders = Physics.OverlapSphere(spawnPos, 0.5f);
        foreach (Collider collider in colliders) {
            if (collider.gameObject.tag == "Cell")
                return null;
        }
        return Instantiate(cellPrefab, spawnPos, Quaternion.Euler(-90f, 0f, 0f), GameObject.FindGameObjectWithTag("CellHolder").transform);


    }
    public void KillCell(Vector3 killPos) {
        killPos = new Vector3(Mathf.Round(killPos.x), Mathf.Round(killPos.y), placementDepth);
        Collider[] colliders = Physics.OverlapSphere(killPos, 0.5f);
        foreach (Collider collider in colliders) {
            if (collider.gameObject.tag == "Cell") {
                Destroy(collider.gameObject);
                return;
            }
                
        }
    }
    private void FixedUpdate() {
        if (!isPaused && lastRun <= 0f && !isPausedByInteracting && !isEvolving) {
            StartCoroutine(Evolution());
        }
        lastRun -= Time.fixedDeltaTime;

    }

    bool isEvolving = false;
    IEnumerator Evolution() {
        UnityEngine.Debug.Log("Got here");
        isEvolving = true;
            Stopwatch sw = Stopwatch.StartNew();
            Stopwatch sw1 = Stopwatch.StartNew();
            yield return CountStep();

            sw1.Stop();
            UnityEngine.Debug.Log($"Counting speed: {sw1.ElapsedMilliseconds}ms");
            sw1 = Stopwatch.StartNew();
            yield return EvolveStep();
            sw1.Stop();
            UnityEngine.Debug.Log($"Evolve speed: {sw1.ElapsedMilliseconds}ms");
            sw.Stop();
            UnityEngine.Debug.Log($"All speed: {sw.ElapsedMilliseconds}ms");
            lastRun = tickSpeed;
        isEvolving = false;
    }
}

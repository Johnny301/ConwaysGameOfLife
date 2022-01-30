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
    /*
    public int size;
    public GameObject cellPrefab;
    public List<Cell> cells = new List<Cell> ();
    private bool isStopped = true;
    public bool IsStopped { get { return isStopped; } set {
            if (value) {
                isStopped = value;
                return;
            }
            
            isStopped = value;
                
        } }


    public Text timeStatus;
    public int ticksPerSecond;
    void Start() {
        Instantiate(cellPrefab, new Vector3(0, 0, 8.5f), Quaternion.Euler(-90f, 0f, 0f));
        StartCoroutine(ConwayGameLoop());   
            }



    bool cellNeighborsUpdated = false;
    IEnumerator UpdateCellNeighbors() {
        
        for(int i = 0; i < cells.Count; i++) {
            Cell cell = cells[i];
            cell.CountAliveNeighbors();
            yield return null;
        }
        cellNeighborsUpdated = true;
    }

    public IEnumerator ConwayGameLoop() {
        UnityEngine.Debug.Log("Started the game");
        Stopwatch sw = new Stopwatch();
        while (true) {
            UnityEngine.Debug.Log($"Number of cells: {cells.Count}");
            sw.Start();
            UnityEngine.Debug.Log("Here1");

            if (isStopped) {
                UnityEngine.Debug.Log("Here2");
                yield return new WaitForSeconds(1f/ticksPerSecond);
                continue;
            }
            UnityEngine.Debug.Log("Here3");

            StartCoroutine(UpdateCellNeighbors());
            while (!cellNeighborsUpdated) {
                UnityEngine.Debug.Log("Here4");

                yield return new WaitForSeconds(0.01f);
                continue;
            }
            UnityEngine.Debug.Log("Here5");

            StartCoroutine(EvolveCells());
            UnityEngine.Debug.Log("Here6");

            cellNeighborsUpdated = false;
            sw.Stop();
            UnityEngine.Debug.Log("Here7");

            yield return new WaitForSeconds(((1000f / (ticksPerSecond * 1000f)) - sw.ElapsedMilliseconds)/1000f);

            UnityEngine.Debug.Log("Looped");
        }
    }

    public IEnumerator EvolveCells() {
        for (int i = 0; i < cells.Count; i++) {
            Cell cell = cells[i];
            cell.Evolve();
            yield return null;
        }


    }

    
    public void Update() {
        if (Input.GetButtonUp("Jump")) {
            UnityEngine.Debug.Log("Jumped");
            
            IsStopped = !IsStopped;
            if (IsStopped) {
                timeStatus.text = "Paused";
            } else if (!IsStopped) {
                timeStatus.text = "Playing";
            }
            
        }
        if (Input.GetKeyUp(KeyCode.F)) {
            UnityEngine.Debug.Log("EVOLVE!!!!!");
            StartCoroutine(ConwayGameLoop());
        }
    }
    */
    public GameObject cellPrefab;
    public int maxCells;
    public Cell[] cells;
    public List<Vector2> cellsToSpawn = new List<Vector2>();
    public ConcurrentDictionary<Vector2, int> timesEmptyCellHasBeenChecked= new ConcurrentDictionary<Vector2, int>();
    private void Start() {
        
        lastRun = tickSpeed;
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


    void CountStep() {
        cells = (from go in GameObject.FindGameObjectsWithTag("Cell") select go.GetComponent<Cell>()).ToArray();

        foreach (Cell cell in cells) {
            cell.CountCells();
        }
        
        UnityEngine.Debug.Log("COUNTED");
    }

    void EvolveStep() {
        /*
        foreach (Vector2 newCellPos in cellsToSpawn.ToArray()) {
            try {
                SpawnCell(newCellPos).GetComponent<Cell>().aliveNeighbors = 3;
            } catch (NullReferenceException) {
                continue;
            }
        }*/
        foreach (Vector2 newCell in (from pos in timesEmptyCellHasBeenChecked where pos.Value == 3 select pos.Key)) {
            try {
                SpawnCell(newCell).GetComponent<Cell>().aliveNeighbors = 3;
            } catch (NullReferenceException) {
                UnityEngine.Debug.LogError("Cooooooooooooooooooookc");
                continue;
            }
        
        }
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
            Evolution();
        }
        lastRun -= Time.fixedDeltaTime;

    }

    bool isEvolving = false;
    void Evolution() {
        isEvolving = true;
            Stopwatch sw = Stopwatch.StartNew();
            Stopwatch sw1 = Stopwatch.StartNew();
            CountStep();

            sw1.Stop();
            UnityEngine.Debug.Log($"Counting speed: {sw1.ElapsedMilliseconds}ms");
            sw1 = Stopwatch.StartNew();
            EvolveStep();
            sw1.Stop();
            UnityEngine.Debug.Log($"Evolve speed: {sw1.ElapsedMilliseconds}ms");
            sw.Stop();
            UnityEngine.Debug.Log($"All speed: {sw.ElapsedMilliseconds}ms");
            lastRun = tickSpeed;
        isEvolving = false;
    }
}

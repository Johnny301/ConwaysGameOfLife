using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{

    /*
    public Material aliveMat;
    public Material deadMat;
    public GameObject cellPrefab;
    public Cell cell = null;
    private Game game;
    public int aliveNeighbors;
    // Start is called before the first frame update
    void Start()
    {
        game = Camera.main.gameObject.GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateCell() { 
        gameObject.AddComponent<Cell>();
        cell = gameObject.GetComponent<Cell>();
        cell.aliveMat = aliveMat;
        cell.deadMat = deadMat;
        
    }
    void KillCell() {
        Destroy(cell);
        cell = null;
    }
    void OnMouseOver() {

        if (Input.GetButtonUp("Fire1")) {
            if (cell == null) { 
                CreateCell(); 
            } else if (cell != null) {
                KillCell();
            }
            
        }
        if (Input.GetButtonUp("Fire2")) {
            CountAliveNeighbors();
            Debug.Log($"{gameObject.name}'s neighbors: {aliveNeighbors}");
        }
    }


    public void CountAliveNeighbors() {
        int neighbors = 0;

        for (float x = -1f; x <= 1f; x += 1f) {
            for (float y = -1f; y <= 1f; y += 1f) {
                if (x == 0 && y == 0)
                    continue;

                Collider[] colliders = Physics.OverlapSphere(new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z), 0.1f);
                foreach (Collider collider in colliders) {
                    if (collider.gameObject.layer == 6) {
                        Cell cell = collider.gameObject.GetComponent<Cell>();
                        if (cell != null && cell.IsAlive) {
                            neighbors++;
                        }
                    }
                }
            }
        }

        if (cell != null) {
            aliveNeighbors = neighbors;
        }

    }
    private void OnBecameVisible() {
        GenerateAround();

    }
    void GenerateAround() {
        for (float x = -1f; x <= 1f; x += 1f) {
            for (float y = -1f; y <= 1f; y += 1f) {
                Collider[] colliders = Physics.OverlapSphere(new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z), 0.1f);
                bool skip = false;
                foreach (Collider collider in colliders) {
                    if (collider.gameObject.layer == 6) {
                        skip = true;
                    }
                }
                if (skip) {
                    continue;
                }
                if (game.cells.Count > Mathf.Pow(game.size, 2))
                    return;
                Instantiate(cellPrefab, new Vector3(transform.position.x + x, transform.position.y + y, 8.5f), Quaternion.Euler(-90f, 0f, 0f));
            }
        }
    }
    */
    

}

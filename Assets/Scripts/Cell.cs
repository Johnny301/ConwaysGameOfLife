using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System;

public class Cell : MonoBehaviour
{
    public int stepsAlive = 0;
    public int aliveNeighbors = 0;
    Game game;
    MeshRenderer cellRenderer;
    public Material aliveMat;
    public Material oldMat;
    
    private void Start() {
        game = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Game>();
        cellRenderer = gameObject.GetComponent<MeshRenderer>();

    }
    public void CountCells() {
        
        aliveNeighbors = CountCellsAround(transform.position);
        //CountCellsAroundNeighbors(gameObject.transform.position);
        
    }
    public void Evolve() {
        if (aliveNeighbors != 2 && aliveNeighbors != 3) {
            game.KillCell(transform.position);
        } else {
            stepsAlive++;
            if (cellRenderer == null) {
                cellRenderer = gameObject.GetComponent<MeshRenderer>();
            }
            cellRenderer.material.Lerp(aliveMat, oldMat, (float)stepsAlive/10);
        }
    }
    public int CountCellsAround(Vector2 pos) {
        int neighbors = 0;
        Vector2[] offsets = new Vector2[] {
            new Vector2(-1, -1),
            new Vector2(-1, 0),
            new Vector2(-1, 1),
            new Vector2(0, -1),
            new Vector2(0, 1),
            new Vector2(1, -1),
            new Vector2(1, 0),
            new Vector2(1, 1)
        };

        foreach (Vector2 offset in offsets) {

            Vector3 checkPos = new Vector3(pos.x + offset.x, pos.y + offset.y, game.placementDepth);
            if (ContainsCell(checkPos)) {
                neighbors++;
            } else {
                if (!game.timesEmptyCellHasBeenChecked.ContainsKey(checkPos))
                    game.timesEmptyCellHasBeenChecked[checkPos] = 1;
                else
                    game.timesEmptyCellHasBeenChecked[checkPos]++;
            }

        }
        
        return neighbors;
    }

    public bool hasBeenCounted = false;
    bool ContainsCell(Vector3 pos) {
        Collider[] colliders = Physics.OverlapSphere(pos, 0.5f);
        foreach (Collider collider in colliders) {
            if (collider.gameObject.tag == "Cell") {
                return true;
            }
        }
        return false;
    }
   
    void OnMouseOver() {
        if (Input.GetKeyUp(KeyCode.Q)) {
            UnityEngine.Debug.Log($"{gameObject.name}'s neighbors: {CountCellsAround(transform.position)}");
        }
    }
    
}

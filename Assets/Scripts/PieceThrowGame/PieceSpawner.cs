using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField]
    Piece electricityPrefab;
    [SerializeField]
    Piece gasPrefab;
    [SerializeField]
    Piece piecesPrefab;
    [SerializeField]
    Vector2 spawnAreaExtends;
    [SerializeField]
    float minimumSeparation;

    bool electricityOut;
    bool gasOut;
    bool piecesOut;

    Vector2[] lastSpawns = new Vector2[2];

    

    // Start is called before the first frame update
    void Start()
    {
        ResourceManager.OnResourceChange += OnResourceChange;
        SpawnElectricity();
        SpawnGas();
        SpawnPieces();
    }

    void OnResourceChange(ResourceType rt, float amount)
    {
        if (amount < 1)
        {
            return;
        }
        if(rt == ResourceType.electricity)
        {
            SpawnElectricity();
        }else if(rt == ResourceType.gas)
        {
            SpawnGas();
        }else if(rt==ResourceType.pieces)
        {
            SpawnPieces();
        }
    }

    public void SpawnElectricity()
    {
        ResourceVector cost = new ResourceVector(1, 0, 0);
        if (electricityOut || !ResourceManager.Instance.CheckSufficieny(cost))
        {
            return;
        }
        electricityOut = true;
        ResourceManager.Instance.RemoveResources(cost);
        Piece newPiece = Instantiate(electricityPrefab, transform.position + (Vector3)ValidSpawn(), Quaternion.identity);
        newPiece.OnPieceDestroyed += () => electricityOut = false;
        newPiece.OnPieceDestroyed += SpawnElectricity;
    }

    public void SpawnGas()
    {
        ResourceVector cost = new ResourceVector(0, 1, 0);
        if (gasOut || !ResourceManager.Instance.CheckSufficieny(cost))
        {
            return;
        }
        gasOut = true;
        ResourceManager.Instance.RemoveResources(cost);
        Piece newPiece = Instantiate(gasPrefab, transform.position + (Vector3)ValidSpawn(), Quaternion.identity);
        newPiece.OnPieceDestroyed += () => gasOut = false;
        newPiece.OnPieceDestroyed += SpawnGas;
    }

    public void SpawnPieces()
    {
        ResourceVector cost = new ResourceVector(0, 0, 1);
        if (piecesOut || !ResourceManager.Instance.CheckSufficieny(cost))
        {
            return;
        }
        piecesOut = true;
        ResourceManager.Instance.RemoveResources(cost);
        Piece newPiece = Instantiate(piecesPrefab, transform.position+(Vector3)ValidSpawn(), Quaternion.identity);
        newPiece.OnPieceDestroyed += () => piecesOut = false;
        newPiece.OnPieceDestroyed += SpawnPieces;
    }

    Vector2 ValidSpawn()
    {
        Vector2 spawn = new Vector2();
        do
        {
            spawn.x = Random.Range(-spawnAreaExtends.x, spawnAreaExtends.x);
            spawn.y = Random.Range(-spawnAreaExtends.y, spawnAreaExtends.y);

        } while ((spawn - lastSpawns[0]).magnitude < minimumSeparation || (spawn - lastSpawns[1]).magnitude < minimumSeparation);
        lastSpawns[1] = lastSpawns[0];
        lastSpawns[0] = spawn;
        return spawn;
    }

    void OnDestroy()
    {
        ResourceManager.OnResourceChange -= OnResourceChange;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnAreaExtends * 2);
    }
}

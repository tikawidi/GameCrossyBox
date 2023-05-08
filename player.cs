using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class player : MonoBehaviour
{
    [SerializeField] penyihir Penyihir;
    [SerializeField] List<Terrain> terrainList;
    [SerializeField] int initialGrassCount = 5;
    [SerializeField] int horizontalSize;
    [SerializeField, Range(0, 1)] float treeprobability;
    [SerializeField] int backViewDistance = -5;
    [SerializeField] int forwardViewDistance = 15;

    Dictionary<int, Terrain> activeTerrain = new Dictionary<int, Terrain>(20);
    [SerializeField] private int travelDistance;

    public UnityEvent<int, int> OnUpdateTerrainLimit;

    private void Start()
    {
        

        // create initial Grass
        for (int zpos = backViewDistance; zpos< initialGrassCount; zpos++)
        {
            
            var terrain  = Instantiate(terrainList[0]);
            terrain.transform.position = new Vector3(0, 0, zpos);
            if (terrain is grass grass)
            {
                grass.SetTreePercentage(zpos < -1 ? 1 : 0);

            }
            
            terrain.Generate(horizontalSize);
            activeTerrain[zpos] = terrain;
        }

        //create initial 
        for (int zpos = initialGrassCount; zpos < forwardViewDistance; zpos++)
        {
            SpawnRandomTerrain(zpos);
           // var terrain = SpawnRandomTerrain(zpos);
            //terrain.Generate(horizontalSize);
            //activeTerrain[zpos] = terrain;
        }
        OnUpdateTerrainLimit.Invoke(horizontalSize, travelDistance + backViewDistance);
        

       
    }

    private Terrain SpawnRandomTerrain(int zpos)
    {
        //throw new System.NotImplementedException();
        Terrain terrainCheck = null;
        int randomIndex;
        for (int z = -1; z >= -3; z--)
        {
            var checkPos = zpos +  z;
            // System.Type checkType = terrainCheck.GetType();
            //System.Type aktiveType = activeTerrain[checkPos].GetType();

            if (terrainCheck == null)
            {
                terrainCheck = activeTerrain[checkPos];
                continue;
            }
            else if (terrainCheck.GetType() != activeTerrain[checkPos].GetType())
            {
                randomIndex = Random.Range(0, terrainList.Count);
                return SpawnTerrain(terrainList[randomIndex], zpos);
            }
            else
            {
                continue;
            }
        }

        var CandidateTerrain = new List<Terrain>(terrainList);
        for (int i = 0; i < CandidateTerrain.Count; i++)
        {
            //System.Type checkType = terrainCheck.GetType();
            //System.Type aktiveType = CandidateTerrain[i].GetType();

            if (terrainCheck.GetType() == CandidateTerrain[i].GetType())
            {
                CandidateTerrain.Remove(CandidateTerrain[i]);
                break;
            }
        }

        randomIndex = Random.Range(0, CandidateTerrain.Count);
        return SpawnTerrain(CandidateTerrain[randomIndex], zpos);
    }
    public Terrain SpawnTerrain(Terrain terrain, int zpos)
    {
        terrain = Instantiate(terrain);
        terrain.transform.position = new Vector3(0, 0, zpos);
        terrain.Generate(horizontalSize);
        activeTerrain[zpos] = terrain;
        return terrain;
    }
    public void UpdateTravelDistance(Vector3 targetposition)
    {
        if( targetposition.z > travelDistance)
        {
            travelDistance = Mathf.CeilToInt(targetposition.z);
            UpdateTerrain();
        }
    }

    public void UpdateTerrain()
    {
        var destroyPos = travelDistance - 1 + backViewDistance;
        Destroy(activeTerrain[destroyPos].gameObject);
        activeTerrain.Remove(destroyPos);
        var spawnPosition = travelDistance - 1 + forwardViewDistance;
        SpawnRandomTerrain(spawnPosition);

        OnUpdateTerrainLimit.Invoke(horizontalSize, travelDistance + backViewDistance);
        
       // SpawnRandomTerrain(travelDistance - 1 +forwardViewDistance);
    }

}

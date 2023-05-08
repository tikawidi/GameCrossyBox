using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grass : Terrain
{
    [SerializeField] List<GameObject> treePerfablist;
    [SerializeField, Range(0, 1)] float treeProbability;

    public void SetTreePercentage(float newProbability)
    {
        this.treeProbability = Mathf.Clamp01(newProbability);
    }
    public override void Generate(int size)
    {
        base.Generate(size);

        var limit = Mathf.FloorToInt((float)size / 2);
        var treeCount = Mathf.FloorToInt((float)size * treeProbability);

        List<int> emptyPosition = new List<int>();
        for (int i = -limit; i <= limit; i++)
        {
            emptyPosition.Add(i);
        }

        for (int i = 0; i < treeCount; i++)
        {
            //memilih posisi kosong secara random
            var randomIndex = Random.Range(0, emptyPosition.Count);
            var pos = emptyPosition[randomIndex];

            //posisi yang terpilih hapus dari daftar posisi kosing
            emptyPosition.RemoveAt(randomIndex);
            SpawnRandomTree(pos);

        }
        //Debug.Log(string.Join(",", emptyPosition));
        //pohon diujung 
        SpawnRandomTree(-limit - 1);
        SpawnRandomTree(limit + 1);
    }
    private void SpawnRandomTree(int pos)
    {
        // set pohon ke posisi yang terpilih
        var randomIndex = Random.Range(0, treePerfablist.Count);
        var prefab = treePerfablist[randomIndex];

        //pilih prefab pohon secara random
        var tree = Instantiate(prefab, transform);
        tree.transform.localPosition = new Vector3(pos, 0, 0);
    }

}

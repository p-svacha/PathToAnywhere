using MapGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorModel : MonoBehaviour
{
    private FiniteMapGenerator MapEditor;


    // Start is called before the first frame update
    void Start()
    {
        MapEditor = new FiniteMapGenerator();
        MapEditor.GenerateMap(new MapGeneration.Finite.FiniteMapGenerationSettings(300, 300));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

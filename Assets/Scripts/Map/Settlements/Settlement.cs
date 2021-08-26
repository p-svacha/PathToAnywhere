using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settlement
{
    private GameModel Model;
    public string Name;
    public List<Structure> Structures;

    public Settlement(GameModel model, string name, List<Structure> structures)
    {
        Model = model;
        Name = name;
        Structures = structures;
        foreach (Structure s in structures) s.Settlement = this;
    }

    public void PlaceStructures()
    {
        foreach (Structure s in Structures) s.PlaceStructure(Model);
    }
}

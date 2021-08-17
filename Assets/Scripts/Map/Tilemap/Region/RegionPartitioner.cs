using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// This class is responsible for creating the region Tilemap by dividing the map into different regions.
/// The region voronoi is a voronoi pattern with big regions that rougly defines the edges of a region.
/// The parcel voronoi is a voronoi pattern with small regions that define the exact edges of the regions.
/// Each region consists of multiple adjacent parcels. This is done the following way:
/// 1. For each tile we find the nearest parcel voronoi point.
/// 2. Then we check the nearest region voronoi point for the found point in step 1.
/// 3. That point is the position Id for the region. Each region has this unique id
/// </summary>
public class RegionPartitioner
{
    private GameModel Model;
    private TilemapGenerator Generator;
    private MapHash RegionVoronoiPointHash;
    private MapHash ParcelVoronoiPointHashSmall;
    private int Resolution = 4;

    public Dictionary<Vector2Int, Region> Regions = new Dictionary<Vector2Int, Region>();

    private Dictionary<RegionType, int> RegionTable = new Dictionary<RegionType, int>()
    {
        {RegionType.Grassland, 150 },
        {RegionType.Desert, 70 },
        {RegionType.Mountain, 100 },
        {RegionType.Ruins, 50 },
        {RegionType.Lake, 100 },
    };

    public RegionPartitioner(GameModel model)
    {
        Model = model;
        Generator = model.TilemapGenerator;
        RegionVoronoiPointHash = new MapHash(256, 10);
        ParcelVoronoiPointHashSmall = new MapHash(16, 40); // 1 in 16 tiles is a voronoi point
    }

    public Region GetRegionAt(Vector2Int gridPosition)
    {
        Vector2Int parcelId = GetNearestVoronoiPoint(ParcelVoronoiPointHashSmall, gridPosition, 10);
        Vector2Int regionId = GetNearestVoronoiPoint(RegionVoronoiPointHash, parcelId, 30);

        Region region;
        Regions.TryGetValue(regionId, out region);
        if (region != null) return region;
        else
        {
            Region newRegion = GetRandomRegion(regionId);
            Regions.Add(regionId, newRegion);
            return newRegion;
        }
    }

    private Vector2Int GetNearestVoronoiPoint(MapHash voronoiHash, Vector2Int gridPosition, int range)
    {
        Vector2Int scaledPosition2D = new Vector2Int(gridPosition.x / Resolution, gridPosition.y / Resolution);

        List<Vector2Int> voronoiPoints = GetVoronoiPointsAround(voronoiHash, scaledPosition2D, range);
        if (voronoiPoints.Count == 0) Debug.Log("Range too little");

        Vector2Int nearestPoint = new Vector2Int(0, 0);
        float nearestDistance = float.MaxValue;
        foreach (Vector2Int vPoint in voronoiPoints)
        {
            float distance = Vector2.Distance(vPoint, gridPosition);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPoint = vPoint;
            }
        }

        return nearestPoint;
    }

    private List<Vector2Int> GetVoronoiPointsAround(MapHash voronoiHash, Vector2Int position, int range)
    {
        List<Vector2Int> points = new List<Vector2Int>();
        for (int y = -range; y < range + 1; y++)
        {
            for (int x = -range; x < range + 1; x++)
            {
                int worldX = (int)position.x + x;
                int worldY = (int)position.y + y;
                int hashedValue = voronoiHash.GetHashedValue(worldX, worldY);
                if (hashedValue == 0) points.Add(new Vector2Int(worldX * Resolution, worldY * Resolution));
            }
        }
        return points;
    }

    protected T GetWeightedRandomEnum<T>(Dictionary<T, int> weightDictionary) where T : System.Enum
    {
        int probabilitySum = weightDictionary.Sum(x => x.Value);
        int rng = Random.Range(0, probabilitySum);
        int tmpSum = 0;
        foreach (KeyValuePair<T, int> kvp in weightDictionary)
        {
            tmpSum += kvp.Value;
            if (rng < tmpSum) return kvp.Key;
        }
        throw new System.Exception();
    }

    private Region GetRandomRegion(Vector2Int id)
    {
        RegionType type = GetWeightedRandomEnum(RegionTable);
        switch(type)
        {
            case RegionType.Grassland:
                return new Region_Grassland(Model, id);
            case RegionType.Desert:
                return new Region_Desert(Model, id);
            case RegionType.Mountain:
                return new Regions_Mountains(Model, id);
            case RegionType.Ruins:
                return new Region_Ruins(Model, id);
            case RegionType.Lake:
                return new Region_Lake(Model, id);
            default:
                throw new System.Exception("Region type not handled");
        }
    }

}
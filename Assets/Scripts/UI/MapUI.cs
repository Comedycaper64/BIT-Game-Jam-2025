using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    private bool mapActive = false;
    private int mapWidth = 5;
    private int mapHeight = 5;
    private float worldMapDiameter = 50f;
    private float playerFlashTime = 1f;
    private float playerFlashTimeRatio = 0.5f;
    private MapTile playerMapTile;
    private MapTile[,] mapTileGrid;
    private List<MapTile> lightPlantMapTiles;
    private Coroutine mapFlashCoroutine;

    [SerializeField]
    private Sprite[] plantSprites;

    [SerializeField]
    private Sprite plantBrokenSprite;

    [SerializeField]
    private MapTile mapTilePrefab;

    [SerializeField]
    private Transform mapGridTransform;

    [SerializeField]
    private LightPlant[] lightPlants;

    private void Start()
    {
        SetupMap();
    }

    private void Update()
    {
        if (!mapActive || !PlayerIdentifier.PlayerTransform)
        {
            return;
        }

        UpdatePlayerLocation(PlayerIdentifier.PlayerTransform.position);
        UpdatePlantSprites();
    }

    private void OnEnable()
    {
        LightManager.OnLightLevelChange += ChangeMapColour;
    }

    private void OnDisable()
    {
        if (mapFlashCoroutine != null)
        {
            StopCoroutine(mapFlashCoroutine);
        }

        LightManager.OnLightLevelChange -= ChangeMapColour;
    }

    private void ChangeMapColour(object sender, Color newColour)
    {
        foreach (MapTile tile in mapTileGrid)
        {
            tile.SetColour(newColour);
        }
    }

    private void SetupMap()
    {
        lightPlantMapTiles = new List<MapTile>();
        mapTileGrid = new MapTile[mapWidth, mapHeight];

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                mapTileGrid[i, j] = Instantiate(mapTilePrefab, mapGridTransform);
            }
        }

        foreach (LightPlant plant in lightPlants)
        {
            Vector2 plantPosition = plant.transform.position;
            Tuple<int, int> plantMapPosition = RemapPositionToMap(plantPosition);
            lightPlantMapTiles.Add(mapTileGrid[plantMapPosition.Item1, plantMapPosition.Item2]);
        }

        foreach (MapTile plantTile in lightPlantMapTiles)
        {
            plantTile.SetSprite(plantSprites[0]);
        }

        mapActive = true;
        StartCoroutine(PlayerMapFlash());
    }

    private void UpdatePlayerLocation(Vector2 playerPosition)
    {
        Tuple<int, int> playerMapPosition = RemapPositionToMap(playerPosition);
        MapTile mapTile = mapTileGrid[playerMapPosition.Item1, playerMapPosition.Item2];

        if (mapTile != playerMapTile)
        {
            if (playerMapTile != null)
            {
                playerMapTile.ToggleTileVisual(true);
            }

            playerMapTile = mapTile;
        }
    }

    private void UpdatePlantSprites()
    {
        for (int i = 0; i < lightPlants.Length; i++)
        {
            int lightPoints = lightPlants[i].GetLightPoints();

            if (lightPoints == -1)
            {
                lightPlantMapTiles[i].SetSprite(plantBrokenSprite);
            }
            else
            {
                lightPlantMapTiles[i].SetSprite(plantSprites[lightPoints]);
            }
        }
    }

    private Tuple<int, int> RemapPositionToMap(Vector2 worldPosition)
    {
        float mapRadius = worldMapDiameter / 2f;
        float xRemap = Mathf.Clamp(
            math.remap(-mapRadius, mapRadius, 0f, mapWidth, worldPosition.x),
            0f,
            mapWidth - 0.1f
        );
        float yRemap = Mathf.Clamp(
            math.remap(-mapRadius, mapRadius, 0f, mapHeight, worldPosition.y),
            0f,
            mapHeight - 0.1f
        );

        return new Tuple<int, int>(Mathf.FloorToInt(xRemap), Mathf.FloorToInt(yRemap));
    }

    private IEnumerator PlayerMapFlash()
    {
        yield return new WaitForSeconds(playerFlashTime * playerFlashTimeRatio);

        if (playerMapTile)
        {
            playerMapTile.ToggleTileVisual(false);
        }

        yield return new WaitForSeconds(playerFlashTime * (1 - playerFlashTimeRatio));
        if (playerMapTile)
        {
            playerMapTile.ToggleTileVisual(true);
        }

        mapFlashCoroutine = StartCoroutine(PlayerMapFlash());
    }
}

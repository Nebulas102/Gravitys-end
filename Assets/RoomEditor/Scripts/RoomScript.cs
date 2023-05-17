using UnityEngine;

public class RoomScript : MonoBehaviour
{
    // The save location for the room data file
    public string SaveLocation = "ExampleRoomData";

    // Models and Materials
    public AssetsScript Assets;
    private readonly int EAST = 2;

    private readonly int NORTH = 0;
    private readonly int SOUTH = 1;
    private readonly int WEST = 3;

    // Data for generated objects
    private Transform _parent;

    private Transform _parentBlocks;

    private Transform _parentDoors;

    private Transform _parentFloor;
    private Transform _parentFloorFrames;
    private Transform _parentFloorTiles;
    private Transform _parentModels;
    private Transform _parentWallFrames;

    private Transform _parentWalls;
    private Transform _parentWallTiles;

    public void CreateParents()
    {
        // Creates the parent object for all objects
        _parent = new GameObject("Room").transform;
        _parent.SetParent(transform);

        // Creates the parent object for all blocks
        _parentBlocks = new GameObject("Blocks").transform;
        _parentBlocks.SetParent(_parent);

        // Creates the parent object for all 3D models
        _parentModels = new GameObject("Room Models").transform;
        _parentModels.SetParent(_parent);

        // Creates parent object for all floors
        _parentFloor = new GameObject("Floors").transform;
        _parentFloor.SetParent(_parentModels);

        // Creates parent object for floor frames
        _parentFloorFrames = new GameObject("Frames").transform;
        _parentFloorFrames.SetParent(_parentFloor);

        // Creates parent object for floor tiles
        _parentFloorTiles = new GameObject("Tiles").transform;
        _parentFloorTiles.SetParent(_parentFloor);

        // Creates parent object for all walls
        _parentWalls = new GameObject("Walls").transform;
        _parentWalls.SetParent(_parentModels);

        // Creates parent object for wall frames
        _parentWallFrames = new GameObject("Frames").transform;
        _parentWallFrames.SetParent(_parentWalls);

        // Creates parent object for wall tiles
        _parentWallTiles = new GameObject("Tiles").transform;
        _parentWallTiles.SetParent(_parentWalls);

        // Creates parent object for doors
        _parentDoors = new GameObject("Doors").transform;
        _parentDoors.SetParent(_parentWalls);
    }

    public bool ClearAllObjects()
    {
        var t = transform.Find("Room");
        if (t is null) return false;

        _parent = t.gameObject.transform;
        DestroyImmediate(_parent.gameObject);
        _parent = null;
        return true;
    }

    public void CreateBlocks(float aWidth, float aHeight, float aWallHeight, float aDoorCellSize, float aThickness,
        int[] aDoorPositions)
    {
        // Creates floor block
        var lPos = transform.position + new Vector3(0.0f, 0.0f - aThickness / 2.0f, 0.0f);
        var lScale = new Vector3(aWidth, aThickness, aHeight);
        CreateBlock(Assets.FloorBlock, lPos, lScale);

        // North Wall
        CreateHorizontalWallBlocks(aWidth, aHeight, aWallHeight, aDoorCellSize, aThickness, aDoorPositions[NORTH],
            true);

        // South Wall
        CreateHorizontalWallBlocks(aWidth, aHeight, aWallHeight, aDoorCellSize, aThickness, aDoorPositions[SOUTH],
            false);

        // East Wall
        CreateVerticalWallBlocks(aWidth, aHeight, aWallHeight, aDoorCellSize, aThickness, aDoorPositions[EAST], true);

        // West Wall
        CreateVerticalWallBlocks(aWidth, aHeight, aWallHeight, aDoorCellSize, aThickness, aDoorPositions[WEST], false);
    }

    private void CreateWallSingleBlockHorizontal(float aMin, float aMax, float aHeight, float aThickness,
        Vector3 aWallSideOrigin, float aVerticalOffset)
    {
        var lWidth = aMax - aMin;
        if (lWidth <= 0.0f) return;

        var lScale = new Vector3(lWidth, aHeight, aThickness);
        var lPos = new Vector3(aMin + lWidth / 2.0f, aVerticalOffset + aHeight / 2.0f, 0.0f);

        CreateBlock(Assets.WallBlock, aWallSideOrigin + lPos, lScale);
    }

    private void CreateWallSingleBlockVertical(float aMin, float aMax, float aHeight, float aThickness,
        Vector3 aWallSideOrigin, float aVerticalOffset)
    {
        var lWidth = aMax - aMin;
        if (lWidth <= 0.0f) return;

        var lScale = new Vector3(aThickness, aHeight, lWidth);
        var lPos = new Vector3(0.0f, aVerticalOffset + aHeight / 2.0f, aMin + lWidth / 2.0f);

        CreateBlock(Assets.WallBlock, aWallSideOrigin + lPos, lScale);
    }

    private void CreateDoorBlockHorizontal(float aCellSize, float aDoorPosition, float aThickness,
        Vector3 aWallSideOrigin)
    {
        var lScale = new Vector3(aCellSize, aCellSize, aThickness);
        var lPos = new Vector3(aDoorPosition + aCellSize / 2.0f, aCellSize / 2.0f, 0.0f);

        CreateBlock(Assets.DoorBlock, aWallSideOrigin + lPos, lScale);
    }

    private void CreateDoorBlockVertical(float aCellSize, float aDoorPosition, float aThickness,
        Vector3 aWallSideOrigin)
    {
        var lScale = new Vector3(aThickness, aCellSize, aCellSize);
        var lPos = new Vector3(0.0f, aCellSize / 2.0f, aDoorPosition + aCellSize / 2.0f);

        CreateBlock(Assets.DoorBlock, aWallSideOrigin + lPos, lScale);
    }

    private void CreateHorizontalWallBlocks(float aWidth, float aHeight, float aWallHeight, float aCellSize,
        float aThickness, float aDoorPosition, bool aIsNorthSide)
    {
        var lWallSideOrigin = new Vector3(-aWidth / 2.0f, 0.0f, 0.0f);
        if (aIsNorthSide)
            lWallSideOrigin.z = aHeight / 2.0f + aThickness / 2.0f;
        else
            lWallSideOrigin.z = -(aHeight / 2.0f + aThickness / 2.0f);

        if (aDoorPosition < 0.0f)
        {
            // Creates the entire wall in one block (if there is no door)
            CreateWallSingleBlockHorizontal(0.0f, aWidth, aWallHeight, aThickness, lWallSideOrigin, 0.0f);
        }
        else
        {
            // Creates bottom layer of wall (with gap for door)
            CreateWallSingleBlockHorizontal(0.0f, aDoorPosition, aCellSize, aThickness, lWallSideOrigin, 0.0f);
            CreateWallSingleBlockHorizontal(aDoorPosition + aCellSize, aWidth, aCellSize, aThickness, lWallSideOrigin,
                0.0f);

            // Creates door block
            CreateDoorBlockHorizontal(aCellSize, aDoorPosition, aThickness, lWallSideOrigin);

            // Creates top layer of wall (if there was a door and the wall is taller than the door)
            if (aWallHeight > aCellSize)
            {
                CreateWallSingleBlockHorizontal(0.0f, aWidth, aWallHeight - aCellSize, aThickness, lWallSideOrigin,
                    aCellSize);
            }
        }
    }

    private void CreateVerticalWallBlocks(float aWidth, float aHeight, float aWallHeight, float aCellSize,
        float aThickness, float aDoorPosition, bool aIsEastSide)
    {
        var lWallSideOrigin = new Vector3(0.0f, 0.0f, -aHeight / 2.0f);
        if (aIsEastSide)
            lWallSideOrigin.x = aWidth / 2.0f + aThickness / 2.0f;
        else
            lWallSideOrigin.x = -(aWidth / 2.0f + aThickness / 2.0f);

        if (aDoorPosition < 0.0f)
        {
            // Creates the entire wall in one block (if there is no door)
            CreateWallSingleBlockVertical(0.0f, aHeight, aWallHeight, aThickness, lWallSideOrigin, 0.0f);
        }
        else
        {
            // Creates bottom layer of wall (with gap for door)
            CreateWallSingleBlockVertical(0.0f, aDoorPosition, aCellSize, aThickness, lWallSideOrigin, 0.0f);
            CreateWallSingleBlockVertical(aDoorPosition + aCellSize, aHeight, aCellSize, aThickness, lWallSideOrigin,
                0.0f);

            // Creates door block
            CreateDoorBlockVertical(aCellSize, aDoorPosition, aThickness, lWallSideOrigin);

            // Creates top layer of wall (if there was a door and the wall is taller than the door)
            if (aWallHeight > aCellSize)
            {
                CreateWallSingleBlockVertical(0.0f, aHeight, aWallHeight - aCellSize, aThickness, lWallSideOrigin,
                    aCellSize);
            }
        }
    }

    private void CreateBlock(GameObject aModel, Vector3 aPosition, Vector3 aScale)
    {
        var lObject = Instantiate(aModel, aPosition, Quaternion.identity, _parentBlocks);
        lObject.transform.localScale = aScale;
    }

    public void CreateFloorFrame(Vector3 aPosition, Quaternion aRotation, int aMaterial)
    {
        var lModel = Assets.FloorFrameModel;
        CreateObject(_parentFloorFrames, lModel, aPosition, aRotation, aMaterial);
    }

    public void CreateFloorTile(int aModel, Vector3 aPosition, Quaternion aRotation, int aMaterial)
    {
        var lModel = Assets.FloorTileModels[aModel];
        CreateObject(_parentFloorTiles, lModel, aPosition, aRotation, aMaterial);
    }

    public void CreateWallFrame(Vector3 aPosition, Quaternion aRotation, int aMaterial)
    {
        var lModel = Assets.WallFrameModel;
        CreateObject(_parentWallFrames, lModel, aPosition, aRotation, aMaterial);
    }

    public void CreateWallTile(int aModel, Vector3 aPosition, Quaternion aRotation, int aMaterial)
    {
        var lModel = Assets.WallTileModels[aModel];
        CreateObject(_parentWallTiles, lModel, aPosition, aRotation, aMaterial);
    }

    public void CreateDoor(Vector3 aPosition, Quaternion aRotation, int aMaterial)
    {
        var lModel = Assets.DoorModel;
        CreateObject(_parentDoors, lModel, aPosition, aRotation, aMaterial);
    }

    private void CreateObject(Transform aParent, GameObject aModel, Vector3 aPosition, Quaternion aRotation,
        int aMaterial)
    {
        var lObject = Instantiate(aModel, aPosition, aRotation, aParent);
        var lMaterial = Assets.Materials[aMaterial];
        lObject.GetComponent<Renderer>().material = lMaterial;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionEnum { SELF_EMPTY = -1, SELF, E, SE, SW, W, NW, NE, EAST, SEAST, SWEST, WEST, NWEST, NEAST }

public class Board : MonoBehaviour
{
    private int boardWidth;
    private int boardLength;
    public float xTileGap = 1.5f;
    public float zTileGap = 1.3f;
    public GameObject hexTilePrefab;
    private List<HexTile> tiles;

    public ElementManager elementManager;
    public TeamManager teamManager;
    public UIManager uiManager; 

    void Start()
    {
        tiles = new List<HexTile>();
        SetDimensions(11, 11);
    }

    private void SetDimensions(int boardWidth, int boardLength)
    {
        this.boardWidth = boardWidth;
        this.boardLength = boardLength;
        GenerateBoard();
    }

    public void SetRadius(int size)
    {
        boardWidth = size;
        boardLength = size;
        GenerateBoard();
    }

    public void SetCharPositions()
    {
        // hardcode for now
        if (teamManager.NumTeams() == 2)
        {
            HexTile p1StartPos = GetNeighbor(GetFurthest(DirectionEnum.W), DirectionEnum.E);
            p1StartPos.SetCharacter(teamManager.GetTeam(0).GetCharacterGO(0));
            HexTile o1StartPos = GetNeighbor(GetFurthest(DirectionEnum.E), DirectionEnum.W);
            o1StartPos.SetCharacter(teamManager.GetTeam(1).GetCharacterGO(0));
        }

        switch (teamManager.NumTeams())
        {
            // be careful of freeplay cases where the board is not standard
            case 2:
                break;
            case 4:
                break;
            case 6:
                break;
            default:
                // handle freeplay case where any number of characters can be chosen
                break;
        }
    }

    public void GenerateBoard()
    {
        // erase current board
        if (transform.childCount != 0)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }

        // generate the board
        int half_width = boardWidth / 2;
        for (int z = half_width; z >= -half_width; z--)
        {
            int num_in_row = boardLength - Math.Abs(z);
            float half_len = num_in_row / 2f;

            for (float x = -half_len; x < half_len; x++)
            {
                GameObject tile = Instantiate(hexTilePrefab, new Vector3(xTileGap * (x + 0.5f), 0, zTileGap * z), Quaternion.identity);
                tile.name = (z & 1) == 1 ? "Tile" + z + "," + x : "Tile" + z + "," + (x + 0.5f);    // bitwise AND works with negative numbers too
                tile.GetComponent<HexTile>().InitWithName();
                tiles.Add(tile.GetComponent<HexTile>());
                tile.transform.parent = transform;
                tile.layer = gameObject.layer;
            }
        }

        // OBSOLETE: ensure board persists to game scene
        // DontDestroyOnLoad(this);
    }


    /*******************************
     * Coordinate System
     *******************************/
    private HexTile GetTile(int x, int y, int z)
    {
        foreach (HexTile tile in tiles)
        {
            if (tile.Equal(x, y, z))
                return tile;
        }
        return null;
    }

    private HexTile GetTile(float x, float y, float z)
    {
        float epsilon = 0.0000005f;
        int rx = (int)Math.Round(x, MidpointRounding.AwayFromZero);
        int ry = (int)Math.Round(y, MidpointRounding.AwayFromZero);
        int rz = (int)Math.Round(z, MidpointRounding.AwayFromZero);
        var x_diff = Math.Abs(rx - x) + epsilon;
        var y_diff = Math.Abs(ry - y) + epsilon;
        var z_diff = Math.Abs(rz - z) + epsilon;

        if (x_diff > y_diff && x_diff > z_diff)
            rx = ry + rz;
        else if (y_diff > z_diff)
            ry = rx - rz;
        else
            rz = rx - ry;

        return GetTile((int)rx, (int)ry, (int)rz);
    }

    public HexTile GetCharTile(Character character)
    {
        foreach (HexTile t in tiles)
        {
            if (t.GetCharacter() == character)
                return t;
        }
        return null;
    }

    private HexTile GetNeighbor(HexTile tile, DirectionEnum dir)
    {
        switch (dir)
        {
            case DirectionEnum.E:  // x + 1 // y + 1
            case DirectionEnum.EAST:
                return GetTile(tile.x + 1, tile.y + 1, tile.z);
            case DirectionEnum.SE: // y + 1 // z - 1
            case DirectionEnum.SEAST:
                return GetTile(tile.x, tile.y + 1, tile.z - 1);
            case DirectionEnum.SW: // x - 1 // z - 1
            case DirectionEnum.SWEST:
                return GetTile(tile.x - 1, tile.y, tile.z - 1);
            case DirectionEnum.W:  // x - 1 // y - 1
            case DirectionEnum.WEST:
                return GetTile(tile.x - 1, tile.y - 1, tile.z);
            case DirectionEnum.NW: // y - 1 // z + 1
            case DirectionEnum.NWEST:
                return GetTile(tile.x, tile.y - 1, tile.z + 1);
            case DirectionEnum.NE: // x + 1 // z + 1
            case DirectionEnum.NEAST:
                return GetTile(tile.x + 1, tile.y, tile.z + 1);
            default:    // DirectionEnum.SELF
                return GetTile(tile.x, tile.y, tile.z);
        }
    }

    private HexTile GetNeighbor(HexTile tile, Vector3 offset)
    {
        return GetTile(tile.x + (int)offset.x, tile.y + (int)offset.y, tile.z + (int)offset.z);
    }

    private List<HexTile> GetNeighbors(HexTile tile)
    {
        // E, SE, SW, W, NW, NE
        List<HexTile> neighbors = new List<HexTile>();
        neighbors.Add(GetNeighbor(tile, DirectionEnum.E));
        neighbors.Add(GetNeighbor(tile, DirectionEnum.SE));
        neighbors.Add(GetNeighbor(tile, DirectionEnum.SW));
        neighbors.Add(GetNeighbor(tile, DirectionEnum.W));
        neighbors.Add(GetNeighbor(tile, DirectionEnum.NW));
        neighbors.Add(GetNeighbor(tile, DirectionEnum.NE));
        return neighbors;
    }

    private HexTile GetFurthest(DirectionEnum dir)
    {
        HexTile curr = GetTile(0, 0, 0);
        HexTile next = GetNeighbor(curr, dir);
        while (next != null)
        {
            curr = next;
            next = GetNeighbor(curr, dir);
        }
        return curr;
    }

    private int GetDistance(HexTile start, HexTile end)
    {
        // the maximum of the distances between the tiles on each axis
        return Math.Max(Math.Abs(start.x - end.x), Math.Max(Math.Abs(start.y - end.y), Math.Abs(start.z - end.z)));
    }

    private Vector3 GetOffset(DirectionEnum dir)
    {
        switch (dir)
        {
            case DirectionEnum.E:  // x + 1 // y + 1
            case DirectionEnum.EAST:
                return new Vector3(1, 1, 0);
            case DirectionEnum.SE: // y + 1 // z - 1
            case DirectionEnum.SEAST:
                return new Vector3(0, 1, -1);
            case DirectionEnum.SW: // x - 1 // z - 1
            case DirectionEnum.SWEST:
                return new Vector3(-1, 0, -1);
            case DirectionEnum.W:  // x - 1 // y - 1
            case DirectionEnum.WEST:
                return new Vector3(-1, -1, 0);
            case DirectionEnum.NW: // y - 1 // z + 1
            case DirectionEnum.NWEST:
                return new Vector3(0, -1, 1);
            case DirectionEnum.NE: // x + 1 // z + 1
            case DirectionEnum.NEAST:
                return new Vector3(1, 0, 1);
            default:    // DirectionEnum.SELF and SELF_EMPTY
                return new Vector3(0, 0, 0);
        }
    }

    private DirectionEnum GetSector(HexTile hoverTile, HexTile charTile, bool inclusive)
    {
        Vector3 offset = new Vector3(hoverTile.x - charTile.x, hoverTile.y - charTile.y, hoverTile.z - charTile.z);
        Vector3 absOffset = new Vector3(Math.Abs(offset.x), Math.Abs(offset.y), Math.Abs(offset.z));

        // East or West on z axis
        if (absOffset.z < absOffset.y && absOffset.z <= absOffset.x)
        {
            if (offset.x > 0 || offset.y > 0)
                if (inclusive)
                    return DirectionEnum.E;
                else
                    return DirectionEnum.EAST;
            if (offset.x < 0 || offset.y < 0)
                if (inclusive)
                    return DirectionEnum.W;
                else
                    return DirectionEnum.WEST;
        }

        // SouthEast or NorthWest on x axis
        if (absOffset.x < absOffset.z && absOffset.x <= absOffset.y)
        {
            if (offset.y > 0 || offset.z < 0)
                if (inclusive)
                    return DirectionEnum.SE;
                else
                    return DirectionEnum.SEAST;
            if (offset.y < 0 || offset.z > 0)
                if (inclusive)
                    return DirectionEnum.NW;
                else
                    return DirectionEnum.NWEST;
        }

        // SouthWest or NorthEast on y axis
        if (absOffset.y < absOffset.x && absOffset.y <= absOffset.z)
        {
            if (offset.x < 0 || offset.z < 0)
                if (inclusive)
                    return DirectionEnum.SW;
                else
                    return DirectionEnum.SWEST;
            if (offset.x > 0 || offset.z > 0)
                if (inclusive)
                    return DirectionEnum.NE;
                else
                    return DirectionEnum.NEAST;
        }

        return DirectionEnum.SELF;
    }

    private List<HexTile> GetInverseArea(List<HexTile> area)
    {
        List<HexTile> inverse = new List<HexTile>();
        foreach (HexTile tile in tiles)
        {
            if (!area.Contains(tile))
                inverse.Add(tile);
        }
        return inverse;
    }

    private HexTile LerpTiles(HexTile a, HexTile b, float t)
    {
        if (a == b)
            return a;

        return GetTile(a.x + (b.x - a.x) * t,
                       a.y + (b.y - a.y) * t,
                       a.z + (b.z - a.z) * t);
    }

    // TODO: Test this
    public List<HexTile> TypedBFS(HexTile startTile, ElementEnum type, int maxDepth)
    {
        List<HexTile> queue = new List<HexTile> { startTile };
        List<HexTile> discovered = new List<HexTile> { startTile };
        while (queue.Count > 0 && GetDistance(startTile, queue[0]) <= maxDepth)
        {
            HexTile first = queue[0];
            discovered.Add(first);
            queue.RemoveAt(0);
            foreach (HexTile n in GetNeighbors(first))
            {
                if (!discovered.Contains(n) && n.currElement == type)
                {
                    queue.Add(n);
                }
            }
        }
        return discovered;
    }


    /*******************************
     * Actions
     *******************************/

    // PathArea parsing method that returns a List of HexTiles
    public List<HexTile> GetActionArea(Action action)
    {
        HexTile tempTile = action.StartTile;
        List<HexTile> area = new List<HexTile>();
        Vector3 offset = Vector3.zero;

        for (int i = 0; i < action.PathArea.Length; i++)
        {
            // add to the total offset
            offset += GetOffset((DirectionEnum)action.PathArea[i]);

            // if null then the pattern has a gap so use the last real tile and offset
            if (tempTile == null)
            {
                int index = area.Count - 1;
                tempTile = GetNeighbor(area[index], offset);
            }
            else // otherwise proceed to the next tile
                tempTile = GetNeighbor(tempTile, offset);

            // if not null the gap is closed and we reset the offset
            if (tempTile != null)
                offset = Vector3.zero;  // happens every time offset is 0

            // check if we should add the current tile
            if (tempTile != null &&
                action.PathArea[i] < (int)DirectionEnum.EAST) // EAST is the start of the gap directions
                area.Add(tempTile);
            // TODO: change position of SELF_EMPTY so that it doesn't include the self tile
        }
        return area;
    }

    // Change the Character's Action based on what the player plans
    public void AdjustAction(Character character, HexTile hoverTile)
    {
        HexTile charTile = GetCharTile(character);
        Action action = character.GetPlanningAction();

        // initialize start tile as character tile
        action.StartTile = charTile;

        switch (action.PathAreaType)
        {
            case PathAreaTypeEnum.FIXED:
                // should not change
                break;
            case PathAreaTypeEnum.SELF:
                break;
            case PathAreaTypeEnum.SPOKES:
                action.RotatePathArea(GetSector(hoverTile, charTile, true));
                break;
            case PathAreaTypeEnum.STRAIGHT_LINE_SPACED:
            // set up some stuff for fall through?
            case PathAreaTypeEnum.STRAIGHT_LINE_SOLID:
                action.ResetPathArea();
                int n = GetDistance(charTile, hoverTile);
                HexTile tempTile = charTile;
                int start = action.PathArea[0] == 0 ? 0 : 1;
                for (int i = start; i < action.Distance; i++)
                {
                    float segment = 1.0f / n * i;
                    HexTile nextTile = LerpTiles(charTile, hoverTile, segment);
                    if (nextTile == null)
                        break;
                    action.AddToPathArea(GetSector(nextTile, tempTile, true));
                    tempTile = nextTile;
                }
                break;
            case PathAreaTypeEnum.FREE_FORM:
                // TODO: preview this somehow
                // handle input
                if (Input.GetMouseButtonDown(0))
                {
                    // if the hoverTile connects to the charTile AND the new length is still within the distance
                    if (action.CanGoFurther())
                    {
                        List<HexTile> area = GetActionArea(action);
                        HexTile lastTile = area[area.Count - 1];
                        // if the hoverTile is new and is adjacent to the lastTile
                        if (!area.Contains(hoverTile) && GetNeighbors(lastTile).Contains(hoverTile))
                        {
                            action.AddToPathArea(GetSector(hoverTile, lastTile, true));
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.LeftShift))
                {
                    action.ShrinkPathArea();
                }
                else
                {
                    // UNTESTED
                    /*
                    bool down = Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.S);
                    bool up = Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.W);
                    bool right = Input.GetKeyDown(KeyCode.RightArrow) && Input.GetKeyDown(KeyCode.D);
                    bool left = Input.GetKeyDown(KeyCode.LeftArrow) && Input.GetKeyDown(KeyCode.A);

                    if (up && right) // NorthEast
                        action.AddToPathArea(DirectionEnum.NE);
                    if (down && right) // SouthEast
                        action.AddToPathArea(DirectionEnum.SE);
                    if (right) // East
                        action.AddToPathArea(DirectionEnum.E);
                    if (up && left) // NorthWest
                        action.AddToPathArea(DirectionEnum.NW);
                    if (down && left) // SouthWest
                        action.AddToPathArea(DirectionEnum.SW);
                    if (left) // West
                        action.AddToPathArea(DirectionEnum.W);
                    */
                }
                break;
            case PathAreaTypeEnum.ROTATE_AROUND:
                action.RotatePathArea(GetSector(hoverTile, charTile, true));
                break;
            case PathAreaTypeEnum.TRUE_GLOBAL:
                action.StartTile = hoverTile;
                break;
            case PathAreaTypeEnum.TYPED_GLOBAL:
                if (hoverTile.currElement == character.element)
                    action.StartTile = hoverTile;
                break;
            default:
                break;
        }
    }

    public void ExecuteAttack(Character character, Action action)
    {
        List<HexTile> attackArea = GetActionArea(action);

        foreach (HexTile tile in attackArea)
        {
            // any characters other than the user (fix for team fire)
            // characters hit by the action take damage
            if (action.GetType().Equals(typeof(Attack)) && tile.HasCharacter() && tile.GetCharacter() != character)
            //if (tile.HasCharacter() && tile.GetCharacter() != character)
            {
                Character c = tile.GetCharacter();
                c.TakeDamage(((Attack)action).damage);
            }

            // change the typing of the tiles
            ChangeEnum typeChange = elementManager.GetChange(tile.currElement, action.Element);
            switch (typeChange)
            {
                case ChangeEnum.NEUTRAL:
                    tile.ChangeElement(ElementEnum.NEUTRAL);
                    break;
                case ChangeEnum.NEUTRALIZE1:
                    tile.ChangeElement(ElementEnum.NEUTRAL);
                    // neutralize all matching neighboring tiles
                    foreach (HexTile neighbor in GetNeighbors(tile))
                        if (!attackArea.Contains(neighbor) && neighbor.currElement == tile.currElement)
                            neighbor.ChangeElement(ElementEnum.NEUTRAL);
                    break;
                case ChangeEnum.NEUTRALIZE2:
                    tile.ChangeElement(ElementEnum.NEUTRAL);
                    // neutralize all matching neighboring and second neighboring tiles
                    foreach (HexTile neighbor in GetNeighbors(tile))
                    {
                        if (!attackArea.Contains(neighbor) && neighbor.currElement == tile.currElement)
                        {
                            foreach (HexTile neighbor2 in GetNeighbors(neighbor))
                            {
                                if (!attackArea.Contains(neighbor2) && neighbor2.currElement == tile.currElement)
                                    neighbor.ChangeElement(ElementEnum.NEUTRAL);
                            }
                        }
                    }
                    break;

                case ChangeEnum.WEAK:
                    tile.ResetType(); // basically does nothing
                    break;
                case ChangeEnum.SHRINK1:
                    // TODO: handle BFS so the first neighbor doesn't change other tiles
                    break;
                case ChangeEnum.SHRINK2:

                    break;

                // TODO: create BFS instead of for loops
                case ChangeEnum.STRONG:
                    tile.ChangeElement(action.Element);
                    break;
                case ChangeEnum.SPREAD1:
                    tile.ChangeElement(action.Element);
                    foreach (HexTile neighbor in GetNeighbors(tile))
                        if (!attackArea.Contains(neighbor) && neighbor.currElement == tile.currElement)
                            neighbor.ChangeElement(action.Element);
                    break;
                case ChangeEnum.SPREAD2:
                    tile.ChangeElement(ElementEnum.NEUTRAL);
                    foreach (HexTile neighbor in GetNeighbors(tile))
                    {
                        if (!attackArea.Contains(neighbor) && neighbor.currElement == tile.currElement)
                        {
                            foreach (HexTile neighbor2 in GetNeighbors(neighbor))
                            {
                                if (!attackArea.Contains(neighbor2) && neighbor2.currElement == tile.currElement)
                                    neighbor.ChangeElement(action.Element);
                            }
                        }
                    }
                    break;
                case ChangeEnum.CONTIGUOUS:
                    tile.ChangeElement(action.Element);
                    List<HexTile> queue = new List<HexTile>();
                    // TODO: handle BFS of tiles
                    break;

                case ChangeEnum.BLOCK:
                    // maybe remove
                    break;
            }
        }
    }

    public void ExecuteMovement(Character character, Movement movement)
    {
        List<HexTile> movePath = GetActionArea(movement);

        foreach (HexTile tile in movePath)
        {
            character.transform.position = tile.transform.position;
        }
    }

    public void ExecuteCombo(Character character, Combo combo)
    {

    }

    public void ExecuteAction(Character character, Action action)
    {
        if (action.GetType().Equals(typeof(Attack)) || action.GetType().Equals(typeof(Action)))
            ExecuteAttack(character, (Attack)action);
        else if (action.GetType().Equals(typeof(Movement)))
            ExecuteMovement(character, (Movement)action);
        else if (action.GetType().Equals(typeof(Combo)))
            ExecuteCombo(character, (Combo)action);
    }


    /*******************************
     * Tile Coloring
     *******************************/
    public void ResetAllTiles()
    {
        foreach (HexTile tile in tiles)
            tile.ResetType();
    }

    public void ViewTiles(List<HexTile> area, ElementEnum tempType)
    {
        UnviewTiles(GetInverseArea(area));
        foreach (HexTile tile in area)
            tile.SetTempElement(tempType);
    }

    public void UnviewTiles(List<HexTile> area)
    {
        foreach (HexTile tile in area)
            tile.ResetTempElement();
    }

    public void SubmitTiles(List<HexTile> area)
    {
        foreach (HexTile tile in area)
            tile.SetAtkElement();
    }

    public void ChangeTiles(List<HexTile> area)
    {
        foreach (HexTile tile in area)
            tile.ChangeToAtkElement();
    }

    public void ChangeTiles(List<HexTile> area, ElementEnum newType)
    {
        foreach (HexTile tile in area)
            tile.ChangeElement(newType);
    }
}
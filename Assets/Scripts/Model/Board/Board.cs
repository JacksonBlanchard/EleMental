using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionEnum { SELF_SELECT, E_SELECT, SE_SELECT, SW_SELECT, W_SELECT, NW_SELECT, NE_SELECT, E_MOVE, SE_MOVE, SW_MOVE, W_MOVE, NW_MOVE, NE_MOVE }

public class Board : MonoBehaviour
{
    private int m_boardWidth;
    private int m_boardLength;
    public float m_xTileGap = 1.5f;
    public float m_zTileGap = 1.3f;
    public GameObject m_hexTilePrefab;
    private HexTile[,] m_tiles;

    public ElementManager m_elementManager;
    public TeamManager m_teamManager;
    public UIManager m_uiManager;

    void Start()
    {
        if (m_boardWidth == 0 || m_boardLength == 0)
        {
            SetRadius(11);
        }
    }

    public void SetRadius(int size)
    {
        m_boardWidth = size;
        m_boardLength = size;
        GenerateBoard();
    }

    private void SetDimensions(int boardWidth, int boardLength)
    {
        m_boardWidth = boardWidth;
        m_boardLength = boardLength;
        GenerateBoard();
    }

    public void SetCharPositions()
    {
        // hardcode for now
        if (m_teamManager.NumTeams() == 2)
        {
            HexTile p1StartPos = GetNeighbor(GetFurthest(DirectionEnum.W_MOVE), DirectionEnum.E_MOVE);
            p1StartPos.SetElementor(m_teamManager.GetTeam(0).GetElementorGO(0));
            HexTile o1StartPos = GetNeighbor(GetFurthest(DirectionEnum.E_MOVE), DirectionEnum.W_MOVE);
            o1StartPos.SetElementor(m_teamManager.GetTeam(1).GetElementorGO(0));
        }
    }

    public void GenerateBoard()
    {
        // Erase the current board.
        if (transform.childCount != 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
 
        // Initialize 2D HexTile array.
        m_tiles = new HexTile[m_boardLength, m_boardWidth];

        /* 
        -------------------
        | 0,0 | 0,1 | 0,2 |
        | nul | hex | hex |
        |------------------
        | 1,0 | 1,1 | 1,2 |
        | hex | hex | hex |
        |------------------
        | 2,0 | 2,1 | 2,2 |
        | hex | hex | nul |
        -------------------
        */

        // Generate the new board.
        for (int i=0; i<m_boardWidth; i++)
        {
            for (int j=0; j<m_boardLength; j++)
            {
                // top left corner
                if (i < m_boardWidth / 2 && j < m_boardWidth / 2)
                {
                    m_tiles[i, j] = null;
                    continue;
                }
                // bottom right corner
                if (i > m_boardWidth / 2 && j < m_boardLength / 2)
                {
                    m_tiles[i, j] = null;
                    continue;
                }

                // Otherwise create and set up new tile GameObject.
                GameObject tile = Instantiate(m_hexTilePrefab, new Vector3(m_xTileGap * (i + 0.5f), 0, m_zTileGap * j), Quaternion.identity);
                tile.name = "Tile[" + i + "," + j + "]";
                tile.GetComponent<HexTile>().SetCoordinates(i, j);
                tile.transform.parent = transform;
                tile.layer = gameObject.layer;
                // Add the new HexTile Component to the 2D HexTile array.
                m_tiles[i, j] = tile.GetComponent<HexTile>();
            }
        }

        // OBSOLETE: ensure board persists to game scene
        // DontDestroyOnLoad(this);
    }

    public void SaveBoard()
    {
        string groundFilename = "ground.txt";
        string surfaceFilename = "surface.txt";
        string atmosphereFilename = "atmosphere.txt";

        // Write each directory name to a file.
        using (StreamWriter sw = new StreamWriter(groundFilename))
        {
            //foreach (DirectoryInfo dir in cDirs)
            {
                //sw.WriteLine(dir.Name);
            }
        }

        // Read and show each line from the file.
        string line = "";
        using (StreamReader sr = new StreamReader("CDriveDirs.txt"))
        {
            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }

    /*******************************
     * Coordinate System
     *******************************/
    private HexTile GetTile(int x, int y, int z)
    {
        foreach (HexTile tile in m_tiles)
        {
            if (tile == null)
            {
                continue;
            }
            if (tile.Equal(x, y, z))
            {
                return tile;
            }
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

    public HexTile GetCharTile(Elementor elementor)
    {
        foreach (HexTile t in m_tiles)
        {
            if (t.GetElementor() == elementor)
                return t;
        }
        return null;
    }

    private HexTile GetNeighbor(HexTile tile, DirectionEnum dir)
    {
        switch (dir)
        {
            case DirectionEnum.E_SELECT:  // x + 1 // y + 1
            case DirectionEnum.E_MOVE:
                return GetTile(tile.m_x + 1, tile.m_y + 1, tile.m_z);
            case DirectionEnum.SE_SELECT: // y + 1 // z - 1
            case DirectionEnum.SE_MOVE:
                return GetTile(tile.m_x, tile.m_y + 1, tile.m_z - 1);
            case DirectionEnum.SW_SELECT: // x - 1 // z - 1
            case DirectionEnum.SW_MOVE:
                return GetTile(tile.m_x - 1, tile.m_y, tile.m_z - 1);
            case DirectionEnum.W_SELECT:  // x - 1 // y - 1
            case DirectionEnum.W_MOVE:
                return GetTile(tile.m_x - 1, tile.m_y - 1, tile.m_z);
            case DirectionEnum.NW_SELECT: // y - 1 // z + 1
            case DirectionEnum.NW_MOVE:
                return GetTile(tile.m_x, tile.m_y - 1, tile.m_z + 1);
            case DirectionEnum.NE_SELECT: // x + 1 // z + 1
            case DirectionEnum.NE_MOVE:
                return GetTile(tile.m_x + 1, tile.m_y, tile.m_z + 1);
            case DirectionEnum.SELF_SELECT:
            default:
                return GetTile(tile.m_x, tile.m_y, tile.m_z);
        }
    }

    private HexTile GetNeighbor(HexTile tile, Vector3 offset)
    {
        return GetTile(tile.m_x + (int)offset.x, tile.m_y + (int)offset.y, tile.m_z + (int)offset.z);
    }

    private List<HexTile> GetNeighbors(HexTile tile)
    {
        // E, SE, SW, W, NW, NE
        List<HexTile> neighbors = new List<HexTile>();
        neighbors.Add(GetNeighbor(tile, DirectionEnum.E_SELECT));
        neighbors.Add(GetNeighbor(tile, DirectionEnum.SE_SELECT));
        neighbors.Add(GetNeighbor(tile, DirectionEnum.SW_SELECT));
        neighbors.Add(GetNeighbor(tile, DirectionEnum.W_SELECT));
        neighbors.Add(GetNeighbor(tile, DirectionEnum.NW_SELECT));
        neighbors.Add(GetNeighbor(tile, DirectionEnum.NE_SELECT));
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
        return Math.Max(Math.Abs(start.m_x - end.m_x), Math.Max(Math.Abs(start.m_y - end.m_y), Math.Abs(start.m_z - end.m_z)));
    }

    private Vector3 GetOffset(DirectionEnum dir)
    {
        switch (dir)
        {
            case DirectionEnum.E_SELECT:  // x + 1 // y + 1
            case DirectionEnum.E_MOVE:
                return new Vector3(1, 1, 0);
            case DirectionEnum.SE_SELECT: // y + 1 // z - 1
            case DirectionEnum.SE_MOVE:
                return new Vector3(0, 1, -1);
            case DirectionEnum.SW_SELECT: // x - 1 // z - 1
            case DirectionEnum.SW_MOVE:
                return new Vector3(-1, 0, -1);
            case DirectionEnum.W_SELECT:  // x - 1 // y - 1
            case DirectionEnum.W_MOVE:
                return new Vector3(-1, -1, 0);
            case DirectionEnum.NW_SELECT: // y - 1 // z + 1
            case DirectionEnum.NW_MOVE:
                return new Vector3(0, -1, 1);
            case DirectionEnum.NE_SELECT: // x + 1 // z + 1
            case DirectionEnum.NE_MOVE:
                return new Vector3(1, 0, 1);
            default:    // DirectionEnum.SELF and SELF_EMPTY
                return new Vector3(0, 0, 0);
        }
    }

    private DirectionEnum GetSector(HexTile hoverTile, HexTile charTile, bool inclusive)
    {
        Vector3 offset = new Vector3(hoverTile.m_x - charTile.m_x, hoverTile.m_y - charTile.m_y, hoverTile.m_z - charTile.m_z);
        Vector3 absOffset = new Vector3(Math.Abs(offset.x), Math.Abs(offset.y), Math.Abs(offset.z));

        // East or West on z axis
        if (absOffset.z < absOffset.y && absOffset.z <= absOffset.x)
        {
            if (offset.x > 0 || offset.y > 0)
                if (inclusive)
                    return DirectionEnum.E_SELECT;
                else
                    return DirectionEnum.E_MOVE;
            if (offset.x < 0 || offset.y < 0)
                if (inclusive)
                    return DirectionEnum.W_SELECT;
                else
                    return DirectionEnum.W_MOVE;
        }

        // SouthEast or NorthWest on x axis
        if (absOffset.x < absOffset.z && absOffset.x <= absOffset.y)
        {
            if (offset.y > 0 || offset.z < 0)
                if (inclusive)
                    return DirectionEnum.SE_SELECT;
                else
                    return DirectionEnum.SE_MOVE;
            if (offset.y < 0 || offset.z > 0)
                if (inclusive)
                    return DirectionEnum.NW_SELECT;
                else
                    return DirectionEnum.NW_MOVE;
        }

        // SouthWest or NorthEast on y axis
        if (absOffset.y < absOffset.x && absOffset.y <= absOffset.z)
        {
            if (offset.x < 0 || offset.z < 0)
                if (inclusive)
                    return DirectionEnum.SW_SELECT;
                else
                    return DirectionEnum.SW_MOVE;
            if (offset.x > 0 || offset.z > 0)
                if (inclusive)
                    return DirectionEnum.NE_SELECT;
                else
                    return DirectionEnum.NE_MOVE;
        }

        return DirectionEnum.SELF_SELECT;
    }

    private List<HexTile> GetInverseArea(List<HexTile> area)
    {
        List<HexTile> inverse = new List<HexTile>();
        foreach (HexTile tile in m_tiles)
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

        return GetTile(a.m_x + (b.m_x - a.m_x) * t,
                       a.m_y + (b.m_y - a.m_y) * t,
                       a.m_z + (b.m_z - a.m_z) * t);
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
                action.PathArea[i] < (int)DirectionEnum.E_MOVE) // The first _MOVE direction
            {
                area.Add(tempTile);
            }
            // TODO: change position of SELF_EMPTY so that it doesn't include the self tile
        }
        return area;
    }

    // Change the Elementor's Action based on what the player plans
    public void AdjustAction(Elementor elementor, HexTile hoverTile)
    {
        HexTile charTile = GetCharTile(elementor);
        Action action = elementor.GetPlanningAction();

        // initialize start tile as elementor tile
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
                //if (hoverTile.currElement == elementor.element)
                //    action.StartTile = hoverTile;
                break;
            default:
                break;
        }
    }

    public void ExecuteAttack(Elementor elementor, Action action)
    {
        List<HexTile> attackArea = GetActionArea(action);

        foreach (HexTile tile in attackArea)
        {
            // any elementors other than the user (fix for team fire)
            // elementors hit by the action take damage
            if (action.GetType().Equals(typeof(Attack)) && tile.HasElementor() && tile.GetElementor() != elementor)
            //if (tile.HasElementor() && tile.GetElementor() != elementor)
            {
                Elementor e = tile.GetElementor();
                e.TakeDamage(((Attack)action).damage);
            }

            // change the typing of the tiles
            ChangeEnum typeChange = m_elementManager.GetChange(tile.currElement, action.Element);
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

    public void ExecuteMovement(Elementor elementor, Movement movement)
    {
        List<HexTile> movePath = GetActionArea(movement);

        foreach (HexTile tile in movePath)
        {
            elementor.transform.position = tile.transform.position;
        }
    }

    public void ExecuteCombo(Elementor elemetor, Combo combo)
    {

    }

    public void ExecuteAction(Elementor elementor, Action action)
    {
        if (action.GetType().Equals(typeof(Attack)) || action.GetType().Equals(typeof(Action)))
            ExecuteAttack(elementor, (Attack)action);
        else if (action.GetType().Equals(typeof(Movement)))
            ExecuteMovement(elementor, (Movement)action);
        else if (action.GetType().Equals(typeof(Combo)))
            ExecuteCombo(elementor, (Combo)action);
    }


    /*******************************
     * Tile Coloring
     *******************************/
    public void ResetAllTiles()
    {
        foreach (HexTile tile in m_tiles)
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
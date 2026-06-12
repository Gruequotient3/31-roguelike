using UnityEngine;
using UnityEngine.InputSystem;

using Roguelike.Utils;
using Roguelike.Tilemap;
using NUnit.Framework;
using UnityEditor.Rendering;


public enum AnimState
{
    Idle, 
    Walk_Forward,
    Walk_Backward,
    Walk_Left,
    Walk_Right,
}

public class PlayerMovement : MonoBehaviour
{
    private InputAction _moveAction;
    public WorldGenerator worldGenerator;
    public float speed;
    
    public Vector3 tilePosition = new Vector3(0, 0, 0);
    private Vector3Int lastTilePosition = new Vector3Int(0, 0, 0);

    private Animator _animator;
    private AnimState _lastState = AnimState.Idle;
    private bool first = true;

    void Start()
    {
        transform.position = Coordinate.IsoToWorld(tilePosition);
        _moveAction = InputSystem.actions.FindAction("Move");
        _animator = transform.GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 dirValue = _moveAction.ReadValue<Vector2>();
        Vector3 worldDir = new Vector3(dirValue.x, dirValue.y, 0);
        Vector3 direction = Coordinate.WorldToIsoCoordinate(worldDir);
        
        Vector3 tempPos = Vector3.Normalize(direction) * speed * Time.deltaTime + tilePosition;
        Vector3Int chunkPos = Coordinate.IsoToChunk(Vector3Int.FloorToInt(tempPos));
        Chunk chunk = worldGenerator.GetChunk(chunkPos);
        if (chunk == null) {
            SelectAnim(new Vector2(0, 0));
            return;
        }
        
        if (first || lastTilePosition != Vector3Int.FloorToInt(tempPos))
        {
            Vector3Int tilePos = Coordinate.IsoToChunkLocalPosition(Vector3Int.FloorToInt(tempPos));
            bool found = false;
            while(chunk.tilemap.GetTile(tilePos) != null || !found)
            {
                if (tilePos.z < 0)
                {
                    tilePos.z = 0;
                    break;
                }
                if (chunk.tilemap.GetTile(tilePos) != null)
                {
                    found = true;
                    ++tilePos.z;
                }
                else --tilePos.z;
            }
            if (!first && Mathf.Abs(tilePosition.z - tilePos.z) > 1) return;
            tilePosition.z = tilePos.z;
            lastTilePosition = tilePos;            
        }

        tilePosition.x = tempPos.x;
        tilePosition.y = tempPos.y;
        Vector3 temp = Coordinate.IsoToWorld(tilePosition);
        transform.position = new Vector3(temp.x, temp.y, 0);
        SetSortingOrder();
        SelectAnim((Vector2)dirValue);
        first = false;
    }

    private void SetSortingOrder()
    {
        int average = 0;
        for (int y = -1; y <= 1; ++y)
        {
            for (int x = -1; x <= 1; ++x)
            {
                average += -1 * (x + Mathf.FloorToInt(tilePosition.x) + y + Mathf.FloorToInt(tilePosition.y)); 
            }
        }
        transform.GetComponent<SpriteRenderer>().sortingOrder = average / 9 + (int)tilePosition.z + 1;
    }

    private void SelectAnim(Vector2 value)
    {
        if (_animator == null) return;
        if (value.x == 0.0f && value.y == 0.0f)
        {
            switch (_lastState)
            {
                case AnimState.Idle:
                    break;
                case AnimState.Walk_Forward:
                    _animator.Play("PlayerIdleForward", 0);
                    break;
                case AnimState.Walk_Backward:
                    _animator.Play("PlayerIdleBackward", 0);
                    break;
                case AnimState.Walk_Left:
                    _animator.Play("PlayerIdleLeft", 0);
                    break;
                case AnimState.Walk_Right:
                    _animator.Play("PlayerIdleRight", 0);
                    break;
            }
        }
        if (value.x > 0.0f)
        {
            _animator.Play("PlayerWalkRight", 0);   
            _lastState = AnimState.Walk_Right;
        }
        else if (value.x < 0.0f)
        {
                
            _animator.Play("PlayerWalkLeft", 0);   
            _lastState = AnimState.Walk_Left;
        }
        else if (value.y > 0.0f)
        {
            _animator.Play("PlayerWalkForward", 0);   
            _lastState = AnimState.Walk_Forward;
        }
        else if (value.y < 0.0f)
        {
            _animator.Play("PlayerWalkBackward", 0);
            _lastState = AnimState.Walk_Backward;
        }


    }
}
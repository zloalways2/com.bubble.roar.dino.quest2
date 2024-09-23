using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BRDQ
{
    public class BRDQGrid : MonoBehaviour
    {
        public BRDQGameOver BRDQGameOver;
        
        public Vector2Int BRDQSize;
        public Vector2 BRDQspacing;

        public float BRDQcellSize;

        public List<BRDQTile> BRDQTilesPrefab = new List<BRDQTile>();
        
        [Space]
        public AudioClip _BRDQDestroyClip;
        public AudioSource _BRDQSfx;

        private Dictionary<Vector2Int, BRDQTile> _brdqActiveTiles = new Dictionary<Vector2Int, BRDQTile>();
        private Dictionary<Vector2Int, Vector3> _brdqGridPositions = new Dictionary<Vector2Int, Vector3>();

        private Sequence _brdqSequence;

        private void Start()
        {
            var offset = new Vector2((BRDQSize.x - 1) * BRDQspacing.x / 2, (BRDQSize.y - 1) * BRDQspacing.y / 2);

            var tilesForSession = new List<BRDQTile>();

            while (tilesForSession.Count < 3)
            {
                var tilePrefab = BRDQTilesPrefab[Random.Range(0, BRDQTilesPrefab.Count)];
                if (!tilesForSession.Contains(tilePrefab))
                    tilesForSession.Add(tilePrefab);
            }
            
            for (var x = 0; x < BRDQSize.x; x++)
            {
                for (var y = 0; y < BRDQSize.y; y++)
                {
                    var position = new Vector3(x * BRDQspacing.x - offset.x, y * BRDQspacing.y - offset.y, 0f);

                    var brdqTile = Instantiate(tilesForSession[Random.Range(0, tilesForSession.Count)], position, Quaternion.identity, transform);
                    var inGridPos = new Vector2Int(x, y);
                    brdqTile.BRDQPosition = inGridPos;
                    brdqTile.transform.localScale = Vector3.one * BRDQcellSize;

                    brdqTile.BRDQOnTileClicked += BRDQOnTileClicked;
                    
                    _brdqActiveTiles.Add(inGridPos, brdqTile);
                    _brdqGridPositions.Add(inGridPos, position);
                }
            }
        }

        private void BRDQOnTileClicked(BRDQTile brdqTile)
        {
            var neighbors = new HashSet<BRDQTile>();

            CheckAdjacentBalls(brdqTile.BRDQPosition, brdqTile.BRDQColor, ref neighbors);

            if (neighbors.Count < 2) 
                return;
            
            _brdqSequence?.Kill();
            _brdqSequence = DOTween.Sequence();
                
            foreach (var neighbor in neighbors)
            {
                _brdqActiveTiles[neighbor.BRDQPosition] = null;
                    
                _brdqSequence.Join
                (neighbor.transform
                    .DOPunchScale(neighbor.transform.localScale + Vector3.one * 0.1f, 0.2f).SetEase(Ease.Flash)
                    .OnComplete(() =>
                    {
                        neighbor.BRDQDestroy();
                        Destroy(neighbor.gameObject);
                    }));
            }
            
            _BRDQSfx.PlayOneShot(_BRDQDestroyClip);

            _brdqSequence.AppendCallback(() =>
            {
                ShiftBallsDown();
                ShiftBallsSideways();
                CheckAvailableMoves();
            });
        }

        private void CheckAvailableMoves()
        {
            foreach (var tile in _brdqActiveTiles)
            {
                if (tile.Value == null)
                    continue;
                
                var neighbors = new HashSet<BRDQTile>();

                CheckAdjacentBalls(tile.Key, tile.Value.BRDQColor, ref neighbors);

                if (neighbors.Count < 2) 
                    continue;
                
                return;
            }
            
            BRDQGameOver.BRDQWin();
        }
        
        private void CheckAdjacentBalls(Vector2Int gridPosition, EBRDQColor color, ref HashSet<BRDQTile> ballsToDestroy)
        {
            if (gridPosition.x < 0 || gridPosition.x >= BRDQSize.x || gridPosition.y < 0 || gridPosition.y >= BRDQSize.y || _brdqActiveTiles[gridPosition] == null || _brdqActiveTiles[gridPosition].BRDQColor != color || ballsToDestroy.Contains(_brdqActiveTiles[gridPosition]))
                return;

            ballsToDestroy.Add(_brdqActiveTiles[gridPosition]);

            var x = gridPosition.x;
            var y = gridPosition.y;
            
            CheckAdjacentBalls(new Vector2Int(x + 1, y), color, ref ballsToDestroy);
            CheckAdjacentBalls(new Vector2Int(x - 1, y), color, ref ballsToDestroy);
            CheckAdjacentBalls(new Vector2Int(x, y + 1), color, ref ballsToDestroy);
            CheckAdjacentBalls(new Vector2Int(x, y - 1), color, ref ballsToDestroy);
        }
        
        private void ShiftBallsDown()
        {
            for (var x = 0; x < BRDQSize.x; x++)
            {
                for (var y = 0; y < BRDQSize.y; y++)
                {
                    var xy = new Vector2Int(x, y);
                    
                    if (_brdqActiveTiles[xy] == null)
                    {
                        for (var i = y + 1; i < BRDQSize.y; i++)
                        {
                            var xi = new Vector2Int(x, i);

                            if (_brdqActiveTiles[xi] != null)
                            {
                                _brdqActiveTiles[xy] = _brdqActiveTiles[xi];
                                _brdqActiveTiles[xi] = null;
                                _brdqActiveTiles[xy].transform.DOMove(_brdqGridPositions[xy], 0.1f).SetEase(Ease.Linear);
                                _brdqActiveTiles[xy].BRDQPosition = xy;
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        private void ShiftBallsSideways()
        {
            for (var x = 0; x < BRDQSize.x; x++)
            {
                var hasBallsAbove = true;
                
                for (var y = 0; y < BRDQSize.y; y++)
                {
                    var xy = new Vector2Int(x, y);
                    
                    if (_brdqActiveTiles[xy] == null)
                        continue;
                    
                    hasBallsAbove = false;
                    
                    break;
                }
                
                if (!hasBallsAbove)
                    continue;
                
                if (x == 0 || x > BRDQSize.x)
                    continue;

                var moved = false;
                
                for (var y = 0; y < BRDQSize.y; y++)
                {
                    var xy = new Vector2Int(x - 1, y);
                    var xyNext = new Vector2Int(x, y);
                    
                    if (_brdqActiveTiles[xy] != null)
                    {
                        _brdqActiveTiles[xyNext] = _brdqActiveTiles[xy];
                        _brdqActiveTiles[xy] = null;
                        _brdqActiveTiles[xyNext].transform.DOMove(_brdqGridPositions[xyNext], 0.1f).SetEase(Ease.Linear);
                        _brdqActiveTiles[xyNext].BRDQPosition = xyNext;

                        moved = true;
                    }
                }

                if (!moved) 
                    continue;
                
                ShiftBallsSideways();
                return;
            }
        }
    }
}
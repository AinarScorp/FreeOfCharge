using System;
using System.Collections;
using System.Collections.Generic;
using DeliveryZoneInfo;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using LevelDesign.Obstacles;
using LevelDesign.Road;

namespace LevelDesign
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField][Range(0,100)] int _minObstaclesToSpawn=0;
        [SerializeField][Range(0,100)] int _maxObstacleToSpawn =1;

        [Header("Obstacle types")] 
        [Tooltip("here you can put different obstacles you want to be spawned")]
        [SerializeField] Obstacle[] _obstacleTypes;

        [Header("Delivery types")] [SerializeField]
        bool spawnObstacles = false;
        [Tooltip("here you can put different obstacles you want to be spawned")]
        [SerializeField] Delivery delivery;

        [Header("Sector Settings")]
        [SerializeField] float _sectorWidth = 10.0f; 
        [SerializeField] float _sectorHeight = 10.0f;

        [Header("amount of sectors")] 
        [SerializeField] int _width;
        [SerializeField]int _height;


        [Header("Gizmos for visualization")] 
        [SerializeField] bool _drawAlways = false;
        [SerializeField] bool _showObstacleBorders;
        [SerializeField] bool _showSectorLines;
        [SerializeField] float _spawnPointSizeMultiplier = 1.0f;
        [SerializeField] bool _showSpawnPoints;


        RoadSimulation _roadSimulation;
        List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();
        void Awake()
        {
            if (_roadSimulation == null)
            {
                _roadSimulation = FindObjectOfType<RoadSimulation>();
            }
        }

        void Start()
        {
            CreateSpawnPoints();
            if (spawnObstacles)
            {
                
                SpawnObstacles();
            }
            else
            {
                SpawnDeliveries();
            }
        }

        void SpawnDeliveries()
        {
            int maxToSpawn = Mathf.Max(_minObstaclesToSpawn, _maxObstacleToSpawn);
            int minToSpawn = Mathf.Min(_minObstaclesToSpawn, _maxObstacleToSpawn);
            int amountOfSpawns = -1;

            if (minToSpawn == maxToSpawn) amountOfSpawns = minToSpawn;
            else
            {
                int randomAmountOfSpanws = Random.Range(minToSpawn, maxToSpawn);
                amountOfSpawns = randomAmountOfSpanws;
            }

            for (int i = 0; i < amountOfSpawns; i++)
            {
                if (_spawnPoints.Count < 1) return;
                int randomSpanwPointIndex = Random.Range(0, _spawnPoints.Count);

                if (_obstacleTypes.Length < 1) return;
                int randomObstacleIndex = Random.Range(0, _obstacleTypes.Length);

                SpawnPoint spawnPoint = _spawnPoints[randomSpanwPointIndex];
                Instantiate(delivery, spawnPoint._position, delivery.transform.rotation, this.transform);
                _spawnPoints.Remove(spawnPoint);
            }
        }
        void SpawnObstacles()
        {
            
            int maxToSpawn = Mathf.Max(_minObstaclesToSpawn, _maxObstacleToSpawn);
            int minToSpawn = Mathf.Min(_minObstaclesToSpawn, _maxObstacleToSpawn);
            int amountOfSpawns = -1;

            if (minToSpawn == maxToSpawn) amountOfSpawns = minToSpawn;
            else
            {
                int randomAmountOfSpanws = Random.Range(minToSpawn, maxToSpawn);
                amountOfSpawns = randomAmountOfSpanws;
            }

            for (int i = 0; i < amountOfSpawns; i++)
            {
                if (_spawnPoints.Count < 1) return;
                int randomSpanwPointIndex = Random.Range(0, _spawnPoints.Count);

                if (_obstacleTypes.Length < 1) return;
                int randomObstacleIndex = Random.Range(0, _obstacleTypes.Length);

                SpawnPoint spawnPoint = _spawnPoints[randomSpanwPointIndex];
                Obstacle obstacleToSpawn = _obstacleTypes[randomObstacleIndex];
                Instantiate(obstacleToSpawn, spawnPoint._position, Quaternion.identity, this.transform);
                _spawnPoints.Remove(spawnPoint);
            }
        }

        void CreateSpawnPoints()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Vector3 middlePos = new Vector3((x + 0.5f) * _sectorWidth, 0, (y + 0.5f) * _sectorHeight) + transform.position;
                    SpawnPoint spawnPoint = new SpawnPoint(middlePos);
                    _spawnPoints.Add(spawnPoint);
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            if (!_drawAlways)
            {
                CreateGizmos();

            }
        }

        void OnDrawGizmos()
        {
            if (_drawAlways)
            {
                CreateGizmos();
                
            }
        }

        void CreateGizmos()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    #region Lines

                    if (_showSectorLines)
                    {
                        Vector3 fromPos = new Vector3(x * _sectorWidth, 0, y * _sectorHeight) + transform.position;

                        Vector3 toHorizontalPos = new Vector3((x + 1) * _sectorWidth, 0, y * _sectorHeight) + transform.position;
                        Vector3 toVericalPos = new Vector3(x * _sectorWidth, 0, (y + 1) * _sectorHeight) + transform.position;
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(fromPos, toHorizontalPos);
                        Gizmos.color = Color.blue;
                        Gizmos.DrawLine(fromPos, toVericalPos);
                        Gizmos.color = Color.black;
                        Gizmos.DrawCube(fromPos, Vector3.one);
                        if (x == _width || y == _height)
                        {
                            Gizmos.DrawCube(toHorizontalPos, Vector3.one);
                            Gizmos.DrawCube(toVericalPos, Vector3.one);
                        }
                    }

                    #endregion


                    if (_showSpawnPoints)
                    {
                        Gizmos.color = Color.magenta;
                        Vector3 middlePos = new Vector3((x + 0.5f) * _sectorWidth, 0, (y + 0.5f) * _sectorHeight) + transform.position;
                        Gizmos.DrawCube(middlePos, Vector3.one * _spawnPointSizeMultiplier);
                    }
                }
            }

            #region Lines

            if (_showSectorLines)
            {
                Gizmos.color = Color.black;
                for (int x = 0; x <= _width; x++)
                {
                    Vector3 toHorizontalPos = new Vector3(x * _sectorWidth, 0, _height * _sectorHeight) + transform.position;
                    Gizmos.DrawCube(toHorizontalPos, Vector3.one);
                }

                for (int y = 0; y < _height; y++)
                {
                    Vector3 toVericalPos = new Vector3(_width * _sectorWidth, 0, y * _sectorHeight) + transform.position;
                    Gizmos.DrawCube(toVericalPos, Vector3.one);
                }
            }

            #endregion

            if (_showObstacleBorders)
            {
                Gizmos.color = Color.green;
                Vector3 bottomLeftCorner = transform.position;
                Vector3 topLeftCorner = new Vector3(0, 0, _sectorHeight * _height) + transform.position;
                Vector3 topRightCorner = new Vector3(_sectorWidth * _width, 0, _sectorHeight * _height) + transform.position;
                Vector3 bottomRightCorner = new Vector3(_sectorWidth * _width, 0, 0) + transform.position;
                Gizmos.DrawLine(topLeftCorner, topRightCorner);
                Gizmos.DrawLine(topLeftCorner, bottomLeftCorner);
                Gizmos.DrawLine(topRightCorner, bottomRightCorner);
                Gizmos.DrawLine(bottomLeftCorner, bottomRightCorner);
            }
        }
    }

    public class SpawnPoint
    {
        public bool _isTaken;
        public Vector3 _position;

        public SpawnPoint(Vector3 position)
        {
            _position = position;
            _isTaken = false;
        }
    }


}

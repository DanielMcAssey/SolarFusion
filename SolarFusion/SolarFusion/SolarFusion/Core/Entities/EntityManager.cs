using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameData;

#if XBOX
using Containers;
#endif

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SolarFusion.Core
{
    public class EntityManager
    {
        ContentManager virtualContent;
        uint objectCount = 0;
        Dictionary<uint, GameObjects> gameObjects;
        Queue<GameObjects> createdGameObjects;
        Queue<GameObjects> destroyedGameObjects;
        public List<uint> dynamicObjects;
        public List<uint> projectileObjects;
        public Camera2D camera;
        protected Vector2 mGravity;

        //Collision Detection
        Dictionary<uint, BoundingBoxes> boundingBoxes;
        List<Bound> horizontalAxis;
        HashSet<CollisionPair> horizontalOverlaps;
        HashSet<CollisionPair> collisions;

        #region "Properties"
        public Vector2 Gravity
        {
            get { return this.mGravity; }
        }
        #endregion

        public EntityManager(ContentManager _content)
        {
            this.virtualContent = _content; //Gets the ContentManager passed.
            gameObjects = new Dictionary<uint, GameObjects>();
            this.mGravity = new Vector2(0f, -198.0f);

            createdGameObjects = new Queue<GameObjects>();
            destroyedGameObjects = new Queue<GameObjects>();

            boundingBoxes = new Dictionary<uint, BoundingBoxes>();
            dynamicObjects = new List<uint>();
            horizontalAxis = new List<Bound>(256);
            horizontalOverlaps = new HashSet<CollisionPair>();
            collisions = new HashSet<CollisionPair>();
        }

        public HashSet<CollisionPair> Collisions
        {
            get { return collisions; } //Returns the Hash Set.
        }

        public void Reset()
        {
            gameObjects.Clear(); //Clears all of the data for a reset.
            createdGameObjects.Clear();
            destroyedGameObjects.Clear();
            boundingBoxes.Clear();
            dynamicObjects.Clear();
            projectileObjects.Clear();
            horizontalAxis.Clear();
            horizontalOverlaps.Clear();
            collisions.Clear();
        }

        public void Update(GameTime gameTime)
        {
            while (createdGameObjects.Count > 0)
            {
                GameObjects go = createdGameObjects.Dequeue();
                if (go is AI || go is PowerUp || go is LevelObject || go is Blast || go is Player)
                    dynamicObjects.Add(go.ID);

                AddGameObject(go);
            }

            while (destroyedGameObjects.Count > 0)
            {
                GameObjects go = destroyedGameObjects.Dequeue();
                if (go is AI || go is PowerUp || go is LevelObject || go is Blast || go is Player)
                {
                    dynamicObjects.Remove(go.ID);
                }

                RemoveGameObject(go);
            }

            UpdateAxisLists();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObjects go in gameObjects.Values)
            {
                go.Draw(spriteBatch);
            }
        }

        public GameObjects GetObject(uint id)
        {
            return gameObjects[id];
        }

        public GameObjects DestroyObject(uint id)
        {
            GameObjects go = gameObjects[id];
            destroyedGameObjects.Enqueue(go);
            return go;
        }

        public uint[] QueryRegion(Rectangle bounds)
        {
            HashSet<uint> queryMatches = new HashSet<uint>(); //Create a new HashSet to compare matches
            foreach (uint goID in this.dynamicObjects)
                if (bounds.Intersects(this.GetObject(goID).Bounds))
                    queryMatches.Add(goID);

            return queryMatches.ToArray();
        }

        public uint NextID()
        {
            uint id = objectCount;
            objectCount++;
            return id;
        }

        private void QueueGameObjectForCreation(GameObjects go)
        {
            createdGameObjects.Enqueue(go);
        }

        public PowerUp CreatePowerup(PowerUpType powerupType, Vector2 position)
        {
            PowerUp powerup;
            uint id = NextID();

            switch (powerupType)
            {
                case PowerUpType.EnergyBall:
                    powerup = new PowerUp_EnergyBall(id, virtualContent, position);
                    break;
                case PowerUpType.Crystal:
                    powerup = new PowerUp_Crystal(id, virtualContent, position);
                    break;
                case PowerUpType.Warp:
                    powerup = new PowerUp_Warp(id, virtualContent, position);
                    break;
                default:
                    powerup = new PowerUp_EnergyBall(id, virtualContent, position);
                    break;
            }

            powerup.ObjectType = ObjectType.PowerUp;
            QueueGameObjectForCreation(powerup);
            return powerup;
        }

        public AI CreateEnemy(EnemyType enemyType, Vector2 position)
        {
            AI enemy;
            uint id = NextID();

            switch (enemyType)
            {
                case EnemyType.MercBot:
                    enemy = new Enemy_MercBot(id, virtualContent, position);
                    break;
                default:
                    enemy = new Enemy_MercBot(id, virtualContent, position);
                    break;
            }

            enemy.ObjectType = ObjectType.Enemy;
            QueueGameObjectForCreation(enemy);
            return enemy;
        }

        public Blast CreateBullet(Blast _bullet)
        {
            QueueGameObjectForCreation(_bullet);
            return _bullet;
        }

        public Player CreatePlayer(Player _player)
        {
            QueueGameObjectForCreation(_player);
            return _player;
        }

        public LevelObject CreateLevelObject(LevelObjectType levelObjectType, Vector2 position)
        {
            LevelObject levelobject;
            uint id = NextID();

            switch (levelObjectType)
            {
                case LevelObjectType.Solid:
                    levelobject = new LevelObject_Solid(id, virtualContent, position);
                    break;
                case LevelObjectType.NonSolid:
                    levelobject = new LevelObject_NonSolid(id, virtualContent, position);
                    break;
                case LevelObjectType.Crate:
                    levelobject = new LevelObject_Crate(id, virtualContent, position);
                    break;
                default:
                    levelobject = new LevelObject_Crate(id, virtualContent, position);
                    break;
            }

            levelobject.ObjectType = ObjectType.LevelObject;
            QueueGameObjectForCreation(levelobject);
            return levelobject;
        }

        private void UpdateAxisLists()
        {
            HashSet<CollisionPair> overlaps = new HashSet<CollisionPair>();
            int i = 0;
            int j = 0;

            for (i = 1; i < horizontalAxis.Count; i++)
            {
                Bound bound = horizontalAxis[i];
                j = i - 1;

                while ((j >= 0) && horizontalAxis[j].CompareTo(bound) > 0)
                {
                    if (horizontalAxis[j].Type == BoundType.Min && bound.Type == BoundType.Max)
                    {
                        collisions.Remove(new CollisionPair(horizontalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                        horizontalOverlaps.Remove(new CollisionPair(horizontalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                    }
                    else if (horizontalAxis[j].Type == BoundType.Max && bound.Type == BoundType.Min)
                    {
                        horizontalOverlaps.Add(new CollisionPair(horizontalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                    }

                    horizontalAxis[j + 1] = horizontalAxis[j];
                    j--;
                }
                horizontalAxis[j + 1] = bound;
            }

            foreach (CollisionPair pair in horizontalOverlaps)
            {
                GameObjects A = GetObject(pair.A);
                GameObjects B = GetObject(pair.B);
                if (A.Bounds.Intersects(B.Bounds))
                {
                    collisions.Add(pair);
                }
            }
        }

        private int InsertBoundIntoAxis(List<Bound> axis, Bound bound)
        {
            int index = axis.BinarySearch(bound);
            if (index < 0)
            {
                index = ~index;
                axis.Insert(index, bound);
            }
            else
            {
                axis.Insert(index, bound);
            }

            return index;
        }

        private void AddGameObject(GameObjects gameObject)
        {
            uint id = gameObject.ID;

            gameObjects.Add(id, gameObject);

            BoundingBoxes box = new BoundingBoxes(id, gameObject.Bounds);
            boundingBoxes.Add(id, box);

            int leftIndex = InsertBoundIntoAxis(horizontalAxis, box.Left);
            int rightIndex = InsertBoundIntoAxis(horizontalAxis, box.Right);

            for (int i = leftIndex + 1; i < rightIndex; i++)
            {
                horizontalOverlaps.Add(new CollisionPair(id, horizontalAxis[i].Box.GameObjectID));
            }
        }

        public void UpdateGameObject(uint gameObjectID)
        {
            BoundingBoxes box = boundingBoxes[gameObjectID];

            GameObjects go = gameObjects[gameObjectID];

            box.Left.Value = go.Bounds.Left;
            box.Right.Value = go.Bounds.Right;
            box.Top.Value = go.Bounds.Top;
            box.Bottom.Value = go.Bounds.Bottom;
        }

        private void RemoveGameObject(GameObjects gameObject)
        {
            uint id = gameObject.ID;
            gameObjects.Remove(id);

            CollisionPair[] pairs = collisions.ToArray();
            foreach (CollisionPair pair in pairs)
            {
                if (pair.A == id || pair.B == id)
                {
                    collisions.Remove(pair);
                }
            }

            CollisionPair[] pairs2 = horizontalOverlaps.ToArray();
            foreach (CollisionPair pair in pairs2)
            {
                if (pair.A == id || pair.B == id)
                {
                    horizontalOverlaps.Remove(pair);
                }
            }

            BoundingBoxes box;

            try
            {
                box = boundingBoxes[id];
            }
            catch (KeyNotFoundException ex)
            {
                return;
            }

            horizontalAxis.Remove(box.Left);
            horizontalAxis.Remove(box.Right);

            boundingBoxes.Remove(id);
        }
    }
}

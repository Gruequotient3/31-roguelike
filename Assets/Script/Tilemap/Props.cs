using UnityEngine;
using System.Collections.Generic;

using Roguelike.Utils;
using Roguelike.Resources;
using UnityEngine.SceneManagement;

namespace Roguelike.Tilemap.NProp{
    
    public enum PropType
    {
        NONE,
        PINE_TREE,
        OAK_TREE,
        ROCK,
        ALTAR,
        BROWN_MUSHROOM,
        COTTON,
        RED_TULIP,
        WHITE_TULIP,
        CRAFTING_STATION,
        SNOWY_PINE_TREE,
        SNOWY_OAK_TREE,
    }

    [CreateAssetMenu(fileName = "PropsData", menuName = "Scriptable Objects/Tilemap/Props Data")]
    public class PropData : ScriptableObject
    {
        public GameObject prefabs;
        public PropType type;
        public Vector2Int size = new Vector2Int(1, 1);
        public bool isPlacedByPlayer; // To differenciate from decorative placed props
        public LootTable lootTable = new LootTable();
        // TODO Configuration to break

        void OnValidate()
        {
            if (size.x < 1) size.x = 1;
            if (size.y < 1) size.y = 1;
        }
    }

    public class Prop
    {
        protected static Dictionary<PropType, PropData> _props = ResourcesManager.LoadPropData();
        public GameObject gameObject = null;

        public Vector3Int position;
        protected PropData _propsData;

        public Prop(Vector3Int position, PropData propsData)
        {
            this.position = position;
            _propsData    = propsData;   
        }

        public void Destroy()
        {
            if (gameObject) UnityEngine.Object.Destroy(gameObject);
            gameObject = null;
        }

        public virtual void Interract() { }

        public void Generate()
        {
            if (gameObject) UnityEngine.Object.Destroy(gameObject);
            gameObject = Object.Instantiate(_propsData.prefabs);
            
            Transform[] transforms = gameObject.transform.GetComponentsInChildren<Transform>();
            // Set Sprites Sorting Order
            foreach(Transform tr in transforms)
            {
                SpriteRenderer sp = tr.GetComponent<SpriteRenderer>();
                if (sp == null) continue;
                SetSortingOrder(sp, (int)(position.z + tr.localPosition.y * 3.0));
            }

            Vector3 pos = Coordinate.IsoToWorld(position);
            gameObject.transform.position = new Vector3(
                                                pos.x + ((float)Size.x - 1.0f) / Mathf.Pow(2.0f, (float)Size.x), 
                                                pos.y + ((float)Size.y - 1.0f) / Mathf.Pow(2.0f, (float)Size.y), 
                                                pos.z
                                            );
        }
        
        public void SetSortingOrder(SpriteRenderer sp, int z)
        {
            int average = 0;
            for (int y = -1; y <= 1; ++y)
            {
                for (int x = -1; x <= 1; ++x)
                {
                    average += -1 * (x + position.x + y + position.y); 
                }
            }
            sp.sortingOrder = average / 9 + z + 1;
        }

        public void SetOutlineValue(bool value)
        {
            if (!gameObject) return;
            SpriteRenderer[] sps = gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer sp in sps)
            {
                if (sp == null) continue;
                sp.material.SetInt("_Active", value ? 1 : 0);
            }
        }

        public static Prop GetPropFromType(PropType type, Vector3Int position)
        {
            return type switch {
                PropType.PINE_TREE => new PineTree(position),
                PropType.OAK_TREE => new OakTree(position),
                PropType.ROCK => new Rock(position),
                PropType.ALTAR => new Altar(position),
                PropType.BROWN_MUSHROOM => new BrownMushroom(position),
                PropType.COTTON => new Cotton(position),
                PropType.RED_TULIP => new RedTulip(position),
                PropType.WHITE_TULIP => new WhiteTulip(position),
                PropType.CRAFTING_STATION => new CraftingStation(position),
                PropType.SNOWY_PINE_TREE => new SnowyPineTree(position),
                PropType.SNOWY_OAK_TREE => new SnowyOakTree(position),
                _             => null
            };
        }

        public PropType Type    { get => _propsData.type; }
        public Vector2Int Size  { get => _propsData.size; }
        public bool IsPlacedByPlayer 
        {
            get => _propsData.isPlacedByPlayer; 
            set => _propsData.isPlacedByPlayer = value;
        }
        public LootTable LootTable { get => _propsData.lootTable; }
    }

#region PropType
    public class PineTree : Prop
    {
        public PineTree(Vector3Int position) : 
            base(position, _props[PropType.PINE_TREE]) 
        { }
    }

    public class OakTree : Prop
    {
        public OakTree(Vector3Int position) : 
            base(position, _props[PropType.OAK_TREE]) 
        { }
    }

    public class Rock : Prop
    {
        public Rock(Vector3Int position) : 
            base(position, _props[PropType.ROCK]) 
        { }
    }

    public class Altar : Prop
    {
        public Altar(Vector3Int position) : 
            base(position, _props[PropType.ALTAR]) 
        { }
        public override void Interract()
        {
            GameManager.singleton.worldSeed = new System.Random().Next(-100000, 100000);
            SceneManager.LoadSceneAsync(1);
        }
    }

    public class BrownMushroom : Prop
    {
        public BrownMushroom(Vector3Int position) : 
            base(position, _props[PropType.BROWN_MUSHROOM]) 
        { }
    }

    public class Cotton : Prop
    {
        public Cotton(Vector3Int position) : 
            base(position, _props[PropType.COTTON]) 
        { }
    }

    public class RedTulip : Prop
    {
        public RedTulip(Vector3Int position) : 
            base(position, _props[PropType.RED_TULIP]) 
        { }
    }

    public class WhiteTulip : Prop
    {
        public WhiteTulip(Vector3Int position) : 
            base(position, _props[PropType.WHITE_TULIP]) 
        { }
    }

    public class CraftingStation : Prop
    {
        public CraftingStation(Vector3Int position) : 
            base(position, _props[PropType.CRAFTING_STATION]) 
        { }
    }

    public class SnowyPineTree : Prop
    {
        public SnowyPineTree(Vector3Int position) : 
            base(position, _props[PropType.SNOWY_PINE_TREE]) 
        { }
    }

    public class SnowyOakTree : Prop
    {
        public SnowyOakTree(Vector3Int position) : 
            base(position, _props[PropType.SNOWY_OAK_TREE]) 
        { }
    }

#endregion
};


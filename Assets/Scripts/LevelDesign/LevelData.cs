using System;
using System.Collections.Generic;
using GameScene.BuildingMap;
using GameScene.Inventory;
using UnityEngine;

namespace LevelDesign
{
    [CreateAssetMenu(fileName = "New LevelData", menuName = "LevelData")]
    public class LevelData : ScriptableObject
    {
        public int level;
        public int startActionsLeft;
        public int moneyNeeded;
        public Vector2Int playerStartPosition;

        public List<DialogNode> IntroTexts = new List<DialogNode>();

        public Vector2Int LevelDimensions;
        public List<StreetData> Streets = new List<StreetData>();
        public List<RiverData> Rivers = new List<RiverData>();
        public List<BridgeData> Bridges = new List<BridgeData>();

        public List<ConsumerData> Consumers = new List<ConsumerData>();
        public List<SupplierData> Suppliers = new List<SupplierData>();
        
        public List<TrafficLightData> TrafficLights = new List<TrafficLightData>();
        public List<PoliceData> Polices = new List<PoliceData>();
        public List<DogData> Dogs = new List<DogData>();
    }

    [Serializable]
    public class BridgeData
    {
        public Vector2Int Position;
        public BridgeType Type;
    }

    [Serializable]
    public class RiverData
    {
        public Vector2Int Position;
    }
    
    [Serializable]
    public class StreetData
    {
        public Vector2Int Position;
    }

    [Serializable]
    public class ConsumerData
    {
        public ItemType Type = ItemType.PIZZA;
        public HouseType HouseType = HouseType.Little;
        public Vector2Int Position;
        public SignPosition SignPosition = SignPosition.Top;
    }

    [Serializable]
    public class SupplierData
    {
        public ItemType Type;
        public Vector2Int Position;
        public int SuppliedItems;
        public SignPosition SignPosition;
    }

    [Serializable]
    public class TrafficLightData
    {
        public Vector2Int Position;
        public TrafficLightStatus StartStatus;
    }

    [Serializable]
    public class PoliceData
    {
        public Vector2Int Position;
    }

    [Serializable]
    public class DogData
    {
        public Vector2Int Position;
    }
    
    [Serializable]
    public class DialogNode
    {
        [TextArea(2, 5)]
        public string Text;
        public bool ShowMafiaGuy;

        public DialogNode(string text, bool showMafiaGuy)
        {
            this.Text = text;
            this.ShowMafiaGuy = showMafiaGuy;
        }
    }

    public enum HouseType
    {
        Little,
        Medium,
        Tall
    }

    public enum BridgeType
    {
        Horizontal,
        Vertical
    }
    
    public enum SignPosition
    {
        Left,
        Top,
        Right,
        Bottom
    }
}
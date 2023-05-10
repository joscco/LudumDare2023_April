using System.Collections.Generic;
using LevelDesign;
using UnityEngine;

namespace GameScene.Buildings.Consumers
{
    public class ConsumerBuildingManager : BuildingManager<ConsumerBuilding>
    {
        [SerializeField] private ConsumerBuilding littleConsumerPrefab;
        [SerializeField] private ConsumerBuilding mediumConsumerPrefab;
        [SerializeField] private ConsumerBuilding largeConsumerPrefab;

        public void InitConsumers(List<ConsumerData> consumers)
        {
            foreach (var consumer in consumers)
            {
                ConsumerBuilding building;
                switch (consumer.HouseType)
                {
                    case HouseType.Little:
                        building = Instantiate(littleConsumerPrefab, transform);
                        break;
                    case HouseType.Medium:
                        building = Instantiate(mediumConsumerPrefab, transform);
                        break;
                    default:
                        building = Instantiate(largeConsumerPrefab, transform);
                        break;
                }
                
                building.demandedItems = 1;
                building.demandedItemType = consumer.Type;
                building.InitHint(consumer.SignPosition, -building.demandedItems);
                AddBuildingAt(building, consumer.Position);
            }
        }
    }
}
ECS implementation of damage bubbles
---------

![bubbles](https://github.com/nicloay/ecs-damage-bubbles/assets/1671030/a782751a-1470-4059-9e8b-0e5ec19ed246)

Installation
-------------
Prerequisities:
* Unity 2022.2
* URP render pipeline

1. open manifest.json and add following dependency to the project
   
   ```"com.mycompany.mypackage": "https://github.com/nicloay/ecs-damage-bubbles.git?path=/Assets/EcsDamageBubbles```
2. From the PackageManager windows import Demo sample

   ![image](https://github.com/nicloay/ecs-damage-bubbles/assets/1671030/02719c12-8e5d-4387-81f1-a07f374fdd34)
3. Open SampleScene. It does all required systems and configurations

Setup existing scene
-------------------
1. Search for **DamageBubblesConfig** prefab in the packages, and drag and drop it to your Entities scene
2. Create Entities and add following components to spawn damage bubbles in your systems
   
```csharp
var entity = EntityManager.CreateEntity()
EntityManager.AddComponent(entity, new DamageRequest
{
   Value = _rnd.NextInt(1, 999999),
   ColorId = colorId
});
```
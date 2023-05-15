ECS implementation of damage bubbles
---------

![bubbles](https://github.com/nicloay/ecs-damage-bubbles/assets/1671030/a782751a-1470-4059-9e8b-0e5ec19ed246)

Installation
-------------
Prerequisites:
* Unity 2022.2
* URP render pipeline

1. Install the package in one of the following ways:
   1. Open `manifest.json` and add the following dependency to your project:
   
      ```"com.nicloay.ecsdamagebubbles": "https://github.com/nicloay/ecs-damage-bubbles.git?path=/Assets/EcsDamageBubbles```
   2. Alternatively, install from the Git URL in the Package Manager. Copy the following link:
      
      ```https://github.com/nicloay/ecs-damage-bubbles.git?path=/Assets/EcsDamageBubbles```

      Then, paste it into the Package Manager's "Add package from git URL" field.
     
      ![image](https://github.com/nicloay/ecs-damage-bubbles/assets/1671030/b5993256-a595-4167-ac8b-2829f0ee10c2)
2. Import the demo sample from the Package Manager window.

   ![image](https://github.com/nicloay/ecs-damage-bubbles/assets/1671030/02719c12-8e5d-4387-81f1-a07f374fdd34)
3. Open the SampleScene to access the demo project with all required systems and configurations.

Setup Existing Scene
-------------------
To set up damage bubbles in your existing scene, follow these steps:

1. Search for the `DamageBubblesConfig` prefab in the package and drag and drop it into your Entities scene.
2. Create entities and add the following components to spawn damage bubbles in your systems:
   
```csharp
var entity = EntityManager.CreateEntity()
EntityManager.AddComponent(entity, new DamageRequest
{
   Value = _rnd.NextInt(1, 999999),
   ColorId = colorId
});
```

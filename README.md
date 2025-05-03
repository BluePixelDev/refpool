# RefPool
[![GitHub Repo stars](https://img.shields.io/github/stars/BluePixelDev/refpool?style=flat-square)](https://github.com/BluePixelDev/refpool/stargazers)
[![GitHub last commit](https://img.shields.io/github/last-commit/BluePixelDev/refpool?style=flat-square)](https://github.com/BluePixelDev/refpool/commits/main)
[![GitHub issues](https://img.shields.io/github/issues/bluepixeldev/refpool?style=flat-square)](https://github.com/bluepixeldev/refpool/issues)
[![Unity Version](https://img.shields.io/badge/unity-6.0%2B-green?style=flat-square)](https://unity.com/releases/editor)

**RefPool** is a lightweight, ScriptableObject-driven pooling system for Unity 6.0 and above.  
It enables clean, efficient reuse of GameObjects across scenes without relying on global registries.

## Features
- ScriptableObject-based pool references for cross-scene linking
- Lazy pool initialization and automatic cleanup on unload
- Efficient memory management through shared pooling
- Supports runtime spawning with configurable options such as spawn rate, position overrides, and randomization
- Clean integration with Unity’s component model
- Extendable through custom `PoolResource` implementations for advanced pooling behaviors

## Installation

### Unity Package Manager (via Git URL)

1. Open your Unity project
2. Go to `Packages/manifest.json`
3. Add the following line to the `dependencies` block:

```json
"com.bluepixeldev.refpool": "https://github.com/bluepixeldev/refpool.git"
```

### Manual Import

Clone or download this repository and move the `RefPool` folder into your project's `Assets` directory.

## Usage

### Create a Pool Resource

1. Right-click in the `Project` window
2. Select `Create > RefPool > Pool or Pool Group`
3. Assign the prefab and configure pool size, increment size, and other settings

### Add a RefPoolSpawner

1. Attach the `RefPoolSpawner` component to a GameObject
2. Assign the `PoolResource` you created
3. Configure spawn behavior as needed

### Trigger Spawning

You can trigger pooled object spawning manually or automatically:

```csharp
[SerializeField] private PoolSpawner spawner;

void SomeEventTrigger()
{
    spawner.Spawn();
}
```

### Extending with Custom Pool Behaviors

RefPool allows you to implement custom pooling logic by extending the `PoolResource` class. This enables advanced use cases such as dynamic pool resizing, custom initialization, or specialized cleanup logic.

Here’s an example of a custom `PoolResource`:

```csharp
using UnityEngine;

namespace BP.RefPool
{
    public class CustomPoolResource : PoolResource
    {
        public override void Initialize()
        {
            // Custom initialization logic
        }

        public override GameObject Get()
        {
            // Custom logic for retrieving a GameObject from the pool
            return null;
        }

        public override bool Release(GameObject gameObject)
        {
            // Custom logic for releasing a GameObject back to the pool
            return true;
        }
    }
}
```

This flexibility allows you to tailor the pooling system to your specific project requirements.

## Contributing

Pull requests and issues are welcome. If you encounter a bug or have a feature suggestion, please open an issue on GitHub.
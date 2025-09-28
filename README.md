# RefPool

[![GitHub Repo stars](https://img.shields.io/github/stars/BluePixelDev/refpool?style=flat-square)](https://github.com/BluePixelDev/refpool/stargazers)
[![GitHub last commit](https://img.shields.io/github/last-commit/BluePixelDev/refpool?style=flat-square)](https://github.com/BluePixelDev/refpool/commits/main)
[![GitHub issues](https://img.shields.io/github/issues/bluepixeldev/refpool?style=flat-square)](https://github.com/bluepixeldev/refpool/issues)
[![Unity Version](https://img.shields.io/badge/unity-6.0%2B-green?style=flat-square)](https://unity.com/releases/editor)

**RefPool** is a lightweight, high-performance pooling system for **Unity 6** and above, designed to efficiently manage GameObjects through ScriptableObjects.

## Features

- Efficient GameObject pooling
- Works seamlessly with Unity 6 and above
- Uses ScriptableObjects for easy configuration and management
- Supports nested pooling through Groups for hierarchical pooling

## Table of Contents

- [RefPool](#refpool)
  - [Features](#features)
  - [Table of Contents](#table-of-contents)
  - [Installation](#installation)
    - [1. Open Unity Package Manager](#1-open-unity-package-manager)
    - [2. Add Package](#2-add-package)
  - [Usage](#usage)
    - [Creating Pools](#creating-pools)
    - [Pools vs Groups](#pools-vs-groups)
    - [Adding Components](#adding-components)
    - [Example Usage](#example-usage)
  - [Contributing](#contributing)
    - [How to Contribute:](#how-to-contribute)
  - [License](#license)

## Installation

### 1. Open Unity Package Manager

In Unity, go to the **Toolbar** -> **Window** -> **Package Management** -> **Package Manager**

### 2. Add Package

1. In the Package Manager window, click the + button at the top left.
2. Select **Install package from git URL...**
3. Paste the following URL: `https://github.com/BluePixelDev/refpool.git`

## Usage

### Creating Pools

To create a **Referenced Pool** in your project:

1. Go to the **Project** window and select a directory where you want to create the pool.
2. Right-click in the directory and select: **Create** -> **RefPool** -> **Pool**
3. Configure the pool using the Inspector. The Pool Asset is the primary endpoint for retrieving and releasing GameObjects.

### Pools vs Groups

Pools are used to manage individual collections of GameObjects. Each pool holds and manages a specific type of object.

**Groups** allow you to organize pools into hierarchical structures. You can nest multiple pools inside a group, enabling more granular control over different object types.

For example, you can create a pool for Debris and then use groups to separate Small Debris and Large Debris into separate pools.

### Adding Components

**RefPool** also includes components that can be added to **GameObjects** for pooling functionality.

- RefPooler
- RefGroupPooler

- RefSpawner (Used to spawn pooled objects)
- RefPlacer (Used alongside `RefSpawner` to place objects.)

### Example Usage

```cs
using BP.RefPool;
public class Example : MonoBehaviour
{
    public RefResource resource; // Base class for both PoolAsset and PoolGroupAsset

    public void Start() {
        // Optionally, prepare the pool before using it
        // resource.Prepare();

        // Retrieve a pooled item (RefItem is a MonoBehaviour)
        RefItem pooledItem = resource.Get()

        // Manipulate the pooled item (e.g., set position, etc.)
        pooledItem.transform.position = Vector3.zero;

        // Once done, release the item back to the pool for reuse
        pooledItem.Release()
    }
}
```
In this example:
- ``RefResource`` is a base class used for both PoolAssets and PoolGroupAssets.
- ``Get()`` retrieves a pooled item, which is a RefItem (a MonoBehaviour for managing the pooled objects).
- ``Release()`` returns the object to the pool when no longer needed.

## Contributing

Contributions are welcome! If you find a bug or have a feature request, feel free to create an issue or submit a pull request.

### How to Contribute:

1. Fork the repository.
2. Create a feature branch (`git checkout -b feature-branch`).
3. Commit your changes (`git commit -am 'Add new feature'`).
4. Push to the branch (`git push origin feature-branch`).
5. Create a new pull request.

I’ll review your changes and merge them after approval.

## License

This project is licensed under the MIT License. See the [LICENSE](https://github.com/BluePixelDev/refpool/blob/master/LICENSE)

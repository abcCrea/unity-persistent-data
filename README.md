A lightweight local persistence module for Unity using PlayerPrefs for primitive values and JSON files for classes and dictionaries with a playable top-down 2D sample demonstrating persistent currencies collected from treasure chests.

# Unity Persistent Data

A lightweight and reusable local persistence module for Unity.

It provides a centralized API for storing primitive values with `PlayerPrefs` and saving serializable classes and dictionaries as JSON files.

The package also includes a playable top-down 2D sample where the player collects persistent rewards from treasure chests.

<img width="1442" height="877" alt="abcCrea_Unity_PersistentData_Sample" src="https://github.com/user-attachments/assets/49a2a755-9abe-4d38-bc8b-5bb3eeb8764a" />


## Who Is This Package For?

This package is intended for:

- Beginners learning how local persistence works in Unity.
- Developers creating prototypes or game jam projects.
- Small offline games that need a straightforward save system.
- Developers who want a simple wrapper around `PlayerPrefs` and `JsonUtility`.
- Educational projects that benefit from an understandable implementation.

It is suitable for storing settings, currencies, progression, checkpoints, unlock states, small inventories, and other lightweight local data.

## Features

- Save and load `int`, `float`, `string`, `bool`, and `Vector3`.
- Save and load serializable classes as JSON.
- Save and load dictionaries through a serializable wrapper.
- Delete individual primitive values.
- Delete individual class or dictionary JSON files.
- Delete all PlayerPrefs and top-level JSON save files.
- Store and load a save-data version.
- Events for save, load, and delete-all operations.
- Editor debug methods for testing primitive persistence.
- Playable treasure collection sample.
- No external runtime library dependencies.
- Uses only native Unity APIs.

## Requirements

- Unity 2022.3 or newer.
- TextMeshPro 3.0.6 or newer.
- Git installed when importing through a Git URL.

## Installation

### Install from a Git URL

In Unity:

1. Open **Window > Package Manager**.
2. Press the **+** button.
3. Select **Add package from Git URL**.
4. Enter:

```text
https://github.com/abcCrea/unity-persistent-data.git#1.0.1
```

To install the current development version from the default branch, use:

```text
https://github.com/abcCrea/unity-persistent-data.git
```

Using a version tag is recommended for production projects.

## Quick Start

```csharp
using abcCrea.PersistentData;
using UnityEngine;

public class CurrencyExample : MonoBehaviour
{
    private const string CoinsKey = "Coins";

    private void Start()
    {
        int coins = PersistentDataManager.LoadInt(CoinsKey, 0);
        Debug.Log($"Loaded coins: {coins}");
    }

    public void AddCoins(int amount)
    {
        int coins = PersistentDataManager.LoadInt(CoinsKey, 0);
        coins += amount;

        PersistentDataManager.SaveInt(CoinsKey, coins);
        PlayerPrefs.Save();
    }
}
```

Primitive values use Unity's `PlayerPrefs`.

`SaveInt`, `SaveFloat`, `SaveString`, `SaveBool`, and `SaveVector3` set their values but do not call `PlayerPrefs.Save()` automatically. Call it when you want to explicitly flush pending values to disk.

## Saving a Serializable Class

```csharp
[System.Serializable]
public class PlayerSaveData
{
    public int Level;
    public int Coins;
}
```

```csharp
PlayerSaveData data = new()
{
    Level = 4,
    Coins = 150
};

PersistentDataManager.SaveClassData(
    "player-save.json",
    data
);

PlayerSaveData loaded =
    PersistentDataManager.LoadClassData<PlayerSaveData>(
        "player-save.json"
    );
```

Classes must follow Unity's `JsonUtility` serialization rules.

## Saving a Dictionary

```csharp
using System.Collections.Generic;
using abcCrea.PersistentData;

Dictionary<string, int> rewards = new()
{
    { "Sapphire", 20 },
    { "Ruby", 10 },
    { "Emerald", 5 }
};

PersistentDataManager.SaveDictionaryData(
    "rewards.json",
    rewards
);
```

Load it with:

```csharp
Dictionary<string, int> loadedRewards =
    PersistentDataManager.LoadDictionaryData<string, int>(
        "rewards.json"
    );
```

Dictionary key and value types must be compatible with Unity serialization.

## Playable Treasure Sample

The package includes a small top-down 2D scene that demonstrates persistence through a normal gameplay loop instead of a traditional CRUD interface.

### How It Works

- Move the character using **WASD** or the **Arrow Keys**.
- Walk over active treasure chests to collect rewards.
- Treasure chests provide Sapphire, Ruby, or Emerald rewards.
- Collected currency values are saved with `PersistentDataManager`.
- Existing reward values are loaded when the sample starts.
- The UI displays the current totals for every reward type.
- Treasures activate over time to create a simple repeatable gameplay loop.

### Concepts Demonstrated

- Primitive value persistence.
- Runtime loading and saving.
- `Rigidbody2D` character movement.
- Trigger-based treasure collection.
- ScriptableObject-based configuration.
- Weighted random reward selection.
- Coroutine-based treasure activation.
- TextMeshPro UI updates.
- Separation between runtime code and optional sample code.

## Importing the Sample

1. Open **Window > Package Manager**.
2. Select **abcCrea Persistent Data**.
3. Expand the **Samples** section.
4. Import **Treasure Collection Sample**.
5. Open the imported scene.
6. Enter Play Mode.

Unity imports the sample into a folder similar to:

```text
Assets/Samples/abcCrea Persistent Data/1.0.1/Treasure Collection Sample/
```

## Public API

### Primitive Data

```csharp
PersistentDataManager.SaveInt(key, value);
PersistentDataManager.LoadInt(key, defaultValue);

PersistentDataManager.SaveFloat(key, value);
PersistentDataManager.LoadFloat(key, defaultValue);

PersistentDataManager.SaveString(key, value);
PersistentDataManager.LoadString(key, defaultValue);

PersistentDataManager.SaveBool(key, value);
PersistentDataManager.LoadBool(key, defaultValue);

PersistentDataManager.SaveVector3(key, value);
PersistentDataManager.LoadVector3(key, defaultValue);

PersistentDataManager.DeletePrimitive(key);
```

### Serializable Classes

```csharp
PersistentDataManager.SaveClassData(fileName, data);
PersistentDataManager.LoadClassData<T>(fileName);
PersistentDataManager.DeleteClassData(fileName);
```

### Dictionaries

```csharp
PersistentDataManager.SaveDictionaryData(fileName, dictionary);
PersistentDataManager.LoadDictionaryData<TKey, TValue>(fileName);
PersistentDataManager.DeleteDictionaryData(fileName);
```

### General Save Management

```csharp
persistentDataManager.SaveVersion();
persistentDataManager.LoadVersion();
persistentDataManager.DeleteAllData();
```

## Advantages

- Small and easy to understand.
- Simple API for common persistence operations.
- Uses native Unity APIs.
- No database setup.
- No external persistence framework.
- Appropriate for beginners and prototypes.
- Supports both primitive and structured data.
- Includes a practical playable sample.
- Runtime and sample code are kept separate.

## Limitations

- It is not a database.
- Data is stored locally.
- Save files are not encrypted.
- File operations are synchronous.
- JSON support is limited by Unity `JsonUtility`.
- Polymorphic data is not handled automatically.
- Dictionaries require serializable key and value types.
- Malformed JSON is not currently recovered automatically.
- There are no cloud saves.
- There are no multiple save slots.
- There are no automatic backups.
- There is no migration framework.
- There is no corruption recovery.
- `DeleteAllData()` deletes all PlayerPrefs and every top-level `.json` file in `Application.persistentDataPath`.

## Namespace

```csharp
using abcCrea.PersistentData;
```

## Package Structure

```text
Runtime/          Runtime persistence system
Samples~/         Optional playable examples
Documentation~/   Documentation assets
package.json      Unity package manifest
README.md         Package documentation
CHANGELOG.md      Version history
LICENSE.md        Package license
```

## License

See [LICENSE.md](LICENSE.md).

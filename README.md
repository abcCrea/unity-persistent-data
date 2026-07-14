# Unity Persistent Data

A lightweight persistence module for Unity that provides a simple API for saving and loading local game data.

The package uses `PlayerPrefs` for primitive values and JSON files for serializable classes and dictionaries, making it suitable for prototypes, game jams, educational projects, and small offline games.

Compatible with Unity 2022.3 or newer.

---

## Dependencies

### Runtime

The runtime library has no third-party dependencies.

### Sample

The included sample uses:
- Unity TextMeshPro (`com.unity.textmeshpro`)

---

## Features

- Save and load `int`, `float`, `string`, `bool`, and `Vector3`.
- Save and load serializable classes as JSON.
- Save and load dictionaries through a serializable wrapper.
- Delete individual saved values.
- Delete JSON save files.
- Delete all package-managed save data.
- Save and load a save-data version.
- Includes a playable sample demonstrating a complete save/load workflow.

---

## Installation

Open **Window → Package Manager**.

Press **+** → **Add package from Git URL**.

Enter:

```text
https://github.com/abcCrea/unity-persistent-data.git
```

---

## Quick Start

```csharp
using abcCrea.PersistentData;

PersistentDataManager.SaveInt("Coins", 100);
PersistentDataManager.SaveBool("TutorialFinished", true);

PlayerPrefs.Save();

int coins = PersistentDataManager.LoadInt("Coins");
bool tutorialFinished = PersistentDataManager.LoadBool("TutorialFinished");
```

---

## Saving Serializable Classes

```csharp
[System.Serializable]
public class PlayerData
{
    public int Coins;
    public int Level;
}
```

```csharp
PlayerData data = new()
{
    Coins = 250,
    Level = 5
};

PersistentDataManager.SaveClassData("player.json", data);

PlayerData loaded =
    PersistentDataManager.LoadClassData<PlayerData>("player.json");
```

---

## Saving Dictionaries

```csharp
Dictionary<string, int> inventory = new()
{
    { "Potion", 5 },
    { "Key", 2 }
};

PersistentDataManager.SaveDictionaryData(
    "inventory.json",
    inventory);
```

```csharp
Dictionary<string, int> loadedInventory =
    PersistentDataManager.LoadDictionaryData<string, int>(
        "inventory.json");
```

---

## Included Sample

The package includes a playable top-down 2D sample that demonstrates the most common persistence workflow.

The sample shows how to:

- Move a character using **WASD** or the **Arrow Keys**.
- Collect rewards from treasure chests.
- Save the collected currencies using `PersistentDataManager`.
- Automatically load the saved values when the scene starts.
- Display the current currency totals through a simple UI.

Instead of a traditional CRUD demo, the sample demonstrates how persistence is typically used during gameplay.

The sample also includes examples of:

- Rigidbody2D character movement.
- Trigger-based interactions.
- ScriptableObject configuration.
- Weighted random rewards.
- Coroutine-based treasure spawning.
- Runtime UI updates.

---

## Runtime API

The package provides support for:

### Primitive values

- `SaveInt()`
- `LoadInt()`
- `SaveFloat()`
- `LoadFloat()`
- `SaveString()`
- `LoadString()`
- `SaveBool()`
- `LoadBool()`
- `SaveVector3()`
- `LoadVector3()`
- `DeletePrimitive()`

### JSON Data

- `SaveClassData()`
- `LoadClassData()`
- `DeleteClassData()`

### Dictionaries

- `SaveDictionaryData()`
- `LoadDictionaryData()`
- `DeleteDictionaryData()`

### Save Management

- Save version support.
- Delete all saved data.
- Save and load events.

---

## Recommended Use Cases

- Player settings
- Game options
- Currencies
- Player progression
- Tutorial completion
- Unlock systems
- Checkpoints
- Small inventories
- Prototypes
- Educational projects
- Game jams

---

## Limitations

- Uses Unity `PlayerPrefs` and `JsonUtility`.
- JSON serialization follows Unity serialization rules.
- File operations are synchronous.
- Save files are not encrypted.
- No cloud save support.
- No save slots.
- No automatic backups.
- No migration system.
- No corruption recovery.

---

## Namespace

```csharp
using abcCrea.PersistentData;
```

---

## License

See the LICENSE file included with the package.
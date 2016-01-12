# LEE - Little Entity Engine

Just an tiny wrapper over [MsgPack](https://github.com/msgpack/msgpack-cli) backed by the file system.

## Usage

```csharp
using System;
using LEE;

public class YourEntity : LEE.Entity
{
    public string Name { get; set; }
    public float Strength { get; set; }

    public YourEntity() : base() { }
}

public static void Main(string[] args)
{
    EntityPersistence persistence = new EntityPersistence("Database");

    var ent = new YourEntity()
    {
        Name = "Test",
        Strength = 5.0f
    };

    // Saves it to disk
    var id = persistence.WriteEntity(ent);

    // Will have both fields populated, including the id
    var retrievedEnt = persistence.RetrieveEntity<YourEntity>(id);

    // Will be null
    var nonExistent = persistence.RetrieveEntity<YourEntity>(id);
}
```
# UnityPackages
## Disclaimer
Most packages have a dependency on the VDFramework which can be found [here](https://github.com/Danny-vD/VDFramework/releases/latest).

### Note
main branch is empty and used as a 'base' for any future packages.
___
## Folder Structure
A package whose sole purpose is to setup all necessary folders at the beginning of a project.  
All the folders are filled by a `PlaceHolder.cs`.

##### How to
Use the search to filter on `PlaceHolder` then select-all and delete them all at once.
___
## SerializableDictionary
A class that acts like a dictionary and can be serialized. It uses a `ReorderableList` for convenience.  
Also included is a `SerializableEnumDictionary` which automatically adds a `SerializableKeyValuePair<,>` for every Enum value.  
Most other packages have a dependency on this package. 
___
## Utility & Editor
A package full of scripts that serve as tools to make development easier.
___
## JSON Localisation
Provides a Parser that reads all JSON files in the `Resources/Language` directory in which all localised strings are mapped per a given `EntryID`.  
The available languages are set in the `Enums.Language` Enum after which the `Localisation.LanguageSettings` take care of it.  
The localised strings are easily retrieved through an utility script (`Localisation.LocalisationUtil`).  
It also supports changing a sprite depending on the current language.
___
## FMOD Package
A package that simplifies FMOD integration with the Unity Engine.
___
## Grid
Contains a couple scripts that manage a 2D grid and place prefabs for each position.  
Also comes with several ways to modify the grid for easy level editing.
___
## Console
Contains a UI Console with input field and a way to easily add new commands to the console.  
It also supports selecting objects in the scene and perform commands on that object.

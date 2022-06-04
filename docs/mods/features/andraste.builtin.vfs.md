# The VFS Feature (`andraste.builtin.vfs`)

## Description

The abbreviation `VFS` stands for virtual file system.
In this context, that means, the real hard-drive is abstracted away via software.
This allows Andraste to redirect every attempt of the game to edit a file (Caution: currently this is not read-only, the game could also overwrite your mod files) into a subdirectory of your mod-directory.

## JSON API

As always, reference the [original code](https://github.com/AndrasteFramework/Shared/blob/master/ModManagement/Json/Features/BuiltinVfsFeature.cs) for the most detailed and up-to-date representation of the settings.

Currently available settings example:

```json
"features": {
    "andraste.builtin.vfs": {
        "directories": [
            "directory1"
        ],
        "files": {
            "file1": "directory2/file2",
            "file2": "INTENTIONAL"
        }
    }
}
```

## API Explanation

### directories

The `directories` setting is the recommended way in most cases:
Define a directory in which Andraste will look for files and those files will then be mounted into the game.

So in this case, if the game had a `a.txt` file in it's root directory, this feature would redirect this file into `directory1/a.txt`.
Keep in mind though, that technically this works the other way around: Andraste scans `directory1` and during loading time adds a redirecting rule for every file found.

### files

If you need more fine grained control over what routes are added, there is the `files` setting:

This setting is an object from game-relative path to mod path, so

```json
{ "a.txt": "my-mod-data/b.txt" }
```

would redirect `a.txt` into `b.txt` in your mod folder.

This is especially relevant, if you want to use the same mod file for multiple game files (e.g. sharing a texture), so you can just point them onto the same file, without having to duplicate them in your mod folder.

You can also use this functionality to redirect the game to an non-existent file. For this, you can redirect it to any path, as long as the path is not invalid (invalid characters etc).

Hint: Andraste will emit a warning, if you redirect onto a non-existant file (because typos happen). If you don't want this to happen, you can redirect to the special "file" `INTENTIONAL`.
This will tell Andraste to not emit a warning.

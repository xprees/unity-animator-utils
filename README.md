# Unity Animator Utils - Better Animator

## Features

- **Animator State Preview**: Quickly preview animation clips in the Animator window.
  TODO images
- **Better Animation Events**: Enhanced animation events with better control and usability.
  TODO images

## Warning

Some features are dependent on the Unity Editor version (6000.0.47f1), I needed to use reflection for a small portion of time animationClip time sync.

## Installation

### Git URL

Using this approach Unity Package Manager will download the package from the Git repository, but the package versioning and updates will not be
properly managed by the Unity Package Manager.

[Unity Docs - Install a UPM package from a Git URL](https://docs.unity3d.com/6000.1/Documentation/Manual/upm-ui-giturl.html)

Install in the Unity Package Manager by adding the following Git URL:

```
https://github.com/xprees/unity-animator-utils.git
```

### Scoped NPM Registry - recommended

Using this approach the unity Package Manager will automatically resolve all my packages and their dependencies and list them in the Package Manager
window with proper versioning and updates.

[Unity Docs - Use a scoped registry in your project](https://docs.unity3d.com/6000.1/Documentation/Manual/upm-scoped-use.html)

Install the package using npm scoped registry in Project Settings > Package Manager > Scoped Registries

```json
{
  "name": "NPM - xprees",
  "url": "https://registry.npmjs.org",
  "scopes": [
    "cz.xprees"
  ]
}
```

Then simply install the package using the Unity Package Manager using the _NPM - xprees_ scope or by the package name `cz.xprees.animator-utils`

## Refs

Inspired by the following videos:

- [git-amend - Using IMPROVED Animation Events in Unity](https://youtu.be/XEDi7fUCQos?si=qyFSOUFqcLmp5zxR)
- [Warped Imagination - Let's Fix Unity's Animator](https://youtu.be/BbNjqlCY9bc?si=WFpVEQv4pFX8PSFP)
- [Warped Imagination - Let's Fix Unity's Animator More](https://youtu.be/wflAXY26yLI?si=1OI-RwRcoQaDrKf0)
# Utility Scripts for the Unity Game Engine
This package contains a set of small, commonly used, narrow-purpose utilities for the [Unity game engine](https://unity.com/), originally authored by the CREATIVE Lab at [NAWCAD Lakehurst](https://www.navair.navy.mil/lakehurst/) for the purposes of virtual training development.

## Using in a project
The current version of this package is intended to be used with the most recent LTS version of Unity 6.

To use this package in a project, open the Packages folder, edit the manifest.json file, and add a new line to the `dependencies` block with the package's name (declared at the top of [package.json](package.json)) and this git repository's URL. More information on this specific feature of Unity can be found [in their documentation](https://docs.unity3d.com/6000.0/Documentation/Manual/upm-git.html).

## Types of Scripts in this Package
The central goal of this package is to extend the functionality of built-in MonoBehaviours and allow for the triggering of basic operations with a single-parameter (or parameterless) function call. This extended functionality can therefore be triggered from a Unity Event in an inspector, such as the OnClick member of a Button component. This allows for a wider variety of features to be implemented before writing any new code, and is especially useful in a prototyping phase. The utilities in this package that fall exactly into this category are:
- [Transform Controller](Runtime/Scripts/TransformController.cs)
- [Image Controller](Runtime/Scripts/ImageController.cs)
- [Game Object Controller](Runtime/Scripts/GameObjectController.cs)

Some utilities in this package also aim to provide single-parameter access to basic engine functionality, but are not related to any specific MonoBehaviour, and are instead constructed as Scriptable Objects. These utilities are:
- [Application Controller](Runtime/Scripts/ApplicationController.cs)
- [Debug Logger](Runtime/Scripts/DebugLogger.cs)

This package also contains some useful utilities that are constructed as MonoBehaviours, but are unrelated to any specific built-in components. These utilities are:
- [Comment](Runtime/Scripts/Comment.cs)
- [Scene Selector](Runtime/Scripts/SceneSelector.cs)

Other utilities in this package aim to provide new Unity Events in the inspector that are activated at times other than a button click. These utilities are:
- [Input Action Processor](Runtime/Scripts/InputActionProcessor.cs)
- [Setup Actions](Runtime/Scripts/SetupActions.cs)

This package also includes an [Android Build](Editor/AndroidBuild.cs) script, which doesn't really fit into any of the above categories.

## More Info
More specific information on each utility can be found in the comments of the C# files, which can also be used to generate HTML documentation with [Doxygen](https://www.doxygen.nl/)

For information on the licensing of this package, please see [INTENT.md](INTENT.md).

For information on how to contribute to this package please see [CONTRIBUTING.md](CONTRIBUTING.md).
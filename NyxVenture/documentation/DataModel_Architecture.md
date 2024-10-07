# DataModel Architecture
This document describes the architecture of the data model. It is crucial to follow all points
outlined in this guide, otherwise the event system of the model won't work properly. For a visual overview
of the model refer to the `datamodel.mdpuml` file in the documents directory.

## Base class for all model classes
The abstract class `ModelBase` is the base for all concrete model classes. Meaning, all classes must derive from this class!
`ModelBase` provides functionality for the event handling and provides two events:

- `public event PropertyChangedEventHandler? PropertyChanged;` : a classical property change event. If any property of
  the model object changes, a PropertyChangedEvent is fire. 
- `public event BubbleChangeEventHander? ModelChanged;` : This is a NyxVenture specific event. It is fired if any
  subobject has changed and propagated to the top of the structure until it reaches the root node of the data model
  (which is the `Game` class).

In addition to this two events, each model class has an attribute `IsObjectChanged` and `IsModelChanged`. 

- `IsObjectChanged` : This flag is `true` if the property of the current object has been changed.
- `IsModelChanged` : This flag is true if the current object or any of its sub objects have been changed.

You can reset the flags of a specific object with the methods `CleanModelChangedFlag()` and `CleanObjectChangedFlag()`. However,
it's a better idea to recusrively clean all flags in the complete hierachy of objects by calling `CleanChangedFlags()`.

`CleanChangedFlags()` recurses the model tree and deletes both flags in each object. It is declared as an abstract method
in the base class, so each model class has to implement it by themself and guarantee that flags are deleted in the
object and in alls sub objects (See class `Game.CleanChangedFlags()` for an example).

## Subclassing `ModelBase`
In order to create a subclass of `ModelBase` there are some rules you have to follow. The first rule is that
your subclass has to extend `ModelBase` and implement the abstract method `CleanChangedFlags()`. The other rules 
adhere to the definition of Properties in your subclass. 

### Defining non-model properties
A non-model property is a property of any basic type like `string` or `int`. The definition of these properties
is quite straighforward. Have a look at the property `Title` in the game class:

```
public class Game : ModelBase
{
	private string? _title;
	...
	public string? Title { get => _title; set => SetProperty(ref _title, value); }
	...
}
```

The most important thing is to call `SetProperty(..)` inside the properties setter. It will take care of
the event handling of your property. If you just set the value as usual, no events will be fired if
the title changes!

### Defining Model properties
When an object has a reference to another model object, we call this a model property. For instance, the `Game` object
has a single `StartChapter`. The most important difference is that there *must not* be a setter but instead we have
three methods to handle the reference called `SetXXX()`, `CreateXXX()` and `RemoveXXX()`.

In order to set a sub object to your object you'll use the `SetXXX()` method. Be aware: if you use this method,
the sub object will be set, but There will be no `ModelChanged` events. You'll use this method only if the sub object
already exists and has been created by the `CreateXXX()` method of any other model class. The goal of this
strategy is that only *one* `ModelChanged` event will be fired if the properties of the sub object change.

Have a look at the `SetStartChapter(..)` method of the `Game` class:

```
public void SetStartChapter(Chapter startChapter)
{
    RemoveStartChapter();                       
    _startChapter = startChapter;                                
    OnPropertyChanged(nameof(StartChapter));
}
```

First thing you have to do is calling the `RemoveXXX()` method. This removes all listeners from the old object. Then the
start chapter is set to the internal field and a `PropertyChangedEvent` is fired.

Note that the `startChapter` parameter of this method is *not nullable*. If you want to delete the content of the
property you should call `RemoveXXX()` instead of calling `SetXXX(null)`!

In order to create *a new sub object* to your object, you'll use the `CreateXXX()` method. It creates the sub ob ject for
you, registers it for event handling and returns it. Look athe `CreateStartChapter()` method of the `Game` class:

```
public Chapter CreateStartChapter()
{
    Chapter chapter = new Chapter();

    RegisterSubnode(chapter);
    SetStartChapter(chapter);

    return chapter;
}
```

`RegisterSubnode()` is defined in the base class. You simply call it and pass the object for which you want to have
`ModelChangeEvents` beeing creates when it changes. To exclude a registeres object, you cann call `UnregisterSubnode()` (see later).

Be sure not to set the sub object manually into your field but call `SetXXX(..)` so that a `PropertyChangeEvent` is fired.

Finally, for removing a property that has previously set, there is a `RemoveXXX()` method. 

```
public void RemoveStartChapter()
{
    if (_startChapter != null)
        UnregisterSubnode(_startChapter);
    _startChapter = null;
    OnPropertyChanged(nameof(StartChapter));
}
```

Before the internal field is set to `null` the subnode is unregistered with the method `UnregisterSubnode(..)` of the base
class so that it won't send ModelChangeEvents anymore.

### Defining List properties
When you have a property in your object class that is a `List` of other model objects, we use a similiar approach as with
the model properties described above. First and important: The `List` property *must not have* a setter, but you have to
define the methods `CreateXXX()`, `AddXXX()` and `RemoveXXX()`.

The `AddXXX()` method is an equivalent to the `SetXXX` method regarding the model properties. It adds an existing sub object
that has been previously created with a `CreateXXX()` method. It is excluded from model events in this path. Have a look at the
`AddFeature(..)` method of the `Game` class:

```
public void AddFeature(Feature feature)
{
    _features.Add(feature);
    OnPropertyChanged(nameof(Features));
}
```

It simply adds the feature to the internal field and fires a `PropertyChangeEvent`. 

For creating a new sub object that is part of the `ModelEvent` hierarchy, you need to have a method like `CreateFeature()`
in the `Game` class:

```
public Feature CreateFeature()
{
    Feature feature = new Feature();
            
    RegisterSubnode(feature);

    AddFeature(feature);
    return feature;
}
```

A new `Feature` is created and registered as a sub node for the `ModelEvent` chain. Then it`s added with the `Add` method
so that the current object fires an appropriate `PropertyChangeEvent`. Again, the process is similar to the `CreateXXX()` 
method of model properties.

Finally, there's a `RemoveXXX()` method that removes a specified sub object from the List. Have a look at the
`RemoveFeature(..)` method of the `Game` class:

```
public void RemoveFeature(Feature feature)
{            
    UnregisterSubnode(feature);

    _features.Remove(feature);
    OnPropertyChanged(nameof(Features));
}
```

Again, this is an equivalent to the `RemoveXXX()` methods of model properties. It unregisters the subnode, then removes the
refence from the `List` an fires a `PropertyChangeEvent`.
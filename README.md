# tween.cs
A .Net (C#) implementation of the Javascript tweening engine (https://github.com/tweenjs/tween.js)

## Features
The features are identical to the Javascript tween engine.
Also the code tried to be as close to the original as possible.

## Usage
```csharp
var position = new Hashtable { { "x", 0.0 } };

var tween = new Tween.Tween(position)
    .To(new Hashtable { { "x", 100.0 }}, 2000)
    .Easing(Easing.Bounce.InOut);

tween.Start();

tween.Updated += (o, args) =>
{
    var y = (double) args.Obj["x"];
    chart.Series["Series"].Points.AddXY(_time, y);
};

for (double i = 0; i < 2000; i = i + 10)
{
    _time = i;
    tween.Update(_time);
}
```

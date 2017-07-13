using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tween
{
    public class TweenCollection 
    {
        private readonly List<Tween> _tweens = new List<Tween>();

        private readonly List<Tween> _tweensAddedDuringUpdate = new List<Tween>();

        public List<Tween> GetAll()
        {
            return _tweens;
        }

        public void RemoveAll()
        {
            _tweens.Clear();
        }

        public void Add(Tween tween)
        {
            Debug.WriteLine($"Adding Tween id: {tween.Id}");

            _tweens.Add(tween);
            _tweensAddedDuringUpdate.Add(tween);
        }

        public void Remove(Tween tween)
        {
            _tweens.Remove(tween);
            _tweensAddedDuringUpdate.Remove(tween);
        }

        public bool Update(double? time, bool? preserve = null)
        {
            // make copy as we might remove items whilst iterating
            var tweens = _tweens.ToArray(); 

            if (tweens.Length == 0)
                return false;

            time = time ?? 0;//TWEEN.now();

            // Tweens are updated in "batches". If you add a new tween during an update, then the
            // new tween will be updated in the next batch.
            // If you remove a tween during an update, it will normally still be updated. However,
            // if the removed tween was added during the current batch, then it will not be updated.
            while (tweens.Length > 0)
            {
                _tweensAddedDuringUpdate.Clear();

                for (var i = 0; i < tweens.Length; i++)
                {
                    if (tweens[i].Update(time.Value) == false /* && !preserve.Value*/)
                    {
                       _tweens.Remove(tweens[i]);
                    }
                }

                tweens = _tweensAddedDuringUpdate.ToArray();
            }

            return true;
        }
    }

    public class Tween
    {
        private static int _nextId;

        public static TweenCollection Tweens = new TweenCollection();

        private readonly Hashtable _obj;

        private readonly Hashtable _valuesStart = new Hashtable();

        private Hashtable _valuesEnd = new Hashtable();

        private readonly Hashtable _valuesStartRepeat = new Hashtable();

        private double _duration = 1; // in seconds

        private int _repeat;

        private int? _repeatDelayTime;

        private bool _yoyo;

        private bool _isPlaying;

        private bool _reversed;

        private double _delayTime;

        private double _startTime;

        private Func<double, double> _easingFunction = global::Tween.Easing.Linear.None;

        private Func<double[], double, double> _interpolationFunction = global::Tween.Interpolation.Linear;

        private readonly List<Tween> _chainedTweens =new List<Tween>();

        private bool _onStartCallbackFired;

        public int Id;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public Tween(Hashtable obj)
        {
            Id = _nextId++;

            _obj = obj;

            // Set all starting values present on the target object
            foreach (var field in obj.Keys)
            {
                _valuesStart[field] = Convert.ToDouble(obj[field]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public Tween To(Hashtable properties, double duration)
        {
            _valuesEnd = properties;
            _duration = duration;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Tween Start(double? time = null)
        {
            Tweens.Add(this);

            _isPlaying = true;

            _onStartCallbackFired = false;

            _startTime = time ?? 0;
            _startTime += _delayTime;

            foreach (var property in new ArrayList(_valuesEnd.Keys))
            {
                // Check if an Array was provided as property value
                if (_valuesEnd[property] is Array)
                {
                    var array = (double[]) _valuesEnd[property];
                    if (array.Length == 0)
                    {
                        continue;
                    }

                    // Create a local copy of the Array with the start value at the front
                    _valuesEnd[property] = new[]{ (double)_valuesStart[property] }.Concat(array).ToArray();
                }

                // If `to()` specifies a property that doesn't exist in the source object,
                // we should not set that property in the object
                if (_valuesStart[property] == null)
                {
                    continue;
                }

                _valuesStart[property] = _obj[property];

                if (_valuesStart[property] is Array == false)
                {
                    //    this._valuesStart[property] *= 1.0; // Ensures we're using numbers, not strings
                }

                _valuesStartRepeat[property] = _valuesStart[property];
            }

            return this;
        }

        public Tween Stop()
        {
            if (!_isPlaying)
            {
                return this;
            }

            Tweens.Remove(this);
            _isPlaying = false;

            RaiseStopped();

            StopChainedTweens();

            return this;
        }

        public Tween End()
        {
            Update(_startTime + _duration);

            return this;
        }

        private void StopChainedTweens()
        {
            foreach (var tween in _chainedTweens)
            {
                tween.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Tween Delay(double amount)
        {
            _delayTime = amount;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public Tween Repeat(int times)
        {
            _repeat = times;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Tween RepeatDelay(int amount)
        {
            _repeatDelayTime = amount;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yoyo"></param>
        /// <returns></returns>
        public Tween Yoyo(bool yoyo)
        {
            _yoyo = yoyo;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Tween Easing(Func<double, double> easing)
        {
            _easingFunction = easing;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Tween Interpolation(Func<double[], double, double> interpolationFunction)
        {
            _interpolationFunction = interpolationFunction;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Tween Chain(Tween arguments)
        {
            _chainedTweens.Add(arguments);

            return this;
        }

        public event EventHandler Started;

        public event EventHandler<TweenEventArgs> Updated;

        public event EventHandler Complete;

        public event EventHandler Stopped;

        protected virtual void RaiseStarted()
        {
            var handler = Started;
            handler?.Invoke(this, new EventArgs());
        }

        protected virtual void RaiseUpdated()
        {
            var handler = Updated;
            handler?.Invoke(this, new TweenEventArgs(_obj));
        }

        protected virtual void RaiseComplete()
        {
            var handler = Complete;
            handler?.Invoke(this, new EventArgs());
        }

        protected virtual void RaiseStopped()
        {
            var handler = Stopped;
            handler?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        public bool Update(double time)
        {
            if (time < _startTime)
            {
                return true;
            }

            if (_onStartCallbackFired == false)
            {
                RaiseStarted();

                _onStartCallbackFired = true;
            }

            var elapsed = (time - _startTime) / _duration;
            elapsed = elapsed > 1 ? 1 : elapsed;

            var value = _easingFunction(elapsed);

            foreach (var property in _valuesEnd.Keys)
            {
                // Don't update properties that do not exist in the source object
                if (_valuesStart[property] == null)
                {
                    continue;
                }

                var start = _valuesStart[property];
                var end = _valuesEnd[property];

                if (end is IEnumerable)
                {
                    var e = (double[])end;
                    _obj[property] = _interpolationFunction(e, value);
                }
                else
                {
                    // Parses relative end values with start as base (e.g.: +10, -3)
                    //if (end is string)
                    //{
                    //}

                    // Protect against non numeric properties.
                    if (end is double)
                    {
                        var e = Convert.ToDouble(end);
                        var s = Convert.ToDouble(start);
                        _obj[property] = s + (e - s) * value;
                    }
                }
            }

            RaiseUpdated();

            if (Math.Abs(elapsed - 1) < double.Epsilon)
            {
                if (_repeat > 0)
                {
                    if (!double.IsInfinity(_repeat))
                    {
                        _repeat--;
                    }

                    // Reassign starting values, restart by making startTime = now
                    foreach (var property in _valuesStartRepeat.Keys)
                    {
                        if (_valuesEnd[property] is string)
                        {
                       //      _valuesStartRepeat[property] = _valuesStartRepeat[property] + parseFloat(_valuesEnd[property], 10);
                        }

                        if (_yoyo)
                        {
                            var tmp = _valuesStartRepeat[property];

                            _valuesStartRepeat[property] = _valuesEnd[property];
                            _valuesEnd[property] = tmp;
                        }
                    }

                    if (_yoyo)
                    {
                        _reversed = !_reversed;
                    }

                    if (_repeatDelayTime != null)
                    {
                        _startTime = time + _repeatDelayTime.Value;
                    }
                    else
                    {
                        _startTime = time + _delayTime;
                    }

                    return true;
                }
                else
                {
                    RaiseComplete();

                    var numChainedTweens = _chainedTweens.Count;
                    for (var i = 0; i < numChainedTweens; i++)
                    {
                        // Make the chained tweens start exactly at the time they should,
                        // even if the `update()` method was called way past the duration of the tween
                        _chainedTweens[i].Start(_startTime + _duration);
                    }

                    return false;
                }
            }

            return true;
        }
    }
}

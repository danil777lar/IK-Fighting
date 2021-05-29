using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using LarjeEnum;


[RequireComponent(typeof(IControll))]
public class PointerFiller : MonoBehaviour
{
    [Serializable]
    public class Pointer 
    {
        public KinematicsPointerType type;
        public Rigidbody2D pointer;

        private Tween _tween = null;
        public Tween Motion 
        {
            get => _tween;
            set 
            {
                _tween?.Kill();
                _tween = value;
                if (_tween != null && _tween.onComplete == null)
                    _tween.onComplete += () => _tween = null;
            }
        }
    }

    public delegate void OnMotionComplete();

    public List<Pointer> pointers = new List<Pointer>(); 

    private IControll _controllInterface;

    private void Start()
    {
        _controllInterface = GetComponent<IControll>();
    }

    public void PushMotion(KinematicsPointerType pt, PointerMotion motion, OnMotionComplete onMotionComplete = null) 
    {
        Pointer pointer = pointers.Find((p) => p.type == pt);
        if (pointer != null)
            pointer.Motion = motion != PointerMotion.None ? MotionBuilder.GetTween(pointer.pointer, motion) : null;
        if (onMotionComplete != null) pointer.Motion.onComplete += () => onMotionComplete.Invoke();
    }

    public void PushMotion(Rigidbody2D rb, PointerMotion motion, OnMotionComplete onMotionComplete = null)
    {
        Pointer pointer = pointers.Find((p) => p.pointer == rb);
        if (pointer != null)
            pointer.Motion = motion != PointerMotion.None ? MotionBuilder.GetTween(pointer.pointer, motion) : null;
        if (onMotionComplete != null) pointer.Motion.onComplete += () => onMotionComplete.Invoke();
    }

    public Rigidbody2D GetPointer(KinematicsPointerType pt) =>
        pointers.Find((p) => p.type == pt).pointer;

    public Tween GetTween(Rigidbody2D rb) =>
        pointers.Find((p) => p.pointer == rb).Motion;

}

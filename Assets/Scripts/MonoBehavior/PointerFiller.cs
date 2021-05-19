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
            }
        }
    }
    public List<Pointer> pointers = new List<Pointer>(); 

    private IControll _controllInterface;

    private void Start()
    {
        _controllInterface = GetComponent<IControll>();
    }

    public void PushMotion(KinematicsPointerType pt, PointerMotion motion) 
    {
        Pointer pointer = pointers.Find((p) => p.type == pt);
        pointer.Motion = MotionBuilder.GetTween(pointer.pointer, motion);
    }
}

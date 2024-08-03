using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gavi
{
    public class Visual : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private Vector2 _size;
        [SerializeField] private Vector2 _padding;
        
        public virtual void SetAlive(bool isAlive)
        {
            
        }
        
        public void SetPosition(int x, int y)
        {
            _root.localPosition = new Vector3(_size.x * _padding.x * x, _size.y * _padding.y * y, 0);
        }
    }
}

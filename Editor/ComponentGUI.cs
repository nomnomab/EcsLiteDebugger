using System;
using System.Reflection;
using Leopotam.EcsLite;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor {
  internal class ComponentGUI: VisualElement {
    private Type _type;
    private FieldInfo[] _fields;
    private EcsWorld _world;
    private int _entity;
    private bool _empty;
    
    public ComponentGUI(Type type, int entity, EcsWorld world) {
      _type = type;
      _fields = _type.GetFields();
      _world = world;
      _entity = entity;
      _empty = _fields.Length == 0;

      if (_empty) {
        Add(new Label("No fields to show"));
      }
    }

    public void Refresh() {
      if (_empty) {
        return;
      }
      
      if (_fields.Length != childCount) {
        Clear();
        
        IEcsPool pool = _world.GetPoolByType(_type);
        
        foreach (FieldInfo fieldInfo in _fields) {
          ComponentField field = new ComponentField(pool, _entity, fieldInfo);
          field.Refresh();
          Add(field);
        }
      }
      
      if (parent is Toggle {value: false}) {
        return;
      }

      foreach (VisualElement visualElement in Children()) {
        ComponentField field = visualElement as ComponentField;
        field.Refresh();
      }
    }
  }
}
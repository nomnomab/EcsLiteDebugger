using System;
using System.Collections.Generic;
using System.Reflection;
using Leopotam.EcsLite;
using Nomnom.EcsLiteDebugger.Editor.Fields;
using UnityEditor;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor {
  public class ComponentField: VisualElement {
    private static Dictionary<Type, Type> _lookup;

    private readonly IEcsPool _pool;
    private readonly int _entity;
    private readonly FieldInfo _fieldInfo;

    private VisualElement _field;
    private IFieldGui _guiInstance;

    static ComponentField() {
      _lookup = new Dictionary<Type, Type>();

      TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom(typeof(FieldGui<>));

      foreach (Type type in types) {
        Type[] args = type.BaseType.GenericTypeArguments;

        _lookup[args[0]] = type;
      }
    }
    
    public ComponentField(IEcsPool pool, int entity, FieldInfo fieldInfo) {
      _pool = pool;
      _entity = entity;
      _fieldInfo = fieldInfo;
    }

    public void Refresh() {
      if (_field == null) {
        _field = GatherValue(GetReference(), _fieldInfo);
        Add(_field);
        return;
      }

      UpdateValue();
    }

    private object GetReference() {
      return _pool.GetRaw(_entity);
    }
    
    private VisualElement GatherValue(object item, FieldInfo fieldInfo) {
      Type type = fieldInfo.FieldType;
      VisualElement element;
      bool hasLookup = _lookup.TryGetValue(type, out Type guiType);

      if (hasLookup || TryFindBaseType(type, out guiType)) {
        _guiInstance = (IFieldGui)Activator.CreateInstance(guiType, this);
        element = _guiInstance.Create(item, fieldInfo);
      } else {
        element = new Label("Unsupported type");
      }

      return element;
    }

    private bool TryFindBaseType(Type type, out Type gui) {
      foreach (Type lookupKey in _lookup.Keys) {
        if (!lookupKey.IsAssignableFrom(type)) {
          continue;
        }

        gui = _lookup[lookupKey];
        return true;
      }

      gui = default;
      return false;
    }

    public void UpdateReference(FieldInfo info, object value) {
      object reference = GetReference();
      info.SetValue(reference, value);
      _pool.SetRaw(_entity, reference);
    }

    private void UpdateValue() {
      if (_guiInstance == null || _guiInstance.IsEditing) {
        return;
      }

      object value = _fieldInfo.GetValue(GetReference());
      _guiInstance.UpdateValue(value);
    }
  }
}
using System;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class EnumGui: FieldGui<Enum> {
    private bool _isFlags;
    
    public EnumGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      if (_isFlags) {
        ((EnumFlagsField)Element).SetValueWithoutNotify((Enum)value);
      } else {
        ((EnumField)Element).SetValueWithoutNotify((Enum)value);
      }
    }

    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      Enum value = (Enum)fieldInfo.GetValue(item);

      _isFlags = value.GetType().GetCustomAttribute<FlagsAttribute>() != null;

      return _isFlags
        ? new EnumFlagsField(fieldInfo.Name, value) {
          value = value
        }
        : new EnumField(fieldInfo.Name, value) {
          value = value
        };
    }
  }
}
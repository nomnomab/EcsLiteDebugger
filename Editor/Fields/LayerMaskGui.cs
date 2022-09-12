using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class LayerMaskGui: FieldGui<LayerMask> {
    public LayerMaskGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((LayerMaskField)Element).SetValueWithoutNotify((LayerMask)value);
    }
    
    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new LayerMaskField(fieldInfo.Name) {
        value = (LayerMask)fieldInfo.GetValue(item)
      };
    }
  }
}
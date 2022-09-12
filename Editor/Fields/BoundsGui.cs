using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class BoundsGui: FieldGui<Bounds> {
    public BoundsGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((BoundsField)Element).SetValueWithoutNotify((Bounds)value);
    }

    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new BoundsField(fieldInfo.Name) {
        value = (Bounds)fieldInfo.GetValue(item)
      };
    }
  }
}
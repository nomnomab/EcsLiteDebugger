using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class RectIntGui: FieldGui<RectInt> {
    public RectIntGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((RectIntField)Element).SetValueWithoutNotify((RectInt)value);
    }

    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new RectIntField(fieldInfo.Name) {
        value = (RectInt)fieldInfo.GetValue(item)
      };
    }
  }
}
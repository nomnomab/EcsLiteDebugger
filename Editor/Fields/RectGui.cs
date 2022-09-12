using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class RectGui: FieldGui<Rect> {
    public RectGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((RectField)Element).SetValueWithoutNotify((Rect)value);
    }

    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new RectField(fieldInfo.Name) {
        value = (Rect)fieldInfo.GetValue(item)
      };
    }
  }
}
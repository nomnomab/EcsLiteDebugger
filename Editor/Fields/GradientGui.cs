using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class GradientGui: FieldGui<Gradient> {
    public GradientGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((GradientField)Element).SetValueWithoutNotify((Gradient)value);
    }

    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new GradientField(fieldInfo.Name) {
        value = (Gradient)fieldInfo.GetValue(item)
      };
    }
  }
}
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class CurveGui: FieldGui<AnimationCurve> {
    public CurveGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((CurveField)Element).SetValueWithoutNotify((AnimationCurve)value);
    }

    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new CurveField(fieldInfo.Name) {
        value = (AnimationCurve)fieldInfo.GetValue(item)
      };
    }
  }
}
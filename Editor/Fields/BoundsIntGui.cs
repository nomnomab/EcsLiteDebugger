using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class BoundsIntGui: FieldGui<BoundsInt> {
    public BoundsIntGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((BoundsIntField)Element).SetValueWithoutNotify((BoundsInt)value);
    }

    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new BoundsIntField(fieldInfo.Name) {
        value = (BoundsInt)fieldInfo.GetValue(item)
      };
    }
  }
}
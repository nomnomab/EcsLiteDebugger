using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class Vector3Gui: FieldGui<Vector3> {
    public Vector3Gui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((Vector3Field)Element).SetValueWithoutNotify((Vector3)value);
    }

    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new Vector3Field(fieldInfo.Name) {
        value = (Vector3)fieldInfo.GetValue(item)
      };
    }
  }
}
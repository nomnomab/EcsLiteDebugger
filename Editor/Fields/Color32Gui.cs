using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class Color32Gui: FieldGui<Color32> {
    public Color32Gui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((ColorField)Element).SetValueWithoutNotify((Color32)value);
    }
    
    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new ColorField(fieldInfo.Name) {
        value = (Color32)fieldInfo.GetValue(item)
      };
    }
  }
}
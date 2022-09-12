using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class ColorGui: FieldGui<Color> {
    public ColorGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((ColorField)Element).SetValueWithoutNotify((Color)value);
    }
    
    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new ColorField(fieldInfo.Name) {
        value = (Color)fieldInfo.GetValue(item)
      };
    }
  }
}
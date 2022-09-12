using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class FloatGui: FieldGui<float> {
    public FloatGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((FloatField)Element).SetValueWithoutNotify((float)value);
    }
    
    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new FloatField(fieldInfo.Name) {
        value = (float)fieldInfo.GetValue(item)
      };
    }
  }
}
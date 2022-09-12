using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class DoubleGui: FieldGui<double> {
    public DoubleGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((DoubleField)Element).SetValueWithoutNotify((double)value);
    }
    
    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new DoubleField(fieldInfo.Name) {
        value = (double)fieldInfo.GetValue(item)
      };
    }
  }
}
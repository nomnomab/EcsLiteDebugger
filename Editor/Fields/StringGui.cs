using System.Reflection;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class StringGui: FieldGui<string> {
    public StringGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((TextField)Element).SetValueWithoutNotify((string)value);
    }
    
    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new TextField(fieldInfo.Name) {
        value = (string)fieldInfo.GetValue(item)
      };
    }
  }
}
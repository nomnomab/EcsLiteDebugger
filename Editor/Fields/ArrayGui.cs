using System;
using System.Reflection;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class ArrayGui: FieldGui<Array> {
    public ArrayGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      Array array = (Array)value;
      ListView listView = (ListView)Element;
      
      listView.itemsSource = array;
      listView.Rebuild();
    }

    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      Array array = (Array)fieldInfo.GetValue(item);
      ListView list = new ListView(array);
      return list;
    }
  }
}
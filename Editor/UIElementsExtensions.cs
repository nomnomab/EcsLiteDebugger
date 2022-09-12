using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor {
  internal static class UIElementsExtensions {
    internal static void SetPadding(this IStyle style, StyleLength length) {
      style.paddingBottom = length;
      style.paddingTop = length;
      style.paddingLeft = length;
      style.paddingRight = length;
    }
    
    internal static void SetMargin(this IStyle style, StyleLength length) {
      style.marginBottom = length;
      style.marginTop = length;
      style.marginLeft = length;
      style.marginRight = length;
    }
    
    internal static void SetBorderRadius(this IStyle style, StyleLength length) {
      style.borderBottomLeftRadius = length;
      style.borderBottomRightRadius = length;
      style.borderTopLeftRadius = length;
      style.borderTopRightRadius = length;
    }
    
    internal static void SetBorderWidth(this IStyle style, StyleFloat length) {
      style.borderBottomWidth = length;
      style.borderTopWidth = length;
      style.borderLeftWidth = length;
      style.borderRightWidth = length;
    }
    
    internal static void SetBorderColor(this IStyle style, Color color) {
      style.borderBottomColor = color;
      style.borderTopColor = color;
      style.borderLeftColor = color;
      style.borderRightColor = color;
    }
  }
}
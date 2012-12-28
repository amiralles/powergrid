namespace PowerGrid.Component {
    public static class StringExtensions {
         public static string ToTrimmedUpperStr(this object target) {
             return target == null ? null : target.ToString().Trim().ToUpper();
         }
    }
}
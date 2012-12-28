namespace PowerGrid.Component {
    using System.Drawing;

    public class Format {
        public Format() {
            ForeColor = Color.Black;
            BackgroundColor = Color.White;
        }

        public string Name { get; set; }
        public Color ForeColor { get; set; }
        public Color BackgroundColor { get; set; }
    }
}
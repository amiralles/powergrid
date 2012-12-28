namespace PowerGrid.Component {
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public static class BuiltinFormats {

        public static List<Format> All = new List<Format>{            
            new Format {                            
                Name = "Highlight Red",
                ForeColor = Color.DarkRed,
                BackgroundColor = Color.FromArgb(255, 253, 177, 168)
            } ,                       
            new Format {                
                Name = "Highlight Green",
                ForeColor = Color.DarkOliveGreen,
                BackgroundColor = Color.FromArgb(255, 197, 197, 169)
            } ,
            new Format {
                Name = "Highlight Yellow",
                BackgroundColor = Color.Yellow
            } ,            
            new Format {
                Name = "Blue Text",
                ForeColor = Color.RoyalBlue,
                BackgroundColor = Color.White
            } ,
            new Format {
                Name = "Red Text",
                ForeColor = Color.DarkRed,
                BackgroundColor = Color.White
            } ,
            new Format {
                Name = "Green Text",
                ForeColor = Color.DarkOliveGreen,
                BackgroundColor = Color.White
            } ,
            new Format {
                Name = "Unformatted",
                ForeColor = Color.Black,
                BackgroundColor = Color.White
            }
        };

        public static Format RowHighlightGreen {
            get { return All.First(f => f.Name == "Resaltar Verde"); }
        }

        public static Format RowHighlightRed {
            get { return All.First(f => f.Name == "Resaltar Rojo"); }
        }

        public static Format Unformatted {
            get { return All.First(f => f.Name == "Sin Formato"); }
        }
    }
}
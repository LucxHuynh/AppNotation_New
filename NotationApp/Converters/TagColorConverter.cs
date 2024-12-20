using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotationApp.Converters
{
    public class TagColorConverter : IValueConverter
    {
        private readonly Dictionary<string, Color> _tagColors = new()
        {
            { "All", Color.FromArgb("#4339F2") },
            { "Personal", Color.FromArgb("#FFB946") },
            { "Work", Color.FromArgb("#1C3E58") },
            { "Family", Color.FromArgb("#FF5733") },
            { "Study", Color.FromArgb("#4CAF50") },
            { "Todo", Color.FromArgb("#9C27B0") },
            { "Shared", Color.FromArgb("#2196F3") },
            { "Other", Color.FromArgb("#607D8B") }
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string currentTag = (string)value;
            string buttonTag = (string)parameter;

            if (currentTag == buttonTag)
            {
                if (_tagColors.TryGetValue(buttonTag, out Color tagColor))
                {
                    return tagColor;
                }
                return Color.FromArgb("#6B4EFF"); // Default color if tag not found
            }

            return Colors.Transparent; // Unselected state
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    

}

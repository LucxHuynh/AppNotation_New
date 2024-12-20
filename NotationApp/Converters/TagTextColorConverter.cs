using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotationApp.Converters
{
    public class TagTextColorConverter : IValueConverter
    {
        // Dictionary định nghĩa màu text cho từng tag
        private readonly Dictionary<string, (Color selected, Color unselected)> _tagTextColors = new()
        {
            { "ALL", (Colors.White, Colors.Black) },
            { "Personal", (Colors.White, Colors.Orange) },
            { "Work", (Colors.White, Colors.Blue) },
            { "Family", (Colors.White, Colors.Red) },
            { "Study", (Colors.White, Colors.Green) },
            { "Todo", (Colors.White, Colors.Purple) },
            { "Shared", (Colors.White, Colors.LightBlue) },
            { "Other", (Colors.White, Colors.Gray) }
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string currentTag = (string)value;
            string buttonTag = (string)parameter;

            if (_tagTextColors.TryGetValue(buttonTag, out var colors))
            {
                return currentTag == buttonTag ? colors.selected : colors.unselected;
            }

            // Default colors if tag not found in dictionary
            return currentTag == buttonTag ? Colors.White : Color.FromArgb("#6B4EFF");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

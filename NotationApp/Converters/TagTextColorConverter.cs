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
        { "All", (Colors.White, Colors.Black) },
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
            string currentTag = value as string;
            string buttonTag = parameter as string;

            if (_tagTextColors.TryGetValue(buttonTag, out var colors))
            {
                // Nếu tag được chọn, trả về màu selected, ngược lại trả về màu unselected
                return currentTag == buttonTag ? colors.selected : colors.unselected;
            }

            // Fallback nếu không tìm thấy tag trong dictionary
            return currentTag == buttonTag ? Colors.White : Colors.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

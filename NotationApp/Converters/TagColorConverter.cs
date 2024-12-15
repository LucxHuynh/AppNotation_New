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
            string currentTag = value as string;
            string buttonTag = parameter as string;

            if (currentTag == buttonTag)
            {
                return _tagColors.TryGetValue(buttonTag, out var color) ? color : Colors.Gray;
            }

            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    

}

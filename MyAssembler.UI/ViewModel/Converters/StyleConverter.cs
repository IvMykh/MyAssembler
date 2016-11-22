using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MyAssembler.UI.ViewModel.Converters
{
    class StyleConverter
        : IMultiValueConverter
    {

        public object Convert(
            object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var targetElement = values[0] as FrameworkElement;
            var style = (StatusBarStyle)Enum.Parse(typeof(StatusBarStyle), values[1].ToString());

            switch (style)
            {
                case StatusBarStyle.Neutral:
                    {
                        return (Style)targetElement.TryFindResource("NeutralStatusBarStyle");
                    } 
                case StatusBarStyle.Success:
                    {
                        return (Style)targetElement.TryFindResource("SuccessStatusBarStyle");
                    }
                case StatusBarStyle.Fail:
                    {
                        return (Style)targetElement.TryFindResource("FailStatusBarStyle");
                    }

                default: return null;
            }
        }

        public object[] ConvertBack(
            object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

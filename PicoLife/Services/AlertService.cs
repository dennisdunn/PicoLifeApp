using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoLife.Services
{
    public class AlertService
    {
        public async Task OK(string title, string text)
        {
            await Application.Current.MainPage.DisplayAlert(title, text, "OK");
        }
        public async Task<bool> OKCancel(string title, string text)
        {
            return await Application.Current.MainPage.DisplayAlert(title, text, "OK", "Cancel");
        }
    }
}

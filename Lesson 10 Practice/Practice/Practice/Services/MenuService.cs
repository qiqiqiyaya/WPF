using Practice.Models;
using Practice.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Practice.Services
{
    public class MenuService
    {
        public Task<List<MenuBar>> GetAll()
        {
            var list = new List<MenuBar>()
            {
                new MenuBar()
                {
                    Icon = "Home", NameSpace = "", Title = "Home",
                    TabItemInfo = new TabItemInfo(typeof(HomeView)){ CloseBtn =Visibility.Collapsed }
                },
                new MenuBar()
                {
                    Icon = "Apps", NameSpace = "", Title = "工作软件",
                    TabItemInfo = new TabItemInfo(typeof(WorkingSoftwareView))
                },
                new MenuBar()
                {
                    Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏",
                    TabItemInfo = new TabItemInfo(typeof(GameView))
                },
                new MenuBar()
                {
                    Icon = "Palette", NameSpace = "", Title = "主题切换",
                    TabItemInfo = new TabItemInfo(typeof(ThemeChangeView))
                },
                new MenuBar()
                {
                    Icon = "React", NameSpace = "", Title = "ReactiveUI",TabItemInfo =
                        new TabItemInfo(typeof(ReactiveView))
                },
                new MenuBar()
                {
                    Icon = "MicrosoftWindows", NameSpace = "", Title = "系统信息",
                    TabItemInfo = new TabItemInfo(typeof(SystemInformationView))
                },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
                new MenuBar() { Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏" },
            };

            return Task.FromResult(list);
        }
    }
}

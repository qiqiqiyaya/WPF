using Practice.Models;
using Practice.Services.interfaces;
using Practice.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Practice.Services
{
    public class MenuService : IMenuService
    {
        public Task<List<MenuBar>> GetAllAsync()
        {
            var list = new List<MenuBar>()
            {
                new MenuBar()
                {
                    Icon = "Home", NameSpace = "", Title = "Home",
                    TabItemMenu = new TabItemMenu(typeof(HomeView),Visibility.Collapsed)
                },
                new MenuBar()
                {
                    Icon = "Apps", NameSpace = "", Title = "工作软件",
                    TabItemMenu = new TabItemMenu(typeof(WorkingSoftwareView))
                },
                new MenuBar()
                {
                    Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏",
                    TabItemMenu = new TabItemMenu(typeof(GameView))
                },
                new MenuBar()
                {
                    Icon = "Palette", NameSpace = "", Title = "主题切换",
                    TabItemMenu = new TabItemMenu(typeof(ThemeChangeView))
                },
                new MenuBar()
                {
                    Icon = "React", NameSpace = "", Title = "ReactiveUI",TabItemMenu =
                        new TabItemMenu(typeof(ReactiveView))
                },
                new MenuBar()
                {
                    Icon = "MicrosoftWindows", NameSpace = "", Title = "系统信息",
                    TabItemMenu = new TabItemMenu(typeof(SystemInformationView))
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

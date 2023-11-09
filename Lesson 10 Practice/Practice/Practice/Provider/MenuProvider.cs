using System;
using Practice.Models;
using Practice.Provider.Interfaces;
using Practice.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Practice.Provider
{
    public class MenuProvider : IMenuProvider
    {
        public Task<List<MenuBar>> GetAllAsync()
        {
            var list = new List<MenuBar>()
            {
                new MenuBar()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Icon = "Home", NameSpace = "", Title = "Home",
                    TabItemMenu = new TabItemMenu(typeof(HomeView),Visibility.Collapsed)
                },
                new MenuBar()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Icon = "Apps", NameSpace = "", Title = "工作软件",
                    TabItemMenu = new TabItemMenu(typeof(WorkingSoftwareView))
                },
                new MenuBar()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Icon = "NintendoGameBoy", NameSpace = "", Title = "游戏",
                    TabItemMenu = new TabItemMenu(typeof(GameView))
                },
                new MenuBar()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Icon = "Palette", NameSpace = "", Title = "主题切换",
                    TabItemMenu = new TabItemMenu(typeof(ThemeChangeView))
                },
                new MenuBar()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Icon = "React", NameSpace = "", Title = "ReactiveUI",
                    TabItemMenu = new TabItemMenu(typeof(ReactiveView))
                },
                new MenuBar()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Icon = "MicrosoftWindows", NameSpace = "", Title = "系统信息",
                    TabItemMenu = new TabItemMenu(typeof(SystemInformationView))
                },
                new MenuBar()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Icon = "Power", NameSpace = "", Title = "开启自启" ,
                    TabItemMenu = new TabItemMenu(typeof(AutoStartupView))
                },
                new MenuBar()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Icon = "GestureTap", NameSpace = "", Title = "最大小化",
                    TabItemMenu = new TabItemMenu(typeof(MinimizedView))
                },
                new MenuBar()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Icon = "Tray", NameSpace = "", Title = "托盘图标",
                    TabItemMenu = new TabItemMenu(typeof(TrayView))
                },
                new MenuBar()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Icon = "TextBoxOutline", NameSpace = "", Title = "系统日志",
                    TabItemMenu = new TabItemMenu(typeof(LogView))
                },
                new MenuBar()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Icon = "ChartLineStacked", NameSpace = "", Title = "图表缩放",
                    TabItemMenu = new TabItemMenu(typeof(ChartZoomView))
                },
                new MenuBar()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Icon = "TestTubeEmpty", NameSpace = "", Title = "Test",
                    TabItemMenu = new TabItemMenu(typeof(TestView))
                },
                new MenuBar()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Icon = "TestTubeEmpty", NameSpace = "", Title = "Test菜单",
                    TabItemMenu = new TabItemMenu(typeof(TestView))
                },
                new MenuBar()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Icon = "TestTubeEmpty", NameSpace = "", Title = "Test菜单",
                    TabItemMenu = new TabItemMenu(typeof(TestView))
                },
                new MenuBar()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Icon = "TestTubeEmpty", NameSpace = "", Title = "Test菜单",
                    TabItemMenu = new TabItemMenu(typeof(TestView))
                }
            };

            return Task.FromResult(list);
        }
    }
}

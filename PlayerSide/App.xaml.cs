﻿using SharedClassLibrary;

namespace PlayerSide;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new Pages.LoginPage();
    }
}

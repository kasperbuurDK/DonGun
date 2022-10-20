using AndroidX.Concurrent.Futures;
using SharedClassLibrary;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.CompilerServices;

namespace PlayerSide.Pages;

public partial class ModSheetPage : ContentPage
{
    private bool _editState;
    private int _sheetId;
    private Action _callbackToParent;

    public MauiPlayer MPlayer { get; set; }
    public bool EditState
    {
        get { return _editState; }
        private set 
        {
            _editState = value;
            RemoveSheetBtn.IsEnabled = value;
            RemoveSheetBtn.IsVisible = value;
        }
    }

    public ModSheetPage(Action callbackToParent)
    {
        InitializeComponent();
        MPlayer = new MauiPlayer();
        BindingContext = MPlayer;
        PopulateRacePicker();
        _callbackToParent = callbackToParent;
        EditState = false;
        ChageSheetBtn.Text = "Add Sheet";
    }

    public ModSheetPage(MauiPlayer p, int id, Action callbackToParent) : this(callbackToParent)
    {
        //Deep copy
        MPlayer = p.CopyObject();
        BindingContext = MPlayer;
        _sheetId = id;
        ChageSheetBtn.Text = "Apply changes";
        RacePicker.IsEnabled = false;
        EditState = true;
    }

    private void PopulateRacePicker()
    {
        List<string> RaceList = new();
        foreach (string race in Enum.GetNames(typeof(Race_abstract.RaceType)))
        {
            RaceList.Add(race);
        }
        RacePicker.ItemsSource = RaceList;
        RacePicker.SelectedItem = MPlayer.Race.Type.ToString();
    }

    private async void ChageSheetBtnClicked(object sender, EventArgs e)
    {
        RestService<Player, Player> restService = new(Globals.RestUserInfo.AuthHeader);
        string user = Globals.RestUserInfo.UserName;
        await restService.SaveDataAsync(MPlayer, Constants.RestUriSheet + user + (!EditState ? "" : "/"+_sheetId), !EditState);

        if (restService.Response.IsSuccessStatusCode)
        {
            await Navigation.PopAsync();
            _callbackToParent();
        }
    }

    private async void RemoveBtnClicked(object sender, EventArgs e)
    {
        RestService<Player, Player> restService = new(Globals.RestUserInfo.AuthHeader);
        string user = Globals.RestUserInfo.UserName;
        await restService.DeleteDataAsync(Constants.RestUriSheet + user + "/" + _sheetId.ToString());

        if (restService.Response.IsSuccessStatusCode)
        {
            await Navigation.PopAsync();
            _callbackToParent();
        }
    }

    private void SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Enum.TryParse((string)RacePicker.SelectedItem, out Race_abstract.RaceType tempRace))
            MPlayer.Race = new Race_abstract(tempRace);
    }

    private void RacePicker_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
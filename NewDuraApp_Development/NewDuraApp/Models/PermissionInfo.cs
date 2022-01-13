﻿using System;
using System.ComponentModel;

namespace NewDuraApp.Models
{
    public class PermissionInfo : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public bool IsGranted { get; set; }
        public Xamarin.Essentials.Permissions.BasePermission Permission { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
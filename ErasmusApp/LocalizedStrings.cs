﻿using ErasmusApp.Resources;

namespace ErasmusApp
{
    public class LocalizedStrings
    {
        private static AppResources _localizedResources = new AppResources();
        public AppResources LocalizedResources { get { return _localizedResources; } }
    }
}
﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Service.Navigation;

namespace Snap.Hutao.ViewModel.Game;

internal sealed class LaunchGameWithUidData : NavigationCompletionSource<string>
{
    public LaunchGameWithUidData(string uid)
        : base(uid)
    {
    }
}
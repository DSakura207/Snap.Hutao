// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Core.IO;

internal readonly struct ValueDirectory
{
    private readonly string value;

    private ValueDirectory(string value)
    {
        this.value = value;
    }

    public bool HasValue { get => value is not null; }

    public static implicit operator string(ValueDirectory value)
    {
        return value.value;
    }

    public static implicit operator ValueDirectory(string? value)
    {
        return new(value!);
    }

    [SuppressMessage("", "CA1307")]
    public override int GetHashCode()
    {
        return value.GetHashCode();
    }

    public override string ToString()
    {
        return value;
    }
}
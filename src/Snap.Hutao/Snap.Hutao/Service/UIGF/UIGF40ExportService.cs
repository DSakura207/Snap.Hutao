﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Core;
using Snap.Hutao.Model.Entity;
using Snap.Hutao.Model.InterChange.GachaLog;
using Snap.Hutao.Service.GachaLog;
using System.Collections.Immutable;
using System.IO;

namespace Snap.Hutao.Service.UIGF;

[ConstructorGenerated]
[Injection(InjectAs.Transient, typeof(IUIGFExportService), Key = UIGFVersion.UIGF40)]
internal sealed partial class UIGF40ExportService : IUIGFExportService
{
    private readonly JsonSerializerOptions jsonOptions;
    private readonly IServiceProvider serviceProvider;
    private readonly ITaskContext taskContext;

    public async ValueTask ExportAsync(UIGFExportOptions exportOptions, CancellationToken token = default)
    {
        await taskContext.SwitchToBackgroundAsync();

        Model.InterChange.GachaLog.UIGF uigf = new()
        {
            Info = new()
            {
                ExportApp = "Snap Hutao",
                ExportAppVersion = $"{HutaoRuntime.Version}",
                ExportTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Version = "v4.0",
            },
        };

        ExportGachaArchives(uigf, exportOptions.GachaArchiveUids);

        using (FileStream stream = File.Create(exportOptions.FilePath))
        {
            await JsonSerializer.SerializeAsync(stream, uigf, jsonOptions, token).ConfigureAwait(false);
        }
    }

    private void ExportGachaArchives(Model.InterChange.GachaLog.UIGF uigf, List<uint> uids)
    {
        if (uids.Count <= 0)
        {
            return;
        }

        IGachaLogRepository gachaLogRepository = serviceProvider.GetRequiredService<IGachaLogRepository>();

        ImmutableArray<UIGFEntry<Hk4eItem>>.Builder results = ImmutableArray.CreateBuilder<UIGFEntry<Hk4eItem>>(uids.Count);
        foreach (uint uid in uids)
        {
            GachaArchive? archive = gachaLogRepository.GetGachaArchiveByUid($"{uid}");
            ArgumentNullException.ThrowIfNull(archive);
            List<GachaItem> dbItems = gachaLogRepository.GetGachaItemListByArchiveId(archive.InnerId);
            UIGFEntry<Hk4eItem> entry = new()
            {
                Uid = uid,
                TimeZone = 0,
                List = dbItems.Select(Hk4eItem.From).ToImmutableArray(),
            };

            results.Add(entry);
        }

        uigf.Hk4e = results.ToImmutable();
    }
}

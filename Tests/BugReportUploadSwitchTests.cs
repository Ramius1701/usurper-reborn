using FluentAssertions;
using UsurperRemake;
using UsurperRemake.Systems;
using Xunit;

namespace UsurperReborn.Tests;

/// <summary>
/// The operator switch for sending ! bug reports to usurper-reborn.net (server_config
/// bug_report_upload). On by default, as before; off keeps reports in bug_reports/ only.
/// </summary>
[Collection("SharedGameSingletons")]
public class BugReportUploadSwitchTests
{
    [Fact]
    public void Setting_IsABoolThatDefaultsOn()
    {
        var d = ServerSettingsRegistry.Get(ServerSettingsRegistry.BugReportUploadKey);
        d.Should().NotBeNull();
        d!.Type.Should().Be(ServerSettingsRegistry.SettingType.Bool);
        d.DefaultValue.Should().Be("true");
        d.Category.Should().Be("Privacy");
    }

    [Fact]
    public void Apply_TurnsTheUploadOffAndOn()
    {
        bool before = GameConfig.BugReportUploadEnabled;
        try
        {
            ServerSettingsRegistry.ApplyConfigValue(ServerSettingsRegistry.BugReportUploadKey, "false");
            GameConfig.BugReportUploadEnabled.Should().BeFalse();
            ServerSettingsRegistry.Get(ServerSettingsRegistry.BugReportUploadKey)!.CurrentValue().Should().Be("false");

            ServerSettingsRegistry.ApplyConfigValue(ServerSettingsRegistry.BugReportUploadKey, "true");
            GameConfig.BugReportUploadEnabled.Should().BeTrue();
        }
        finally
        {
            GameConfig.BugReportUploadEnabled = before;
        }
    }
}

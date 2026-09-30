using System.Reflection;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using Range = SemanticVersioning.Range;

namespace WTTTheLongLostHeadsOfYojenkz;

public record ModMetadata : IModMetadata
{
    public string ModGuid { get; init; } = "com.wtt.yojenkzheads";
    public string Name { get; init; } = "WTT-TheLongLostHeadsOfYojenkz";
    public string Author { get; init; } = "GrooveypenguinX";
    public List<string>? Contributors { get; init; } = null;
    public SemanticVersioning.Version Version { get; init; } = new(typeof(ModMetadata).Assembly.GetName().Version?.ToString(3));
    public Range SptVersion { get; init; } = new("~4.1.6");
    public List<string>? Incompatibilities { get; init; }
    public Dictionary<string, Range>? ModDependencies { get; init; } = new()
    {
        { "com.wtt.commonlib", new Range("~3.0.6") }
    };
    public string? Url { get; init; }
    public bool HasPrepatcher { get; init; } = false;
    public string License { get; init; } = "MIT";
}

[Injectable(TypePriority = OnLoadOrder.Preload + 2)]
public class YojenkzHeads(
    WTTServerCommonLib.WTTServerCommonLib wttCommon
) : IOnLoad
{
    internal static readonly Dictionary<string, string> CharacterAudioMap = new()
    {
        {"Big Boss", "big_boss"},
        {"Kaz Miller", "kaz_miller"},
        {"Revolver Ocelot", "revolver_ocelot"},
        {"Homelander", "homelander"},
        {"Chris Redfield", "chris_redfield"},
        {"Dante", "dante"},
        {"Duke Nukem", "duke_nukem"},
        {"Geralt", "geralt"},
        {"Norman Reedus", "norman_reedus"},
        {"Sam Fisher", "sam_fisher"}
    };
    
    internal static readonly List<string> AudioBundleKeys = new()
    {
        "audio/yojenkz.bundle"
    };
    public async Task OnLoadAsync(CancellationToken cancellationToken)
    {
        
        Assembly assembly = Assembly.GetExecutingAssembly();
        await wttCommon.CustomHeadService.CreateCustomHeads(assembly);
        await wttCommon.CustomVoiceService.CreateCustomVoices(assembly);
        wttCommon.CustomAudioService.RegisterAudioBundles(AudioBundleKeys);
        foreach (var kvp in CharacterAudioMap)
        {
            wttCommon.CustomAudioService.CreateFaceCardAudio(kvp.Key, kvp.Value, true);
        }
    }
}

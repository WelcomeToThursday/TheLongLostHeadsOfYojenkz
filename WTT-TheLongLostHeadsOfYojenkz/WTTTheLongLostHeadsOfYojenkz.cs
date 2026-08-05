using System.Reflection;
using JetBrains.Annotations;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;

namespace WTTTheLongLostHeadsOfYojenkz;

[Injectable(TypePriority = OnLoadOrder.Preload + 2), UsedImplicitly]
public class YojenkzHeads(WTTServerCommonLib.WTTServerCommonLib wttCommon) : IOnLoad
{
    private static readonly Dictionary<string, string> CharacterAudioMap = new()
    {
        { "Big Boss", "big_boss" },
        { "Kaz Miller", "kaz_miller" },
        { "Revolver Ocelot", "revolver_ocelot" },
        { "Homelander", "homelander" },
        { "Chris Redfield", "chris_redfield" },
        { "Dante", "dante" },
        { "Duke Nukem", "duke_nukem" },
        { "Geralt", "geralt"},
        { "Norman Reedus", "norman_reedus" },
        { "Sam Fisher", "sam_fisher" }
    };

    private static readonly List<string> AudioBundleKeys = new()
    {
        "audio/yojenkz.bundle"
    };
    
    public async Task OnLoadAsync(CancellationToken cancellationToken)
    {
        
        var assembly = Assembly.GetExecutingAssembly();
        await wttCommon.CustomHeadService.CreateCustomHeads(assembly);
        await wttCommon.CustomVoiceService.CreateCustomVoices(assembly);
        wttCommon.CustomAudioService.RegisterAudioBundles(AudioBundleKeys);
        foreach (var kvp in CharacterAudioMap)
        {
            wttCommon.CustomAudioService.CreateFaceCardAudio(kvp.Key, kvp.Value, true);
        }
    }
}

using System.IO;
using UnityEditor;

public class SyncDemoFolder
{
    const string SRC_FOLDER = @"Assets/Demo";
    const string TARGET_FOLDER = @"Assets/EcsDamageBubbles/Samples~/Demo";
    
    [MenuItem("Window/SyncDemoFolder")]
    public static void SyncDemoFolderRecursively()
    {

        if (Directory.Exists(TARGET_FOLDER + "/Demo")) Directory.Delete(TARGET_FOLDER, true);

        var srcInfo = new DirectoryInfo(SRC_FOLDER);
        var dstInfo = new DirectoryInfo(TARGET_FOLDER);
        CopyAll(srcInfo, dstInfo);
    }

    private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        if (Directory.Exists(target.FullName) == false) Directory.CreateDirectory(target.FullName);

        foreach (var fi in source.GetFiles()) fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
        foreach (var diSourceDir in source.GetDirectories())
        {
            var nextTargetDir = target.CreateSubdirectory(diSourceDir.Name);
            CopyAll(diSourceDir, nextTargetDir);
        }
    }
}
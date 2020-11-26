#if UNITY_EDITOR
using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    [Serializable]
    public struct MightyDocVersion
    {
        public static readonly MightyDocVersion Invalid = new MightyDocVersion
            {isValid = false, versionNumber = -1, byteSize = -1, docURL = null};

        public bool isValid;

        public int versionNumber;
        public long byteSize;
        public string docURL;
    }

    [InitializeOnLoad]
    public static class MightyDocServices
    {
        private class MightyWebClient : WebClient
        {
            public string Path { get; set; }
            public MightyDocVersion DocVersion { get; set; }
        }

        private const string DOC_NAME = "MightyAttributes_Doc.pdf";
        private const string DOC_EXTENSION = "pdf";

        private const string FILE_PANEL_TITLE = "Download Mighty Doc";

        static MightyDocServices()
        {
            if (!SessionStateUtilities.GetBoolOnce("MightyDocUpdateChecked"))
                CheckForDocUpdate();
        }

        [MenuItem("Tools/[Mighty]Attributes/Check for documentation update", false, -11)]
        public static async void CheckForDocUpdate()
        {
            var docVersion = await DownloadDocVersion();

            if (!docVersion.isValid) return;

            if (MightySettingsServices.FirstTime)
            {
                MightyDownloadDocWindow.Open(MightyDownloadDocWindow.WindowState.FirstTime, docVersion);
                return;
            }

            if (MightySettingsServices.DocVersion < docVersion.versionNumber)
                MightyDownloadDocWindow.Open(MightyDownloadDocWindow.WindowState.NewVersion, docVersion);
        }

        public static async Task<MightyDocVersion> DownloadDocVersion()
        {
            try
            {
                using (var client = new WebClient())
                    return JsonUtility.FromJson<MightyDocVersion>(await client.DownloadStringTaskAsync(new Uri(MightyDocVersionURL.URL)));
            }
            catch (Exception e)
            {
                if (e is WebException)
                    return MightyDocVersion.Invalid;
                
                Debug.LogError(e);
            }

            return MightyDocVersion.Invalid;
        }

        [MenuItem("Tools/[Mighty]Attributes/Download Documentation", false, -10)]
        private static async void DownloadDoc()
        {
            var path = OpenSavePanel();

            if (!string.IsNullOrWhiteSpace(path))
                DownloadDocAtPath(path, await DownloadDocVersion());
        }

        public static void DownloadDocFromVersion(MightyDocVersion docVersion)
        {
            if (!docVersion.isValid) return;
            
            var path = OpenSavePanel();

            if (!string.IsNullOrWhiteSpace(path))
                DownloadDocAtPath(path, docVersion);
        }

        private static string OpenSavePanel() => EditorUtility.SaveFilePanel(FILE_PANEL_TITLE,
            FileUtilities.GetDirectoryPath(MightySettingsServices.DocPath ?? MightySettingsServices.GetDefaultDocPath()), DOC_NAME,
            DOC_EXTENSION);

        private static void DownloadDocAtPath(string path, MightyDocVersion docVersion)
        {
            using (var client = new MightyWebClient())
            {
                client.Path = path;
                client.DocVersion = docVersion;
                client.DownloadFileCompleted += OnDownloadDocCompleted;
                client.DownloadProgressChanged += OnDownloadProgressChanged;
                client.DownloadFileAsync(new Uri(docVersion.docURL), path);

                DownloadingWindowUtilities.Open();
            }
        }

        private static void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (sender is MightyWebClient mightyWebClient)
                DownloadingWindowUtilities.SetPercent((float) e.BytesReceived / mightyWebClient.DocVersion.byteSize);
        }

        private static void OnDownloadDocCompleted(object sender, AsyncCompletedEventArgs e)
        {
            DownloadingWindowUtilities.Close();

            if (!(sender is MightyWebClient mightyWebClient)) return;

            MightySettingsServices.DocPath = mightyWebClient.Path;
            FileUtilities.OpenAtPath(FileUtilities.GetDirectoryPath(mightyWebClient.Path));
            UpdateDocVersion(mightyWebClient.DocVersion.versionNumber);
        }


        public static void UpdateDocVersion(int versionNumber)
        {
            if (versionNumber > MightySettingsServices.DocVersion)
                MightySettingsServices.DocVersion = versionNumber;
        }
    }
}
#endif